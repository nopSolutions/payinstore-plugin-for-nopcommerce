using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Payments.PayInStore.Models;
using Nop.Services.Configuration;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Payments.PayInStore.Components
{
    [ViewComponent(Name = PayInStoreDefaults.PAYMENT_INFO_VIEW_COMPONENT_NAME)]
    public class PaymentPayInStoreViewComponent : NopViewComponent
    {
        private readonly ISettingService _settingService;
        private readonly IStoreContext _storeContext;

        public PaymentPayInStoreViewComponent(ISettingService settingService, IStoreContext storeContext)
        {
            _settingService = settingService;
            _storeContext = storeContext;
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
