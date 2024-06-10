using Dapper;
using System.Data.SqlClient;

namespace DapperQuery;

public class Repository
{
    private readonly string connectionString = "Server=.;Initial Catalog=Dapper_Db; Integrated Security=true; TrustServerCertificate=true";

    public int Add()
    {
        string sql = "Insert Into Customers (FirstName, LastName) Values(@FirstName, @LastName); Select SCOPE_IDENTITY()";

        var connection = new SqlConnection(connectionString);

        var result = connection.QuerySingle<int>(sql, new { FirstName = "Your Name", LastName = "Your Famiy" });

        return result;
    }

    //Join One To Many
    public List<Order> GetOrders()
    {
        string sql = "Select Top 10 * From Orders As O Inner Join OrderDetails As OD On O.Id = OD.OrderId";

        var connection = new SqlConnection(connectionString);

        var orderDic = new Dictionary<long, Order>();

        var orderList = connection.Query<Order, OrderDetail, Order>(sql,
            (order, orderDetail) =>
            {
                Order entity;
                if (!orderDic.TryGetValue(order.Id, out entity))
                {
                    entity = order;
                    entity.OrderDetails = new List<OrderDetail>();
                    orderDic.Add(entity.Id, entity);
                }
                entity.OrderDetails.Add(orderDetail);
                return entity;
            },

            splitOn: "Id")
            .Distinct()
            .ToList();

        return orderList;
    }
}

public class Order
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public List<OrderDetail> OrderDetails { get; set; }
}

public class OrderDetail
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public string ProductName { get; set; }
}

