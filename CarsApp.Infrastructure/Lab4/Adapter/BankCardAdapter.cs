using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CarsApp.Domain.Lab4.Adapter;

namespace CarsApp.Infrastructure.Lab4.Adapter
{
    public class BankCardAdapter : IPaymentProvider
    {
        private readonly ExternalBankApi _bank = new();
        public string ProviderName => "Bank Card";

        public PaymentResult ProcessPayment(string customerName, decimal amount)
        {
            // Banca vrea float si currency string — adaptam
            var code = _bank.InitiateTransfer(customerName, (float)amount, "RON");
            var status = _bank.GetTransferStatus(code);
            var success = status == "COMPLETED";
            return new PaymentResult(success, $"BANK-{code}", $"Bank transfer {status} for {customerName}");
        }
    }
}