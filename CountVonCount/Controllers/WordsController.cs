using CountVonCount.Infrastructure;
using CountVonCount.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace CountVonCount.Controllers
{
    public sealed class WordsController : ApiController
    {
        private readonly ICollect _collector;
        private readonly IStorage _store;

        public WordsController(ICollect collector, IStorage store)
        {
            _collector = collector;
            _store = store;
        }

        public List<WordMetric> GetAllWords()
        {
            var wordMetrics = new List<WordMetric>();
            var position = 1;
            foreach (var word in _store.Get())
            {
                wordMetrics.Add(new WordMetric { Position = position++, Name = word.Key, Count = word.Value });
            }

            return wordMetrics;
        }

        [HttpPost]
        public IHttpActionResult Post([FromBody]FormData data)
        {
            var words = _collector.CollectWords(data.url);

            _store.Save(words);

            return Ok(data.url);
        }
    }
}
