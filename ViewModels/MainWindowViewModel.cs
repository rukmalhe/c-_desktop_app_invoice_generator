/*namespace InvoiceBuilder.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
#pragma warning disable CA1822 // Mark members as static
    public string Greeting => "Welcome to Avalonia!";
#pragma warning restore CA1822 // Mark members as static
}
*/
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using ReactiveUI;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
// Add iTextSharp using directives
using iText.Layout;
using iText.Layout.Element;
using iText.Kernel.Pdf;
//my sql package
using MySql.Data.MySqlClient;
using Avalonia.Controls.Shapes; // Add the MySQL library


namespace InvoiceBuilder.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string _customerName = "";
        private string _customerAddress = "";

        private string _companyName = "";
        private string _companyAddress = "";
        private decimal _invoiceAmount;
        private string _generatedInvoice = "";
        private string _saveMessage = "";
        private string _customerPhoneNumber = "";
        private string _customerId = "";
        private string _companyPhoneNumber = "";
        private string _itemName = "";
        private string _itemDescription = "";
        decimal _itemPrice;


        public string CustomerPhoneNumber
        {
            get => _customerPhoneNumber;
            set => this.RaiseAndSetIfChanged(ref _customerPhoneNumber, value);
        }
        public string CustomerID
        {
            get => _customerId;
            set => this.RaiseAndSetIfChanged(ref _customerId, value);
        }
        public string CustomerName
        {
            get => _customerName;
            set => this.RaiseAndSetIfChanged(ref _customerName, value);
        }

        public string CustomerAddress
        {
            get => _customerAddress;
            set => this.RaiseAndSetIfChanged(ref _customerAddress, value);
        }

        public string CompanyName
        {
            get => _companyName;
            set => this.RaiseAndSetIfChanged(ref _companyName, value);
        }

        public string CompanyAddress
        {
            get => _companyAddress;
            set => this.RaiseAndSetIfChanged(ref _companyAddress, value);
        }
        public string CompanyPhoneNumber
        {
            get => _companyPhoneNumber;
            set => this.RaiseAndSetIfChanged(ref _companyPhoneNumber, value);
        }
        public string SaveMessage
        {
            get => _saveMessage;
            set => this.RaiseAndSetIfChanged(ref _saveMessage, value);
        }

        public decimal InvoiceAmount
        {
            get => _invoiceAmount;
            set => this.RaiseAndSetIfChanged(ref _invoiceAmount, value);
        }

        public string ItemName
        {
            get => _itemName;
            set => this.RaiseAndSetIfChanged(ref _itemName, value);
        }
        public string ItemDescription
        {
            get => _itemDescription;
            set => this.RaiseAndSetIfChanged(ref _itemDescription, value);
        }
        public decimal ItemPrice
        {
            get => _itemPrice;
            set => this.RaiseAndSetIfChanged(ref _itemPrice, value);
        }
        public string GeneratedInvoice
        {
            get => _generatedInvoice;
            set => this.RaiseAndSetIfChanged(ref _generatedInvoice, value);
        }

        public ReactiveCommand<Unit, Unit> SaveInvoice { get; }
        public ReactiveCommand<Unit, Unit> RetrieveCustomerDetailsCommand { get; }
        public ReactiveCommand<Unit, Unit> RetrieveCompanyDetailsCommand { get; }
        public ReactiveCommand<Unit, Unit> RetrieveItemDetailsCommand { get; }

        public MainWindowViewModel()
        {
            SaveInvoice = ReactiveCommand.Create(GenerateInvoice);
            RetrieveCustomerDetailsCommand = ReactiveCommand.Create(RetrieveCustomerDetails);
            RetrieveCompanyDetailsCommand = ReactiveCommand.Create(RetrieveCompanyDetails);
            RetrieveItemDetailsCommand = ReactiveCommand.Create(RetrieveItemDetails);

            // Optionally, you can observe the changes in properties and update the generated invoice accordingly.
            this.WhenAnyValue(
                x => x.CustomerName,
                x => x.CustomerAddress,
                x => x.CompanyName,
                x => x.CompanyAddress,
                x => x.InvoiceAmount,
                x => x.ItemName)
                .Subscribe(_ => ClearSaveMessage());
        }

        private void GenerateInvoice()
        {
            // Perform invoice generation logic here based on the captured details.
            //string invoiceID = $"Invoice_{DateTime.Now:yyyyMMddHHmmss}";
            string invoiceNumber = $"INV_{DateTime.Now:yyyyMMddHHmmss}";
            string fileName = invoiceNumber + ".pdf";

            // Create a PdfWriter object to write to the file
            using (var writer = new PdfWriter(fileName))
            {
                // Create a PdfDocument object
                using (var pdf = new PdfDocument(writer))
                {
                    // Create a Document object for adding elements
                    using (var document = new Document(pdf))
                    {
                        // Add content to the PDF (customize this based on your needs)
                        document.Add(new Paragraph($"TopMax Gardening"));
                        //document.Add(new Line().StrokeLineCap());
                        document.Add(new Paragraph($"Invoice Number : {invoiceNumber}"));
                        document.Add(new Paragraph($"Customer Name: {CustomerName}"));
                        document.Add(new Paragraph($"Customer Address: {CustomerAddress}"));
                        document.Add(new Paragraph($"Invoice Amount: {InvoiceAmount:C}"));

                        // You can add more details to the PDF as needed

                        // Close the document
                        document.Close();
                    }


                    // Insert invoice details into the database
                    MainWindowViewModel.InsertInvoiceIntoDatabase(CustomerName, CustomerAddress, InvoiceAmount, fileName);
                    // Set the save message
                    SaveMessage = "Invoice details saved successfully!";
                }
            }

        }
        private void UpdateGeneratedInvoice()
        {
            // This method can be called when any of the relevant properties change.
            // If you want to update the generated invoice in real-time, implement the logic here.
            // For simplicity, this example updates the invoice when any property changes.
            //GenerateInvoice();
        }
        private void ClearSaveMessage()
        {
            SaveMessage = string.Empty;
        }

        private void RetrieveCustomerDetails()
        {
            // Perform logic to retrieve customer details based on the provided phone number
            // You need to implement the MySQL query to fetch the details using CustomerPhoneNumber
            // For simplicity, let's assume you have a method to fetch details from the database

            // Example method (you need to implement this based on your database structure)
            var customerDetails = RetrieveCustomerDetailsFromDatabase(CustomerPhoneNumber);

            // Update the UI with the retrieved details
            if (customerDetails != null)
            {
                CustomerName = customerDetails.CustomerName;
                CustomerAddress = customerDetails.CustomerAddress;
                CustomerID = customerDetails.CustomerID;
                // Set other relevant properties based on your Customer model
            }
            else
            {
                // Handle the case when customer details are not found
                // For example, display an error message
                SaveMessage = "Customer details not found for the provided phone number.";
            }
        }
        private void RetrieveCompanyDetails()
        {
           
            // Example method (you need to implement this based on your database structure)
            var companyDetails = RetrieveCompanyDetailsFromDatabase(CompanyPhoneNumber);

            // Update the UI with the retrieved details
            if (companyDetails != null)
            {
                CompanyName = companyDetails.CompanyName;
                CompanyAddress = companyDetails.CompanyAddress;
                // Set other relevant properties based on your Customer model
            }
            else
            {
                // Handle the case when customer details are not found
                // For example, display an error message
                SaveMessage = "Company details not found for the provided phone number.";
            }
        }
        private void RetrieveItemDetails()
        {
           
            // Example method (you need to implement this based on your database structure)
            var itemDetails = RetrieveItemDetailsFromDatabase(ItemName, CustomerID);

            // Update the UI with the retrieved details
            if (itemDetails != null)
            {
                ItemName = itemDetails.ItemName;
                ItemDescription = itemDetails.ItemDescription;
                ItemPrice = itemDetails.ItemPrice;
                // Set other relevant properties based on your Customer model
            }
            else
            {
                // Handle the case when customer details are not found
                // For example, display an error message
                SaveMessage = "Item details not found for the provided Customer ID " + CustomerID;
            }
        }
        private static void InsertInvoiceIntoDatabase(string customerName, string customerAddress, decimal invoiceAmount, string fileName)
        {
            // Connection string - replace with your MySQL connection details
            string connectionString = "Server=localhost;Database=topmaxg;User ID=pxuser;Password=pxuser;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Insert data into the "invoices" table
               string query = "INSERT INTO invoices (customer_name, customer_address, invoice_amount, generated_invoice) " +
                              "VALUES (@CustomerName, @CustomerAddress, @InvoiceAmount, @GeneratedInvoice)";

                //string query = "INSERT INTO invoices VALUES (@CustomerName, @CustomerAddress, @InvoiceAmount, @GeneratedInvoice)";
                //string invoiceID = $"Invoice_{DateTime.Now:yyyyMMddHHmmss}";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    //cmd.Parameters.AddWithValue("@Id", invoiceID);
                    cmd.Parameters.AddWithValue("@CustomerName", customerName);
                    cmd.Parameters.AddWithValue("@CustomerAddress", customerAddress);
                    cmd.Parameters.AddWithValue("@InvoiceAmount", invoiceAmount);
                    cmd.Parameters.AddWithValue("@GeneratedInvoice", fileName);
                    
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private Customer RetrieveCustomerDetailsFromDatabase(string phoneNumber)
        {
            // Implement logic to query the database and retrieve customer details based on the phone number
            // You need to write the actual query and data retrieval logic


            // Example: replace this with your actual database query
            string connectionString = "Server=localhost;Database=topmaxg;User ID=pxuser;Password=pxuser;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // 09-1234567890
                //string query = "SELECT * FROM customer WHERE TelNumber1 = @PhoneNumber";
                string query = "select Customer.CustomerID AS CustomerID, Customer.Name AS CustomerName," +
                 "Customer.EmailAddress AS CustomerEmail, Address.Address AS CustomerAddress1," +
                 "Address.AddressLine2 AS CustomerAddress2, Address.AddressLine3 AS CustomerAddress3," +
                 "Address.AddressLine4 AS CustomerAddress4 " +
                 "from Customer inner join Address on Customer.AddressID = Address.AddressID " +
                 "where Customer.TelNumber1 = @PhoneNumber;";


                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Create a Customer object and populate it with retrieved details
                            return new Customer
                            { 
                                CustomerID = reader["CustomerID"].ToString(),
                                CustomerName = reader["CustomerName"].ToString(),
                                CustomerAddress = reader["CustomerAddress1"].ToString(),

                                // Populate other properties based on your Customer model
                            };
                        }
                    }
                }
            }

            return null; // Return null if customer details are not found
        }
        
        private Company RetrieveCompanyDetailsFromDatabase(string phoneNumber2)
        {
    
            // Example: replace this with your actual database query
            string connectionString = "Server=localhost;Database=topmaxg;User ID=pxuser;Password=pxuser;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // 09-1234567890
                //string query = "SELECT * FROM customer WHERE TelNumber1 = @PhoneNumber";
                string query = "select " +
                 "Company.CompanyID AS CompanyID," +
                 "Company.Name AS CompanyName, Company.EmailAddress AS CompanyEmail," +
                 "Address.Address AS CompanyAddress1," +
                 "Company.TelNumber1 AS CompanyTelephone1  " +
                 "from Company inner join Address on Company.AddressID = Address.AddressID " +
                 "where Company.TelNumber1 = @PhoneNumber;";


                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber2);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Create a Customer object and populate it with retrieved details
                            return new Company
                            { 
                                CompanyName = reader["CompanyName"].ToString(),
                                CompanyAddress = reader["CompanyAddress1"].ToString(),
                                // Populate other properties based on your Customer model
                            };
                        }
                    }
                }
            }

            return null; // Return null if customer details are not found
        }

        private Items RetrieveItemDetailsFromDatabase(string itemName, string customerId)
        {
    
            // Example: replace this with your actual database query
            string connectionString = "Server=localhost;Database=topmaxg;User ID=pxuser;Password=pxuser;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // 09-1234567890
                string query = "SELECT * FROM Item WHERE Name like  '%@Name%'";
                
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Name", "%" + itemName + "%");
                    cmd.Parameters.AddWithValue("@CustomerID", customerId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Create a Customer object and populate it with retrieved details
                            return new Items
                            {
                                ItemName = reader["ItemName"] is DBNull ? string.Empty : reader["ItemName"].ToString(),
                                ItemDescription = reader["ItemDescription"] is DBNull ? string.Empty : reader["ItemDescription"].ToString(),
                                ItemPrice = reader["Price"] is DBNull ? 0.0m : Convert.ToDecimal(reader["Price"])

                                // Populate other properties based on your Customer model
                            };
                        }
                    }
                }
            }

            return null; // Return null if customer details are not found
        }
    
    }
    
    
}
