using ApplicationLayer.Commands;
using DomainLayer.Domain.Projects;

namespace ApplicationLayer.Factories
{
    /// <summary>Maps application commands to domain aggregates (use-case construction).</summary>
    public static class ProjectFromCommandFactory
    {
        public static ProjectBase Create(CreateProjectCommand cmd)
        {
            return cmd.Type switch
            {   
                ProjectType.Standard => Project.Builder()
                    .WithTitle(cmd.Title)
                    .WithDates(cmd.StartDate, cmd.EndDate)
                    .WithBudget((int)cmd.AllocatedHours)
                    .ManagedBy(cmd.ManagerId) //TODO: Rever
                    .ForTeam(cmd.TeamId) //TODO: REVER
                    .WithClientName(cmd.ClientName ?? string.Empty)
                    .Build(),

                ProjectType.SickLeave => SickLeave.Builder()
                    .WithTitle(cmd.Title)
                    .WithDates(cmd.StartDate, cmd.EndDate)
                    .WithMissedHours(cmd.AllocatedHours)
                    .WithCertificate(cmd.MedicalCertificateId ?? "Sem Atestado")
                    .SetPaid(cmd.IsPaid ?? true)
                    .Build(),

                ProjectType.Holiday => Holiday.Builder()
                    .WithTitle(cmd.Title)
                    .WithDates(cmd.StartDate, cmd.EndDate)
                    .WhichType(cmd.HolidayType.GetValueOrDefault())
                    .Build(),

                ProjectType.Training => Training.Builder()
                    .WithTitle(cmd.Title)
                    .WithDates(cmd.StartDate, cmd.EndDate)
                    .WithDuration(cmd.AllocatedHours)
                    .WhichCourse(cmd.CourseName ?? "Geral")
                    .WithCertificationLink(cmd.CertificationLink ?? string.Empty)
                    .Build(),

                _ => throw new ArgumentException($"Tipo de projeto '{cmd.Type}' desconhecido."),
            };
        }
    }
}
