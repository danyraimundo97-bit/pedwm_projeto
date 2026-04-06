namespace DomainLayer.Domain.Users
{
    /// <summary>Stable identity for the seeded super-user (persisted in <c>Users</c>).</summary>
    public static class SuperUser
    {
        public static readonly Guid Id = Guid.Parse("00000000-0000-0000-0000-000000000001");

        public const string Name = "Super Admin";

        public const string Email = "superuser@localhost";
    }
}
