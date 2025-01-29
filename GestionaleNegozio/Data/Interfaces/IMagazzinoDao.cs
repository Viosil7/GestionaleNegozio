using GestionaleNegozio.Models;

namespace GestionaleNegozio.Data.Interfaces
{
    public interface IMagazzinoDao
    {
        /// <summary>
        /// Gets inventory for a specific store
        /// </summary>
        List<Magazzino> GetByNegozio(int idNegozio);

        /// <summary>
        /// Gets inventory for a specific product across all stores
        /// </summary>
        List<Magazzino> GetByProdotto(int idProdotto);

        /// <summary>
        /// Updates product quantity in a store
        /// </summary>
        void UpdateQuantita(int idNegozio, int idProdotto, int nuovaQuantita);

        /// <summary>
        /// Gets current quantity of a product in a store
        /// </summary>
        int GetQuantita(int idNegozio, int idProdotto);

        /// <summary>
        /// Checks if a product is available in a store
        /// </summary>
        bool IsDisponibile(int idNegozio, int idProdotto, int quantitaRichiesta);
    }
}