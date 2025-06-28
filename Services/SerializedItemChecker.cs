using Newtonsoft.Json.Linq;
using Sitecore.Data.Items;

namespace Foundation.SerializationWarnings.Services
{
    public enum SerializationScope
    {
        Ignored,
        SingleItem,
        ItemAndChildren,
        ItemAndDescendants
    }

    public class SerializedItemChecker
    {
        private readonly Dictionary<string, (SerializationScope Scope, string ModuleFile)> _paths = new();
        private bool _isLoaded;

        public SerializedItemChecker()
        {
            if (!_isLoaded) { LoadPaths("~/App_Data/SerializationJsons"); }
        }

        public void LoadPaths(string rootDirectory)
        {
            var resolvedPath = System.Web.Hosting.HostingEnvironment.MapPath(rootDirectory);
            if (_isLoaded || string.IsNullOrEmpty(resolvedPath) || !Directory.Exists(resolvedPath))
                return;

            foreach (var file in Directory.GetFiles(resolvedPath, "*.module.json", SearchOption.AllDirectories))
            {
                var json = JObject.Parse(File.ReadAllText(file));

                var items = json["items"]?["includes"] as JArray;
                if (items == null) continue;

                foreach (var item in items)
                {
                    var path = item.Value<string>("path")?.TrimEnd('/');
                    if (string.IsNullOrWhiteSpace(path)) continue;

                    var scopeText = item.Value<string>("scope")?.ToLowerInvariant();

                    var serializationScope = scopeText switch
                    {
                        "singleitem" => SerializationScope.SingleItem,
                        "itemandchildren" => SerializationScope.ItemAndChildren,
                        "itemanddescendants" => SerializationScope.ItemAndDescendants,
                        _ => SerializationScope.ItemAndDescendants // default if unspecified
                    };

                    _paths[path] = (serializationScope, Path.GetFileName(file));
                }
            }

            _isLoaded = true;
        }

        public (bool isSerialized, string moduleFile) IsItemSerialized(Item item)
        {
            var itemPath = item.Paths.FullPath.TrimEnd('/');

            foreach (var kvp in _paths)
            {
                var path = kvp.Key;
                var (scope, moduleFile) = kvp.Value;

                switch (scope)
                {
                    case SerializationScope.SingleItem:
                        if (itemPath.Equals(path, StringComparison.OrdinalIgnoreCase))
                            return (true, moduleFile);
                        break;

                    case SerializationScope.ItemAndChildren:
                        if (itemPath.StartsWith(path + "/", StringComparison.OrdinalIgnoreCase) &&
                            !itemPath.Substring(path.Length + 1).Contains("/"))
                            return (true, moduleFile);
                        break;

                    case SerializationScope.ItemAndDescendants:
                        if (itemPath.Equals(path, StringComparison.OrdinalIgnoreCase) ||
                            itemPath.StartsWith(path + "/", StringComparison.OrdinalIgnoreCase))
                            return (true, moduleFile);
                        break;
                }
            }

            return (false, null);
        }
    }
}
