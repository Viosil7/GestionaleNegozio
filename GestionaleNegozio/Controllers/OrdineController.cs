using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GestionaleNegozio.Models;
using System.Transactions;
using System.Reflection;
using System.Collections.Generic;
using System.Collections;

namespace GestionaleNegozio.Controllers
{
    [Authorize]
    public class OrdineController : BaseController
    {
        private readonly OrdineDao _ordineDao;
        private readonly NegozioDao _negozioDao;
        private readonly ProdottoDao _prodottoDao;
        private readonly MagazzinoDao _magazzinoDao;
        private OrderViewModel _orderViewModel;

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
                ViewBag.Prodotti = _prodottoDao.GetAll();
                return View(order);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        public ActionResult Create()
        {
            try
            {
                ViewBag.Negozi = _negozioDao.GetAll();
                ViewBag.Prodotti = _prodottoDao.GetAll();
                return View(new OrderViewModel
                {
                    DataOrdine = DateTime.Now,
                    Items = new List<OrderItem>()
                });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OrderViewModel model)
        {
            try
            {
                if (ModelState.IsValid && model.Items != null && model.Items.Any())
                {
                    
                    List<OrderItem> unavailableItems = new();
                    foreach (var item in model.Items)
                    {
                        if (_magazzinoDao.IsDisponibile(model.IdNegozio, item.IdProdotto, item.Quantita) == false)
                            unavailableItems.Add(item);
                    }

                    if (unavailableItems.Count > 0)
                    {
                        ViewBag.Negozi = _negozioDao.GetAll();
                        ViewBag.Prodotti = _prodottoDao.GetAll();
                        return View(model);
                    }
                    _ordineDao.Insert(model);

                    foreach (var item in model.Items)
                    {
                        int stock = _magazzinoDao.GetQuantita(model.IdNegozio, item.IdProdotto);
                        _magazzinoDao.UpdateQuantita(model.IdNegozio, item.IdProdotto, (stock - item.Quantita));
                    }

                    return RedirectToAction(nameof(Index));
                }

                if (!model.Items?.Any() ?? true)
                {
                    ModelState.AddModelError("", "At least one item is required.");
                }

                ViewBag.Negozi = _negozioDao.GetAll();
                ViewBag.Prodotti = _prodottoDao.GetAll();
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while creating the order.");
                ViewBag.Negozi = _negozioDao.GetAll();
                ViewBag.Prodotti = _prodottoDao.GetAll();
                return View(model);
            }
        }

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
        public ActionResult Edit(OrderViewModel model)
        {
            try
            {
                if (ModelState.IsValid && model.Items != null && model.Items.Any())
                {
                    List<OrderItem> unavailableItems = new();
                    var listaVecchia = _ordineDao.GetById(model.Id).Items;

                    var listaIntersezione = from newItem in model.Items
                                    join oldItem in listaVecchia on newItem.IdProdotto equals oldItem.IdProdotto
                                    select new { newItem, oldItem };

                    var listaAggiunti = model.Items
                        .GroupJoin(listaVecchia, newItem => newItem.IdProdotto, oldItem => oldItem.IdProdotto, (newItem, matching) => new { newItem, matching })
                        .Where(x => !x.matching.Any())
                        .Select(x => x.newItem);

                    foreach (var item in listaIntersezione)
                    {
                        if (_magazzinoDao.IsDisponibile(model.IdNegozio, item.newItem.IdProdotto, item.newItem.Quantita - item.oldItem.Quantita) == false)
                            unavailableItems.Add(item.newItem);
                    }
                    foreach (var item in listaAggiunti)
                    {
                        if (_magazzinoDao.IsDisponibile(model.IdNegozio, item.IdProdotto, item.Quantita) == false)
                            unavailableItems.Add(item);
                    }
                

                    if (unavailableItems.Count > 0)
                    {
                        ViewBag.Negozi = _negozioDao.GetAll();
                        ViewBag.Prodotti = _prodottoDao.GetAll();
                        return View(model);
                    }

                    _ordineDao.Update(model);

                    foreach (var item in listaIntersezione)
                    {
                        int stock = _magazzinoDao.GetQuantita(model.IdNegozio, item.newItem.IdProdotto);
                        _magazzinoDao.UpdateQuantita(model.IdNegozio, item.newItem.IdProdotto, (stock - (item.newItem.Quantita - item.oldItem.Quantita)));
                    }
                    foreach (var item in listaAggiunti)
                    {
                        int stock = _magazzinoDao.GetQuantita(model.IdNegozio, item.IdProdotto);
                        _magazzinoDao.UpdateQuantita(model.IdNegozio, item.IdProdotto, (stock - item.Quantita));
                    }

                    return RedirectToAction(nameof(Index));
                }

                if (!model.Items?.Any() ?? true)
                {
                    ModelState.AddModelError("", "At least one item is required.");
                }

                ViewBag.Negozi = _negozioDao.GetAll();
                ViewBag.Prodotti = _prodottoDao.GetAll();
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while updating the order.");
                ViewBag.Negozi = _negozioDao.GetAll();
                ViewBag.Prodotti = _prodottoDao.GetAll();
                return View(model);
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                var order = _ordineDao.GetById(id);
                if (order == null)
                    return NotFound();

                ViewBag.Negozio = _negozioDao.GetById(order.IdNegozio);
                ViewBag.Prodotti = _prodottoDao.GetAll();
                return View(order);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var order = _ordineDao.GetById(id);
                foreach (var item in order.Items)
                {
                    int stock = _magazzinoDao.GetQuantita(order.IdNegozio, item.IdProdotto);
                    _magazzinoDao.UpdateQuantita(order.IdNegozio, item.IdProdotto, (stock + item.Quantita));
                }
                _ordineDao.Delete(id);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while deleting the order.";
                return RedirectToAction(nameof(Index));
            }
        }

        public ActionResult GetByStore(int idNegozio)
        {
            try
            {
                ViewBag.Negozi = _negozioDao.GetAll();
                ViewBag.Prodotti = _prodottoDao.GetAll();
                var orders = _ordineDao.GetByNegozio(idNegozio);
                return View("Index", orders);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        public ActionResult GetByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                ViewBag.Negozi = _negozioDao.GetAll();
                ViewBag.Prodotti = _prodottoDao.GetAll();
                var orders = _ordineDao.GetByDateRange(startDate, endDate);
                return View("Index", orders);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        public ActionResult GetRecentOrders()
        {
            try
            {
                ViewBag.Negozi = _negozioDao.GetAll();
                ViewBag.Prodotti = _prodottoDao.GetAll();
                var orders = _ordineDao.GetRecentOrders();
                return View("Index", orders);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
