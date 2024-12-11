// See https://aka.ms/new-console-template for more information
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Numerics;
using System.Reflection;

Console.WriteLine("Welcome!");

//List of Assets class
List<Assets> inventory = new List<Assets>(); 

//Load premade assets 
PreloadedAssets();

//Run Program function
Program();

// Display list function
void DisplayList()
{
    // Set padding based on Max Length - (Linq)
    int countryWidth = Math.Max("Country".Length, inventory.Max(p => p.Country.Length) + 5);
    int assetTypeWidth = Math.Max("Asset".Length, inventory.Max(p => p.AssetType.Length) + 5);
    int brandWidth = Math.Max("Brand".Length, inventory.Max(p => p.Brand.Length) + 5);
    int modelWidth = Math.Max("Model".Length, inventory.Max(p => p.Model.Length) + 5);
    int priceWidth = Math.Max("Price (USD)".Length, inventory.Max(p => p.Price.ToString().Length) + 10);
    int localPriceWidth = Math.Max("Price (Local)".Length, inventory.Max(p => p.Price.ToString().Length) + 10);
    int addedWidth = Math.Max("Added".Length, inventory.Max(p => p.Added.ToString("yyyy-MM-dd").Length));

    // Sort and display list - Country - AddedDate
    Console.WriteLine("Inventory (sorterad)");

    List<Assets> sortedInventory = inventory.OrderBy(asset => asset.Country).ThenBy(asset => asset.Added).ToList();

    if (sortedInventory.Count > 0)
    {

        Console.WriteLine(
            "Country".PadRight(countryWidth) +
            "Asset".PadRight(assetTypeWidth) +
            "Brand".PadRight(brandWidth) +
            "Model".PadRight(modelWidth) +
            "Price (USD)".PadRight(priceWidth +4) +
            "Price (Local)".PadRight(localPriceWidth + 4) +
            "Purchase Date".PadRight(addedWidth)
        );

        //Calcuate Local Currency
        foreach (var asset in sortedInventory)
        {
            decimal localPrice = asset.Price;
            string localCurrency = "USD ";

            switch (asset.Country.ToLower())
            {
                case "sweden":
                    localPrice *= 10.5m;
                    localCurrency = "SEK ";
                    break;
                case "germany":
                    localPrice *= 0.91m;
                    localCurrency = "EUR ";
                    break;
                case "usa":
                    localCurrency = "USD ";
                    break;
            }
 
            //Set text color based on date
            if (asset.Added <= DateTime.Now.AddMonths(-36 + 3))
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else if (asset.Added <= DateTime.Now.AddMonths(-36 + 6))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.WriteLine(
                asset.Country.PadRight(countryWidth) +
                asset.AssetType.PadRight(assetTypeWidth) +
                asset.Brand.PadRight(brandWidth) +
                asset.Model.PadRight(modelWidth) +
                "USD " +
                asset.Price.ToString("F2").PadRight(priceWidth) +
                localCurrency +
                localPrice.ToString("F2").PadRight(localPriceWidth) +
                asset.Added.ToString("yyyy-MM-dd").PadRight(addedWidth)
            );
        }
        Console.ForegroundColor = ConsoleColor.White;
    }
    else
    {
        Console.WriteLine("Inga produkter i listan.");
    }

    Console.Write("Tryck på valfri knapp för att fortsätta...");
    Console.ReadLine();
    Program();
}

