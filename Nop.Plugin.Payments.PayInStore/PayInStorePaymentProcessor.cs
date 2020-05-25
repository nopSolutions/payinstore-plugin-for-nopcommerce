using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Nop.Core;
using Nop.Core.Domain.Orders;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Payments;
using Nop.Services.Plugins;

namespace Nop.Plugin.Payments.PayInStore
{
    /// <summary>
    /// PayInStore payment processor
    /// </summary>
    public class PayInStorePaymentProcessor : BasePlugin, IPaymentMethod
    {
        #region Fields

        private readonly ILocalizationService _localizationService;
        private readonly IPaymentService _paymentService;
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;
        private readonly PayInStorePaymentSettings _payInStorePaymentSettings;

        #endregion

        #region Ctor

        public PayInStorePaymentProcessor(ILocalizationService localizationService,
            IPaymentService paymentService,
            ISettingService settingService,
            IWebHelper webHelper,
            PayInStorePaymentSettings payInStorePaymentSettings)
        {
            _localizationService = localizationService;
            _paymentService = paymentService;
            _settingService = settingService;
            _webHelper = webHelper;
            _payInStorePaymentSettings = payInStorePaymentSettings;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Process a payment
        /// </summary>
        /// <param name="processPaymentRequest">Payment info required for an order processing</param>
        /// <returns>Process payment result</returns>
        public ProcessPaymentResult ProcessPayment(ProcessPaymentRequest processPaymentRequest)
        {
            return new ProcessPaymentResult();
        }

        /// <summary>
        /// Post process payment (used by payment gateways that require redirecting to a third-party URL)
        /// </summary>
        /// <param name="postProcessPaymentRequest">Payment info required for an order processing</param>
        public void PostProcessPayment(PostProcessPaymentRequest postProcessPaymentRequest)
        {
            //nothing
        }

        /// <summary>
        /// Returns a value indicating whether payment method should be hidden during checkout
        /// </summary>
        /// <param name="cart">Shoping cart</param>
        /// <returns>true - hide; false - display.</returns>
        public bool HidePaymentMethod(IList<ShoppingCartItem> cart)
        {
            //you can put any logic here
            //for example, hide this payment method if all products in the cart are downloadable
            //or hide this payment method if current customer is from certain country
            return false;
        }

        /// <summary>
        /// Gets additional handling fee
        /// </summary>
        /// <param name="cart">Shoping cart</param>
        /// <returns>Additional handling fee</returns>
        public decimal GetAdditionalHandlingFee(IList<ShoppingCartItem> cart)
        {
            var result = _paymentService.CalculateAdditionalFee(cart,
                _payInStorePaymentSettings.AdditionalFee, _payInStorePaymentSettings.AdditionalFeePercentage);
            return result;
        }

        /// <summary>
        /// Captures payment
        /// </summary>
        /// <param name="capturePaymentRequest">Capture payment request</param>
        /// <returns>Capture payment result</returns>
        public CapturePaymentResult Capture(CapturePaymentRequest capturePaymentRequest)
        {
            var result = new CapturePaymentResult();
            result.AddError("Capture method not supported");
            return result;
        }

        /// <summary>
        /// Refunds a payment
        /// </summary>
        /// <param name="refundPaymentRequest">Request</param>
        /// <returns>Result</returns>
        public RefundPaymentResult Refund(RefundPaymentRequest refundPaymentRequest)
        {
            var result = new RefundPaymentResult();
            result.AddError("Refund method not supported");
            return result;
        }

        /// <summary>
        /// Voids a payment
        /// </summary>
        /// <param name="voidPaymentRequest">Request</param>
        /// <returns>Result</returns>
        public VoidPaymentResult Void(VoidPaymentRequest voidPaymentRequest)
        {
            var result = new VoidPaymentResult();
            result.AddError("Void method not supported");
            return result;
        }

        /// <summary>
        /// Process recurring payment
        /// </summary>
        /// <param name="processPaymentRequest">Payment info required for an order processing</param>
        /// <returns>Process payment result</returns>
        public ProcessPaymentResult ProcessRecurringPayment(ProcessPaymentRequest processPaymentRequest)
        {
            var result = new ProcessPaymentResult();
            result.AddError("Recurring payment not supported");
            return result;
        }

        /// <summary>
        /// Cancels a recurring payment
        /// </summary>
        /// <param name="cancelPaymentRequest">Request</param>
        /// <returns>Result</returns>
        public CancelRecurringPaymentResult CancelRecurringPayment(CancelRecurringPaymentRequest cancelPaymentRequest)
        {
            var result = new CancelRecurringPaymentResult();
            result.AddError("Recurring payment not supported");
            return result;
        }

        /// <summary>
        /// Gets a value indicating whether customers can complete a payment after order is placed but not completed (for redirection payment methods)
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>Result</returns>
        public bool CanRePostProcessPayment(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            //it's not a redirection payment method. So we always return false
            return false;
        }

        public IList<string> ValidatePaymentForm(IFormCollection form)
        {
            return new List<string>();
        }

        public ProcessPaymentRequest GetPaymentInfo(IFormCollection form)
        {
            return new ProcessPaymentRequest();
        }

        public string GetPublicViewComponentName()
        {
            return "PaymentPayInStore";
        }

        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/PaymentPayInStore/Configure";
        }

