namespace checkout_kata.library;

public record PricingRule(string SKU, uint UnitPrice, uint? SpecialAmount, float? SpecialPrice);