using Microsoft.EntityFrameworkCore;
using DomainLayer.Domain.Projects;
using DomainLayer.Domain.Tasks;
using DomainLayer.Domain.Users;
using DomainLayer.Domain.Teams;

namespace InfrastructureLayer.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Tabelas para os tipos base, que vão armazenar todas as entidades derivadas
        public DbSet<ProjectBase> Projects { get; set; }
        public DbSet<TaskBase> Tasks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Team> Teams { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // TPH (Table-Per-Hierarchy) PARA PROJETOS
            modelBuilder.Entity<ProjectBase>()
                .HasDiscriminator<ProjectType>("ProjectType") // Coluna discriminadora "ProjectType"
                .HasValue<Project>(ProjectType.Standard)
                .HasValue<SickLeave>(ProjectType.SickLeave)
                .HasValue<Holiday>(ProjectType.Holiday)
                .HasValue<Training>(ProjectType.Training);

            // TPH (Table-Per-Hierarchy) PARA TAREFAS
            modelBuilder.Entity<TaskBase>()
                .HasDiscriminator<TaskType>("TaskType") // Coluna discriminadora "TaskType"
                .HasValue<BugTask>(TaskType.Bug)
                .HasValue<FeatureTask>(TaskType.Feature);
        }
    }
}