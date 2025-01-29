using GestionaleNegozio.Models;

namespace GestionaleNegozio.Data.Interfaces
{
    public interface IProdottoDao : IBaseDao<Prodotto>
    {
        /// <summary>
        /// Gets products by category
        /// </summary>
        List<Prodotto> GetByCategoria(string categoria);

        /// <summary>
        /// Gets products by price range
        /// </summary>
        List<Prodotto> GetByPriceRange(decimal minPrice, decimal maxPrice);

        /// <summary>
        /// Searches products by name or description
        /// </summary>
        List<Prodotto> Search(string searchTerm);
    }
}