using PostMortemTests.Models;

namespace PostMortemTests.Repositories;

public class OrderRepository : IOrderRepository
{
    public async Task<(string name, string address)> GetCustomerNameAndAddress(int customerId)
    {
        Thread.Sleep(100);

        return ("John Doe", "123 Main St.");
    }

    public async Task SaveOrder(Order order)
    {
        Thread.Sleep(100);
    }
}

public interface IOrderRepository
{
    Task<(string name, string address)> GetCustomerNameAndAddress(int customerId);
    Task SaveOrder(Order order);
}
