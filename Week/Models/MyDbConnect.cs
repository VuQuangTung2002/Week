using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Week.Models;
namespace Week.Models
{
    public class MyDbConnect : DbContext
    {
        public MyDbConnect(DbContextOptions<MyDbConnect> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // string StringConnection = "Server=DESKTOP-BTDQTLC\\SQLEXPRESS;Database= Dotnet23_BTL; Integrated Security=True;TrustServerCertificate=True;";
            // doc thong tin trong file appsetting.json, tra ve doi tuong config
            var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

            string strConnectionString = config.GetConnectionString("MyConnectionString");
            optionsBuilder.UseNpgsql(strConnectionString);
          
        }
        public async Task AddUserWithRole(string username,string email, string password, string Role)
        {
            await Database.ExecuteSqlRawAsync("Call add_user_with_role({0},{1},{2},{3})", username, email, password, Role);
        } 
        // them role vao chuong trinh
        public async Task Add_with_role(int UserId, string NameRole)
        {
            await Database.ExecuteSqlRawAsync("Call Add_with_role({0},{1})", UserId, NameRole);
        }
        // dong nay de lam ket noi csdl Itemuser voi Users la bang CSDL
        public DbSet<ItemUsers> Users { get; set; }
        public DbSet<ItemCategories> Categories { get; set; }
        public DbSet<ItemTasks> Tasks { get; set; }
        public DbSet<ItemUserToken> UserTokens { get; set; }
        public DbSet<ItemRole> Roles { get; set; }
        public DbSet<ItemPosts> Posts { get; set; }
        public DbSet<ItemPermission> Permisson { get; set; }
        public DbSet<ItemRolePermisson> RolePermisson { get; set; }
    }
}
