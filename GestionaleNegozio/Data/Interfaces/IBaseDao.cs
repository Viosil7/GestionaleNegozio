using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionaleNegozio.Data.Interfaces
{
    public interface IBaseDao<T>
    {
        /// <summary>
        /// Retrieves all entities of type T
        /// </summary>
        /// <returns>List of all entities</returns>
        List<T> GetAll();

        /// <summary>
        /// Retrieves an entity by its ID
        /// </summary>
        /// <param name="id">The ID of the entity to retrieve</param>
        /// <returns>The entity if found, null otherwise</returns>
        T GetById(int id);

        /// <summary>
        /// Inserts a new entity
        /// </summary>
        /// <param name="entity">The entity to insert</param>
        void Insert(T entity);

        /// <summary>
        /// Updates an existing entity
        /// </summary>
        /// <param name="entity">The entity to update</param>
        void Update(T entity);

        /// <summary>
        /// Deletes an entity by its ID
        /// </summary>
        /// <param name="id">The ID of the entity to delete</param>
        void Delete(int id);

        /// <summary>
        /// Checks if an entity exists by its ID
        /// </summary>
        /// <param name="id">The ID to check</param>
        /// <returns>True if exists, false otherwise</returns>
        bool Exists(int id);
    }
}
