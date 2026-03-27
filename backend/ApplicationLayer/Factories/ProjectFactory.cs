using System;
using System.Collections.Generic;
using DomainLayer.Domain;
using DomainLayer.Domain.Projects;
using DomainLayer.Builders;
using ApplicationLayer.Commands;

namespace ApplicationLayer.Factories
{
    public class ProjectFactory
    {
        private readonly Dictionary<ProjectType, Func<CreateProjectCommand, ProjectBase>> _buildersMap;

        public ProjectFactory()
        {
            // Inicializa o dicionário de builders para cada tipo de projeto.
            _buildersMap = new Dictionary<ProjectType, Func<CreateProjectCommand, ProjectBase>>()
            {
                { ProjectType.Standard, cmd => new ProjectBuilder()
                    .WithTitle(cmd.Title)
                    .WithDates(cmd.StartDate, cmd.EndDate)
                    .WithBudget((int)cmd.AllocatedHours)
                    .ManagedBy(cmd.ManagerId)
                    .Build() },

                { ProjectType.SickLeave, cmd => new SickLeaveBuilder()
                    .WithTitle(cmd.Title)
                    .WithDates(cmd.StartDate, cmd.EndDate)
                    .WithMissedHours(cmd.AllocatedHours)
                    .WithCertificate(cmd.MedicalCertificateId ?? "Sem Atestado")
                    .SetPaid(cmd.IsPaid ?? true)
                    .Build() },

                { ProjectType.Holiday, cmd => new HolidayBuilder()
                    .WithTitle(cmd.Title)
                    .WithDates(cmd.StartDate, cmd.EndDate)
                    .WhichType(Enum.TryParse<HolidayType>(cmd.HolidayType, true, out var ht) ? ht : HolidayType.Optional)
                    .Build() },

                { ProjectType.Training, cmd => new TrainingBuilder()
                    .WithTitle(cmd.Title)
                    .WithDates(cmd.StartDate, cmd.EndDate)
                    .WithDuration((int)cmd.AllocatedHours)
                    .WhichCourse(cmd.CourseName ?? "Geral")
                    .Build() }
            };
        }

        // Método para criar um projeto utilizando o dicionário de builders
        public ProjectBase CreateFromCommand(CreateProjectCommand cmd)
        {
            if (_buildersMap.TryGetValue(cmd.Type, out var builderFunc))
            {
                return builderFunc(cmd);
            }

            throw new ArgumentException($"Tipo de projeto '{cmd.Type}' desconhecido.");
        }
    }
}