using Cake.MinVer.Tests.Support;
using FluentAssertions;
using Xunit;

namespace Cake.MinVer.Tests
{
    public sealed class MinVerGlobalToolTests
    {
        [Fact]
        public void Should_Not_Add_Any_Default_Arguments()
        {
            var fixture = new MinVerGlobalToolFixture();
            var result = fixture.Run();

            result.Args.Should().BeEmpty();
        }
    }
}
