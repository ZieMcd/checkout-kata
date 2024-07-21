namespace checkout_kata.library.models;

public record PricingRule(string SKU, uint UnitPrice, uint? SpecialAmount, uint? SpecialPrice);