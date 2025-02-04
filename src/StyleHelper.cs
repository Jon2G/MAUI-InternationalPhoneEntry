using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternationalPhoneEntry
{
    internal interface IResourcesProvider
    {
        ResourceDictionary Resources { get; set; }
    }
    internal static class StyleHelper
    {
        internal static T? GetResource<T>(this IResourcesProvider obj, string key) where T : class => obj.Resources.GetResource<T>(key);

        private static T? GetResource<T>(this ResourceDictionary obj, string key) where T : class
        {
            object resource = default(T);
            if (obj.TryGetValue(key, out resource))
            {
                return resource as T;
            }
            return resource as T;
        }
    }
}
