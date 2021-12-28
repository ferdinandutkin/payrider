using System.Text.Json;
using System.Threading.Tasks;
using Core;
using System.Linq;
using System.Text.Json.Serialization;

namespace PageObjects;

public static class TradingPageWatchListExtension
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new() { Converters = { new JsonStringEnumConverter() }, PropertyNameCaseInsensitive = true };
    private const string ExtractWatchlistItemsScript = @"els => els.map(el => {
                        const {children} = el;
                        return JSON.stringify({
                            Symbol : el.children[1].innerText.split('\n')[0]
                        })
                });";
    
    private const string WatchlistItemsSelector = "li[draggable='true']";

    private static async Task<WatchlistItem[]> GetWatchListItemsAsync(this TradingPage tradingPage)
    {
        var result = await tradingPage.Page.EvalOnSelectorAllAsync(WatchlistItemsSelector,
            ExtractWatchlistItemsScript);

        return result.Value.Deserialize<JSONArrayElement[]>(JsonSerializerOptions)
            .Select(jsonTrade => JsonSerializer.Deserialize<WatchlistItem>(jsonTrade.S, JsonSerializerOptions))
            .ToArray();
    }
    
    public static async  Task<WatchlistItem[]> GetUpdatedWatchListItemsAsync(this TradingPage tradingPage)
    {

        return await tradingPage.GetWatchListItemsAsync();
    }


}