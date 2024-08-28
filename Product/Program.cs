using System.Collections;
using System.Data.Common;
using System.Text;
using System.Data.SqlClient;

namespace Product;

class Program
{
    private static string connectionString = $"Server=(localdb)\\MSSQLLocalDB";

    static void Main(string[] args)
    {
        Menu();

        static void Menu()
        {
            Console.Clear();
            Console.WriteLine("Enter 1 to add product");
            Console.WriteLine("Enter 2 to view products");
            string option = Console.ReadLine();
            if (option == "1") AddProduct();
            else if (option == "2") ViewProducts();
            else
            {
                Console.WriteLine("Error. Try again");
                Console.ReadKey();
                Menu();
            }
        }

        static void AddProduct()
        {
            Product p = new Product(GetId(), GetName(), GetPrice());


            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = $"insert into ProductTable values ('{p.Id}', '{p.Name}', '{p.Price}')";
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            Console.WriteLine($"Successfully added {p.Details}");
            Console.WriteLine("Press any key to return to menu");
            Console.ReadKey();
            Menu();
        }

        static void ViewProducts()
        {
            List<Product> productList = new List<Product>();
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = "select * from ProductTable";
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var dbIdentity = reader.GetInt32(0);
                        var dbName = reader.GetString(1);
                        var dbPrice = reader.GetDecimal(2);
                        Product dbDetails = new Product(dbIdentity, dbName, dbPrice);
                        productList.Add(dbDetails);
                    }
                }
                conn.Close();
            }
            if(productList == null) Console.WriteLine("No products found");
            else
            {
                foreach (var item in productList)
                {
                    Console.WriteLine(item);
                }
            }
            Console.WriteLine("Press any key to return to menu");
            Console.ReadKey();
            Menu();
        }
    }

    static string GetName()
    {
        Console.WriteLine("Enter product name:");
        string name = Console.ReadLine();
        return name;
    }

    static int GetId()
    {
        Console.WriteLine("Enter product ID:");
        int id = int.Parse(Console.ReadLine());
        return id;
    }

    static decimal GetPrice()
    {
        Console.WriteLine("Enter product price:");
        decimal price = Convert.ToDecimal(Console.ReadLine());

        return price;
    }
}

class Product
{
    internal int Id { get; set; }
    internal string Name { get; set; }
    internal decimal Price { get; set; }
    public (int, string, decimal) Details { get; set; }

    public Product(int id, string name, decimal price)
    {
        Id = id;
        Name = name;
        Price = price;
        Details = (Id, Name, Price);
    }
}