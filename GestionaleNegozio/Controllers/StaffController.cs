using GestionaleNegozio.Models;
using Microsoft.AspNetCore.Mvc;

public class StaffController : BaseController
{
    private readonly StaffDao _staffDao;

    public StaffController()
    {
        _staffDao = new StaffDao(_connectionString);
    }

    public ActionResult Index()
    {
        var staff = _staffDao.GetAll();
        return View(staff);
    }

    public ActionResult Details(int id)
    {
        var staff = _staffDao.GetById(id);
        if (staff == null) return NotFound();
        return View(staff);
    }

    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Create(Staff staff)
    {
        if (ModelState.IsValid)
        {
            _staffDao.Insert(staff);
            return RedirectToAction(nameof(Index));
        }
        return View(staff);
    }

    public ActionResult Edit(int id)
    {
        var staff = _staffDao.GetById(id);
        if (staff == null) return NotFound();
        return View(staff);
    }

    [HttpPost]
    public ActionResult Edit(Staff staff)
    {
        if (ModelState.IsValid)
        {
            _staffDao.Update(staff);
            return RedirectToAction(nameof(Index));
        }
        return View(staff);
    }

    public ActionResult Delete(int id)
    {
        var staff = _staffDao.GetById(id);
        if (staff == null) return NotFound();
        return View(staff);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
        _staffDao.Delete(id);
        return RedirectToAction(nameof(Index));
    }
}
