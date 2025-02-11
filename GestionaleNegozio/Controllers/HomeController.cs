using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionaleNegozio.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly OrdineDao _ordineDao;
        private readonly MagazzinoDao _magazzinoDao;
        private readonly NegozioDao _negozioDao;
        private readonly ProdottoDao _prodottoDao;

        public HomeController(IConfiguration configuration) : base(configuration)
        {
            _ordineDao = new OrdineDao(_connectionString);
            _magazzinoDao = new MagazzinoDao(_connectionString);
            _negozioDao = new NegozioDao(_connectionString);
            _prodottoDao = new ProdottoDao(_connectionString);
        }

        public IActionResult Index(int page = 1, int orderPage = 1)
        {
            const int pageSize = 10;

            var allOrders = _ordineDao.GetRecentOrders(50);
            var ordersWithDetails = allOrders
                .Select(o => new
                {
                    Id = o.Id,
                    StoreName = _negozioDao.GetById(o.IdNegozio)?.Città ?? "Unknown",
                    Date = o.DataOrdine.ToString("dd/MM/yyyy HH:mm"),
                    TotalItems = o.Items.Sum(i => i.Quantita),
                    Nota = o.Nota
                })
                .ToList();

            var paginatedOrders = ordersWithDetails
                .Skip((orderPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var allLowStockItems = _magazzinoDao.GetLowStock(10)
                .Select(m => new
                {
                    ProductName = _prodottoDao.GetById(m.IdProdotto)?.Nome ?? "Unknown",
                    StoreName = _negozioDao.GetById(m.IdNegozio)?.Città ?? "Unknown",
                    m.Quantità
                }).ToList();

            var lowStockItems = allLowStockItems
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.RecentOrders = paginatedOrders;
            ViewBag.LowStockItems = lowStockItems;
            ViewBag.RecentOrdersCount = allOrders.Count;
            ViewBag.LowStockCount = allLowStockItems.Count;
            ViewBag.ActiveStoresCount = _negozioDao.GetAll().Count;

            ViewBag.CurrentPage = page;
            ViewBag.CurrentOrderPage = orderPage;
            ViewBag.TotalPages = (int)Math.Ceiling(allLowStockItems.Count / (double)pageSize);
            ViewBag.TotalOrderPages = (int)Math.Ceiling(allOrders.Count / (double)pageSize);

            return View();
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Error()
        {
            return View();
        }
    }
}
