namespace DKAC.Models.EntityModel
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DKACDbContext : DbContext
    {
        public DKACDbContext()
            : base("name=DKACDbContext")
        {
        }

        public virtual DbSet<Action> Actions { get; set; }
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Bill> Bills { get; set; }
        public virtual DbSet<BillDetail> BillDetails { get; set; }
        public virtual DbSet<Dish> Dishes { get; set; }
        public virtual DbSet<DishType> DishTypes { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<History> Histories { get; set; }
        public virtual DbSet<Jugment> Jugments { get; set; }
        public virtual DbSet<Menu> Menus { get; set; }
        public virtual DbSet<MenuDetail> MenuDetails { get; set; }
        public virtual DbSet<Modul> Moduls { get; set; }
        public virtual DbSet<Page> Pages { get; set; }
        public virtual DbSet<PermissionAction> PermissionActions { get; set; }
        public virtual DbSet<Register> Registers { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserGroup> UserGroups { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Action>()
                .Property(e => e.ActionName)
                .IsFixedLength();

            modelBuilder.Entity<Action>()
                .Property(e => e.Description)
                .IsFixedLength();

            modelBuilder.Entity<Employee>()
                .Property(e => e.Tel)
                .IsFixedLength();

            modelBuilder.Entity<Modul>()
                .Property(e => e.ModulName)
                .IsFixedLength();

            modelBuilder.Entity<Modul>()
                .Property(e => e.Description)
                .IsFixedLength();

            modelBuilder.Entity<Page>()
                .Property(e => e.PageName)
                .IsFixedLength();

            modelBuilder.Entity<Page>()
                .Property(e => e.Description)
                .IsFixedLength();

            modelBuilder.Entity<Role>()
                .Property(e => e.RoleName)
                .IsFixedLength();

            modelBuilder.Entity<Role>()
                .Property(e => e.Description)
                .IsFixedLength();

            modelBuilder.Entity<UserRole>()
                .Property(e => e.Description)
                .IsFixedLength();
        }
    }
}
