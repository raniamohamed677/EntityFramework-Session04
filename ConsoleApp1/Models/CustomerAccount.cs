using System;
using System.Collections.Generic;
using System.Text;

namespace NationalBankManagementSystem.Models;

public class CustomerAccount
{
    public int CustomerId { get; set; }

    public string AccountNumber { get; set; } = null!;

    public string OwnershipType { get; set; } = null!;

    public DateTime OwnershipStartDate { get; set; }

    public Customer Customer { get; set; } = null!;

    public Account Account { get; set; } = null!;
}