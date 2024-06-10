using DapperQuery;

Console.WriteLine("Hello, World!");

Repository repository = new Repository();

//Console.WriteLine(repository.Add());

var orders = repository.GetOrders();

foreach (var item in orders)
{
    Console.WriteLine($"Order: {item.Id} On Date: {item.Date}");

    foreach (var detail in item.OrderDetails)
    {
        Console.WriteLine($"Id: {detail.Id} {detail.ProductName}");
    }
}