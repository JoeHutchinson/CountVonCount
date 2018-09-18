using System.Collections.Generic;

namespace CountVonCount.Infrastructure
{
    public interface IStorage
    {
        void Save(IEnumerable<KeyValuePair<string, int>> words);
        Dictionary<string, int> Get();
    }
}