using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Payments.PayInStore.Models;
using Nop.Services.Configuration;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Payments.PayInStore.Components
{
    [ViewComponent(Name = "PaymentPayInStore")]
    public class PaymentPayInStoreViewComponent : NopViewComponent
    {
        private readonly IStoreContext _storeContext;
        private readonly ISettingService _settingService;

        public PaymentPayInStoreViewComponent(IStoreContext storeContext, ISettingService settingService)
        {
            this._storeContext = storeContext;
            this._settingService = settingService;
        }

        public IViewComponentResult Invoke()
        {
            var payInStorePaymentSettings = _settingService.LoadSetting<PayInStorePaymentSettings>(_storeContext.CurrentStore.Id);
            var model = new PaymentInfoModel
            {
                DescriptionText = payInStorePaymentSettings.DescriptionText
            };

            return View("~/Plugins/Payments.PayInStore/Views/PaymentInfo.cshtml", model);
        }
    }
}
