namespace checkout_kata.library;

public class Item()
{
    public string SKU { get; set; }
    public uint UnitPrice { get; set; }
    public int Count { get; set; }
    public uint? SpecialAmount { get; set; }
    public float? SpecialPrice { get; set; }
}