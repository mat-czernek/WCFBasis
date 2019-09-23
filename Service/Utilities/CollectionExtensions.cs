using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace Service.Utilities
{
    /// <summary>
    /// Class defines additional methods that could be launched on collections
    /// </summary>
    internal static class CollectionExtensions
    {
        public static void ExecuteCallbackMethod<T>(this List<T> list, Action<T> method)
        {
            if(list == null) return;
            
            if(list.Count == 0) return;
            
            foreach (var item in list)
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