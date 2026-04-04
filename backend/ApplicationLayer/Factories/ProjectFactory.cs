using ApplicationLayer.Commands;
using DomainLayer.Domain.Projects;

namespace ApplicationLayer.Factories
{
    /// <summary>Maps <see cref="CreateProjectCommand"/> to domain aggregates via fluent builders.</summary>
    public static class ProjectFactory
    {
        public static ProjectBase Create(CreateProjectCommand cmd)
        {
            return cmd.Type switch
            {
                ProjectType.Standard => CreateStandard(cmd),
                ProjectType.SickLeave => CreateSickLeave(cmd),
                ProjectType.Holiday => CreateHoliday(cmd),
                ProjectType.Training => CreateTraining(cmd),
                _ => throw new ArgumentException($"Tipo de projeto '{cmd.Type}' desconhecido."),
            };
        }

        private static ProjectBase CreateStandard(CreateProjectCommand cmd)
        {
            if (cmd.ManagerId is null || cmd.ManagerId == Guid.Empty)
            {
                throw new ArgumentException("Projetos standard requerem um gestor (ManagerId) válido.");
            }

            return Project.Builder()
                .WithTitle(cmd.Title)
                .WithDates(cmd.StartDate, cmd.EndDate)
                .WithBudget((int)cmd.AllocatedHours)
                .ManagedBy(cmd.ManagerId)
                .ForTeam(cmd.TeamId)
                .WithClientName(cmd.ClientName ?? string.Empty)
                .Build();
        }

        private static ProjectBase CreateSickLeave(CreateProjectCommand cmd)
        {
            return SickLeave.Builder()
                .WithTitle(cmd.Title)
                .WithDates(cmd.StartDate, cmd.EndDate)
                .WithMissedHours(cmd.AllocatedHours)
                .WithCertificate(cmd.MedicalCertificateId ?? "Sem Atestado")
                .SetPaid(cmd.IsPaid ?? true)
                .Build();
        }

        private static ProjectBase CreateHoliday(CreateProjectCommand cmd)
        {
            return Holiday.Builder()
                .WithTitle(cmd.Title)
                .WithDates(cmd.StartDate, cmd.EndDate)
                .WhichType(cmd.HolidayType ?? HolidayType.Optional)
                .Build();
        }

        private static ProjectBase CreateTraining(CreateProjectCommand cmd)
        {
            return Training.Builder()
                .WithTitle(cmd.Title)
                .WithDates(cmd.StartDate, cmd.EndDate)
                .WithDuration(cmd.AllocatedHours)
                .WhichCourse(cmd.CourseName ?? "Geral")
                .WithCertificationLink(cmd.CertificationLink ?? string.Empty)
                .Build();
        }
    }
}
