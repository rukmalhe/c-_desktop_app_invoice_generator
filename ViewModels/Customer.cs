public class Customer
{
    // Ensure these properties are initialized in the constructor or declared as nullable.
    public string CustomerName { get; set; } = string.Empty; // Initializing with an empty string
    public string CustomerAddress { get; set; } = string.Empty; // Initializing with an empty string
    public string CustomerID { get; set; } = string.Empty; // Initializing with an empty string
    

    // Default constructor
    public Customer()
    {
        //CustomerName = "Test Customer Name";
        //CustomerAddress = "Test Customer Address";

    }
}
