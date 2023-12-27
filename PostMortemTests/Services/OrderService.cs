using PostMortemTests.Helpers;
using PostMortemTests.Models;
using PostMortemTests.Repositories;

namespace PostMortemTests.Services;

public class OrderService(IOrderMapper _orderMapper, IClockService _clockService, IOrderRepository _orderRepository) : IOrderService
{
    public bool IsOrderValid(OrderDTO orderDto) => true;

    public void PlaceOrder(OrderDTO orderDto)
    {
        Order order = _orderMapper.Map(orderDto);

        if (!IsOrderValid(orderDto))
        {
            throw new Exception("Order is not valid!");
        }

        if (!_clockService.IsPlacedAtValid(order.PlacedAt, out DateTime? DateTimeNow))
        {
            order.PlacedAt = (DateTime)DateTimeNow!;
        }

        (string name, string address) = _orderRepository.GetCustomerNameAndAddress(order.CustomerId);

        if (name is not null && address is not null)
        {
            order.CustomerName = name;
            order.CustomerAddress = address;
            order.ReceiveAt = _clockService.CalculateReceiveAt(address, order.PlacedAt);

            _orderRepository.SaveOrder(order);
        }
        else
        {
            throw new Exception("Customer could not be found!");
        }
    }
}

public interface IOrderService
{
    bool IsOrderValid(OrderDTO orderDto);
    void PlaceOrder(OrderDTO orderDto);
}
