
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel;

namespace StudentBazaar.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        // Core user-related
        public DbSet<ApplicationUser> Users { get; set; }

        // Academic structure
        public DbSet<University> Universities { get; set; }
        public DbSet<College> Colleges { get; set; }
        public DbSet<Major> Majors { get; set; }
        public DbSet<StudyYear> StudyYears { get; set; }

        // Product-related
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; } // إضافة: صور المنتج
        public DbSet<Listing> Listings { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        // E-commerce flow
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; } // إضافة: سلة التسوق
                                                                       // This section defines the Fluent API configuration 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ==========================================
            // 🎓 العلاقات الأكاديمية (University / College / Major / StudyYear)
            // ==========================================

            // University → Colleges (Cascade) / Users (Restrict)
            modelBuilder.Entity<University>()
                .HasMany(u => u.Colleges)
                .WithOne(c => c.University)
                .HasForeignKey(c => c.UniversityId)
                .OnDelete(DeleteBehavior.Cascade); // حذف الكليات إذا حُذفت الجامعة

            modelBuilder.Entity<University>()
                .HasMany(u => u.Users)
                .WithOne(u => u.University)
                .HasForeignKey(u => u.UniversityId)
                .OnDelete(DeleteBehavior.Restrict); // منع حذف الجامعة إذا بها مستخدمون

            // College → Users (Restrict) / Majors (Cascade)
            modelBuilder.Entity<College>()
                .HasMany(c => c.Users)
                .WithOne(u => u.College)
                .HasForeignKey(u => u.CollegeId)
                .OnDelete(DeleteBehavior.Restrict); // منع حذف الكلية لو بها طلاب

            modelBuilder.Entity<College>()
                .HasMany(c => c.Majors)
                .WithOne(m => m.College)
                .HasForeignKey(m => m.CollegeId)
                .OnDelete(DeleteBehavior.Cascade); // حذف التخصصات مع الكلية

            // Major → StudyYears (Cascade)
            modelBuilder.Entity<Major>()
                .HasMany(m => m.StudyYears)
                .WithOne(sy => sy.Major)
                .HasForeignKey(sy => sy.MajorId)
                .OnDelete(DeleteBehavior.Cascade);

            // ==========================================
            // 🛍️ علاقات المنتج والبيع (Product / Listing / Rating / Category)
            // ==========================================

            // StudyYear → Products (Cascade)
            modelBuilder.Entity<StudyYear>()
                .HasMany(sy => sy.Products)
                .WithOne(p => p.StudyYear)
                .HasForeignKey(p => p.StudyYearId)
                .OnDelete(DeleteBehavior.Cascade);

            // ProductCategory → Products (Cascade)
            modelBuilder.Entity<ProductCategory>()
                .HasMany(pc => pc.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // Product → Listings / Ratings / Images (Cascade)
            modelBuilder.Entity<Product>()
                .HasMany(p => p.Listings)
                .WithOne(l => l.Product)
                .HasForeignKey(l => l.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.Ratings)
                .WithOne(r => r.Product)
                .HasForeignKey(r => r.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.Images)
                .WithOne(pi => pi.Product)
                .HasForeignKey(pi => pi.ProductId)
                .OnDelete(DeleteBehavior.Cascade); // حذف صور المنتج مع المنتج

            // Listing → Orders (Cascade)
            modelBuilder.Entity<Listing>()
                .HasMany(l => l.Orders)
                .WithOne(o => o.Listing)
                .HasForeignKey(o => o.ListingId)
                .OnDelete(DeleteBehavior.Cascade);

            // Listing → ShoppingCartItems (Restrict)
            modelBuilder.Entity<Listing>()
                .HasMany(l => l.ShoppingCartItems)
                .WithOne(sci => sci.Listing)
                .HasForeignKey(sci => sci.ListingId)
                .OnDelete(DeleteBehavior.Restrict); // منع حذف إعلان في سلة تسوق

            // ==========================================
            // 👤 علاقات المستخدم (User) مع باقي الكيانات
            // ==========================================

            // User → Listings (Seller)
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.ListingsPosted)
                .WithOne(l => l.Seller)
                .HasForeignKey(l => l.SellerId)
                .OnDelete(DeleteBehavior.Restrict);

            // User → Orders (Buyer)
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.OrdersPlaced)
                .WithOne(o => o.Buyer)
                .HasForeignKey(o => o.BuyerId)
                .OnDelete(DeleteBehavior.Restrict);

            // User → Ratings (Restrict)
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.RatingsGiven)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // User → Shipments (Restrict)
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.ShipmentsHandled)
                .WithOne(s => s.Shipper)
                .HasForeignKey(s => s.ShipperId)
                .OnDelete(DeleteBehavior.Restrict);

            // User → ShoppingCartItems (Cascade)
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.ShoppingCartItems)
                .WithOne(sci => sci.User)
                .HasForeignKey(sci => sci.UserId)
                .OnDelete(DeleteBehavior.Cascade); // حذف محتوى السلة لو المستخدم اتحذف

            // ==========================================
            // 🚚 علاقات الشحن (Order / Shipment)
            // ==========================================

            // Order ↔ Shipment (1:1)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Shipment)
                .WithOne(s => s.Order)
                .HasForeignKey<Shipment>(s => s.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // ==========================================
            // 🔒 Unique Constraints (منع التكرار)
            // ==========================================

            // منع تكرار الإيميل
            modelBuilder.Entity<ApplicationUser>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // منع تكرار اسم التصنيف
            modelBuilder.Entity<ProductCategory>()
                .HasIndex(pc => pc.CategoryName)
                .IsUnique();

            // منع تكرار اسم الجامعة
            modelBuilder.Entity<University>()
                .HasIndex(u => u.UniversityName)
                .IsUnique();

            // منع تكرار اسم الكلية داخل نفس الجامعة
            modelBuilder.Entity<College>()
                .HasIndex(c => new { c.CollegeName, c.UniversityId })
                .IsUnique();
        }

        internal void UpdateCategoryAttribute(CategoryAttribute categoryAttribute)
        {
            throw new NotImplementedException();
        }
    }
}