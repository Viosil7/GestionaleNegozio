using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GestionaleNegozio.Models;

namespace GestionaleNegozio.Controllers
{
    [Authorize]
    public class NegozioController : BaseController
    {
        private readonly NegozioDao _negozioDao;

        public NegozioController(IConfiguration configuration) : base(configuration)
        {
            _negozioDao = new NegozioDao(_connectionString);
        }

        public ActionResult Index()
        {
            try
            {
                var negozi = _negozioDao.GetAll();
                return View(negozi);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        public ActionResult Details(int id)
        {
            try
            {
                var negozio = _negozioDao.GetById(id);
                if (negozio == null)
                {
                    return NotFound();
                }
                return View(negozio);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [Authorize(Roles = "Manager")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public ActionResult Create(Negozio negozio)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _negozioDao.Insert(negozio);
                    return RedirectToAction(nameof(Index));
                }
                return View(negozio);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while creating the store.");
                return View(negozio);
            }
        }

        [Authorize(Roles = "Manager")]
        public ActionResult Edit(int id)
        {
            try
            {
                var negozio = _negozioDao.GetById(id);
                if (negozio == null)
                {
                    return NotFound();
                }
                return View(negozio);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public ActionResult Edit(Negozio negozio)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _negozioDao.Update(negozio);
                    return RedirectToAction(nameof(Index));
                }
                return View(negozio);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while updating the store.");
                return View(negozio);
            }
        }

        [Authorize(Roles = "Manager")]
        public ActionResult Delete(int id)
        {
            try
            {
                var negozio = _negozioDao.GetById(id);
                if (negozio == null)
                {
                    return NotFound();
                }
                return View(negozio);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                _negozioDao.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while deleting the store.";
                return RedirectToAction(nameof(Index));
            }
        }

        public ActionResult ByRegione(string regione)
        {
            try
            {
                var negozi = _negozioDao.GetByRegione(regione);
                return View("Index", negozi);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        public ActionResult ByCitta(string citta)
        {
            try
            {
                var negozi = _negozioDao.GetByCitta(citta);
                return View("Index", negozi);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