//Preloaded Assets
void PreloadedAssets()
{
    inventory.Add(new Phone(200, DateTime.Now.AddMonths(-36 + 4), "X3", "Motorola", "Usa", "Smartphone"));
    inventory.Add(new Phone(400, DateTime.Now.AddMonths(-36 + 5), "X3", "Motorola", "Usa", "Smartphone"));
    inventory.Add(new Phone(400, DateTime.Now.AddMonths(-36 + 10), "X2", "Motorola", "Usa", "Smartphone"));
    inventory.Add(new Phone(4500, DateTime.Now.AddMonths(-36 + 6), "Galaxy 10", "Samsung", "Sweden", "Smartphone"));
    inventory.Add(new Phone(4500, DateTime.Now.AddMonths(-36 + 7), "Galaxy 10", "Samsung", "Sweden", "Smartphone"));
    inventory.Add(new Phone(3000, DateTime.Now.AddMonths(-36 + 4), "XPeria 7", "Sony", "Sweden", "Smartphone"));
    inventory.Add(new Phone(3000, DateTime.Now.AddMonths(-36 + 5), "XPeria 7", "Sony", "Sweden", "Smartphone"));
    inventory.Add(new Phone(220, DateTime.Now.AddMonths(-36 + 12), "Brick", "Siemens", "Germany", "Smartphone"));
    inventory.Add(new Computer(100, DateTime.Now.AddMonths(-38), "Desktop 900", "Dell", "Usa", "Laptop"));
    inventory.Add(new Computer(100, DateTime.Now.AddMonths(-37), "Desktop 900", "Dell", "Usa", "Laptop"));
    inventory.Add(new Computer(300, DateTime.Now.AddMonths(-36 + 1), "X100", "Lenovo", "Usa", "Laptop"));
    inventory.Add(new Computer(300, DateTime.Now.AddMonths(-36 + 4), "X200", "Lenovo", "Usa", "Laptop"));
    inventory.Add(new Computer(500, DateTime.Now.AddMonths(-36 + 9), "X300", "Lenovo", "Usa", "Laptop"));
    inventory.Add(new Computer(1500, DateTime.Now.AddMonths(-36 + 7), "Optiplex 100", "Dell", "Sweden", "Laptop"));
    inventory.Add(new Computer(1400, DateTime.Now.AddMonths(-36 + 8), "Optiplex 200", "Dell", "Sweden", "Laptop"));
    inventory.Add(new Computer(1300, DateTime.Now.AddMonths(-36 + 9), "Optiplex 300", "Dell", "Sweden", "Laptop"));
    inventory.Add(new Computer(1600, DateTime.Now.AddMonths(-36 + 14), "ROG 600", "Asus", "Germany", "Laptop"));
    inventory.Add(new Computer(1200, DateTime.Now.AddMonths(-36 + 4), "ROG 500", "Asus", "Germany", "Laptop"));
    inventory.Add(new Computer(1200, DateTime.Now.AddMonths(-36 + 3), "ROG 500", "Asus", "Germany", "Laptop"));
    inventory.Add(new Computer(1300, DateTime.Now.AddMonths(-36 + 2), "ROG 500", "Asus", "Germany", "Laptop"));
}

