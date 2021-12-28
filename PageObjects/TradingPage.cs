using Core;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Position = Core.Position;

namespace PageObjects
{
    public class TradingPage : PageObject
    {

        private readonly JsonSerializerOptions _jsonSerializerOptions = new() { Converters = { new JsonStringEnumConverter() }, PropertyNameCaseInsensitive = true };
        private const string limitPriceSelector = "text=SideBuySellOrder TypeLIMITAmountLimit PriceTime-in-ForceDayExtended HoursNo >> :nth-match(input[type='text'], 2)";

        private const string activeTradesSelector = ":nth-match(table, 1) tbody > *";

        private const string lastActiveTradeSelector = ":nth-match(table, 1) tbody > tr:first-child";
        private const string extractActiveTrades =
                   @"els => els.map(el => {
                        const {children} = el;
                        return JSON.stringify({
                            Symbol: children[1].innerText,
                            Side: children[3].innerText,
                            Price: parseFloat(children[5].innerText)
                        });
                   })";

        private const string activePositionsSelector = ":nth-match(table, 2) tbody > *";
        private const string extractActivePositions =
            @"els => els.map(el => {
                        const {children} = el;
                        return JSON.stringify({
                            Symbol: children[1].innerText,
                            Name: children[2].innerText,
                            Quantity: parseInt(children[3].innerText),
                            MarketValue: parseFloat(children[4].innerText)
                        });
                   })";


        private const string limitPriceXpathSelector = "//div/span[text() = 'Limit Price']//ancestor::div[2]//div[2]/div[1]/input";


        private const string symbolInputSelector = "[placeholder='Symbol']";
        private const string netAccountValueXpathSelector = "//div[starts-with(text(), 'Net Account Value')]//following-sibling::div";

      


   

        private static readonly IReadOnlyDictionary<UnitOfMeasure, string> _unitOfMeasureCssClassDictionary = new Dictionary<UnitOfMeasure, string>()
        {
            { UnitOfMeasure.Amount, "webull-Amount__" },
            { UnitOfMeasure.Quantity, "webull-Quantity__" },
            { UnitOfMeasure.Percentage, "webull-Percentage__" }

        };

       
        private readonly NumberFormatInfo _currencyFormatter = new() { CurrencyGroupSeparator = ",", CurrencySymbol = "", CurrencyDecimalSeparator = ".", NumberDecimalDigits = 2 };

        public TradingPage(IPage page) : base(page)
        {
        }


 

        public async Task<bool> HasInsufficientFundsPopup()
        {
            try
            {
                await Page.WaitForSelectorAsync("text=Insufficient funds, order failure", new() { State = WaitForSelectorState.Visible, Timeout = 3000 });
            }
            catch (TimeoutException e)
            {
                return false;
            }
            return true;

        }
        
        public async Task<TradingPage> SelectSide(Side side)
        {

            string buyElementText = "Buy";
            string sellElementText = "Sell";

            string elementText = (side == Side.Buy) ? buyElementText : sellElementText;

            await Page.ClickAsync($"'{elementText}'");

            return this;
        }

        private UnitOfMeasure _currentUnitOfMeasure;
        
        public async Task<TradingPage> SelectUnitOfMeasure(UnitOfMeasure unitOfMeasure)
        {
            _currentUnitOfMeasure = unitOfMeasure;
            var nextUnitOfMeasureButtonSelector =
                string.Join(", ", Enum.GetNames<UnitOfMeasure>().Select(className => $"i.webull-{className}__"));
           
            IReadOnlyCollection<IElementHandle> found;
            do
            {
                await Page.ClickAsync(nextUnitOfMeasureButtonSelector);
                found = await Page.QuerySelectorAllAsync($"i.webull-{unitOfMeasure}__");
            }
            while (found!.Count != 1);

            

            await Page.WaitForTimeoutAsync(600);

            return this;
        }


        public async Task<int> GetQuantityOfPosition(string positionSymbol)
        {

            var positions = await GetActivePositions();
            
            return positions.FirstOrDefault(p => p.Symbol == positionSymbol)?.Quantity ?? 0;
        }

        public async Task<Position[]> GetActivePositions()
        {

            await Page.WaitForSelectorAsync(activePositionsSelector);
            var result = await Page.EvalOnSelectorAllAsync(activePositionsSelector, extractActivePositions);

            return result.Value.Deserialize<JSONArrayElement[]>(_jsonSerializerOptions)
                .Select(jsonTrade => JsonSerializer.Deserialize<Position>(jsonTrade.S, _jsonSerializerOptions))
                .ToArray();

        }


        public async Task<Trade[]> GetActiveTradesAsync()
        {
            var result = await Page.EvalOnSelectorAllAsync(activeTradesSelector, extractActiveTrades);

            return result.Value.Deserialize<JSONArrayElement[]>(_jsonSerializerOptions)
               .Select(jsonTrade => JsonSerializer.Deserialize<Trade>(jsonTrade.S, _jsonSerializerOptions))
               .ToArray();

        }

        public async Task<Trade[]> GetUpdatedActiveTradesAsync()
        {
            int initialRowCount = await Page.EvaluateAsync<int>("document.querySelector(`table`).rows.length");

            await Page.WaitForFunctionAsync("init => document.querySelector(`table`).rows.length !== init", initialRowCount);

            return await GetActiveTradesAsync();
        }
        private async Task WaitUntilContainsDot(string xpathSelector, string textAccessor)
        {
            string baseFunction = "document.evaluate(`{0}`, document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.{1}.includes(`.`)";
            string formatted = string.Format(baseFunction, xpathSelector, textAccessor);
            await Page.WaitForFunctionAsync(formatted);
        }
        public async Task<decimal> GetNetAccountValue()
        {
            await Page.WaitForSelectorAsync($"xpath={netAccountValueXpathSelector}");

            await WaitUntilContainsDot(netAccountValueXpathSelector, "innerText");

            var netAccountValueText = await Page.InnerTextAsync($"xpath={netAccountValueXpathSelector}");

            return decimal.Parse(netAccountValueText, NumberStyles.Any, _currencyFormatter);
        }

        public async Task<TradingPage> AddCurrentCompanyToMyWatchList()
        {
            await Page.ClickAsync(":nth-match(canvas, 2)", new PageClickOptions
            {
                Button = MouseButton.Right,
            });

            await Page.HoverAsync("text=Add to Watchlist");

            await Page.ClickAsync("li:has-text('My Watchlist')");
            return this;

        }


        public async Task<TradingPage> CancelLastActiveTrade()
        {
            await Page.ClickAsync(lastActiveTradeSelector, new PageClickOptions
            {
                Button = MouseButton.Right,
            });

            await Page.ClickAsync("text=Cancel Order");
            await Page.ClickAsync("text=Ok");
            return this;

        }
        public async Task<TradingPage> RemoveLastAddedEntryFromMyWatchlist()
        {
            await Page.ClickAsync("li[draggable='true'] div:first-child span:first-child", new PageClickOptions
            {
                Button = MouseButton.Right,
            });

            await Page.ClickAsync("text=Delete");

            return this;

        }

        public async Task<decimal> GetLimitPrice()
        {
            await WaitUntilContainsDot(limitPriceXpathSelector, "value");

            var limitPriceText = await Page.InputValueAsync(limitPriceSelector);

            return decimal.Parse(limitPriceText, NumberStyles.Any, _currencyFormatter);

        }

        public async Task<TradingPage> GoToPaperTrading()
        {
            await Page.ClickAsync(".webull-Papertrading__");
            return this;
        }

 

        public async Task<TradingPage> EnterAmount(decimal amount)
        {
            
            await Page.ClickAsync(":nth-match(input:left-of(li),4)");

            
            var amountText = amount.ToString(_currencyFormatter);

            await Page.FillAsync(":nth-match(input:left-of(li),4)", amountText);


            return this;
        }

        public async Task<string> GetCurrentCompanySymbol()
        {
            await Page.WaitForSelectorAsync(symbolInputSelector, new() { State = WaitForSelectorState.Visible });
            await Page.WaitForFunctionAsync("document.querySelector(`[placeholder = 'Symbol']`).value.trim().length > 0");
            return await Page.InputValueAsync(symbolInputSelector);
        }

        public async Task<TradingPage> ClickPaperTrade()
        {
            await Page.ClickAsync("text=Paper Trade");
            return this;
        }

    }
}
