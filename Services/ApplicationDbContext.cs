using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Van_Authentication.Models;

namespace Van_Authentication.Services
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {       
        }

        public DbSet<Part> Parts { get; set; }
        public DbSet<Robot> Robots { get; set; }
        public DbSet<Weld> Welds { get; set; }
        public DbSet<RobotWeld> RobotWelds { get; set; }
        public DbSet<PartWeld> PartWelds { get; set; }
        public DbSet<Defect> Defects { get; set; }
        public DbSet<Audit> Audits { get; set; }
        public DbSet<WeldConcern> WeldConcerns { get; set; }
        public DbSet<Tcp> Tcps { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);

            var manager = new IdentityRole("manager");
            manager.NormalizedName = "manager";

            var auditor = new IdentityRole("auditor");
            auditor.NormalizedName = "auditor";

            var supervisor = new IdentityRole("supervisor");
            supervisor.NormalizedName = "supervisor";

            var coordinator = new IdentityRole("coordinator");
            coordinator.NormalizedName = "coordinator";

            builder.Entity<IdentityRole>().HasData(manager, auditor, supervisor, coordinator);
            builder.Entity<Part>().ToTable("Part").HasKey(p => p.PartID);
            builder.Entity<Robot>().ToTable("Robot").HasKey(r => r.RobotID);
            builder.Entity<Weld>().ToTable("Weld").HasKey(w => new { w.WeldID });
            builder.Entity<RobotWeld>().ToTable("RobotWeld").HasKey(r => new { r.RobotID, r.WeldID });
            builder.Entity<RobotWeld>().HasOne(c => c.Robot).WithMany(d => d.RobotWelds).HasForeignKey(e => e.RobotID);
            builder.Entity<PartWeld>().ToTable("PartWeld").HasKey(pw => new {pw.PartID, pw.WeldID});
            builder.Entity<WeldConcern>().ToTable("WeldAudit").HasKey(wa => wa.WelConcernID);
        }
    }
}
