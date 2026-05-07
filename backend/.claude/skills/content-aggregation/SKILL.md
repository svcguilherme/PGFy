---
name: content-aggregation
description: Use this skill when working with the Conteudo module — fetching RSS feeds, aggregating .NET or Angular articles, implementing the content service, or anything related to external content that must NOT be persisted. Trigger on: "conteúdo", "rss", "artigos", "feed", "notícias .net", "angular blog".
---

# Content Aggregation — CareerHub

## Regra de Ouro
> **NUNCA persistir conteúdo externo no banco de dados.**  
> Este módulo é somente busca + cache em memória + repasse ao frontend.

## Fontes RSS

```csharp
// Infrastructure/ContentAggregation/RssSources.cs
public static class RssSources
{
    public static readonly Dictionary<string, string> Feeds = new()
    {
        ["dotnet-blog"]    = "https://devblogs.microsoft.com/dotnet/feed/",
        ["aspnet-blog"]    = "https://devblogs.microsoft.com/aspnet/feed/",
        ["angular-blog"]   = "https://blog.angular.io/feed",
        ["devto-dotnet"]   = "https://dev.to/feed/tag/dotnet",
        ["devto-angular"]  = "https://dev.to/feed/tag/angular",
    };
}
```

## Content Service

```csharp
public interface IContentService
{
    Task<IEnumerable<ConteudoItemDto>> GetConteudoAsync(
        string? tag = null, int page = 1, int pageSize = 20,
        CancellationToken ct = default);
}

// Infrastructure/ContentAggregation/RssContentService.cs
public class RssContentService(
    IHttpClientFactory httpClientFactory,
    IMemoryCache cache,
    ILogger<RssContentService> logger) : IContentService
{
    private const string CacheKey = "content_aggregated";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(1);

    public async Task<IEnumerable<ConteudoItemDto>> GetConteudoAsync(
        string? tag, int page, int pageSize, CancellationToken ct)
    {
        var allItems = await cache.GetOrCreateAsync(CacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = CacheDuration;
            return await BuscarTodosOsFeeds(ct);
        });

        var items = allItems ?? [];

        if (!string.IsNullOrWhiteSpace(tag))
            items = items.Where(i => i.Tags.Contains(tag,
                StringComparer.OrdinalIgnoreCase));

        return items
            .OrderByDescending(i => i.PublicadoEm)
            .Skip((page - 1) * pageSize)
            .Take(pageSize);
    }

    private async Task<List<ConteudoItemDto>> BuscarTodosOsFeeds(CancellationToken ct)
    {
        var tasks = RssSources.Feeds.Select(feed => BuscarFeed(feed.Key, feed.Value, ct));
        var results = await Task.WhenAll(tasks);
        return results.SelectMany(r => r).ToList();
    }

    private async Task<IEnumerable<ConteudoItemDto>> BuscarFeed(
        string fonte, string url, CancellationToken ct)
    {
        try
        {
            var client = httpClientFactory.CreateClient("rss");
            var xml = await client.GetStringAsync(url, ct);
            return ParseRss(xml, fonte);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Falha ao buscar feed {Fonte}", fonte);
            return []; // Falha silenciosa — não quebrar se um feed cair
        }
    }
}
```

## DTO (somente repasse)

```csharp
public record ConteudoItemDto
{
    public string Titulo { get; init; } = string.Empty;
    public string Url { get; init; } = string.Empty;
    public string Resumo { get; init; } = string.Empty;
    public string Fonte { get; init; } = string.Empty;  // "dotnet-blog", "devto-angular"...
    public DateTime PublicadoEm { get; init; }
    public IEnumerable<string> Tags { get; init; } = [];
    public string? ImagemUrl { get; init; }
}
```

## Endpoint

```
GET /api/v1/conteudo?tag=dotnet&page=1&pageSize=20
GET /api/v1/conteudo?tag=angular&page=1&pageSize=20
GET /api/v1/conteudo/fontes   → lista as fontes disponíveis
```

## Program.cs — Registrar HttpClient

```csharp
builder.Services.AddHttpClient("rss", client => {
    client.Timeout = TimeSpan.FromSeconds(10);
    client.DefaultRequestHeaders.UserAgent.ParseAdd("CareerHub/1.0");
});
builder.Services.AddMemoryCache();
builder.Services.AddScoped<IContentService, RssContentService>();
```
