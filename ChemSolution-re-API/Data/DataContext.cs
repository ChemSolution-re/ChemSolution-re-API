using ChemSolution_re_API.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChemSolution_re_API.Data
{
    public class DataContext : DbContext
    {
        private readonly IConfiguration Configuration;

        public DbSet<Status> Status => Set<Status>();
        public DbSet<Achievement> Achievements => Set<Achievement>();
        public DbSet<User> Users => Set<User>();
        public DbSet<BlogPost> BlogPosts => Set<BlogPost>();
        public DbSet<Element> Elements => Set<Element>();
        public DbSet<Material> Materials => Set<Material>();
        public DbSet<MaterialGroup> MaterialGroups => Set<MaterialGroup>();
        public DbSet<Request> Requests => Set<Request>();
        public DbSet<ResearchHistory> ResearchHistories => Set<ResearchHistory>();
        public DbSet<Valence> Valences => Set<Valence>();

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
            modelBuilder.Entity<Valence>().HasData(
                    new Valence { ValenceId = 1 },
                    new Valence { ValenceId = 2 },
                    new Valence { ValenceId = 3 },
                    new Valence { ValenceId = 4 },
                    new Valence { ValenceId = 5 },
                    new Valence { ValenceId = 6 },
                    new Valence { ValenceId = 7 }
                );

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