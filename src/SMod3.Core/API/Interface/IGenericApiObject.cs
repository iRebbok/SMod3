using UnityEngine;

namespace SMod3.API
{
    /// <summary>
    ///     An interface for API objects that support this.
    /// </summary>
    public interface IGenericApiObject
    {
        /// <summary>
        ///     Gets the source object.
        /// </summary>
        public GameObject GetGameObject();
    }
}
