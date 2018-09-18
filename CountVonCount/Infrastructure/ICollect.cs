using System.Collections.Generic;

namespace CountVonCount.Infrastructure
{
    public interface ICollect
    {
        IEnumerable<KeyValuePair<string, int>> CollectWords(string location);
    }
}
