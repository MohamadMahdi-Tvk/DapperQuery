using DapperQuery;

Console.WriteLine("Hello, World!");

Repository repository = new Repository();

#region Add

//Console.WriteLine(repository.Add());

#endregion

#region GetOrders

//var orders = repository.GetOrders();

//foreach (var item in orders)
//{
//    Console.WriteLine($"Order: {item.Id} On Date: {item.Date}");

//    foreach (var detail in item.OrderDetails)
//    {
//        Console.WriteLine($"Id: {detail.Id} {detail.ProductName}");
//    }
//}

#endregion

#region GetInvoice

var orders = repository.GetInvoice();

#endregion

#region MultipleQueries

repository.MultipleQueries();

#endregion

#region Queries

//repository.Queries();

#endregion

#region RunStoredProcedure

//var ordersFromProc = repository.RunSp();

#endregion

Console.ReadKey();