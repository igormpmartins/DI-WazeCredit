using WazeCredit.Models;

namespace WazeCredit.Service
{
    public class MarketForecasterV2: IMarketForecaster
    {
        public MarketResult GetMarketPrediction() => 
            new MarketResult { MarketCondition = MarketCondition.Volatile };
    }
}
