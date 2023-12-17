using PostMortemTests.Models;

namespace PostMortemTests.Repositories;

public class OrderRepository : IOrderRepository
{
    public async Task<(string name, string address)> GetCustomerNameAndAddress(int customerId)
    {
        return ("John Doe", "123 Main St.");
    }

    public async Task SaveOrder(Order order)
    {
    }
}

public interface IOrderRepository
{
    Task<(string name, string address)> GetCustomerNameAndAddress(int customerId);
    Task SaveOrder(Order order);
}
