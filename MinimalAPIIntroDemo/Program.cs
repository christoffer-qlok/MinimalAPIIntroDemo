using System.Net;

namespace MinimalAPIIntroDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Hämta en parameter från appsettings.json
            string defaultGreeting = builder.Configuration.GetValue<string>("DefaultGreeting");

            var app = builder.Build();

            app.MapGet("/", () => "Hello World!");

            // Route parametrar + konfiguration
            app.MapGet("/greeting/{name?}", (string? name) =>
            {
                if (name == null)
                {
                    return defaultGreeting;
                }
                return $"Hello {name}!";
            });


            // Respons codes result
            string[] jokes =
            {
                "Why don't scientists trust atoms? Because they make up everything!",
                "I told my wife she was drawing her eyebrows too high. She looked surprised.",
                "Why did the bicycle fall over? Because it was two-tired!"
            };
            app.MapGet("/jokes/{id}", (int id) =>
            {
                if (id < 0 || id >= jokes.Length)
                {
                    return Results.NotFound("No such joke");
                }
                return Results.Ok(jokes[id]);
            });

            // Return JSON
            List<Product> products = new List<Product>()
            {
                new Product() { Name = "Umbrella", Price = 150, Description = "Protects you from rain"},
                new Product() { Name = "Shoes", Price = 500, Description = "Protects your feet"},
                new Product() { Name = "Shirt", Price = 100, Description = "Office attire"}
            };
            app.MapGet("/products/{id}", (int id) =>
            {
                if (id < 0 || id >= products.Count())
                {
                    return Results.NotFound();
                }
                return Results.Json(products[id]);
            });

            app.MapGet("/products", () => Results.Json(products.Select(p => p.Name)));

            // POST new objects
            app.MapPost("/products", (Product product) =>
            {
                products.Add(product);
                return Results.StatusCode((int)HttpStatusCode.Created);
            });

            app.Run();
        }
    }
}