using CarsApp.Domain.Lab4.Adapter;
using CarsApp.Infrastructure.Lab4.Adapter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class PlataModel : PageModel
{
    public string OrderId { get; set; } = "";
    public decimal Amount { get; set; } = 0;
    public string ProviderName { get; set; } = "";
    public PaymentResult? PaymentResult { get; set; }

    public void OnGet(string? orderId, decimal? amount)
    {
        OrderId = orderId ?? "";
        Amount = amount ?? 0;
    }

    public IActionResult OnPostPlateste(
        string customerName, string orderId,
        decimal amount, string provider)
    {
        OrderId = orderId;
        Amount = amount;

        IPaymentProvider paymentProvider = provider switch
        {
            "paypal" => new PayPalAdapter(),
            "bank" => new BankCardAdapter(),
            _ => new StripeAdapter()
        };

        ProviderName = paymentProvider.ProviderName;
        PaymentResult = paymentProvider.ProcessPayment(customerName, amount);

        return Page();
    }
}