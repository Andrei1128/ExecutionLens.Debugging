using PostMortemTests.Models;

namespace PostMortemTests.Helpers;

public class OrderMapper : IOrderMapper
{
    public Order Map(OrderDTO orderDto)
    {
        return new Order
        {
            CustomerId = orderDto.CustomerId,
            Items = orderDto.Items,
            TotalPrice = orderDto.TotalPrice,
            PlacedAt = orderDto.PlacedAt
        };
    }
}

public interface IOrderMapper
{
    Order Map(OrderDTO orderDto);
}
