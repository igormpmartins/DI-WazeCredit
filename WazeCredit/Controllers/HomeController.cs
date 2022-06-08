using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WazeCredit.Data;
using WazeCredit.Data.Repository.IRepository;
using WazeCredit.Models;
using WazeCredit.Models.ViewModels;
using WazeCredit.Service;
using WazeCredit.Utility.AppSettingsClasses;

namespace WazeCredit.Controllers
{
    public class HomeController : Controller
    {
        public HomeVM homeVM { get; set; }
        private readonly IMarketForecaster _marketForecaster;
        private readonly ICreditValidator _creditValidator;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<HomeController> logger;
        private readonly WazeForecastSettings _wazeForecastOptions;

        [BindProperty]
        public CreditApplication CreditModel { get; set; }
        

        public HomeController(IMarketForecaster marketForecaster,
            IOptions<WazeForecastSettings> wazeForecastOptions,
            ICreditValidator creditValidator,
            IUnitOfWork unitOfWork,
            ILogger<HomeController> logger)
        {
            homeVM = new HomeVM();
            this._marketForecaster = marketForecaster;
            this._creditValidator = creditValidator;
            this.unitOfWork = unitOfWork;
            this.logger = logger;
            this._wazeForecastOptions = wazeForecastOptions.Value;
        }

        public IActionResult AllConfigSettings(
            [FromServices] IOptions<SendGridSettings> sendGridOptions,
            [FromServices] IOptions<TwilioSettings> twilioOptions,
            [FromServices] IOptions<StripeSettings> stripeOptions)
        {
            var messages = new List<string>();

            messages.Add($"Waze config - Forecast Tracker: " + _wazeForecastOptions.ForecastTrackerEnabled);

            messages.Add($"Stripe pkey: " + stripeOptions.Value.PublishableKey);
            messages.Add($"Stripe secret " + stripeOptions.Value.SecretKey);

            messages.Add($"SenGrid Key" + sendGridOptions.Value.SendGridKey);

            messages.Add($"Twilio Phone Number: " + twilioOptions.Value.PhoneNumber);
            messages.Add($"Twilio acc sid: " + twilioOptions.Value.AccountSid);
            messages.Add($"Twilio AuthToken: " + twilioOptions.Value.AuthToken);

            return View(messages);
        }

        public IActionResult Index()
        {
            logger.LogInformation("Yo - Home controller Index Action called!");

            var currentMarket = _marketForecaster.GetMarketPrediction();

            switch (currentMarket.MarketCondition)
            {
                case MarketCondition.StableDown:
                    homeVM.MarketForecast = "Market shows signs to go down in a stable state! It is a not a good sign to apply for credit applications! But extra credit is always piece of mind if you have handy when you need it.";
                    break;
                case MarketCondition.StableUp:
                    homeVM.MarketForecast = "Market shows signs to go up in a stable state! It is a great sign to apply for credit applications!";
                    break;
                case MarketCondition.Volatile:
                    homeVM.MarketForecast = "Market shows signs of volatility. In uncertain times, it is good to have credit handy if you need extra funds!";
                    break;
                default:
                    homeVM.MarketForecast = "Apply for a credit card using our application!";
                    break;
            }

            logger.LogInformation("Yo - Home controller Index Action ended, requesting view.");

            return View(homeVM);
        }

        public IActionResult CreditApplication()
        {
            CreditModel = new CreditApplication();
            return View(CreditModel);
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        [ActionName("CreditApplication")]
        public async Task<IActionResult> CreditApplicationPost(
            [FromServices] Func<CreditApprovedEnum, ICreditApproved> creditService
            )
        {
            if (ModelState.IsValid)
            {
                var (valid, errors) = await _creditValidator.PassAllValidations(CreditModel);

                var creditResult = new CreditResult
                {
                    ErrorList = errors,
                    CreditID = 0,
                    Success = valid
                };

                if (valid)
                {
                    CreditModel.CreditApproved = creditService(
                        CreditModel.Salary > 50000 ? CreditApprovedEnum.High : CreditApprovedEnum.Low
                        ).GetCreditApproved(CreditModel);

                    unitOfWork.CreditApplication.Add(CreditModel);
                    unitOfWork.Save();

                    creditResult.CreditID = CreditModel.Id;
                    creditResult.CreditApproved = CreditModel.CreditApproved;
                }

                return RedirectToAction(nameof(CreditResult), creditResult);

            }

            return View(CreditModel);
        }

        public IActionResult CreditResult(CreditResult creditResult) => View(creditResult);

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
