using Microsoft.Extensions.Logging;

namespace checkout_kata.library;

public class Checkout(List<PricingRule> pricingRules, ILogger<Checkout> logger) : ICheckout
{
    public void Scan(string item)
    {
        throw new NotImplementedException();
    }

    public int GetTotalPrice()
    {
        throw new NotImplementedException();
    }
}