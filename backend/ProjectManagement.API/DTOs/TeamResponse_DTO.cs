public class TeamResponse_DTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    // No GraphQL, podemos devolver a lista de membros já convertida
    public List<UserResponse_DTO> Members { get; set; } = new();
}