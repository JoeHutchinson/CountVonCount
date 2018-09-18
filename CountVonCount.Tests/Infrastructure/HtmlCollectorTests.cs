using System.IO;
using System.Linq;
using System.Reflection;
using CountVonCount.Infrastructure;
using HtmlAgilityPack;
using Moq;
using NUnit.Framework;

namespace CountVonCount.Tests.Infrastructure
{
    public class HtmlCollectorTests
    {
        [Test]
        public void HtmlCollectorCountsWordOccurances()
        {
            // arrange
            var htmlProvider = new Mock<IHtmlProvider>();
            var sut = new HtmlCollector(htmlProvider.Object);

            // act
            var wordCnt = sut.CountWordOccurances(@"the little fox jumped over the little hill").ToList();

            // assert
            Assert.AreEqual(6, wordCnt.Count, "Unique words are 6");
            Assert.AreEqual(2, wordCnt.First(x => x.Key == "the").Value);
            Assert.AreEqual(2, wordCnt.First(x => x.Key == "little").Value);
            Assert.AreEqual(1, wordCnt.First(x => x.Key == "fox").Value);
            Assert.AreEqual(1, wordCnt.First(x => x.Key == "jumped").Value);
            Assert.AreEqual(1, wordCnt.First(x => x.Key == "over").Value);
            Assert.AreEqual(1, wordCnt.First(x => x.Key == "hill").Value);
        }

        [Test]
        public void HtmlCollectorCanCollectWords_SmokeTest()
        {
            // arrange
            var htmlProvider = new Mock<IHtmlProvider>(MockBehavior.Strict);
            htmlProvider.Setup(x => x.Load(It.IsAny<string>())).Returns(
                () =>
                {
                    var htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(GetHTMLDocument());
                    return htmlDoc;
                });
            var sut = new HtmlCollector(htmlProvider.Object);

            // act
            var words = sut.CollectWords("https://www.bbc.co.uk/news").ToList();

            // assert
            Assert.AreEqual(650, words.Count);
            Assert.AreEqual(15, words.First(x => x.Key == "bbc").Value);
        }

        private static string GetHTMLDocument()
        {
            var assembly = Assembly.GetExecutingAssembly();
            const string resourceName = "CountVonCount.Tests.Resources.ValidHTMLPage.txt";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
