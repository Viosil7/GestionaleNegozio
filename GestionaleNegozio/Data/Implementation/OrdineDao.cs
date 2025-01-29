using GestionaleNegozio.Data.Interfaces;
using GestionaleNegozio.Models;
using Microsoft.Data.SqlClient;

public class OrdineDao : BaseDao<Ordine>, IOrdineDao
{
    public OrdineDao(string connectionString) : base(connectionString) { }

    public List<Ordine> GetAll()
    {
        var ordini = new List<Ordine>();
        using var conn = CreateConnection();
        using var cmd = new SqlCommand("SELECT Id, idNegozio, idProdotto, nota, quantitaTotale, dataOrdine FROM Ordini", conn);
        conn.Open();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            ordini.Add(new Ordine
            {
                Id = reader.GetInt32(0),
                IdNegozio = reader.GetInt32(1),
                IdProdotto = reader.GetInt32(2),
                Nota = reader.IsDBNull(3) ? null : reader.GetString(3),
                QuantitaTotale = reader.GetInt32(4),
                DataOrdine = reader.GetDateTime(5)
            });
        }
        return ordini;
    }

    public Ordine GetById(int id)
    {
        using var conn = CreateConnection();
        using var cmd = new SqlCommand("SELECT Id, idNegozio, idProdotto, nota, quantitaTotale, dataOrdine FROM Ordini WHERE Id = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", id);
        conn.Open();
        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return new Ordine
            {
                Id = reader.GetInt32(0),
                IdNegozio = reader.GetInt32(1),
                IdProdotto = reader.GetInt32(2),
                Nota = reader.IsDBNull(3) ? null : reader.GetString(3),
                QuantitaTotale = reader.GetInt32(4),
                DataOrdine = reader.GetDateTime(5)
            };
        }
        return null;
    }

    public List<Ordine> GetByNegozio(int idNegozio)
    {
        var ordini = new List<Ordine>();
        using var conn = CreateConnection();
        using var cmd = new SqlCommand("SELECT Id, idNegozio, idProdotto, nota, quantitaTotale, dataOrdine FROM Ordini WHERE idNegozio = @IdNegozio", conn);
        cmd.Parameters.AddWithValue("@IdNegozio", idNegozio);
        conn.Open();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            ordini.Add(new Ordine
            {
                Id = reader.GetInt32(0),
                IdNegozio = reader.GetInt32(1),
                IdProdotto = reader.GetInt32(2),
                Nota = reader.IsDBNull(3) ? null : reader.GetString(3),
                QuantitaTotale = reader.GetInt32(4),
                DataOrdine = reader.GetDateTime(5)
            });
        }
        return ordini;
    }

    public List<Ordine> GetByDateRange(DateTime startDate, DateTime endDate)
    {
        var ordini = new List<Ordine>();
        using var conn = CreateConnection();
        using var cmd = new SqlCommand(
            "SELECT Id, idNegozio, idProdotto, nota, quantitaTotale, dataOrdine " +
            "FROM Ordini WHERE dataOrdine BETWEEN @StartDate AND @EndDate", conn);
        cmd.Parameters.AddWithValue("@StartDate", startDate);
        cmd.Parameters.AddWithValue("@EndDate", endDate);
        conn.Open();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            ordini.Add(new Ordine
            {
                Id = reader.GetInt32(0),
                IdNegozio = reader.GetInt32(1),
                IdProdotto = reader.GetInt32(2),
                Nota = reader.IsDBNull(3) ? null : reader.GetString(3),
                QuantitaTotale = reader.GetInt32(4),
                DataOrdine = reader.GetDateTime(5)
            });
        }
        return ordini;
    }

    public List<Ordine> GetByProdotto(int idProdotto)
    {
        var ordini = new List<Ordine>();
        using var conn = CreateConnection();
        using var cmd = new SqlCommand(
            "SELECT Id, idNegozio, idProdotto, nota, quantitaTotale, dataOrdine " +
            "FROM Ordini WHERE idProdotto = @IdProdotto", conn);
        cmd.Parameters.AddWithValue("@IdProdotto", idProdotto);
        conn.Open();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            ordini.Add(new Ordine
            {
                Id = reader.GetInt32(0),
                IdNegozio = reader.GetInt32(1),
                IdProdotto = reader.GetInt32(2),
                Nota = reader.IsDBNull(3) ? null : reader.GetString(3),
                QuantitaTotale = reader.GetInt32(4),
                DataOrdine = reader.GetDateTime(5)
            });
        }
        return ordini;
    }

    public List<Ordine> GetRecentOrders(int limit = 10)
    {
        var ordini = new List<Ordine>();
        using var conn = CreateConnection();
        using var cmd = new SqlCommand(
            "SELECT TOP (@Limit) Id, idNegozio, idProdotto, nota, quantitaTotale, dataOrdine " +
            "FROM Ordini ORDER BY dataOrdine DESC", conn);
        cmd.Parameters.AddWithValue("@Limit", limit);
        conn.Open();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            ordini.Add(new Ordine
            {
                Id = reader.GetInt32(0),
                IdNegozio = reader.GetInt32(1),
                IdProdotto = reader.GetInt32(2),
                Nota = reader.IsDBNull(3) ? null : reader.GetString(3),
                QuantitaTotale = reader.GetInt32(4),
                DataOrdine = reader.GetDateTime(5)
            });
        }
        return ordini;
    }

    public void Insert(Ordine ordine)
    {
        using var conn = CreateConnection();
        using var cmd = new SqlCommand(
            "INSERT INTO Ordini (idNegozio, idProdotto, nota, quantitaTotale, dataOrdine) " +
            "VALUES (@IdNegozio, @IdProdotto, @Nota, @QuantitaTotale, @DataOrdine)", conn);
        cmd.Parameters.AddWithValue("@IdNegozio", ordine.IdNegozio);
        cmd.Parameters.AddWithValue("@IdProdotto", ordine.IdProdotto);
        cmd.Parameters.AddWithValue("@Nota", (object)ordine.Nota ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@QuantitaTotale", ordine.QuantitaTotale);
        cmd.Parameters.AddWithValue("@DataOrdine", ordine.DataOrdine);
        conn.Open();
        cmd.ExecuteNonQuery();
    }

    public void Update(Ordine ordine)
    {
        using var conn = CreateConnection();
        using var cmd = new SqlCommand(
            "UPDATE Ordini SET idNegozio = @IdNegozio, idProdotto = @IdProdotto, " +
            "nota = @Nota, quantitaTotale = @QuantitaTotale, dataOrdine = @DataOrdine " +
            "WHERE Id = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", ordine.Id);
        cmd.Parameters.AddWithValue("@IdNegozio", ordine.IdNegozio);
        cmd.Parameters.AddWithValue("@IdProdotto", ordine.IdProdotto);
        cmd.Parameters.AddWithValue("@Nota", (object)ordine.Nota ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@QuantitaTotale", ordine.QuantitaTotale);
        cmd.Parameters.AddWithValue("@DataOrdine", ordine.DataOrdine);
        conn.Open();
        cmd.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        using var conn = CreateConnection();
        using var cmd = new SqlCommand("DELETE FROM Ordini WHERE Id = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", id);
        conn.Open();
        cmd.ExecuteNonQuery();
    }

    public bool Exists(int id)
    {
        return Exists("SELECT 1 FROM Ordini WHERE Id = @Id",
            new[] { new SqlParameter("@Id", id) });
    }
}
