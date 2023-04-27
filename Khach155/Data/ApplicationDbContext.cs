using Khach155.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Khach155.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<BangMain> BangMain { get; set; }
        public DbSet<LuuTruMua> LuuTruMua { get; set; }
        public DbSet<DataUser> DataUser { get; set; }

    }
}
