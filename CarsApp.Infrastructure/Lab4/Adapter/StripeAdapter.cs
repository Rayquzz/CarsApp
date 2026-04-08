using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CarsApp.Domain.Lab4.Adapter;

namespace CarsApp.Infrastructure.Lab4.Adapter
{
    public class StripeAdapter : IPaymentProvider
    {
        private readonly ExternalStripeApi _stripe = new();
        public string ProviderName => "Stripe";

        public PaymentResult ProcessPayment(string customerName, decimal amount)
        {
            // Stripe vrea double in cents — adaptam
            var result = _stripe.CreateCharge(customerName, (double)(amount * 100));
            return new PaymentResult(result.Charged, result.ChargeId, result.Details);
        }
    }
}