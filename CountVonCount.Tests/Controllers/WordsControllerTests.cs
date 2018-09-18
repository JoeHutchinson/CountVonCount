using CountVonCount.Controllers;
using CountVonCount.Infrastructure;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace CountVonCount.Tests.Controllers
{
    public class WordsControllerTests
    {
        [Test]
        public void GetAllWordsGetsFromStore()
        {
            // arrange
            var collector = new Mock<ICollect>();
            var store = new Mock<IStorage>();
            store.Setup(x => x.Get()).Returns(ExampleWords);
            var sut = new WordsController(collector.Object, store.Object);

            // act
            var result = sut.GetAllWords();

            // assert
            var expectedPosition = 1;
            var index = 0;
            foreach (var expectedWord in ExampleWords())
            {
                var actualWord = result[index];
                Assert.AreEqual(expectedPosition, actualWord.Position);
                Assert.AreEqual(expectedWord.Key, actualWord.Name);
                Assert.AreEqual(expectedWord.Value, actualWord.Count);
                expectedPosition++;
                index++;
            }
        }

        [Test]
        public void PostCallsCollectWords()
        {
            // arrange
            var collector = new Mock<ICollect>();
            var store = new Mock<IStorage>();
            var sut = new WordsController(collector.Object, store.Object);

            // act
            sut.Post(new FormData());

            // assert
            collector.Verify(x => x.CollectWords(It.IsAny<string>()), Times.Once);
        }

        private static Dictionary<string, int> ExampleWords()
        {
            return new Dictionary<string, int>
            {
                {"Frank", 1},
                {"Mary", 45},
                {"Harry", 30}
            };
        }
    }
}
