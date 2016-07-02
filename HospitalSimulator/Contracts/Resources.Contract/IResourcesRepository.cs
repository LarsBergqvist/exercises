using System;
using System.Collections.Generic;

namespace Resources.Contract
{
    public class ResourcesRepositoryException : Exception
    {
        public ResourcesRepositoryException(string message)
            : base(message)
        {
        }
    }


    /// <summary>
    /// Interface for a repository of room- and doctor resources.</summary>
    /// <remarks></remarks>
    public interface IResourcesRepository
    {
        /// <summary>
        /// Adds a doctor resource to the repository
        /// Throws a ResourceRepositoryException if a doctor object with the same name already exists
        /// </summary>
        /// <param name="doctor">The doctor resource to add</param>
        /// <returns></returns>
        void AddDoctor(Doctor doctor);

        /// <summary>
        /// Get a collection of all doctor resources in the repository
        /// </summary>
        /// <returns>A collection of doctor objects</returns>
        IList<Doctor> GetAllDoctors();

        /// <summary>
        /// Returns a doctor resource with a specified name (case-sensitive)
        /// </summary>
        /// <param name="name">The name to look for</param>
        /// <returns>A doctor resource if found, null if no match found</returns>
        Doctor GetDoctorByName(string name);

        /// <summary>
        /// Adds a room resource to the repository
        /// Throws a ResourceRepositoryException if a room object with the same name already exists
        /// </summary>
        /// <param name="room">The treatment room resource to add</param>
        /// <returns>T</returns>
        void AddRoom(TreatmentRoom room);

        /// <summary>
        /// Get a collection of all treatment room resources in the repository
        /// </summary>
        /// <returns>A collection of treatment room resources</returns>
        IList<TreatmentRoom> GetAllRooms();

        /// <summary>
        /// Returns a room resource with a specified name (case-sensitive)
        /// </summary>
        /// <param name="name">The name to look for</param>
        /// <returns>A treatment room resource if found, null if no match found</returns>
        TreatmentRoom GetRoomByName(string name);

        /// <summary>
        /// Removes all resources from the repository
        /// </summary>
        /// <returns></returns>
        void Clear();
    }
}