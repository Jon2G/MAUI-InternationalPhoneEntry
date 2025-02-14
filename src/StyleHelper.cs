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
            if (obj.TryGetValue(key, out var resource))
            {
                return resource as T;
            }
            return resource as T;
        }
    }
}
