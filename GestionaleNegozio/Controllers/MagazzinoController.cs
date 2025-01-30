using GestionaleNegozio.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
[Authorize]
public class MagazzinoController : BaseController
{
    private readonly MagazzinoDao _magazzinoDao;

    public MagazzinoController(IConfiguration configuration) : base(configuration)
    {
        _magazzinoDao = new MagazzinoDao(_connectionString);
    }

    public ActionResult GetByNegozio(int idNegozio)
    {
        var magazzino = _magazzinoDao.GetByNegozio(idNegozio);
        return View(magazzino);
    }

    public ActionResult GetByProdotto(int idProdotto)
    {
        var magazzino = _magazzinoDao.GetByProdotto(idProdotto);
        return View(magazzino);
    }

    [HttpPost]
    public ActionResult UpdateQuantita(int idNegozio, int idProdotto, int nuovaQuantita)
    {
        _magazzinoDao.UpdateQuantita(idNegozio, idProdotto, nuovaQuantita);
        return RedirectToAction(nameof(GetByNegozio), new { idNegozio });
    }

    public ActionResult CheckDisponibilita(int idNegozio, int idProdotto, int quantitaRichiesta)
    {
        var isDisponibile = _magazzinoDao.IsDisponibile(idNegozio, idProdotto, quantitaRichiesta);
        return Json(new { disponibile = isDisponibile });
    }
}
