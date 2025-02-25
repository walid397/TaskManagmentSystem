using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;




namespace Infrastructure.DbContext
{
    public class Context : IdentityDbContext<User,Role,int>
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {

        }
        public DbSet<TaskItem> TaskItems { get; set; }
    }
}
