using GestionaleNegozio.Data.Interfaces;
using GestionaleNegozio.Models;
using Microsoft.Data.SqlClient;
using System.Data;
public class MagazzinoDao : BaseDao<Magazzino>, IMagazzinoDao
{
    public MagazzinoDao(string connectionString) : base(connectionString) { }

    public List<Magazzino> GetByNegozio(int idNegozio)
    {
        var magazzino = new List<Magazzino>();
        using var conn = CreateConnection();
        using var cmd = new SqlCommand(
            "SELECT idNegozio, idProdotto, quantità " +
            "FROM Magazzino WHERE idNegozio = @IdNegozio", conn);
        cmd.Parameters.AddWithValue("@IdNegozio", idNegozio);
        conn.Open();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            magazzino.Add(new Magazzino
            {
                IdNegozio = reader.GetInt32(0),
                IdProdotto = reader.GetInt32(1),
                Quantità = reader.GetInt32(2)
            });
        }
        return magazzino;
    }

    public List<Magazzino> GetByProdotto(int idProdotto)
    {
        var magazzino = new List<Magazzino>();
        using var conn = CreateConnection();
        using var cmd = new SqlCommand(
            "SELECT idNegozio, idProdotto, quantità " +
            "FROM Magazzino WHERE idProdotto = @IdProdotto", conn);
        cmd.Parameters.AddWithValue("@IdProdotto", idProdotto);
        conn.Open();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            magazzino.Add(new Magazzino
            {
                IdNegozio = reader.GetInt32(0),
                IdProdotto = reader.GetInt32(1),
                Quantità = reader.GetInt32(2)
            });
        }
        return magazzino;
    }

    public void UpdateQuantita(int idNegozio, int idProdotto, int nuovaQuantita)
    {
        using var conn = CreateConnection();
        using var cmd = new SqlCommand(
            "UPDATE Magazzino SET quantità = @Quantita " +
            "WHERE idNegozio = @IdNegozio AND idProdotto = @IdProdotto", conn);
        cmd.Parameters.AddWithValue("@Quantita", nuovaQuantita);
        cmd.Parameters.AddWithValue("@IdNegozio", idNegozio);
        cmd.Parameters.AddWithValue("@IdProdotto", idProdotto);
        conn.Open();
        cmd.ExecuteNonQuery();
    }

    public List<Magazzino> GetLowStock(int threshold)
    {
        var magazzino = new List<Magazzino>();
        using var conn = CreateConnection();
        using var cmd = new SqlCommand(
            "SELECT idNegozio, idProdotto, quantità " +
            "FROM Magazzino WHERE quantità <= @Threshold", conn);
        cmd.Parameters.AddWithValue("@Threshold", threshold);
        conn.Open();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            magazzino.Add(new Magazzino
            {
                IdNegozio = reader.GetInt32(0),
                IdProdotto = reader.GetInt32(1),
                Quantità = reader.GetInt32(2)
            });
        }
        return magazzino;
    }

    public List<Magazzino> GetOutOfStock()
    {
        var magazzino = new List<Magazzino>();
        using var conn = CreateConnection();
        using var cmd = new SqlCommand(
            "SELECT idNegozio, idProdotto, quantità " +
            "FROM Magazzino WHERE quantità = 0", conn);
        conn.Open();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            magazzino.Add(new Magazzino
            {
                IdNegozio = reader.GetInt32(0),
                IdProdotto = reader.GetInt32(1),
                Quantità = reader.GetInt32(2)
            });
        }
        return magazzino;
    }

    public decimal GetInventoryValue()
    {
        using var conn = CreateConnection();
        using var cmd = new SqlCommand(
            "SELECT SUM(m.quantità * p.prezzo) " +
            "FROM Magazzino m " +
            "INNER JOIN Prodotti p ON m.idProdotto = p.Id", conn);
        conn.Open();
        var result = cmd.ExecuteScalar();
        return result != DBNull.Value ? Convert.ToDecimal(result) : 0;
    }
    public int GetQuantita(int idNegozio, int idProdotto)
    {
        using var conn = CreateConnection();
        using var cmd = new SqlCommand(
            "SELECT quantità FROM Magazzino " +
            "WHERE idNegozio = @IdNegozio AND idProdotto = @IdProdotto", conn);
        cmd.Parameters.AddWithValue("@IdNegozio", idNegozio);
        cmd.Parameters.AddWithValue("@IdProdotto", idProdotto);
        conn.Open();
        var result = cmd.ExecuteScalar();
        return result != DBNull.Value ? Convert.ToInt32(result) : 0;
    }

    public bool IsDisponibile(int idNegozio, int idProdotto, int quantitaRichiesta)
    {
        var quantitaDisponibile = GetQuantita(idNegozio, idProdotto);
        return quantitaDisponibile >= quantitaRichiesta;
    }

    public List<Magazzino> GetByCategoria(string categoria)
    {
        var magazzino = new List<Magazzino>();
        using var conn = CreateConnection();
        using var cmd = new SqlCommand(
            "SELECT m.idNegozio, m.idProdotto, m.quantità " +
            "FROM Magazzino m " +
            "INNER JOIN Prodotti p ON m.idProdotto = p.Id " +
            "WHERE p.categoria = @Categoria", conn);
        cmd.Parameters.AddWithValue("@Categoria", categoria);
        conn.Open();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            magazzino.Add(new Magazzino
            {
                IdNegozio = reader.GetInt32(0),
                IdProdotto = reader.GetInt32(1),
                Quantità = reader.GetInt32(2)
            });
        }
        return magazzino;
    }

}
