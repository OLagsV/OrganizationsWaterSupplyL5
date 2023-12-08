using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OrganizationsWaterSupplyL4.Models;

namespace OrganizationsWaterSupplyL4.Data;

public class UsersDbContext : IdentityDbContext<User>
{
    public UsersDbContext(DbContextOptions<UsersDbContext> options)
        : base(options)
    {
    }
}
