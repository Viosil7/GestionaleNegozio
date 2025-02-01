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

        public IActionResult Index()
        {
            var orders = _ordineDao.GetRecentOrders(5);
            var ordersWithDetails = orders.Select(o => new
            {
                Id = o.Id,
                StoreName = _negozioDao.GetById(o.IdNegozio)?.Città ?? "Unknown",
                Date = o.DataOrdine.ToString("dd/MM/yyyy HH:mm"),
                TotalItems = o.Items.Sum(i => i.Quantita),
                Nota = o.Nota
            }).ToList();

            var lowStockItems = _magazzinoDao.GetLowStock(10)
                .Select(m => new
                {
                    ProductName = _prodottoDao.GetById(m.IdProdotto)?.Nome ?? "Unknown",
                    StoreName = _negozioDao.GetById(m.IdNegozio)?.Città ?? "Unknown",
                    m.Quantità
                }).ToList();

            ViewBag.RecentOrders = ordersWithDetails;
            ViewBag.LowStockItems = lowStockItems;
            ViewBag.RecentOrdersCount = orders.Count;
            ViewBag.LowStockCount = lowStockItems.Count;
            ViewBag.ActiveStoresCount = _negozioDao.GetAll().Count;

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
