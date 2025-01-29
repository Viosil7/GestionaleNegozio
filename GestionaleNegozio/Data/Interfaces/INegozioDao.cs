using GestionaleNegozio.Models;

namespace GestionaleNegozio.Data.Interfaces
{
    public interface INegozioDao : IBaseDao<Negozio>
    {
        /// <summary>
        /// Gets stores by region
        /// </summary>
        List<Negozio> GetByRegione(string regione);

        /// <summary>
        /// Gets stores by city
        /// </summary>
        List<Negozio> GetByCitta(string citta);

        /// <summary>
        /// Gets stores with specific product in stock
        /// </summary>
        List<Negozio> GetByProdottoDisponibile(int idProdotto);
    }
}