//Add new Asset function
void AddNewAsset()
{
    Console.WriteLine("Add New Asset");
    Console.Write("Välj land för ny Asset - Sweden (swe), Germany (de) or USA: ");
    string newLand;
    Dictionary<string, string> countryMap = new Dictionary<string, string>
    {
        { "sweden", "Sweden" },
        { "swe", "Sweden" },
        { "germany", "Germany" },
        { "de", "Germany" },
        { "usa", "USA" }
    };

    while (true)
    {
        string newLandInput = Console.ReadLine().ToLower();

        if (newLandInput == "inventory" || newLandInput == "i")
        {
            DisplayList();
          
        }else if (countryMap.TryGetValue(newLandInput, out newLand))
        {
            break; 
        }
        else
        {
            Console.WriteLine("Ogiltigt val. Vänligen välj Sweden (swe), Germany (de) eller USA.");
            Console.Write("Försök igen: ");
        }
    }

    Console.Write("Välj Type för ny Asset - Laptop (lt) eller SmartPhone (sp): ");
    string newAssetType;
    Dictionary<string, string> assetMap = new Dictionary<string, string>
    {
        { "laptop", "Laptop" },
        { "lt", "Laptop" },
        { "smartphone", "SmartPhone" },
        { "sp", "SmartPhone" }
    };

    while (true)
    {
        string newAssetInput = Console.ReadLine().ToLower();

        if (assetMap.TryGetValue(newAssetInput, out newAssetType))
        {
            break;
        }
        else
        {
            Console.WriteLine("Ogiltigt val. Vänligen välj  Laptop (lt) eller SmartPhone (sp)");
            Console.Write("Försök igen: ");
        }
    }

    Console.Write("Ange Brand för ny Asset: ");
    string newBrand;
    while (true)
    {
        newBrand = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newBrand))
        {
            break;
        }
        Console.WriteLine("Ogiltigt, du måste ange ett Brand.");
        Console.Write("Försök igen: ");
    }

    Console.Write("Ange Model för ny Asset: ");
    string newModel;
    while (true)
    {
        newModel = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newModel))
        {
            break;
        }
        Console.WriteLine("Ogiltigt, du måste ange en Model.");
        Console.Write("Försök igen: ");
    }

    Console.Write("Ange Price i $USD för ny Asset: ");
    decimal newPrice;

    while (!decimal.TryParse(Console.ReadLine(), out newPrice))
    {
        Console.WriteLine("Ogiltligt Pris. Ange decimal tal");
        Console.Write("Försök igen: ");
    }

    Console.WriteLine("Lämna blank för idag (enter) eller ange datum (yyyy-MM-dd) för ny Asset");
    DateTime newAdded;

    while (true)
    {
        string dateInput = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(dateInput))
        {
            newAdded = DateTime.Now;
            break;
        }
        else if (DateTime.TryParse(dateInput, out newAdded))
        {
            break;
        }
        else
        {
            Console.WriteLine("Ogiltigt datumformat. Vänligen använd formatet yyyy-MM-dd.");
            Console.Write("Försök igen: ");
        }
    }

    Assets newAsset;
    if (newAssetType == "laptop")
    {
        newAsset = new Computer(newPrice, newAdded, newModel, newBrand, newLand, newAssetType);
    }
    else
    {
        newAsset = new Phone(newPrice, newAdded, newModel, newBrand, newLand, newAssetType);
    }

    inventory.Add(newAsset);
    Console.WriteLine("Ny Asset har lagts till!");
    Program();
}

//Program function
void Program()
{
    Console.WriteLine("Skriv: ");
    Console.WriteLine("- Inventory (i) för att visa Inventory");
    Console.WriteLine("- New (n) för att lägga till en ny Asset");
    var input = Console.ReadLine();

    if (!string.IsNullOrWhiteSpace(input))
    {
        input = input.ToLower();

        if (input == "inventory" || input == "i")
        {
            DisplayList();
        }
        else if (input == "new" || input == "n")
        {
            AddNewAsset();
        }
        else
        {
            Console.WriteLine("Ogiltigt val. Försök igen.");
            Program();
        }
    }
    else
    {
        Console.WriteLine("Ogiltigt val. Försök igen.");
        Program();
    }
}

//Base class for Assets
abstract class Assets
{
    public decimal Price { get; set; }
    public DateTime Added { get; set; }
    public string Model { get; set; }
    public string Brand { get; set; }
    public string Country { get; set; }
    public string AssetType { get; set; }

    // Constructor for Assets
    protected Assets(decimal price, DateTime added, string model, string brand, string country, string assetType)
    {
        Price = price;
        Added = added;
        Model = model;
        Brand = brand;
        Country = country;
        AssetType = assetType;
    }
    public override string ToString()
    {
        return $"{Country}, {AssetType}, {Brand}, {Model}, USD {Price}, Local {Price}, {Added.ToShortDateString()}";
    }

    
}

// Subclass for Computers
class Computer : Assets
{
    public Computer(decimal price, DateTime added, string model, string brand, string country, string assetType)
        : base(price, added, model, brand, country, assetType)
    {
        //pass
    }
}

// Subclass for Phones
class Phone : Assets
{
    public Phone(decimal price, DateTime added, string model, string brand, string country, string assetType)
        : base(price, added, model, brand, country, assetType)
    {
        //pass
    }
}
