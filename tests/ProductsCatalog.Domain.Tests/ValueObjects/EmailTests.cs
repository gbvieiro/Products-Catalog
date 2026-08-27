using FluentAssertions;
using ProductsCatalog.Domain.ValueObjects;
using Xunit;

namespace ProductsCatalog.Domain.Tests.ValueObjects;

public class EmailTests
{
    [Theory]
    [InlineData("gabriel@example.com", true)]
    [InlineData("not-an-email", false)]
    [InlineData("", false)]
    public void IsValid_ReturnsExpectedResult(string address, bool expected)
    {
        var email = new Email(address);

        email.IsValid().Should().Be(expected);
    }
}
