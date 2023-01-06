// This Source Code Form is subject to the terms of the GNU GPL-3.0.
// If a copy of the GPL was not distributed with this file, You can obtain one at https://www.gnu.org/licenses/gpl-3.0.en.html.
// Copyright (C) 2022 Leszek Pomianowski and W3.WS.CLI Contributors.
// All Rights Reserved.

using W3.WS.Cli.Ratios;
using Xunit;

namespace W3.WS.Cli.UnitTests;

public class ResolutionTests
{
    [Fact]
    public void ResolutionCorrectlyCalcsRatio()
    {
        var resolution = new Resolution() { Horizontal = 3440, Vertical = 1440 };

        var expectedSequence = new byte[] { 0x8E, 0xE3, 0x18, 0x40 };

        Assert.Equal(resolution.GetRatio(), expectedSequence);
    }
}
