namespace PostMortemTests.Models;

public class Order
{
    public DateTime PlacedAt { get; set; }
    public string[] Items { get; set; } = [];
    public double TotalPrice { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerAddress { get; set; } = string.Empty;
    public DateTime ReceiveAt { get; set; }
}
