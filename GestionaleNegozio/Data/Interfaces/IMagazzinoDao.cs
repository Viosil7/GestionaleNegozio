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
        /// <summary>
        /// Gets all inventory items with quantity below or equal to the specified threshold
        /// </summary>
        /// <param name="threshold">The quantity threshold to check against</param>
        /// <returns>List of inventory items with low stock</returns>
        List<Magazzino> GetLowStock(int threshold);

        /// <summary>
        /// Gets all inventory items with zero quantity
        /// </summary>
        /// <returns>List of out-of-stock inventory items</returns>
        List<Magazzino> GetOutOfStock();

        /// <summary>
        /// Gets all inventory items for products in a specific category
        /// </summary>
        /// <param name="categoria">The product category to filter by</param>
        /// <returns>List of inventory items in the specified category</returns>
        List<Magazzino> GetByCategoria(string categoria);

        /// <summary>
        /// Calculates the total value of all inventory across all stores
        /// </summary>
        /// <returns>The total monetary value of all inventory</returns>
        decimal GetInventoryValue();

        void CreateProductRecord(int idNegozio, int idProdotto, int quantita);
    }
}