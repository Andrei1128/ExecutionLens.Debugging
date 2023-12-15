namespace PostMortemTests.Models;

public class OrderDTO
{
    public DateTime PlacedAt { get; set; }
    public int CustomerId { get; set; }
    public string[] Items { get; set; } = [];
    public double TotalPrice { get; set; }
}
