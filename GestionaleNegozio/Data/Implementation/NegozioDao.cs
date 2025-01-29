using GestionaleNegozio.Data.Interfaces;
using GestionaleNegozio.Models;
using Microsoft.Data.SqlClient;

public class NegozioDao : BaseDao<Negozio>, INegozioDao
{
    public NegozioDao(string connectionString) : base(connectionString) { }

    public List<Negozio> GetAll()
    {
        var negozi = new List<Negozio>();
        using var conn = CreateConnection();
        using var cmd = new SqlCommand("SELECT Id, regione, città, indirizzo FROM Negozi", conn);
        conn.Open();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            negozi.Add(new Negozio
            {
                Id = reader.GetInt32(0),
                Regione = reader.GetString(1),
                Città = reader.GetString(2),
                Indirizzo = reader.GetString(3)
            });
        }
        return negozi;
    }

    public Negozio GetById(int id)
    {
        using var conn = CreateConnection();
        using var cmd = new SqlCommand("SELECT Id, regione, città, indirizzo FROM Negozi WHERE Id = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", id);
        conn.Open();
        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return new Negozio
            {
                Id = reader.GetInt32(0),
                Regione = reader.GetString(1),
                Città = reader.GetString(2),
                Indirizzo = reader.GetString(3)
            };
        }
        return null;
    }

    public List<Negozio> GetByRegione(string regione)
    {
        var negozi = new List<Negozio>();
        using var conn = CreateConnection();
        using var cmd = new SqlCommand("SELECT Id, regione, città, indirizzo FROM Negozi WHERE regione = @Regione", conn);
        cmd.Parameters.AddWithValue("@Regione", regione);
        conn.Open();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            negozi.Add(new Negozio
            {
                Id = reader.GetInt32(0),
                Regione = reader.GetString(1),
                Città = reader.GetString(2),
                Indirizzo = reader.GetString(3)
            });
        }
        return negozi;
    }

    public void Insert(Negozio negozio)
    {
        using var conn = CreateConnection();
        using var cmd = new SqlCommand("INSERT INTO Negozi (regione, città, indirizzo) VALUES (@Regione, @Citta, @Indirizzo)", conn);
        cmd.Parameters.AddWithValue("@Regione", negozio.Regione);
        cmd.Parameters.AddWithValue("@Citta", negozio.Città);
        cmd.Parameters.AddWithValue("@Indirizzo", negozio.Indirizzo);
        conn.Open();
        cmd.ExecuteNonQuery();
    }

    public void Update(Negozio negozio)
    {
        using var conn = CreateConnection();
        using var cmd = new SqlCommand("UPDATE Negozi SET regione = @Regione, città = @Citta, indirizzo = @Indirizzo WHERE Id = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", negozio.Id);
        cmd.Parameters.AddWithValue("@Regione", negozio.Regione);
        cmd.Parameters.AddWithValue("@Citta", negozio.Città);
        cmd.Parameters.AddWithValue("@Indirizzo", negozio.Indirizzo);
        conn.Open();
        cmd.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        using var conn = CreateConnection();
        using var cmd = new SqlCommand("DELETE FROM Negozi WHERE Id = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", id);
        conn.Open();
        cmd.ExecuteNonQuery();
    }

    public bool Exists(int id)
    {
        return Exists("SELECT 1 FROM Negozi WHERE Id = @Id",
            new[] { new SqlParameter("@Id", id) });
    }
    public List<Negozio> GetByCitta(string citta)
    {
        var negozi = new List<Negozio>();
        using var conn = CreateConnection();
        using var cmd = new SqlCommand("SELECT Id, regione, città, indirizzo FROM Negozi WHERE città = @Citta", conn);
        cmd.Parameters.AddWithValue("@Citta", citta);
        conn.Open();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            negozi.Add(new Negozio
            {
                Id = reader.GetInt32(0),
                Regione = reader.GetString(1),
                Città = reader.GetString(2),
                Indirizzo = reader.GetString(3)
            });
        }
        return negozi;
    }

    public List<Negozio> GetByProdottoDisponibile(int idProdotto)
    {
        var negozi = new List<Negozio>();
        using var conn = CreateConnection();
        using var cmd = new SqlCommand(
            "SELECT n.Id, n.regione, n.città, n.indirizzo " +
            "FROM Negozi n " +
            "INNER JOIN Magazzino m ON n.Id = m.idNegozio " +
            "WHERE m.idProdotto = @IdProdotto AND m.quantità > 0", conn);
        cmd.Parameters.AddWithValue("@IdProdotto", idProdotto);
        conn.Open();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            negozi.Add(new Negozio
            {
                Id = reader.GetInt32(0),
                Regione = reader.GetString(1),
                Città = reader.GetString(2),
                Indirizzo = reader.GetString(3)
            });
        }
        return negozi;
    }

}
