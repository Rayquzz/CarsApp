using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CarsApp.Domain.Lab4.Adapter
{
    public interface IPaymentProvider
{
    string ProviderName { get; }
    PaymentResult ProcessPayment(string customerName, decimal amount);
}

public record PaymentResult(bool Success, string TransactionId, string Message);
}