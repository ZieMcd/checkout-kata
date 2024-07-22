namespace checkout_kata.library.models;

internal class CartItem
{
    public string SKU { get; set; }
    public uint UnitPrice { get; set; }
    public int Count { get; set; }
    public uint? SpecialAmount { get; set; }
    public float? SpecialPrice { get; set; }
}