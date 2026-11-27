using Microsoft.EntityFrameworkCore;
using NationalBankManagementSystem.Data;
using NationalBankManagementSystem.Models;

namespace NationalBankManagementSystem;

internal class Program
{
    static void Main(string[] args)
    {
        using BankDbContext db = new BankDbContext();

        while (true)
        {
            Console.Clear();

            Console.WriteLine("========================================");
            Console.WriteLine("       National Bank - Management");
            Console.WriteLine("========================================");
            Console.WriteLine("1) Add a new Customer");
            Console.WriteLine("2) Open a new Account for a Customer");
            Console.WriteLine("3) Update Account Status");
            Console.WriteLine("4) Remove an Account from a Customer");
            Console.WriteLine("5) List all Customers (with accounts)");
            Console.WriteLine("0) Exit");
            Console.WriteLine("----------------------------------------");
            Console.Write("Enter choice: ");

            string? choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        AddCustomer(db);
                        break;

                    case "2":
                        OpenAccount(db);
                        break;

                    case "3":
                        UpdateAccountStatus(db);
                        break;

                    case "4":
                        RemoveAccountFromCustomer(db);
                        break;

                    case "5":
                        ListCustomers(db);
                        break;

                    case "0":
                        Console.WriteLine("Goodbye!");
                        return;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("Invalid choice. Please enter a number from 0 to 5.");
                        Pause();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("An error occurred:");
                Console.WriteLine(ex.Message);
                Pause();
            }
        }
    }

    // =========================================================
    // 1) ADD NEW CUSTOMER
    // =========================================================

    static void AddCustomer(BankDbContext db)
    {
        Console.Clear();

        Console.WriteLine("--- Add New Customer ---");

        Console.Write("Full Name: ");
        string fullName = Console.ReadLine() ?? "";

        if (string.IsNullOrWhiteSpace(fullName))
        {
            Console.WriteLine("Full Name is required.");
            Pause();
            return;
        }

        Console.Write("National ID: ");
        string nationalId = Console.ReadLine() ?? "";

        if (string.IsNullOrWhiteSpace(nationalId))
        {
            Console.WriteLine("National ID is required.");
            Pause();
            return;
        }

        // Check if National ID already exists
        bool nationalIdExists = db.Customers
            .Any(c => c.NationalId == nationalId);

        if (nationalIdExists)
        {
            Console.WriteLine("A customer with this National ID already exists.");
            Pause();
            return;
        }

        Console.Write("Date of Birth (yyyy-MM-dd): ");

        if (!DateTime.TryParse(
                Console.ReadLine(),
                out DateTime dateOfBirth))
        {
            Console.WriteLine("Invalid date.");
            Pause();
            return;
        }

        Console.Write("Email: ");
        string email = Console.ReadLine() ?? "";

        if (string.IsNullOrWhiteSpace(email))
        {
            Console.WriteLine("Email is required.");
            Pause();
            return;
        }

        Console.Write("Phone: ");
        string phoneNumber = Console.ReadLine() ?? "";

        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            Console.WriteLine("Phone number is required.");
            Pause();
            return;
        }

        Console.Write("Address: ");
        string address = Console.ReadLine() ?? "";

        if (string.IsNullOrWhiteSpace(address))
        {
            Console.WriteLine("Address is required.");
            Pause();
            return;
        }

        Console.WriteLine();
        Console.WriteLine("Customer Type:");
        Console.WriteLine("1) Individual");
        Console.WriteLine("2) Business");
        Console.Write("Choice: ");

        string? typeChoice = Console.ReadLine();

        string customerType;

        if (typeChoice == "1")
        {
            customerType = "Individual";
        }
        else if (typeChoice == "2")
        {
            customerType = "Business";
        }
        else
        {
            Console.WriteLine("Invalid customer type.");
            Pause();
            return;
        }

        Customer customer = new Customer
        {
            FullName = fullName,
            NationalId = nationalId,
            DateOfBirth = dateOfBirth,
            Email = email,
            PhoneNumber = phoneNumber,
            Address = address,
            CustomerType = customerType
        };

        db.Customers.Add(customer);
        db.SaveChanges();

        Console.WriteLine();
        Console.WriteLine(
            $"Customer created successfully. CustomerId = {customer.Id}");

        Pause();
    }

    // =========================================================
    // 2) OPEN NEW ACCOUNT
    // =========================================================

    static void OpenAccount(BankDbContext db)
    {
        Console.Clear();

        Console.WriteLine("--- Open New Account ---");

        Console.Write("Account Number: ");
        string accountNumber = Console.ReadLine() ?? "";

        if (string.IsNullOrWhiteSpace(accountNumber))
        {
            Console.WriteLine("Account Number is required.");
            Pause();
            return;
        }

        // Check if account already exists
        bool accountExists = db.Accounts
            .Any(a => a.AccountNumber == accountNumber);

        if (accountExists)
        {
            Console.WriteLine("This account number already exists.");
            Pause();
            return;
        }

        Console.WriteLine();
        Console.WriteLine("Account Type:");
        Console.WriteLine("1) Savings");
        Console.WriteLine("2) Current");
        Console.WriteLine("3) Business");
        Console.Write("Choice: ");

        string? accountTypeChoice = Console.ReadLine();

        string accountType;

        if (accountTypeChoice == "1")
        {
            accountType = "Savings";
        }
        else if (accountTypeChoice == "2")
        {
            accountType = "Current";
        }
        else if (accountTypeChoice == "3")
        {
            accountType = "Business";
        }
        else
        {
            Console.WriteLine("Invalid account type.");
            Pause();
            return;
        }

        Console.Write("Branch Code: ");
        string branchCode = Console.ReadLine() ?? "";

        if (string.IsNullOrWhiteSpace(branchCode))
        {
            Console.WriteLine("Branch Code is required.");
            Pause();
            return;
        }

        Console.Write("Customer Id: ");

        if (!int.TryParse(
                Console.ReadLine(),
                out int customerId))
        {
            Console.WriteLine("Invalid Customer Id.");
            Pause();
            return;
        }

        Console.WriteLine();
        Console.WriteLine("Ownership Role:");
        Console.WriteLine("1) Primary");
        Console.WriteLine("2) CoHolder");
        Console.Write("Choice: ");

        string? ownershipChoice = Console.ReadLine();

        string ownershipType;

        if (ownershipChoice == "1")
        {
            ownershipType = "Primary";
        }
        else if (ownershipChoice == "2")
        {
            ownershipType = "CoHolder";
        }
        else
        {
            Console.WriteLine("Invalid ownership role.");
            Pause();
            return;
        }

        // Check Branch
        Branch? branch = db.Branches
            .FirstOrDefault(b => b.Code == branchCode);

        if (branch == null)
        {
            Console.WriteLine(
                $"Branch '{branchCode}' does not exist.");

            Pause();
            return;
        }

        // Check Customer
        Customer? customer = db.Customers
            .FirstOrDefault(c => c.Id == customerId);

        if (customer == null)
        {
            Console.WriteLine(
                $"Customer with ID {customerId} does not exist.");

            Pause();
            return;
        }

        // Create Account
        Account account = new Account
        {
            AccountNumber = accountNumber,
            AccountType = accountType,
            BranchCode = branchCode,
            CurrentBalance = 0,
            OpeningDate = DateTime.Now,
            AccountStatus = "Active"
        };

        // Create CustomerAccount
        CustomerAccount customerAccount = new CustomerAccount
        {
            CustomerId = customerId,
            AccountNumber = accountNumber,
            OwnershipType = ownershipType,
            OwnershipStartDate = DateTime.Now
        };

        db.Accounts.Add(account);
        db.CustomerAccounts.Add(customerAccount);

        db.SaveChanges();

        Console.WriteLine();
        Console.WriteLine(
            $"Validating branch '{branchCode}' and customer #{customerId}...");

        Console.WriteLine(
            $"Account '{accountNumber}' created and linked to customer {customerId} as {ownershipType} owner.");

        Pause();
    }

    // =========================================================
    // 3) UPDATE ACCOUNT STATUS
    // =========================================================

    static void UpdateAccountStatus(BankDbContext db)
    {
        Console.Clear();

        Console.WriteLine("--- Update Account Status ---");

        Console.Write("Account Number: ");
        string accountNumber = Console.ReadLine() ?? "";

        Console.Write("Customer Id: ");

        if (!int.TryParse(
                Console.ReadLine(),
                out int customerId))
        {
            Console.WriteLine("Invalid Customer Id.");
            Pause();
            return;
        }

        CustomerAccount? customerAccount =
            db.CustomerAccounts
                .Include(ca => ca.Account)
                .FirstOrDefault(ca =>
                    ca.AccountNumber == accountNumber &&
                    ca.CustomerId == customerId);

        if (customerAccount == null)
        {
            Console.WriteLine(
                "Account ownership link was not found.");

            Pause();
            return;
        }

        Console.WriteLine();
        Console.WriteLine("New Status:");
        Console.WriteLine("1) Active");
        Console.WriteLine("2) Closed");
        Console.Write("Choice: ");

        string? statusChoice = Console.ReadLine();

        if (statusChoice == "1")
        {
            customerAccount.Account.AccountStatus = "Active";
        }
        else if (statusChoice == "2")
        {
            customerAccount.Account.AccountStatus = "Closed";
        }
        else
        {
            Console.WriteLine("Invalid status.");
            Pause();
            return;
        }

        db.SaveChanges();

        Console.WriteLine();
        Console.WriteLine(
            $"Status updated to {customerAccount.Account.AccountStatus}.");

        Pause();
    }

    // =========================================================
    // 4) REMOVE ACCOUNT FROM CUSTOMER
    // =========================================================

    static void RemoveAccountFromCustomer(BankDbContext db)
    {
        Console.Clear();

        Console.WriteLine("--- Remove Account From Customer ---");

        Console.Write("Account Number: ");
        string accountNumber = Console.ReadLine() ?? "";

        Console.Write("Customer Id: ");

        if (!int.TryParse(
                Console.ReadLine(),
                out int customerId))
        {
            Console.WriteLine("Invalid Customer Id.");
            Pause();
            return;
        }

        CustomerAccount? customerAccount =
            db.CustomerAccounts
                .FirstOrDefault(ca =>
                    ca.AccountNumber == accountNumber &&
                    ca.CustomerId == customerId);

        if (customerAccount == null)
        {
            Console.WriteLine(
                "Ownership link was not found.");

            Pause();
            return;
        }

        // Delete CustomerAccount link
        db.CustomerAccounts.Remove(customerAccount);

        db.SaveChanges();

        Console.WriteLine();
        Console.WriteLine("Ownership link deleted.");

        // Check if account has another owner
        bool hasOtherOwners =
            db.CustomerAccounts.Any(ca =>
                ca.AccountNumber == accountNumber);

        // If there are no other owners,
        // delete the account itself
        if (!hasOtherOwners)
        {
            Account? account =
                db.Accounts
                    .FirstOrDefault(a =>
                        a.AccountNumber == accountNumber);

            if (account != null)
            {
                db.Accounts.Remove(account);

                db.SaveChanges();

                Console.WriteLine(
                    $"That was the last owner - account '{accountNumber}' was also removed.");
            }
        }

        Pause();
    }

    // =========================================================
    // 5) LIST ALL CUSTOMERS WITH ACCOUNTS
    // =========================================================

    static void ListCustomers(BankDbContext db)
    {
        Console.Clear();

        Console.WriteLine("--- All Customers ---");
        Console.WriteLine();

        // Eager Loading
        List<Customer> customers = db.Customers
            .Include(c => c.CustomerAccounts)
            .ThenInclude(ca => ca.Account)
            .ThenInclude(a => a.Branch)
            .ToList();

        if (customers.Count == 0)
        {
            Console.WriteLine("No customers found.");
            Pause();
            return;
        }

        foreach (Customer customer in customers)
        {
            Console.WriteLine(
                $"#{customer.Id} {customer.FullName} ({customer.CustomerType})");

            if (customer.CustomerAccounts.Count == 0)
            {
                Console.WriteLine("    (no accounts)");
                Console.WriteLine();
                continue;
            }

            foreach (CustomerAccount customerAccount
                     in customer.CustomerAccounts)
            {
                Account account = customerAccount.Account;

                Console.WriteLine(
                    $"    {account.AccountNumber} - " +
                    $"{account.AccountType} - " +
                    $"Balance: {account.CurrentBalance:N2} - " +
                    $"{customerAccount.OwnershipType} - " +
                    $"{account.AccountStatus} - " +
                    $"@ {account.Branch.Name}");
            }

            Console.WriteLine();
        }

        Pause();
    }

    // =========================================================
    // PAUSE
    // =========================================================

    static void Pause()
    {
        Console.WriteLine();
        Console.WriteLine(
            "Press any key to return to the menu...");

        Console.ReadKey();
    }
}