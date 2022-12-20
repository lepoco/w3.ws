using W3.WS.Cli.Ratios;
using Xunit;

namespace W3.WS.Cli.UnitTests;

public class ResolutionTests
{
    [Fact]
    public void ResolutionCorrectlyCalcsRatio()
    {
        var resolution = new Resolution()
        {
            Horizontal = 3440,
            Vertical = 1440
        };

        var expectedSequence = new byte[] { 0x8E, 0xE3, 0x18, 0x40 };

        Assert.Equal(resolution.GetRatio(), expectedSequence);
    }
}
