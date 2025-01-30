using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GestionaleNegozio.Models;

namespace GestionaleNegozio.Controllers
{
    [Authorize]
    public class OrdineController : BaseController
    {
        private readonly OrdineDao _ordineDao;
        private readonly NegozioDao _negozioDao;
        private readonly ProdottoDao _prodottoDao;

        public OrdineController(IConfiguration configuration) : base(configuration)
        {
            _ordineDao = new OrdineDao(_connectionString);
            _negozioDao = new NegozioDao(_connectionString);
            _prodottoDao = new ProdottoDao(_connectionString);
        }

        public ActionResult Index()
        {
            try
            {
                ViewBag.Negozi = _negozioDao.GetAll();
                ViewBag.Prodotti = _prodottoDao.GetAll();
                var orders = _ordineDao.GetAll();
                return View(orders);
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
                var order = _ordineDao.GetById(id);
                if (order == null)
                    return NotFound();

                ViewBag.Negozio = _negozioDao.GetById(order.IdNegozio);
                ViewBag.Prodotto = _prodottoDao.GetById(order.IdProdotto);
                return View(order);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [Authorize(Roles = "Manager")]
        public ActionResult Create()
        {
            try
            {
                ViewBag.Negozi = _negozioDao.GetAll();
                ViewBag.Prodotti = _prodottoDao.GetAll();
                return View(new Ordine { DataOrdine = DateTime.Now });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public ActionResult Create(Ordine ordine)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _ordineDao.Insert(ordine);
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.Negozi = _negozioDao.GetAll();
                ViewBag.Prodotti = _prodottoDao.GetAll();
                return View(ordine);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while creating the order.");
                ViewBag.Negozi = _negozioDao.GetAll();
                ViewBag.Prodotti = _prodottoDao.GetAll();
                return View(ordine);
            }
        }

        [Authorize(Roles = "Manager")]
        public ActionResult Edit(int id)
        {
            try
            {
                var order = _ordineDao.GetById(id);
                if (order == null)
                    return NotFound();

                ViewBag.Negozi = _negozioDao.GetAll();
                ViewBag.Prodotti = _prodottoDao.GetAll();
                return View(order);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public ActionResult Edit(Ordine ordine)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _ordineDao.Update(ordine);
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.Negozi = _negozioDao.GetAll();
                ViewBag.Prodotti = _prodottoDao.GetAll();
                return View(ordine);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while updating the order.");
                ViewBag.Negozi = _negozioDao.GetAll();
                ViewBag.Prodotti = _prodottoDao.GetAll();
                return View(ordine);
            }
        }

        [Authorize(Roles = "Manager")]
        public ActionResult Delete(int id)
        {
            try
            {
                var order = _ordineDao.GetById(id);
                if (order == null)
                    return NotFound();

                ViewBag.Negozio = _negozioDao.GetById(order.IdNegozio);
                ViewBag.Prodotto = _prodottoDao.GetById(order.IdProdotto);
                return View(order);
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
                _ordineDao.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while deleting the order.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
