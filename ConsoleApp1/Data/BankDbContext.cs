using Microsoft.EntityFrameworkCore;
using NationalBankManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace NationalBankManagementSystem.Data;

public class BankDbContext : DbContext
{
    public DbSet<Branch> Branches => Set<Branch>();
    public DbSet<Manager> Managers => Set<Manager>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<CustomerAccount> CustomerAccounts => Set<CustomerAccount>();
    public DbSet<Transaction> Transactions => Set<Transaction>();

    protected override void OnConfiguring(
        DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=(localdb)\\MSSQLLocalDB;Database=NationalBankDB;Trusted_Connection=True;TrustServerCertificate=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Branch
        modelBuilder.Entity<Branch>()
            .HasKey(b => b.Code);

        modelBuilder.Entity<Branch>()
            .Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(100);

        // Manager
        modelBuilder.Entity<Manager>()
            .HasKey(m => m.Id);

        // Branch - Manager 1:1
        modelBuilder.Entity<Branch>()
            .HasOne(b => b.Manager)
            .WithOne(m => m.Branch)
            .HasForeignKey<Manager>(m => m.BranchCode)
            .OnDelete(DeleteBehavior.Restrict);

        // Branch - Account 1:M
        modelBuilder.Entity<Account>()
            .HasKey(a => a.AccountNumber);

        modelBuilder.Entity<Account>()
            .HasOne(a => a.Branch)
            .WithMany(b => b.Accounts)
            .HasForeignKey(a => a.BranchCode)
            .OnDelete(DeleteBehavior.Restrict);

        // Customer
        modelBuilder.Entity<Customer>()
            .HasKey(c => c.Id);

        modelBuilder.Entity<Customer>()
            .HasIndex(c => c.NationalId)
            .IsUnique();

        // CustomerAccount
        modelBuilder.Entity<CustomerAccount>()
            .HasKey(ca => new
            {
                ca.CustomerId,
                ca.AccountNumber
            });

        // Customer - CustomerAccount
        modelBuilder.Entity<CustomerAccount>()
            .HasOne(ca => ca.Customer)
            .WithMany(c => c.CustomerAccounts)
            .HasForeignKey(ca => ca.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        // Account - CustomerAccount
        modelBuilder.Entity<CustomerAccount>()
            .HasOne(ca => ca.Account)
            .WithMany(a => a.CustomerAccounts)
            .HasForeignKey(ca => ca.AccountNumber)
            .OnDelete(DeleteBehavior.Cascade);

        // Account - Transaction 1:M
        modelBuilder.Entity<Transaction>()
            .HasKey(t => t.TransactionNumber);

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.Account)
            .WithMany(a => a.Transactions)
            .HasForeignKey(t => t.AccountNumber)
            .OnDelete(DeleteBehavior.Cascade);

        // Decimal precision
        modelBuilder.Entity<Account>()
            .Property(a => a.CurrentBalance)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Transaction>()
            .Property(t => t.Amount)
            .HasPrecision(18, 2);

        // Seed Branches
        modelBuilder.Entity<Branch>().HasData(
            new Branch
            {
                Code = "CAI-01",
                Name = "Cairo Main Branch",
                Address = "Cairo",
                PhoneNumber = "01000000001"
            },
            new Branch
            {
                Code = "ALEX-01",
                Name = "Alexandria Branch",
                Address = "Alexandria",
                PhoneNumber = "01000000002"
            }
        );

        // Seed Managers
        modelBuilder.Entity<Manager>().HasData(
            new Manager
            {
                Id = 1,
                FullName = "Ahmed Ali",
                Email = "ahmed@bank.com",
                PhoneNumber = "01111111111",
                HireDate = new DateTime(2020, 1, 1),
                BranchCode = "CAI-01"
            },
            new Manager
            {
                Id = 2,
                FullName = "Sara Mohamed",
                Email = "sara@bank.com",
                PhoneNumber = "01222222222",
                HireDate = new DateTime(2021, 2, 1),
                BranchCode = "ALEX-01"
            }
        );
    }
}