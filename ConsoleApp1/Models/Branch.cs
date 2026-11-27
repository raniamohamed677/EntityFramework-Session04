using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace NationalBankManagementSystem.Models;

public class Branch
{
    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public Manager? Manager { get; set; }

    public ICollection<Account> Accounts { get; set; }
        = new List<Account>();
}
