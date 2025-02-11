using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GestionaleNegozio.Models;
[Authorize]
public class MagazzinoController : BaseController
{
    private readonly MagazzinoDao _magazzinoDao;
    private readonly NegozioDao _negozioDao;
    private readonly ProdottoDao _prodottoDao;

    public MagazzinoController(IConfiguration configuration) : base(configuration)
    {
        _magazzinoDao = new MagazzinoDao(_connectionString);
        _negozioDao = new NegozioDao(_connectionString);
        _prodottoDao = new ProdottoDao(_connectionString);
    }

    public ActionResult Index(string searchTerm, int page = 1)
    {
        const int PageSize = 10;
        var allStores = _negozioDao.GetAll().ToList();

        if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                allStores = allStores.Where(s =>
                    s.Città.ToLower().Contains(searchTerm) ||
                    s.Regione.ToLower().Contains(searchTerm)
                ).ToList();
            }
        
        var paginatedStores = allStores
            .Skip((page - 1) * PageSize)
            .Take(PageSize)
            .ToList();

        ViewBag.CurrentSearch = searchTerm;
        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = (int)Math.Ceiling(allStores.Count / (double)PageSize);

        return View(paginatedStores);
    }

    public ActionResult GetByNegozio(int idNegozio)
    {
        var inventory = _magazzinoDao.GetByNegozio(idNegozio);
        ViewBag.Negozio = _negozioDao.GetById(idNegozio);
        ViewBag.Prodotti = _prodottoDao.GetAll();
        return View(inventory);
    }

    public ActionResult GetByProdotto(int idProdotto)
    {
        var inventory = _magazzinoDao.GetByProdotto(idProdotto);
        ViewBag.Negozi = _negozioDao.GetAll();
        ViewBag.Prodotto = _prodottoDao.GetById(idProdotto);
        return View(inventory);
    }

    [HttpPost]
    [Authorize(Roles = "Manager")]
    [ValidateAntiForgeryToken]
    public ActionResult UpdateQuantita(int idNegozio, int idProdotto, int nuovaQuantita)
    {
        try
        {
            _magazzinoDao.UpdateQuantita(idNegozio, idProdotto, nuovaQuantita);
            TempData["Success"] = "Quantity updated successfully.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Error updating quantity.";
        }
        return RedirectToAction("LowStock", "Magazzino");
    }

    public ActionResult LowStock(int threshold = 10)
    {
        var inventory = _magazzinoDao.GetLowStock(threshold);
        ViewBag.Negozi = _negozioDao.GetAll();
        ViewBag.Prodotti = _prodottoDao.GetAll();
        return View(inventory);
    }

    public ActionResult OutOfStock()
    {
        var inventory = _magazzinoDao.GetOutOfStock();
        ViewBag.Negozi = _negozioDao.GetAll();
        ViewBag.Prodotti = _prodottoDao.GetAll();
        return View(inventory);
    }

    [Authorize(Roles = "Manager")]
    public ActionResult InventoryValue()
    {
        try
        {
            var totalValue = _magazzinoDao.GetInventoryValue();
            return View(totalValue);
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Error calculating inventory value.";
            return RedirectToAction(nameof(Index));
        }
    }
}
