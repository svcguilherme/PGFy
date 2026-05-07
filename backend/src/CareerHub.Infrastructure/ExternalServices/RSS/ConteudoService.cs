using System.Xml.Linq;
using CareerHub.Application.Conteudo;
using CareerHub.Application.Conteudo.DTOs;
using Microsoft.Extensions.Caching.Memory;

namespace CareerHub.Infrastructure.ExternalServices.RSS;

public class ConteudoService(IHttpClientFactory httpClientFactory, IMemoryCache cache) : IConteudoService
{
    private const string CacheKey = "conteudo_rss";
    private static readonly TimeSpan CacheTtl = TimeSpan.FromHours(1);

    private static readonly (string Url, string Fonte)[] Feeds =
    [
        ("https://dev.to/feed/tag/dotnet", "DEV.to .NET"),
        ("https://devblogs.microsoft.com/dotnet/feed/", ".NET Blog"),
        ("https://blog.angular.io/feed", "Angular Blog")
    ];

    public async Task<IEnumerable<ConteudoItemDto>> GetConteudoAsync(CancellationToken ct = default)
    {
        if (cache.TryGetValue(CacheKey, out IEnumerable<ConteudoItemDto>? cached) && cached is not null)
            return cached;

        var tasks = Feeds.Select(f => BuscarFeedAsync(f.Url, f.Fonte, ct));
        var results = await Task.WhenAll(tasks);

        var itens = results
            .SelectMany(x => x)
            .OrderByDescending(i => i.PublicadoEm)
            .ToList();

        cache.Set(CacheKey, itens, CacheTtl);
        return itens;
    }

    private async Task<IEnumerable<ConteudoItemDto>> BuscarFeedAsync(string url, string fonte, CancellationToken ct)
    {
        try
        {
            var client = httpClientFactory.CreateClient();
            var xml = await client.GetStringAsync(url, ct);
            var doc = XDocument.Parse(xml);
            XNamespace ns = "http://www.w3.org/2005/Atom";

            // Tenta RSS 2.0 primeiro, depois Atom
            var rssItems = doc.Descendants("item").Select(item => new ConteudoItemDto(
                Titulo: item.Element("title")?.Value ?? string.Empty,
                Link: item.Element("link")?.Value ?? string.Empty,
                Resumo: item.Element("description")?.Value,
                Fonte: fonte,
                PublicadoEm: DateTime.TryParse(item.Element("pubDate")?.Value, out var dt) ? dt.ToUniversalTime() : null
            ));

            var atomItems = doc.Descendants(ns + "entry").Select(entry => new ConteudoItemDto(
                Titulo: entry.Element(ns + "title")?.Value ?? string.Empty,
                Link: entry.Element(ns + "link")?.Attribute("href")?.Value ?? string.Empty,
                Resumo: entry.Element(ns + "summary")?.Value,
                Fonte: fonte,
                PublicadoEm: DateTime.TryParse(entry.Element(ns + "published")?.Value, out var dt) ? dt.ToUniversalTime() : null
            ));

            var todos = rssItems.Concat(atomItems).Where(i => !string.IsNullOrEmpty(i.Titulo)).ToList();
            return todos.Count != 0 ? todos : atomItems.ToList();
        }
        catch
        {
            return [];
        }
    }
}
