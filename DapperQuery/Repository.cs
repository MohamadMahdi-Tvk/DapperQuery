using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace DapperQuery;

public class Repository
{
    private readonly string connectionString = "Server=.;Initial Catalog=Dapper_Db; Integrated Security=true; TrustServerCertificate=true";

    //Add
    public int Add()
    {
        string sql = "Insert Into Customers (FirstName, LastName) Values(@FirstName, @LastName); Select SCOPE_IDENTITY()";

        var connection = new SqlConnection(connectionString);

        var result = connection.QuerySingle<int>(sql, new { FirstName = "Your Name", LastName = "Your Famiy" });

        return result;
    }

    //Join One To Many
    public List<Orders> GetOrders()
    {
        string sql = "Select Top 10 * From Orders As O Inner Join OrderDetails As OD On O.Id = OD.OrderId";

        var connection = new SqlConnection(connectionString);

        var orderDic = new Dictionary<long, Orders>();

        var orderList = connection.Query<Orders, OrderDetails, Orders>(sql,
            (order, orderDetail) =>
            {
                Orders entity;
                if (!orderDic.TryGetValue(order.Id, out entity))
                {
                    entity = order;
                    entity.OrderDetails = new List<OrderDetails>();
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

    //Join One To One
    public List<Orders> GetInvoice()
    {
        string sql = "SELECT * FROM Orders As O INNER JOIN Invoice As I ON O.Id = I.Id";

        var connection = new SqlConnection(connectionString);

        var invoice = connection.Query<Orders, Invoice, Orders>(sql,
            (order, invoice) =>
            {
                order.Invoice = invoice;
                return order;

            }, splitOn: "Id").Distinct().ToList();

        return invoice;

    }

    //Multiple Queries
    public void MultipleQueries()
    {
        string sql = "Select * From Orders; Select * From Invoice";

        var connection = new SqlConnection(connectionString);

        var result = connection.QueryMultiple(sql);

        var orders = result.Read<Orders>().ToList();

        var invoice = result.Read<Invoice>().ToList();
    }

    //QueryFirst, QyeryFirstOrDefault, QuerySingle, QuerySingleOrDefault
    public void Queries()
    {
        string sql = "Select * From Orders";

        var connection = new SqlConnection(connectionString);

        var resultQueryFirst = connection.QueryFirst<Orders>(sql);

        var resultQueryFirstOrDefault = connection.QueryFirstOrDefault<Orders>(sql);

        var resultQueySingle = connection.QuerySingle<Orders>(sql);

        var resultQuerySingleOrDefault = connection.QuerySingleOrDefault<Orders>(sql);
    }

    //Use Stored Procedures
    public List<Orders> RunSp()
    {
        string procedureName = "SelectOrders";

        var connection = new SqlConnection(connectionString);

        var result = connection.Query<Orders>(procedureName, CommandType.StoredProcedure);

        return result.ToList();
    }

}

public class Orders
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public List<OrderDetails> OrderDetails { get; set; }

    public Invoice Invoice { get; set; }
}

public class OrderDetails
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public string ProductName { get; set; }
}

public class Invoice
{
    public int Id { get; set; }
    public long Price { get; set; }
}

