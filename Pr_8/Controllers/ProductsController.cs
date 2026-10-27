using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using Pr_8.Models;

namespace Pr_8.Controllers
{
    public class ProductsController : Controller
    {
        private readonly string cs =
            ConfigurationManager.ConnectionStrings["Pr8Connection"]
            .ConnectionString;

        // READ: Display all products
        public ActionResult Index()
        {
            List<Product> products = new List<Product>();

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query =
                    "SELECT ProductId, ProductName, Quantity, Price " +
                    "FROM Products";

                SqlCommand cmd = new SqlCommand(query, con);

                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Product p = new Product();

                    p.ProductId = System.Convert.ToInt32(
                        dr["ProductId"]);

                    p.ProductName = dr["ProductName"].ToString();

                    p.Quantity = System.Convert.ToInt32(
                        dr["Quantity"]);

                    p.Price = System.Convert.ToDecimal(
                        dr["Price"]);

                    products.Add(p);
                }
            }

            return View(products);
        }

        // CREATE: Display form
        public ActionResult Create()
        {
            return View();
        }

        // CREATE: Insert product
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product p)
        {
            if (!ModelState.IsValid)
                return View(p);

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query =
                    "INSERT INTO Products " +
                    "(ProductName, Quantity, Price) " +
                    "VALUES (@ProductName, @Quantity, @Price)";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.Add(
                    "@ProductName", SqlDbType.VarChar, 100)
                    .Value = p.ProductName;

                cmd.Parameters.Add(
                    "@Quantity", SqlDbType.Int)
                    .Value = p.Quantity;

                SqlParameter price =
                    cmd.Parameters.Add("@Price", SqlDbType.Decimal);

                price.Precision = 10;
                price.Scale = 2;
                price.Value = p.Price;

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        // UPDATE: Display existing product
        public ActionResult Edit(int id)
        {
            Product p = GetProduct(id);

            if (p == null)
                return HttpNotFound();

            return View(p);
        }

        // UPDATE: Save changes
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product p)
        {
            if (!ModelState.IsValid)
                return View(p);

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query =
                    "UPDATE Products SET " +
                    "ProductName = @ProductName, " +
                    "Quantity = @Quantity, " +
                    "Price = @Price " +
                    "WHERE ProductId = @ProductId";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.Add(
                    "@ProductId", SqlDbType.Int)
                    .Value = p.ProductId;

                cmd.Parameters.Add(
                    "@ProductName", SqlDbType.VarChar, 100)
                    .Value = p.ProductName;

                cmd.Parameters.Add(
                    "@Quantity", SqlDbType.Int)
                    .Value = p.Quantity;

                SqlParameter price =
                    cmd.Parameters.Add("@Price", SqlDbType.Decimal);

                price.Precision = 10;
                price.Scale = 2;
                price.Value = p.Price;

                con.Open();

                int rows = cmd.ExecuteNonQuery();

                if (rows == 0)
                    return HttpNotFound();
            }

            return RedirectToAction("Index");
        }

        // DELETE: Display confirmation
        public ActionResult Delete(int id)
        {
            Product p = GetProduct(id);

            if (p == null)
                return HttpNotFound();

            return View(p);
        }

        // DELETE: Remove product
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query =
                    "DELETE FROM Products WHERE ProductId = @ProductId";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.Add(
                    "@ProductId", SqlDbType.Int).Value = id;

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        // Find one product using its ID
        private Product GetProduct(int id)
        {
            Product p = null;

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query =
                    "SELECT ProductId, ProductName, Quantity, Price " +
                    "FROM Products WHERE ProductId = @ProductId";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.Add(
                    "@ProductId", SqlDbType.Int).Value = id;

                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    p = new Product
                    {
                        ProductId = System.Convert.ToInt32(
                            dr["ProductId"]),

                        ProductName = dr["ProductName"].ToString(),

                        Quantity = System.Convert.ToInt32(
                            dr["Quantity"]),

                        Price = System.Convert.ToDecimal(
                            dr["Price"])
                    };
                }
            }

            return p;
        }
    }
}