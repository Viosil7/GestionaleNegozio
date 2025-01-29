using GestionaleNegozio.Models;
using Microsoft.AspNetCore.Mvc;

public class NegozioController : BaseController
{
    private readonly NegozioDao _negozioDao;

    public NegozioController()
    {
        _negozioDao = new NegozioDao(_connectionString);
    }

    public ActionResult Index()
    {
        var negozi = _negozioDao.GetAll();
        return View(negozi);
    }

    public ActionResult Details(int id)
    {
        var negozio = _negozioDao.GetById(id);
        if (negozio == null) return NotFound();
        return View(negozio);
    }

    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Create(Negozio negozio)
    {
        if (ModelState.IsValid)
        {
            _negozioDao.Insert(negozio);
            return RedirectToAction(nameof(Index));
        }
        return View(negozio);
    }

    public ActionResult Edit(int id)
    {
        var negozio = _negozioDao.GetById(id);
        if (negozio == null) return NotFound();
        return View(negozio);
    }

    [HttpPost]
    public ActionResult Edit(Negozio negozio)
    {
        if (ModelState.IsValid)
        {
            _negozioDao.Update(negozio);
            return RedirectToAction(nameof(Index));
        }
        return View(negozio);
    }

    public ActionResult Delete(int id)
    {
        var negozio = _negozioDao.GetById(id);
        if (negozio == null) return NotFound();
        return View(negozio);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
        _negozioDao.Delete(id);
        return RedirectToAction(nameof(Index));
    }
}
