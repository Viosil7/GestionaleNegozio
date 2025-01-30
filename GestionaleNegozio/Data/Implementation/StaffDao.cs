using GestionaleNegozio.Data.Interfaces;
using GestionaleNegozio.Models;
using Microsoft.Data.SqlClient;

public class StaffDao : BaseDao<Staff>, IStaffDao
{
    public StaffDao(string connectionString) : base(connectionString) { }

    public List<Staff> GetAll()
    {
        var staffList = new List<Staff>();
        using var conn = CreateConnection();
        using var cmd = new SqlCommand("SELECT Id, username, passw, ruolo FROM Staff", conn);
        conn.Open();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            staffList.Add(new Staff
            {
                Id = reader.GetInt32(0),
                Username = reader.GetString(1),
                Passw = reader.GetString(2),
                Ruolo = reader.GetString(3)
            });
        }
        return staffList;
    }

    public Staff GetById(int id)
    {
        using var conn = CreateConnection();
        using var cmd = new SqlCommand("SELECT Id, username, passw, ruolo FROM Staff WHERE Id = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", id);
        conn.Open();
        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return new Staff
            {
                Id = reader.GetInt32(0),
                Username = reader.GetString(1),
                Passw = reader.GetString(2),
                Ruolo = reader.GetString(3)
            };
        }
        return null;
    }

    public Staff GetByUsername(string username)
    {
        using var conn = CreateConnection();
        using var cmd = new SqlCommand("SELECT Id, username, passw, ruolo FROM Staff WHERE username = @Username", conn);
        cmd.Parameters.AddWithValue("@Username", username);
        conn.Open();
        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return new Staff
            {
                Id = reader.GetInt32(0),
                Username = reader.GetString(1),
                Passw = reader.GetString(2),
                Ruolo = reader.GetString(3)
            };
        }
        return null;
    }

    public void Insert(Staff staff)
    {
        using var conn = CreateConnection();
        using var cmd = new SqlCommand("INSERT INTO Staff (username, passw, ruolo) VALUES (@Username, @Passw, @Ruolo)", conn);
        cmd.Parameters.AddWithValue("@Username", staff.Username);
        cmd.Parameters.AddWithValue("@Passw", staff.Passw);
        cmd.Parameters.AddWithValue("@Ruolo", staff.Ruolo);
        conn.Open();
        cmd.ExecuteNonQuery();
    }

    public void Update(Staff staff)
    {
        using var conn = CreateConnection();
        using var cmd = new SqlCommand("UPDATE Staff SET username = @Username, passw = @Passw, ruolo = @Ruolo WHERE Id = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", staff.Id);
        cmd.Parameters.AddWithValue("@Username", staff.Username);
        cmd.Parameters.AddWithValue("@Passw", staff.Passw);
        cmd.Parameters.AddWithValue("@Ruolo", staff.Ruolo);
        conn.Open();
        cmd.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        using var conn = CreateConnection();
        using var cmd = new SqlCommand("DELETE FROM Staff WHERE Id = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", id);
        conn.Open();
        cmd.ExecuteNonQuery();
    }

    public bool Exists(int id)
    {
        return Exists("SELECT 1 FROM Staff WHERE Id = @Id",
            new[] { new SqlParameter("@Id", id) });
    }
    public Staff Authenticate(string username, string password)
    {
        using var conn = CreateConnection();
        using var cmd = new SqlCommand("SELECT Id, username, passw, ruolo FROM Staff WHERE username = @Username AND passw = @Password", conn);
        cmd.Parameters.AddWithValue("@Username", username);
        cmd.Parameters.AddWithValue("@Password", password);
        conn.Open();
        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return new Staff
            {
                Id = reader.GetInt32(0),
                Username = reader.GetString(1),
                Passw = reader.GetString(2),
                Ruolo = reader.GetString(3)
            };
        }
        return null;
    }

    public List<Staff> GetByRole(string role)
    {
        var staffList = new List<Staff>();
        using var conn = CreateConnection();
        using var cmd = new SqlCommand("SELECT Id, username, passw, ruolo FROM Staff WHERE ruolo = @Role", conn);
        cmd.Parameters.AddWithValue("@Role", role);
        conn.Open();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            staffList.Add(new Staff
            {
                Id = reader.GetInt32(0),
                Username = reader.GetString(1),
                Passw = reader.GetString(2),
                Ruolo = reader.GetString(3)
            });
        }
        return staffList;
    }
    public int CountManagers()
    {
        using var conn = CreateConnection();
        using var cmd = new SqlCommand("SELECT COUNT(*) FROM Staff WHERE ruolo = 'Manager'", conn);
        conn.Open();
        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    public bool IsLastManager(int staffId)
    {
        using var conn = CreateConnection();
        using var cmd = new SqlCommand(
            "SELECT CASE WHEN EXISTS (" +
            "SELECT 1 FROM Staff " +
            "WHERE ruolo = 'Manager' AND Id != @StaffId" +
            ") THEN 0 ELSE 1 END", conn);
        cmd.Parameters.AddWithValue("@StaffId", staffId);
        conn.Open();
        return Convert.ToBoolean(cmd.ExecuteScalar());
    }

}
