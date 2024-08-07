using checkout_kata.library.models;
using Microsoft.Extensions.Logging;

namespace checkout_kata.library;

public class Checkout(IEnumerable<PricingRule> pricingRules, ILogger<Checkout> logger) : ICheckout
{
    private readonly List<CartItem> _cart = pricingRules.Select(x => new CartItem { SKU = x.SKU, UnitPrice = x.UnitPrice, Count = 0, SpecialAmount = x.SpecialAmount, SpecialPrice = x.SpecialPrice }).ToList();

    public void Scan(string item)
    {
        if (string.IsNullOrEmpty(item))
            throw new ArgumentException("item can not be empty string.", nameof(item));

        if (pricingRules.All(x => x.SKU != item))
            throw new ArgumentException("item X is invalid item.", nameof(item));

        _cart.FirstOrDefault(x => x.SKU == item)!.Count++;
    }

    public int GetTotalPrice()
    {
        var total = 0;
        if (_cart.All(x => x.Count == 0))
        {
            logger.LogWarning("No Items Scanned");
            return total;
        }

        foreach (var item in _cart)
        {
            if (item.SpecialAmount is not null && item.SpecialPrice is not null)
            {
                var specialOffer = item.Count / item.SpecialAmount;
                //just following interface given in README, I'm assuming there is not a scenario where total goes beyond int max 
                total += (int)(specialOffer * item.SpecialPrice);
                total += (int)(item.Count % item.SpecialAmount * item.UnitPrice);
                continue;
            }

            //just following interface given in README, I'm assuming there is not a scenario where total goes beyond int max 
            total += (int)(item.Count * item.UnitPrice);
        }

        return total;
    }
}