using GestionaleNegozio.Models;
using Microsoft.AspNetCore.Mvc;

public class OrdineController : BaseController
{
    private readonly OrdineDao _ordineDao;

    public OrdineController()
    {
        _ordineDao = new OrdineDao(_connectionString);
    }

    public ActionResult Index()
    {
        var ordini = _ordineDao.GetAll();
        return View(ordini);
    }

    public ActionResult Details(int id)
    {
        var ordine = _ordineDao.GetById(id);
        if (ordine == null) return NotFound();
        return View(ordine);
    }

    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Create(Ordine ordine)
    {
        if (ModelState.IsValid)
        {
            ordine.DataOrdine = DateTime.Now;
            _ordineDao.Insert(ordine);
            return RedirectToAction(nameof(Index));
        }
        return View(ordine);
    }

    public ActionResult Edit(int id)
    {
        var ordine = _ordineDao.GetById(id);
        if (ordine == null) return NotFound();
        return View(ordine);
    }

    [HttpPost]
    public ActionResult Edit(Ordine ordine)
    {
        if (ModelState.IsValid)
        {
            _ordineDao.Update(ordine);
            return RedirectToAction(nameof(Index));
        }
        return View(ordine);
    }

    public ActionResult Delete(int id)
    {
        var ordine = _ordineDao.GetById(id);
        if (ordine == null) return NotFound();
        return View(ordine);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
        _ordineDao.Delete(id);
        return RedirectToAction(nameof(Index));
    }
}
