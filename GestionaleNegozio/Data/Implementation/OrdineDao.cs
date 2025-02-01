using GestionaleNegozio.Models;
using Microsoft.Data.SqlClient;

public class OrdineDao : BaseDao<OrderViewModel>, IOrdineDao
{
    public OrdineDao(string connectionString) : base(connectionString) { }

    public List<OrderViewModel> GetAll()
    {
        var orders = new List<OrderViewModel>();
        using var conn = CreateConnection();
        using var cmd = new SqlCommand(@"
            SELECT o.Id, o.idNegozio, o.nota, o.dataOrdine,
                   d.Id as DettaglioId, d.idProdotto, d.quantita
            FROM Ordini o
            LEFT JOIN DettagliOrdine d ON o.Id = d.idOrdine
            ORDER BY o.Id", conn);

        conn.Open();
        using var reader = cmd.ExecuteReader();

        OrderViewModel currentOrder = null;
        int currentOrderId = -1;

        while (reader.Read())
        {
            if (currentOrderId != reader.GetInt32(0))
            {
                currentOrder = new OrderViewModel
                {
                    Id = reader.GetInt32(0),
                    IdNegozio = reader.GetInt32(1),
                    Nota = reader.IsDBNull(2) ? null : reader.GetString(2),
                    DataOrdine = reader.GetDateTime(3),
                    Items = new List<OrderItem>()
                };
                orders.Add(currentOrder);
                currentOrderId = currentOrder.Id;
            }

            if (!reader.IsDBNull(4))
            {
                currentOrder.Items.Add(new OrderItem
                {
                    Id = reader.GetInt32(4),
                    IdProdotto = reader.GetInt32(5),
                    Quantita = reader.GetInt32(6)
                });
            }
        }
        return orders;
    }

    public OrderViewModel GetById(int id)
    {
        using var conn = CreateConnection();
        using var cmd = new SqlCommand(@"
            SELECT o.Id, o.idNegozio, o.nota, o.dataOrdine,
                   d.Id as DettaglioId, d.idProdotto, d.quantita
            FROM Ordini o
            LEFT JOIN DettagliOrdine d ON o.Id = d.idOrdine
            WHERE o.Id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", id);
        conn.Open();
        using var reader = cmd.ExecuteReader();

        OrderViewModel order = null;

        while (reader.Read())
        {
            if (order == null)
            {
                order = new OrderViewModel
                {
                    Id = reader.GetInt32(0),
                    IdNegozio = reader.GetInt32(1),
                    Nota = reader.IsDBNull(2) ? null : reader.GetString(2),
                    DataOrdine = reader.GetDateTime(3),
                    Items = new List<OrderItem>()
                };
            }

            if (!reader.IsDBNull(4))
            {
                order.Items.Add(new OrderItem
                {
                    Id = reader.GetInt32(4),
                    IdProdotto = reader.GetInt32(5),
                    Quantita = reader.GetInt32(6)
                });
            }
        }
        return order;
    }

    public List<OrderViewModel> GetByNegozio(int idNegozio)
    {
        var orders = new List<OrderViewModel>();
        using var conn = CreateConnection();
        using var cmd = new SqlCommand(@"
            SELECT o.Id, o.idNegozio, o.nota, o.dataOrdine,
                   d.Id as DettaglioId, d.idProdotto, d.quantita
            FROM Ordini o
            LEFT JOIN DettagliOrdine d ON o.Id = d.idOrdine
            WHERE o.idNegozio = @IdNegozio
            ORDER BY o.Id", conn);

        cmd.Parameters.AddWithValue("@IdNegozio", idNegozio);
        conn.Open();
        using var reader = cmd.ExecuteReader();

        OrderViewModel currentOrder = null;
        int currentOrderId = -1;

        while (reader.Read())
        {
            if (currentOrderId != reader.GetInt32(0))
            {
                currentOrder = new OrderViewModel
                {
                    Id = reader.GetInt32(0),
                    IdNegozio = reader.GetInt32(1),
                    Nota = reader.IsDBNull(2) ? null : reader.GetString(2),
                    DataOrdine = reader.GetDateTime(3),
                    Items = new List<OrderItem>()
                };
                orders.Add(currentOrder);
                currentOrderId = currentOrder.Id;
            }

            if (!reader.IsDBNull(4))
            {
                currentOrder.Items.Add(new OrderItem
                {
                    Id = reader.GetInt32(4),
                    IdProdotto = reader.GetInt32(5),
                    Quantita = reader.GetInt32(6)
                });
            }
        }
        return orders;
    }

    public List<OrderViewModel> GetByDateRange(DateTime startDate, DateTime endDate)
    {
        var orders = new List<OrderViewModel>();
        using var conn = CreateConnection();
        using var cmd = new SqlCommand(@"
            SELECT o.Id, o.idNegozio, o.nota, o.dataOrdine,
                   d.Id as DettaglioId, d.idProdotto, d.quantita
            FROM Ordini o
            LEFT JOIN DettagliOrdine d ON o.Id = d.idOrdine
            WHERE o.dataOrdine BETWEEN @StartDate AND @EndDate
            ORDER BY o.Id", conn);

        cmd.Parameters.AddWithValue("@StartDate", startDate);
        cmd.Parameters.AddWithValue("@EndDate", endDate);
        conn.Open();
        using var reader = cmd.ExecuteReader();

        OrderViewModel currentOrder = null;
        int currentOrderId = -1;

        while (reader.Read())
        {
            if (currentOrderId != reader.GetInt32(0))
            {
                currentOrder = new OrderViewModel
                {
                    Id = reader.GetInt32(0),
                    IdNegozio = reader.GetInt32(1),
                    Nota = reader.IsDBNull(2) ? null : reader.GetString(2),
                    DataOrdine = reader.GetDateTime(3),
                    Items = new List<OrderItem>()
                };
                orders.Add(currentOrder);
                currentOrderId = currentOrder.Id;
            }

            if (!reader.IsDBNull(4))
            {
                currentOrder.Items.Add(new OrderItem
                {
                    Id = reader.GetInt32(4),
                    IdProdotto = reader.GetInt32(5),
                    Quantita = reader.GetInt32(6)
                });
            }
        }
        return orders;
    }

    public List<OrderViewModel> GetByProdotto(int idProdotto)
    {
        var orders = new List<OrderViewModel>();
        using var conn = CreateConnection();
        using var cmd = new SqlCommand(@"
            SELECT o.Id, o.idNegozio, o.nota, o.dataOrdine,
                   d.Id as DettaglioId, d.idProdotto, d.quantita
            FROM Ordini o
            INNER JOIN DettagliOrdine d ON o.Id = d.idOrdine
            WHERE d.idProdotto = @IdProdotto
            ORDER BY o.Id", conn);

        cmd.Parameters.AddWithValue("@IdProdotto", idProdotto);
        conn.Open();
        using var reader = cmd.ExecuteReader();

        OrderViewModel currentOrder = null;
        int currentOrderId = -1;

        while (reader.Read())
        {
            if (currentOrderId != reader.GetInt32(0))
            {
                currentOrder = new OrderViewModel
                {
                    Id = reader.GetInt32(0),
                    IdNegozio = reader.GetInt32(1),
                    Nota = reader.IsDBNull(2) ? null : reader.GetString(2),
                    DataOrdine = reader.GetDateTime(3),
                    Items = new List<OrderItem>()
                };
                orders.Add(currentOrder);
                currentOrderId = currentOrder.Id;
            }

            if (!reader.IsDBNull(4))
            {
                currentOrder.Items.Add(new OrderItem
                {
                    Id = reader.GetInt32(4),
                    IdProdotto = reader.GetInt32(5),
                    Quantita = reader.GetInt32(6)
                });
            }
        }
        return orders;
    }

    public List<OrderViewModel> GetRecentOrders(int limit = 10)
    {
        var orders = new List<OrderViewModel>();
        using var conn = CreateConnection();
        using var cmd = new SqlCommand(@"
            SELECT o.Id, o.idNegozio, o.nota, o.dataOrdine,
                   d.Id as DettaglioId, d.idProdotto, d.quantita
            FROM (
                SELECT TOP (@Limit) *
                FROM Ordini
                ORDER BY dataOrdine DESC
            ) o
            LEFT JOIN DettagliOrdine d ON o.Id = d.idOrdine
            ORDER BY o.Id", conn);

        cmd.Parameters.AddWithValue("@Limit", limit);
        conn.Open();
        using var reader = cmd.ExecuteReader();

        OrderViewModel currentOrder = null;
        int currentOrderId = -1;

        while (reader.Read())
        {
            if (currentOrderId != reader.GetInt32(0))
            {
                currentOrder = new OrderViewModel
                {
                    Id = reader.GetInt32(0),
                    IdNegozio = reader.GetInt32(1),
                    Nota = reader.IsDBNull(2) ? null : reader.GetString(2),
                    DataOrdine = reader.GetDateTime(3),
                    Items = new List<OrderItem>()
                };
                orders.Add(currentOrder);
                currentOrderId = currentOrder.Id;
            }

            if (!reader.IsDBNull(4))
            {
                currentOrder.Items.Add(new OrderItem
                {
                    Id = reader.GetInt32(4),
                    IdProdotto = reader.GetInt32(5),
                    Quantita = reader.GetInt32(6)
                });
            }
        }
        return orders;
    }

    public int Insert(OrderViewModel order)
    {
        using var conn = CreateConnection();
        conn.Open();
        using var transaction = conn.BeginTransaction();

        try
        {
            using var cmdOrder = new SqlCommand(@"
                INSERT INTO Ordini (idNegozio, nota, dataOrdine)
                VALUES (@IdNegozio, @Nota, @DataOrdine);
                SELECT SCOPE_IDENTITY()", conn, transaction);

            cmdOrder.Parameters.AddWithValue("@IdNegozio", order.IdNegozio);
            cmdOrder.Parameters.AddWithValue("@Nota", (object)order.Nota ?? DBNull.Value);
            cmdOrder.Parameters.AddWithValue("@DataOrdine", order.DataOrdine);

            int orderId = Convert.ToInt32(cmdOrder.ExecuteScalar());

            using var cmdDetails = new SqlCommand(@"
                INSERT INTO DettagliOrdine (idOrdine, idProdotto, quantita)
                VALUES (@IdOrdine, @IdProdotto, @Quantita)", conn, transaction);

            foreach (var item in order.Items)
            {
                cmdDetails.Parameters.Clear();
                cmdDetails.Parameters.AddWithValue("@IdOrdine", orderId);
                cmdDetails.Parameters.AddWithValue("@IdProdotto", item.IdProdotto);
                cmdDetails.Parameters.AddWithValue("@Quantita", item.Quantita);
                cmdDetails.ExecuteNonQuery();
            }

            transaction.Commit();
            return orderId;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public void Update(OrderViewModel order)
    {
        using var conn = CreateConnection();
        conn.Open();
        using var transaction = conn.BeginTransaction();

        try
        {
            using var cmdOrder = new SqlCommand(@"
                UPDATE Ordini 
                SET idNegozio = @IdNegozio, 
                    nota = @Nota, 
                    dataOrdine = @DataOrdine
                WHERE Id = @Id", conn, transaction);

            cmdOrder.Parameters.AddWithValue("@Id", order.Id);
            cmdOrder.Parameters.AddWithValue("@IdNegozio", order.IdNegozio);
            cmdOrder.Parameters.AddWithValue("@Nota", (object)order.Nota ?? DBNull.Value);
            cmdOrder.Parameters.AddWithValue("@DataOrdine", order.DataOrdine);
            cmdOrder.ExecuteNonQuery();

            DeleteOrderItems(order.Id);

            using var cmdDetails = new SqlCommand(@"
                INSERT INTO DettagliOrdine (idOrdine, idProdotto, quantita)
                VALUES (@IdOrdine, @IdProdotto, @Quantita)", conn, transaction);

            foreach (var item in order.Items)
            {
                cmdDetails.Parameters.Clear();
                cmdDetails.Parameters.AddWithValue("@IdOrdine", order.Id);
                cmdDetails.Parameters.AddWithValue("@IdProdotto", item.IdProdotto);
                cmdDetails.Parameters.AddWithValue("@Quantita", item.Quantita);
                cmdDetails.ExecuteNonQuery();
            }

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public void Delete(int id)
    {
        using var conn = CreateConnection();
        conn.Open();
        using var transaction = conn.BeginTransaction();

        try
        {
            DeleteOrderItems(id);

            using var cmdOrder = new SqlCommand(
                "DELETE FROM Ordini WHERE Id = @Id",
                conn, transaction);
            cmdOrder.Parameters.AddWithValue("@Id", id);
            cmdOrder.ExecuteNonQuery();

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public void DeleteOrderItems(int orderId)
    {
        using var conn = CreateConnection();
        using var cmd = new SqlCommand(
            "DELETE FROM DettagliOrdine WHERE idOrdine = @OrderId", conn);
        cmd.Parameters.AddWithValue("@OrderId", orderId);
        conn.Open();
        cmd.ExecuteNonQuery();
    }

    public bool Exists(int id)
    {
        return Exists("SELECT 1 FROM Ordini WHERE Id = @Id",
            new[] { new SqlParameter("@Id", id) });
    }
}
