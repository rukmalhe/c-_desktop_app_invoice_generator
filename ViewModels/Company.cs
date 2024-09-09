public class Company
{
    // Ensure these properties are initialized in the constructor or declared as nullable.
    public string CompanyName { get; set; } = string.Empty; // Initializing with an empty string
    public string CompanyAddress { get; set; } = string.Empty; // Initializing with an empty string
    // Add other properties as needed

    // Default constructor
    public Company()
    {
        CompanyName = "Test Company Name";
        CompanyAddress = "Test Company Address";

    }
}
