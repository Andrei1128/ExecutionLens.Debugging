using PostMortemTests.Models;

namespace PostMortemTests.Repositories;

public class OrderRepository : IOrderRepository
{
    public (string name, string address) GetCustomerNameAndAddress(int customerId)
    {
        return ("Andrei", "123 Main St.");
    }

    public void SaveOrder(Order order)
    {
    }
}

public interface IOrderRepository
{
    (string name, string address) GetCustomerNameAndAddress(int customerId);
    void SaveOrder(Order order);
}
