using ChemSolution_re_API.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChemSolution_re_API.Data
{
    public class DataContext : DbContext
    {
        private readonly IConfiguration Configuration;

        public DbSet<Achievement> Achievements => Set<Achievement>();
        public DbSet<User> Users => Set<User>();
        public DbSet<BlogPost> BlogPosts => Set<BlogPost>();
        public DbSet<Element> Elements => Set<Element>();
        public DbSet<Material> Materials => Set<Material>();
        public DbSet<Request> Requests => Set<Request>();
        public DbSet<ResearchHistory> ResearchHistories => Set<ResearchHistory>();
        public DbSet<ElementValence> ElementValences => Set<ElementValence>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<ElementMaterial> ElementMaterials => Set<ElementMaterial>();

        public DataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to SqlServer database
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ElementValence>().HasKey(x => new { x.ElementId, x.Valence });
            //Start ResearchHistorys
            modelBuilder
                .Entity<User>()
                .HasMany(c => c.Materials)
                .WithMany(s => s.Users)
                .UsingEntity<ResearchHistory>(
            j => j
                .HasOne(pt => pt.Material)
                .WithMany(t => t.ResearchHistories)
                .HasForeignKey(pt => pt.MaterialId),
            j => j
                .HasOne(pt => pt.User)
                .WithMany(p => p.ResearchHistorys)
                .HasForeignKey(pt => pt.UserId),   
            j =>
                {
                    j.HasKey(t => new { t.UserId, t.MaterialId });
                    j.ToTable("ResearchHistorys");
                });
            //End ResearchHistorys

            //Start ElementMaterial
            modelBuilder
                .Entity<Element>()
                .HasMany(c => c.Materials)
                .WithMany(s => s.Elements)
                .UsingEntity<ElementMaterial>(
                j => j
                    .HasOne(pt => pt.Material)
                    .WithMany(t => t.ElementMaterials)
                    .HasForeignKey(pt => pt.MaterialId),
                j => j
                    .HasOne(pt => pt.Element)
                    .WithMany(p => p.ElementMaterials)
                    .HasForeignKey(pt => pt.ElementId),
                j =>
                {
                    j.HasKey(t => new { t.ElementId, t.MaterialId });
                    j.ToTable("ElementMaterials");
                });
            //End ElementMaterial
        }
    }
}