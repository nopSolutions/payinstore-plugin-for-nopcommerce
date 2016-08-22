using Nop.Core.Configuration;

namespace Nop.Plugin.Payments.PayInStore
{
    public class PayInStorePaymentSettings : ISettings
    {
        /// <summary>
        /// Gets or sets description text
        /// </summary>
        public string DescriptionText { get; set; }

        /// <summary>
        /// Gets or sets an additional fee
        /// </summary>
        public decimal AdditionalFee { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to "additional fee" is specified as percentage. true - percentage, false - fixed value.
        /// </summary>
        public bool AdditionalFeePercentage { get; set; }
    }
}
