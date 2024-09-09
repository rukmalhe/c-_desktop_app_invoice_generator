public class Items
{
    // Ensure these properties are initialized in the constructor or declared as nullable.
    public string ItemName { get; set; } = string.Empty; // Initializing with an empty string
    public string ItemDescription { get; set; } = string.Empty; // Initializing with an empty string
    public decimal ItemPrice { get; set; } // Initializing with an empty string

    // Default constructor
    public Items()
    {
        ItemName = "Item Name";
        ItemDescription = "Item Description";
        ItemPrice = 0.0m;


    }
}
