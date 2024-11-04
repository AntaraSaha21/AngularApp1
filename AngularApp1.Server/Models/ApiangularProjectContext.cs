using AngularApp1.Server.Models;
using Microsoft.EntityFrameworkCore;

public class ApiangularProjectContext : DbContext
{
    public ApiangularProjectContext(DbContextOptions<ApiangularProjectContext> options)
        : base(options)
    {
    }

    public DbSet<Products> Products { get; set; }
}
