using WazeCredit.Models;

namespace WazeCredit.Service
{
    public class MarketForecaster : IMarketForecaster
    {
        public MarketResult GetMarketPrediction() =>
            new MarketResult { MarketCondition = MarketCondition.StableUp };
    }
}
