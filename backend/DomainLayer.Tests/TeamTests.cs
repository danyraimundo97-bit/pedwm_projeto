using System;
using Xunit;
using FluentAssertions;
using DomainLayer.Domain.Teams;
using DomainLayer.Domain.Builders;

namespace DomainLayer.Tests.Domain
{
    public class TeamTests
    {
        [Fact]
        public void Team_ShouldStartWithNoMembers()
        {
            var team = new TeamBuilder().WithName("Os Vingadores").Build();

            team.Members.Should().NotBeNull();
            team.Members.Should().BeEmpty();
        }

        [Fact]
        public void Team_ShouldHaveName_WhenCreated()
        {
            var team = new TeamBuilder().WithName("Os Incríveis").Build();
            team.Name.Should().Be("Os Incríveis");
        }
    }
}