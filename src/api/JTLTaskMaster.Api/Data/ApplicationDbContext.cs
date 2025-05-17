// In src/api/JTLTaskMaster.Api/Data/ApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;

namespace JTLTaskMaster.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Hier kommen später Ihre DbSet<> Properties hinzu
    }
}
