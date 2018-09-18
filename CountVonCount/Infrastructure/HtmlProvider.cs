using HtmlAgilityPack;

namespace CountVonCount.Infrastructure
{
    public class HtmlProvider : HtmlWeb, IHtmlProvider
    {
    }

    public interface IHtmlProvider
    {
        HtmlDocument Load(string url);
    }
}