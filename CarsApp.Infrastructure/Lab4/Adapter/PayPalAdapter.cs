using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CarsApp.Domain.Lab4.Adapter;

namespace CarsApp.Infrastructure.Lab4.Adapter
{
    public class PayPalAdapter : IPaymentProvider
    {
        private readonly ExternalPayPalApi _paypal = new();
        public string ProviderName => "PayPal";

        public PaymentResult ProcessPayment(string customerName, decimal amount)
        {
            // PayPal vrea email — folosim customerName ca email simulat
            var paymentId = _paypal.ExecutePayment($"{customerName.ToLower().Replace(" ", ".")}@client.com", amount);
            var verified = _paypal.VerifyPayment(paymentId);
            return new PaymentResult(verified, paymentId, $"PayPal payment {(verified ? "verified" : "failed")} for {customerName}");
        }
    }
}