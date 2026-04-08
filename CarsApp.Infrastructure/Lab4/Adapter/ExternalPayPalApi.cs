using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsApp.Infrastructure.Lab4.Adapter
{
    // Simuleaza SDK-ul real al PayPal — interfata incompatibila
    public class ExternalPayPalApi
    {
        public string ExecutePayment(string payerEmail, decimal total)
        {
            return "PAYPAL-" + Guid.NewGuid().ToString("N")[..8].ToUpper();
        }

        public bool VerifyPayment(string paymentId)
        {
            return true;
        }
    }
}