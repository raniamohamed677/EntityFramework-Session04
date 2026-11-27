using System;
using System.Collections.Generic;
using System.Text;

namespace NationalBankManagementSystem.Models;

public class Customer
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string NationalId { get; set; } = null!;

    public DateTime DateOfBirth { get; set; }

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string CustomerType { get; set; } = null!;

    public ICollection<CustomerAccount> CustomerAccounts { get; set; }
        = new List<CustomerAccount>();
}
