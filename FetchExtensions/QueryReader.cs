using Newtonsoft.Json;
using System;
using System.IO;

namespace FetchExtensions
{
    public class QueryReader<T> where T : Query, new()
    {
        public virtual T Read() {
            var fileName = "filter.json";
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            while (!File.Exists(path) && Path.GetDirectoryName(path) != Directory.GetDirectoryRoot(path))
                path = Path.Combine(Directory.GetParent(Path.GetDirectoryName(path)).FullName, fileName);
            var s = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<T>(s);
        }
    }
}
