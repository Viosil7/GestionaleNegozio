using GestionaleNegozio.Models;

namespace GestionaleNegozio.Data.Interfaces
{
    public interface IStaffDao : IBaseDao<Staff>
    {
        /// <summary>
        /// Retrieves a staff member by username
        /// </summary>
        Staff GetByUsername(string username);

        /// <summary>
        /// Authenticates a staff member
        /// </summary>
        Staff Authenticate(string username, string password);

        /// <summary>
        /// Gets all staff members by role
        /// </summary>
        List<Staff> GetByRole(string role);
    }
}