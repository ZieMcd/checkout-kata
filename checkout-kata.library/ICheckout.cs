namespace checkout_kata.library;

interface ICheckout
{
    void Scan(string item);
    int GetTotalPrice();
}