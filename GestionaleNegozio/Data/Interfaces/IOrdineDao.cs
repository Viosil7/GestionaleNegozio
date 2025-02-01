using GestionaleNegozio.Models;

public interface IOrdineDao
{
    List<OrderViewModel> GetAll();
    OrderViewModel GetById(int id);
    int Insert(OrderViewModel order);
    void Update(OrderViewModel order);
    void Delete(int id);
    void DeleteOrderItems(int orderId);
    List<OrderViewModel> GetByNegozio(int idNegozio);
    List<OrderViewModel> GetByDateRange(DateTime startDate, DateTime endDate);
    List<OrderViewModel> GetByProdotto(int idProdotto);
    List<OrderViewModel> GetRecentOrders(int limit = 10);
    bool Exists(int id);
}
