using System;
using Xunit;
using FluentAssertions;
using DomainLayer.Domain.Users;
using DomainLayer.Domain.Builders;

namespace DomainLayer.Tests.Domain
{
    public class UserTests
    {
        [Fact]
        public void User_ShouldBeCreatedCorrecty()
        {
            var user = new UserBuilder().WithName("Teste").WithEmail("a@a.com").Build();
            user.Name.Should().Be("Teste");
        }

        [Fact]
        public void User_ShouldBeCreated_WithValidEmail()
        {
            var user = new UserBuilder()
                .WithName("Carlos")
                .WithEmail("carlos@teste.com")
                .Build();

            user.Email.Should().Contain("@");
            user.Name.Should().Be("Carlos");
        }
    }
}