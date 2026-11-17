using System;
using System.Collections.Generic;
using System.Linq;

// Product Class
public class Product
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = "";
    public decimal Price { get; set; }
    public string Description { get; set; } = "";
    public int QuantityInStock { get; set; }
}

// Interfaces
public interface IProductRepository
{
    void AddProduct(Product product);
    Product? GetProductById(int productId);
    List<Product> SearchProductsByName(string name);
    List<Product> GetAllProducts();
}

public interface ICartRepository
{
    void AddToCart(Product product, int quantity);
    void RemoveFromCart(int productId);
    List<(Product product, int quantity)> GetCartItems();
    decimal GetCartTotal();
    void ClearCart();
}

// Product Repository
public class ProductRepository : IProductRepository
{
    private List<Product> products = new();

    public void AddProduct(Product product) => products.Add(product);

    public Product? GetProductById(int productId)
        => products.FirstOrDefault(p => p.ProductId == productId);

    public List<Product> SearchProductsByName(string name)
        => products.Where(p => p.ProductName.ToLower().Contains(name.ToLower())).ToList();

    public List<Product> GetAllProducts() => products;
}

// Cart Repository
public class CartRepository : ICartRepository
{
    private List<(Product product, int quantity)> cartItems = new();

    public void AddToCart(Product product, int quantity)
    {
        var existing = cartItems.FirstOrDefault(c => c.product.ProductId == product.ProductId);
        if (existing.product != null)
        {
            cartItems.Remove(existing);
            cartItems.Add((product, existing.quantity + quantity));
        }
        else
        {
            cartItems.Add((product, quantity));
        }
    }

    public void RemoveFromCart(int productId)
    {
        var item = cartItems.FirstOrDefault(c => c.product.ProductId == productId);
        if (item.product != null)
            cartItems.Remove(item);
    }

    public List<(Product product, int quantity)> GetCartItems() => cartItems;

    public decimal GetCartTotal() => cartItems.Sum(c => c.product.Price * c.quantity);

    public void ClearCart() => cartItems.Clear();
}

// Service Layer
public class ShoppingCartService
{
    private readonly IProductRepository productRepo;
    private readonly ICartRepository cartRepo;

    public ShoppingCartService(IProductRepository p, ICartRepository c)
    {
        productRepo = p;
        cartRepo = c;
    }

    // Display products without stock quantity
    public void DisplayProducts()
    {
        Console.WriteLine("\nAvailable Products:");
        foreach (var p in productRepo.GetAllProducts())
        {
            Console.WriteLine($"ID: {p.ProductId}, Name: {p.ProductName}, Price: {p.Price}");
        }
    }

    // Search products by name
    public void SearchProductsByName()
    {
        Console.Write("Enter product name to search: ");
        string name = Console.ReadLine() ?? "";

        var results = productRepo.SearchProductsByName(name);

        if (results.Count == 0)
        {
            Console.WriteLine("No products found.");
            return;
        }

        Console.WriteLine("\nSearch Results:");
        foreach (var p in results)
        {
            Console.WriteLine($"ID: {p.ProductId}, Name: {p.ProductName}, Price: {p.Price}");
        }
    }

    // Add to cart
    public void AddToCart()
    {
        Console.Write("Enter Product ID: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID!");
            return;
        }

        var product = productRepo.GetProductById(id);
        if (product == null)
        {
            Console.WriteLine("Product not found!");
            return;
        }

        Console.Write("Enter Quantity: ");
        if (!int.TryParse(Console.ReadLine(), out int qty) || qty <= 0)
        {
            Console.WriteLine("Invalid quantity!");
            return;
        }

        if (qty > product.QuantityInStock)
        {
            Console.WriteLine("Insufficient stock!");
            return;
        }

