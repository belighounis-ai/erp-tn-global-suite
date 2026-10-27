using ERP.TN.Application.Services;
using ERP.TN.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP.TN.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Country> Countries => Set<Country>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<TaxRule> TaxRules => Set<TaxRule>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<ChartOfAccount> ChartOfAccounts => Set<ChartOfAccount>();
    public DbSet<Journal> Journals => Set<Journal>();
    public DbSet<Voucher> Vouchers => Set<Voucher>();
    public DbSet<VoucherLine> VoucherLines => Set<VoucherLine>();
    public DbSet<BankAccount> BankAccounts => Set<BankAccount>();
    public DbSet<AppUser> AppUsers => Set<AppUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Code).IsRequired();
            entity.Property(c => c.Name).IsRequired();
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name).IsRequired();
            entity.Property(c => c.CountryCode).IsRequired();
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name).IsRequired();
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Name).IsRequired();
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Code).IsRequired();
            entity.Property(p => p.Name).IsRequired();
        });

        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Code).IsRequired();
            entity.Property(a => a.Name).IsRequired();
        });

        modelBuilder.Entity<TaxRule>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Code).IsRequired();
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FirstName).IsRequired();
            entity.Property(e => e.LastName).IsRequired();
        });

        modelBuilder.Entity<ChartOfAccount>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Code).IsRequired();
            entity.Property(a => a.Name).IsRequired();
        });

        modelBuilder.Entity<Journal>(entity =>
        {
            entity.HasKey(j => j.Id);
            entity.Property(j => j.Code).IsRequired();
        });

        modelBuilder.Entity<Voucher>(entity =>
        {
            entity.HasKey(v => v.Id);
            entity.Property(v => v.Number).IsRequired();
        });

        modelBuilder.Entity<VoucherLine>(entity =>
        {
            entity.HasKey(v => v.Id);
        });

        modelBuilder.Entity<BankAccount>(entity =>
        {
            entity.HasKey(b => b.Id);
            entity.Property(b => b.Name).IsRequired();
        });

        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Username).IsRequired();
            entity.Property(u => u.PasswordHash).IsRequired();
        });

        base.OnModelCreating(modelBuilder);
    }
}