        /// <summary>
        /// Install the plugin
        /// </summary>
        public override void Install()
        {
            //settings
            _settingService.SaveSetting(new PayInStorePaymentSettings()
            {
                DescriptionText = "<p>Reserve items at your local store, and pay in store when you pick up your order.<br />Our store location: USA, New York,...</p><p>P.S. You can edit this text from admin panel.</p>"
            });

            //locales
            _localizationService.AddPluginLocaleResource(new Dictionary<string, string>
            {
                ["Plugins.Payment.PayInStore.AdditionalFee"] = "Additional fee",
                ["Plugins.Payment.PayInStore.AdditionalFee.Hint"] = "The additional fee.",
                ["Plugins.Payment.PayInStore.AdditionalFeePercentage"] = "Additional fee. Use percentage",
                ["Plugins.Payment.PayInStore.AdditionalFeePercentage.Hint"] = "Determines whether to apply a percentage additional fee to the order total. If not enabled, a fixed value is used.",
                ["Plugins.Payment.PayInStore.DescriptionText"] = "Description",
                ["Plugins.Payment.PayInStore.DescriptionText.Hint"] = "Enter info that will be shown to customers during checkout",
                ["Plugins.Payment.PayInStore.PaymentMethodDescription"] = "Paying directly in the store"
            });

            base.Install();
        }

        /// <summary>
        /// Uninstall the plugin
        /// </summary>
        public override void Uninstall()
        {
            //settings
            _settingService.DeleteSetting<PayInStorePaymentSettings>();

            //locales
            _localizationService.DeletePluginLocaleResources("Plugins.Payment.PayInStore");

            base.Uninstall();
        }

        #endregion

        #region Properies

        /// <summary>
        /// Gets a value indicating whether capture is supported
        /// </summary>
        public bool SupportCapture => false;

        /// <summary>
        /// Gets a value indicating whether partial refund is supported
        /// </summary>
        public bool SupportPartiallyRefund => false;

        /// <summary>
        /// Gets a value indicating whether refund is supported
        /// </summary>
        public bool SupportRefund => false;

        /// <summary>
        /// Gets a value indicating whether void is supported
        /// </summary>
        public bool SupportVoid => false;

        /// <summary>
        /// Gets a recurring payment type of payment method
        /// </summary>
        public RecurringPaymentType RecurringPaymentType => RecurringPaymentType.NotSupported;

        /// <summary>
        /// Gets a payment method type
        /// </summary>
        public PaymentMethodType PaymentMethodType => PaymentMethodType.Standard;

        /// <summary>
        /// Gets a value indicating whether we should display a payment information page for this plugin
        /// </summary>
        public bool SkipPaymentInfo => false;

        /// <summary>
        /// Gets a payment method description that will be displayed on checkout pages in the public store
        /// </summary>
        public string PaymentMethodDescription => _localizationService.GetResource("Plugins.Payment.PayInStore.PaymentMethodDescription");

        #endregion

    }
}
