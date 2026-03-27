using ApplicationLayer.Commands;
using ApplicationLayer.Factories;
using ApplicationLayer.Repositories;
using ApplicationLayer.Strategy;
using DomainLayer.Domain;
using DomainLayer.Domain.Projects;
using System.Threading.Tasks;

namespace ApplicationLayer.Handlers
{
    public class CreateProjectHandler
    {
        private readonly ProjectFactory _factory;
        private readonly IProjectRepository _repository;
        private readonly NotificationSender _notificationSender;

        // Injetar as dependências que o Handler precisa
        public CreateProjectHandler(
            ProjectFactory factory,
            IProjectRepository repository,
            NotificationSender notificationSender)
        {
            _factory = factory;
            _repository = repository;
            _notificationSender = notificationSender;
        }

        public async Task<ProjectBase> HandleAsync(CreateProjectCommand command)
        {
            // Criar o projeto (Usa a Factory e o Builder)
            var project = _factory.CreateFromCommand(command);

            //TODO: Criar service e passar a lógica de criação para lá, para manter o Handler mais limpo e focado apenas em orquestrar as chamadas
            //TODO: Dentro do service, poderíamos adicionar validações adicionais (ex: verificar se as datas são válidas, se o gestor existe, etc) e lançar exceções específicas para cada tipo de erro, que o Handler poderia capturar e tratar de forma adequada (ex: devolver mensagens de erro claras para o cliente)
            //TODO: Poderíamos também adicionar logging detalhado dentro do service para acompanhar o processo de criação do projeto (ex: início, validações, sucesso, falhas)
            //TODO: Poderíamos implementar uma abordagem de Domain Events, onde o projeto emitiria um evento "ProjectCreated" após ser criado, e um handler separado para esse evento seria responsável por enviar a notificação, desacoplando ainda mais as responsabilidades

            // Guardar na Base de Dados (Repository)
            await _repository.SaveAsync(project);

            // Preparar a notificação
            var user = new User { Id = command.ManagerId, Name = "Gestor do Projeto" };
            var notif = new Notification
            {
                UserId = user.Id,
                Type = NotificationType.Info,
                Message = $"O projeto '{project.Title}' foi criado e guardado com sucesso!"
            };

            // Enviar a notificação (Strategy)
            await _notificationSender.DeliverAsync(user, notif);

            return project; // Devolve o projeto criado
        }
    }
}