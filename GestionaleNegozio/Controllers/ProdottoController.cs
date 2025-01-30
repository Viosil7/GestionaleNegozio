using GestionaleNegozio.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
[Authorize]
public class ProdottoController : BaseController
{
    private readonly ProdottoDao _prodottoDao;

    public ProdottoController(IConfiguration configuration) : base(configuration)
    {
        _prodottoDao = new ProdottoDao(_connectionString);
    }

    public ActionResult Index()
    {
        var prodotti = _prodottoDao.GetAll();
        return View(prodotti);
    }

    public ActionResult Details(int id)
    {
        var prodotto = _prodottoDao.GetById(id);
        if (prodotto == null) return NotFound();
        return View(prodotto);
    }

    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Create(Prodotto prodotto)
    {
        if (ModelState.IsValid)
        {
            _prodottoDao.Insert(prodotto);
            return RedirectToAction(nameof(Index));
        }
        return View(prodotto);
    }

    public ActionResult Edit(int id)
    {
        var prodotto = _prodottoDao.GetById(id);
        if (prodotto == null) return NotFound();
        return View(prodotto);
    }

    [HttpPost]
    public ActionResult Edit(Prodotto prodotto)
    {
        if (ModelState.IsValid)
        {
            _prodottoDao.Update(prodotto);
            return RedirectToAction(nameof(Index));
        }
        return View(prodotto);
    }

    public ActionResult Delete(int id)
    {
        var prodotto = _prodottoDao.GetById(id);
        if (prodotto == null) return NotFound();
        return View(prodotto);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
        _prodottoDao.Delete(id);
        return RedirectToAction(nameof(Index));
    }
}
