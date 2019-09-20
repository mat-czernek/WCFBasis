using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Service.Utilities
{
    /// <summary>
    /// Class defines additional methods that could be launched on collections
    /// </summary>
    internal static class CollectionsOperations
    {
        /// <summary>
        /// Method enumerates items on the collection and executes method on each item
        /// This method it's used to avoid multiple foreach statement in cases when collection needs to be enumerated
        /// </summary>
        /// <param name="method">Method to be called by collection item</param>
        /// <param name="targetCollection">Target collection</param>
        /// <typeparam name="T">The type of the collection items</typeparam>
        public static void ExecuteMethodOnItems<T>(Action<T> method, IEnumerable<T> targetCollection)
        {
            foreach (var item in targetCollection)
            {
                try
                {
                    method(item);
                }
                catch (CommunicationException) {}
            }
        }
    }
}