using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GestionaleNegozio.Models;
using GestionaleNegozio.Data.Interfaces;

[Authorize(Roles = "Manager")]
public class ProdottoController : Controller
{
    private readonly ProdottoDao _prodottoDao;
    private readonly MagazzinoDao _magazzinoDao;
    private readonly NegozioDao _negozioDao;

    public ProdottoController(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        _prodottoDao = new ProdottoDao(connectionString);
        _magazzinoDao = new MagazzinoDao(connectionString);
        _negozioDao = new NegozioDao(connectionString);
    }


    public IActionResult Index(string searchTerm, int page = 1)
    {
        const int pageSize = 7;
        ViewBag.CurrentSearch = searchTerm;

        var allProducts = string.IsNullOrEmpty(searchTerm)
            ? _prodottoDao.GetAll()
            : _prodottoDao.Search(searchTerm);

        var paginatedProducts = allProducts
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = (int)Math.Ceiling(allProducts.Count() / (double)pageSize);

        return View(paginatedProducts);
    }


    public IActionResult Create()
    {
        PrepareViewBags();
        return View(new Prodotto());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Prodotto prodotto, Dictionary<int, int> inventory)
    {
        if (ModelState.IsValid)
        {
            try
            {
                _prodottoDao.Insert(prodotto);

                foreach (var entry in inventory)
                {
                    _magazzinoDao.CreateProductRecord(entry.Key, prodotto.Id, entry.Value);
                }

                TempData["Success"] = "Product created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error creating product: {ex.Message}";
            }
        }

        ViewBag.Negozi = _negozioDao.GetAll();
        return View(prodotto);
    }
    public IActionResult Edit(int id)
    {
        var prodotto = _prodottoDao.GetById(id);
        if (prodotto == null)
        {
            return NotFound();
        }

        PrepareViewBags(id);
        return View(prodotto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Prodotto prodotto, Dictionary<int, int> inventory)
    {
        if (id != prodotto.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _prodottoDao.Update(prodotto);

                foreach (var entry in inventory)
                {
                    _magazzinoDao.UpdateQuantita(entry.Key, prodotto.Id, entry.Value);
                }

                TempData["Success"] = "Product updated successfully";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error updating product: {ex.Message}";
            }
            return RedirectToAction(nameof(Index));
        }

        PrepareViewBags(id);
        return View(prodotto);
    }

    public IActionResult Delete(int id)
    {
        var prodotto = _prodottoDao.GetById(id);
        if (prodotto == null)
        {
            return NotFound();
        }
        return View(prodotto);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        try
        {
            _prodottoDao.Delete(id);
            TempData["Success"] = "Product deleted successfully";
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Error deleting product: {ex.Message}";
        }
        return RedirectToAction(nameof(Index));
    }

    private void PrepareViewBags(int? prodottoId = null)
    {
        ViewBag.Negozi = _negozioDao.GetAll();

        var inventory = new Dictionary<int, int>();
        if (prodottoId.HasValue)
        {
            foreach (var negozio in ViewBag.Negozi)
            {
                inventory[negozio.Id] = _magazzinoDao.GetQuantita(negozio.Id, prodottoId.Value);
            }
        }
        ViewBag.Inventory = inventory;
    }
    public IActionResult Details(int id)
    {
        var prodotto = _prodottoDao.GetById(id);

        if (prodotto == null)
        {
            TempData["Error"] = "Product not found";
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Negozi = _negozioDao.GetAll();
        var inventory = new Dictionary<int, int>();
        foreach (var negozio in ViewBag.Negozi)
        {
            inventory[negozio.Id] = _magazzinoDao.GetQuantita(negozio.Id, id);
        }
        ViewBag.Inventory = inventory;

        return View(prodotto);
    }

}
