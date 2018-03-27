using System.ComponentModel;

namespace CMSCore.Data.Enums
{
    public enum PaymentMethod
    {
        [Description("Thanh toán khi giao hàng")]
        CashOnDelivery,

        [Description("Thanh toán trực tuyến")]
        OnlinBanking,

        [Description("Cổng thanh toán")]
        PaymentGateway,

        [Description("Thanh toán thẻ visa")]
        Visa,

        [Description("Thanh toán thẻ master card")]
        MasterCard,

        [Description("Thanh toán paypal")]
        PayPal,

        [Description("Thanh toán thẻ atm")]
        Atm
    }
}