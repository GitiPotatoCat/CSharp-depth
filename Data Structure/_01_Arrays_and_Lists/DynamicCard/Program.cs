using System.Globalization;

namespace DynamicCart;

public class Program
{
    // Indian locale → ₹ symbol + Indian grouping (e.g., ₹1,29,999.00)
    private static readonly CultureInfo IN = new("en-IN");

    // ─────────────────────────────────────────────────────────────
    // ARRAY — fixed-size product catalog
    //   • Size known at compile time, never changes at runtime
    //   • Contiguous memory → O(1) index access, cache-friendly
    //   • Cannot grow or shrink
    // ─────────────────────────────────────────────────────────────
    private readonly (int Id, string Name, double Price)[] _catalog =
    {
        (101, "Mechanical Keyboard",  8499.00),
        (102, "USB-C Hub",            2999.00),
        (103, "Webcam HD",            5499.00),
        (104, "Mouse Pad XL",         1299.00),
        (105, "Wireless Mouse",       1899.00),
        (106, "Laptop Stand",         2499.00),
        (107, "Noise-Cancel Headset", 12999.00),
    };

    // ─────────────────────────────────────────────────────────────
    // LIST — dynamic shopping cart
    //   • Size unknown upfront, changes as user shops
    //   • Backed by an internal array that auto-resizes (4→8→16…)
    //   • O(1) amortized Add, O(n) Remove/Find
    // ─────────────────────────────────────────────────────────────
    private readonly List<(int Id, string Name, double Price, int Qty)> _cart = new();

    static void Main()
    {
        var app = new Program();

        app.ShowCatalog();

        app.AddToCart(101, 1);      // Keyboard
        app.AddToCart(103, 1);      // Webcam
        app.AddToCart(105, 2);      // 2x Mouse
        app.AddToCart(107, 1);      // Headset
        app.AddToCart(105, 1);      // duplicate → qty increments, not duplicated
        app.AddToCart(999, 1);      // not in catalog → rejected

        app.ViewCart();
        app.CartSummary();

        app.UpdateQuantity(105, 3); // set mouse qty to 3
        app.RemoveFromCart(103);    // drop webcam

        app.SearchCatalog("mouse");

        app.SortCartByPrice();
        app.ViewCart();
        app.CartSummary();
    }

    // O(n) — iterate once over catalog
    public void ShowCatalog()
    {
        Console.WriteLine("\n🛍️  Product Catalog  (Array — fixed size)");
        Console.WriteLine(new string('─', 55));
        foreach (var p in _catalog)
            Console.WriteLine($"   [{p.Id}] {p.Name,-22} {p.Price.ToString("C", IN),12}");
        Console.WriteLine(new string('─', 55));
        Console.WriteLine($"   Array length: {_catalog.Length}  (fixed at compile time)\n");
    }

    // O(n) — scan catalog + O(n) check cart for duplicates
    public void AddToCart(int productId, int qty)
    {
        var product = Array.Find(_catalog, p => p.Id == productId);
        if (product == default)
        {
            Console.WriteLine($"⚠️  Product #{productId} not in catalog");
            return;
        }

        int idx = _cart.FindIndex(x => x.Id == productId);
        if (idx >= 0)
        {
            var it = _cart[idx];
            _cart[idx] = (it.Id, it.Name, it.Price, it.Qty + qty);
            Console.WriteLine($"➕ Updated  : {it.Name,-22} qty → {_cart[idx].Qty}");
        }
        else
        {
            _cart.Add((product.Id, product.Name, product.Price, qty));
            Console.WriteLine($"✅ Added    : {product.Name,-22} {product.Price.ToString("C", IN),12} × {qty}");
        }
    }

    // O(n) — FindIndex + RemoveAt (which shifts elements)
    public void RemoveFromCart(int productId)
    {
        int idx = _cart.FindIndex(x => x.Id == productId);
        if (idx < 0) { Console.WriteLine($"⚠️  Item #{productId} not in cart"); return; }

        var removed = _cart[idx];
        _cart.RemoveAt(idx);
        Console.WriteLine($"🗑️  Removed  : {removed.Name}");
    }

    // O(n) — find then mutate in place
    public void UpdateQuantity(int productId, int newQty)
    {
        int idx = _cart.FindIndex(x => x.Id == productId);
        if (idx < 0) { Console.WriteLine($"⚠️  Item #{productId} not in cart"); return; }

        var it = _cart[idx];
        _cart[idx] = (it.Id, it.Name, it.Price, newQty);
        Console.WriteLine($"🔄 Updated  : {it.Name,-22} qty → {newQty}");
    }

    // O(n) — linear search over the array
    public void SearchCatalog(string keyword)
    {
        Console.WriteLine($"\n🔍 Search: \"{keyword}\"");
        var matches = Array.FindAll(_catalog,
            p => p.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase));

        if (matches.Length == 0) { Console.WriteLine("   No matches.\n"); return; }
        foreach (var m in matches)
            Console.WriteLine($"   [{m.Id}] {m.Name,-22} {m.Price.ToString("C", IN),12}");
        Console.WriteLine();
    }

    // O(n log n) — List<T>.Sort uses introsort under the hood
    public void SortCartByPrice()
    {
        _cart.Sort((a, b) => a.Price.CompareTo(b.Price));
        Console.WriteLine("📊 Cart sorted by price (ascending)");
    }

    // O(n) — walk the whole cart to print and total
    public void ViewCart()
    {
        Console.WriteLine("\n🛒 Your Shopping Cart  (List — dynamic)");
        Console.WriteLine(new string('─', 55));

        if (_cart.Count == 0) { Console.WriteLine("   Cart is empty.\n"); return; }

        double total = 0;
        for (int i = 0; i < _cart.Count; i++)
        {
            var it = _cart[i];
            double subtotal = it.Price * it.Qty;
            total += subtotal;
            Console.WriteLine($"   {i + 1}. {it.Name,-22} {it.Price.ToString("C", IN),12} × {it.Qty} = {subtotal.ToString("C", IN),12}");
        }

        Console.WriteLine(new string('─', 55));
        Console.WriteLine($"   Total : {total.ToString("C", IN)}");
        Console.WriteLine($"   Items : {_cart.Sum(x => x.Qty)}  ({_cart.Count} unique)\n");
    }

    // O(1) — Count and Capacity are just property reads
    public void CartSummary()
    {
        Console.WriteLine("📦 Memory Footprint:");
        Console.WriteLine($"   Cart Count    : {_cart.Count}  (items currently in cart)");
        Console.WriteLine($"   Cart Capacity : {_cart.Capacity}  (internal array slots reserved)");
        Console.WriteLine($"   Catalog Length: {_catalog.Length}  (fixed — cannot change)\n");
    }
}