using System.ComponentModel.DataAnnotations;

namespace CountVonCount.Models
{
    public sealed class WordMetric
    {
        public int Position { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
    }
}