using GestionaleNegozio.Models;

namespace GestionaleNegozio.Data.Interfaces
{
    public interface IOrdineDao : IBaseDao<Ordine>
    {
        /// <summary>
        /// Gets orders for a specific store
        /// </summary>
        List<Ordine> GetByNegozio(int idNegozio);

        /// <summary>
        /// Gets orders within a date range
        /// </summary>
        List<Ordine> GetByDateRange(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Gets orders for a specific product
        /// </summary>
        List<Ordine> GetByProdotto(int idProdotto);

        /// <summary>
        /// Gets recent orders with optional limit
        /// </summary>
        List<Ordine> GetRecentOrders(int limit = 10);
    }
}