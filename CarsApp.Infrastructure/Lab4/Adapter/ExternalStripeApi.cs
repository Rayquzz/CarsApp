using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsApp.Infrastructure.Lab4.Adapter
{
    // Simuleaza SDK-ul real al Stripe — interfata incompatibila
    public class ExternalStripeApi
    {
        public StripeChargeResult CreateCharge(string cardHolder, double amountInCents)
        {
            var txId = "stripe_" + Guid.NewGuid().ToString("N")[..8].ToUpper();
            return new StripeChargeResult(true, txId, $"Stripe charge OK for {cardHolder}");
        }
    }

    public record StripeChargeResult(bool Charged, string ChargeId, string Details);
}