        cartRepo.AddToCart(product, qty);
        Console.WriteLine("Item added successfully!");
    }

    // Remove from cart
    public void RemoveFromCart()
    {
        Console.Write("Enter Product ID to remove: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID!");
            return;
        }

        cartRepo.RemoveFromCart(id);
        Console.WriteLine("Item removed.");
    }

    // View cart
    public void ViewCart()
    {
        var items = cartRepo.GetCartItems();
        if (items.Count == 0)
        {
            Console.WriteLine("Cart is empty.");
            return;
        }

        Console.WriteLine("\nCart Items:");
        foreach (var item in items)
        {
            Console.WriteLine($"{item.product.ProductName} - {item.quantity} x {item.product.Price}");
        }

        Console.WriteLine($"Total: {cartRepo.GetCartTotal()}");
    }

    // Buy entire cart
    public void BuyCart()
    {
        var items = cartRepo.GetCartItems();
        if (items.Count == 0)
        {
            Console.WriteLine("Cart is empty. Nothing to buy!");
            return;
        }

        Console.WriteLine("\nBuying all items in cart:");
        foreach (var item in items)
        {
            Console.WriteLine($"{item.product.ProductName} - {item.quantity} x {item.product.Price}");
            item.product.QuantityInStock -= item.quantity;
        }

        Console.WriteLine($"Total Paid: {cartRepo.GetCartTotal()}");
        cartRepo.ClearCart();
        Console.WriteLine("Purchase successful! Cart is now empty.");
    }

    // Buy single product directly
    public void BuyProduct()
    {
        Console.Write("Enter Product ID to buy: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID!");
            return;
        }

        var product = productRepo.GetProductById(id);
        if (product == null)
        {
            Console.WriteLine("Product not found!");
            return;
        }

        Console.Write("Enter Quantity: ");
        if (!int.TryParse(Console.ReadLine(), out int qty) || qty <= 0)
        {
            Console.WriteLine("Invalid quantity!");
            return;
        }

        if (qty > product.QuantityInStock)
        {
            Console.WriteLine("Insufficient stock!");
            return;
        }

        product.QuantityInStock -= qty;
        decimal total = product.Price * qty;
        Console.WriteLine($"You bought {qty} x {product.ProductName} for {total}.");
    }
}

// Main Program
class Program
{
    static void Main()
    {
        IProductRepository productRepo = new ProductRepository();
        ICartRepository cartRepo = new CartRepository();
        ShoppingCartService service = new ShoppingCartService(productRepo, cartRepo);

        // Add sample products
        productRepo.AddProduct(new Product { ProductId = 1, ProductName = "Backpack", Price = 2000, QuantityInStock = 15 });
        productRepo.AddProduct(new Product { ProductId = 2, ProductName = "Water Bottle", Price = 500, QuantityInStock = 30 });
        productRepo.AddProduct(new Product { ProductId = 3, ProductName = "Notebook", Price = 300, QuantityInStock = 50 });
        productRepo.AddProduct(new Product { ProductId = 4, ProductName = "Pen Pack", Price = 150, QuantityInStock = 40 });
        productRepo.AddProduct(new Product { ProductId = 5, ProductName = "Desk Lamp", Price = 2500, QuantityInStock = 10 });

        while (true)
        {
            Console.WriteLine("\n===== SHOPPING MENU =====");
            Console.WriteLine("1. View Products");
            Console.WriteLine("2. Search Product by Name");
            Console.WriteLine("3. Add to Cart");
            Console.WriteLine("4. Remove from Cart");
            Console.WriteLine("5. View Cart");
            Console.WriteLine("6. Buy Product Directly");
            Console.WriteLine("7. Buy Cart");
            Console.WriteLine("8. Exit");
            Console.Write("Choose option: ");

            if (!int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.WriteLine("Invalid input!");
                continue;
            }

            switch (choice)
            {
                case 1: service.DisplayProducts(); break;
                case 2: service.SearchProductsByName(); break;
                case 3: service.AddToCart(); break;
                case 4: service.RemoveFromCart(); break;
                case 5: service.ViewCart(); break;
                case 6: service.BuyProduct(); break;
                case 7: service.BuyCart(); break;
                case 8: return;
                default: Console.WriteLine("Invalid choice."); break;
            }
        }
    }
}