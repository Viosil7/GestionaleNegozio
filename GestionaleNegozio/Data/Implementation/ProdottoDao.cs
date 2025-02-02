using GestionaleNegozio.Data.Interfaces;
using GestionaleNegozio.Models;
using Microsoft.Data.SqlClient;

public class ProdottoDao : BaseDao<Prodotto>, IProdottoDao
{
    public ProdottoDao(string connectionString) : base(connectionString) { }

    public List<Prodotto> GetAll()
    {
        var prodotti = new List<Prodotto>();
        using var conn = CreateConnection();
        using var cmd = new SqlCommand("SELECT Id, nome, categoria, descrizione, prezzo FROM Prodotti", conn);
        conn.Open();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            prodotti.Add(new Prodotto
            {
                Id = reader.GetInt32(0),
                Nome = reader.GetString(1),
                Categoria = reader.GetString(2),
                Descrizione = reader.IsDBNull(3) ? null : reader.GetString(3),
                Prezzo = reader.GetDecimal(4)
            });
        }
        return prodotti;
    }

    public Prodotto GetById(int id)
    {
        using var conn = CreateConnection();
        using var cmd = new SqlCommand("SELECT Id, nome, categoria, descrizione, prezzo FROM Prodotti WHERE Id = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", id);
        conn.Open();
        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return new Prodotto
            {
                Id = reader.GetInt32(0),
                Nome = reader.GetString(1),
                Categoria = reader.GetString(2),
                Descrizione = reader.IsDBNull(3) ? null : reader.GetString(3),
                Prezzo = reader.GetDecimal(4)
            };
        }
        return null;
    }

    public List<Prodotto> GetByCategoria(string categoria)
    {
        var prodotti = new List<Prodotto>();
        using var conn = CreateConnection();
        using var cmd = new SqlCommand("SELECT Id, nome, categoria, descrizione, prezzo FROM Prodotti WHERE categoria = @Categoria", conn);
        cmd.Parameters.AddWithValue("@Categoria", categoria);
        conn.Open();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            prodotti.Add(new Prodotto
            {
                Id = reader.GetInt32(0),
                Nome = reader.GetString(1),
                Categoria = reader.GetString(2),
                Descrizione = reader.IsDBNull(3) ? null : reader.GetString(3),
                Prezzo = reader.GetDecimal(4)
            });
        }
        return prodotti;
    }

    public List<Prodotto> GetByPriceRange(decimal minPrice, decimal maxPrice)
    {
        var prodotti = new List<Prodotto>();
        using var conn = CreateConnection();
        using var cmd = new SqlCommand("SELECT Id, nome, categoria, descrizione, prezzo FROM Prodotti WHERE prezzo BETWEEN @MinPrice AND @MaxPrice", conn);
        cmd.Parameters.AddWithValue("@MinPrice", minPrice);
        cmd.Parameters.AddWithValue("@MaxPrice", maxPrice);
        conn.Open();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            prodotti.Add(new Prodotto
            {
                Id = reader.GetInt32(0),
                Nome = reader.GetString(1),
                Categoria = reader.GetString(2),
                Descrizione = reader.IsDBNull(3) ? null : reader.GetString(3),
                Prezzo = reader.GetDecimal(4)
            });
        }
        return prodotti;
    }

    public List<Prodotto> Search(string searchTerm)
    {
        var prodotti = new List<Prodotto>();
        using var conn = CreateConnection();
        using var cmd = new SqlCommand(
            "SELECT Id, nome, categoria, descrizione, prezzo FROM Prodotti " +
            "WHERE nome LIKE @SearchTerm OR descrizione LIKE @SearchTerm", conn);
        cmd.Parameters.AddWithValue("@SearchTerm", $"%{searchTerm}%");
        conn.Open();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            prodotti.Add(new Prodotto
            {
                Id = reader.GetInt32(0),
                Nome = reader.GetString(1),
                Categoria = reader.GetString(2),
                Descrizione = reader.IsDBNull(3) ? null : reader.GetString(3),
                Prezzo = reader.GetDecimal(4)
            });
        }
        return prodotti;
    }

    public void Insert(Prodotto prodotto)
    {
        using var conn = CreateConnection();
        conn.Open();
        using var transaction = conn.BeginTransaction();

        try
        {
            using var cmd = new SqlCommand(
                "INSERT INTO Prodotti (nome, categoria, descrizione, prezzo) " +
                "OUTPUT INSERTED.Id " +
                "VALUES (@Nome, @Categoria, @Descrizione, @Prezzo)",
                conn, transaction);

            cmd.Parameters.AddWithValue("@Nome", prodotto.Nome);
            cmd.Parameters.AddWithValue("@Categoria", prodotto.Categoria);
            cmd.Parameters.AddWithValue("@Descrizione", (object)prodotto.Descrizione ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Prezzo", prodotto.Prezzo);

            var newId = (int)cmd.ExecuteScalar();
            prodotto.Id = newId;

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public void Update(Prodotto prodotto)
    {
        using var conn = CreateConnection();
        using var cmd = new SqlCommand(
            "UPDATE Prodotti SET nome = @Nome, categoria = @Categoria, descrizione = @Descrizione, prezzo = @Prezzo WHERE Id = @Id",
            conn);

        cmd.Parameters.AddWithValue("@Id", prodotto.Id);
        cmd.Parameters.AddWithValue("@Nome", prodotto.Nome);
        cmd.Parameters.AddWithValue("@Categoria", prodotto.Categoria);
        cmd.Parameters.AddWithValue("@Descrizione", (object)prodotto.Descrizione ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Prezzo", prodotto.Prezzo);  // Ensure this is a decimal

        conn.Open();
        cmd.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        using var conn = CreateConnection();
        conn.Open();
        using var transaction = conn.BeginTransaction();

        try
        {
            using var deleteInventoryCmd = new SqlCommand(
                "DELETE FROM Magazzino WHERE idProdotto = @Id",
                conn, transaction);
            deleteInventoryCmd.Parameters.AddWithValue("@Id", id);
            deleteInventoryCmd.ExecuteNonQuery();

            using var deleteProductCmd = new SqlCommand(
                "DELETE FROM Prodotti WHERE Id = @Id",
                conn, transaction);
            deleteProductCmd.Parameters.AddWithValue("@Id", id);
            deleteProductCmd.ExecuteNonQuery();

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public bool Exists(int id)
    {
        return Exists("SELECT 1 FROM Prodotti WHERE Id = @Id",
            new[] { new SqlParameter("@Id", id) });
    }
}
