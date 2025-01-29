using GestionaleNegozio.Data.Interfaces;
using GestionaleNegozio.Models;
using Microsoft.Data.SqlClient;

public class MagazzinoDao : BaseDao<Magazzino>, IMagazzinoDao
{
    public MagazzinoDao(string connectionString) : base(connectionString) { }

    public List<Magazzino> GetByNegozio(int idNegozio)
    {
        var magazzino = new List<Magazzino>();
        using var conn = CreateConnection();
        using var cmd = new SqlCommand("SELECT idNegozio, idProdotto, quantità FROM Magazzino WHERE idNegozio = @IdNegozio", conn);
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
        using var cmd = new SqlCommand("SELECT idNegozio, idProdotto, quantità FROM Magazzino WHERE idProdotto = @IdProdotto", conn);
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
        return result != null ? Convert.ToInt32(result) : 0;
    }
    public bool IsDisponibile(int idNegozio, int idProdotto, int quantitaRichiesta)
    {
        using var conn = CreateConnection();
        using var cmd = new SqlCommand(
            "SELECT CASE WHEN quantità >= @QuantitaRichiesta THEN 1 ELSE 0 END " +
            "FROM Magazzino " +
            "WHERE idNegozio = @IdNegozio AND idProdotto = @IdProdotto", conn);
        cmd.Parameters.AddWithValue("@IdNegozio", idNegozio);
        cmd.Parameters.AddWithValue("@IdProdotto", idProdotto);
        cmd.Parameters.AddWithValue("@QuantitaRichiesta", quantitaRichiesta);
        conn.Open();
        var result = cmd.ExecuteScalar();
        return result != null && Convert.ToInt32(result) == 1;
    }

}
