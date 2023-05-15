// This Source Code Form is subject to the terms of the GNU GPL-3.0.
// If a copy of the GPL was not distributed with this file, You can obtain one at https://www.gnu.org/licenses/gpl-3.0.en.html.
// Copyright (C) 2022 Leszek Pomianowski and W3.WS.CLI Contributors.
// All Rights Reserved.

using W3.WS.Cli.Binary;
using Xunit;

namespace W3.WS.Cli.UnitTests;

public class BinaryContainerTests
{
    [Fact]
    public void ReplacementWorksOnSingleOccurence()
    {
        var binaryContents = new byte[] { 0xFF, 0x00, 0x01, 0x02, 0x03, 0xFF, 0xFF, 0x00, };
        var searchedValue = new byte[] { 0x02, 0x03, 0xFF };
        var replaceValue = new byte[] { 0xAA, 0xAA, 0xAA };

        var expectedOutput = new byte[] { 0xFF, 0x00, 0x01, 0xAA, 0xAA, 0xAA, 0xFF, 0x00, };

        var container = new BinaryContainer(binaryContents);
        container.ReplaceAll(searchedValue, replaceValue);

        Assert.Equal(container.Content, expectedOutput);
    }

    [Fact]
    public void ReplacementAfterWorksOnSingleOccurence()
    {
        var binaryContents = new byte[]
        {
            0xFF,
            0xB0,
            0xBC,
            0xC2,
            0x03,
            0xFF,
            0xFF,
            0x00,
            0xBE,
            0xAE,
            0xBB
        };
        var searchedValue = new byte[] { 0xBC, 0xC2, 0x03 };
        var replaceValue = new byte[] { 0xAA, 0xAA, 0xAA };

        var expectedOutput = new byte[]
        {
            0xFF,
            0xB0,
            0xBC,
            0xC2,
            0x03,
            0xAA,
            0xAA,
            0xAA,
            0xBE,
            0xAE,
            0xBB
        };

        var container = new BinaryContainer(binaryContents);
        container.ReplaceAfter(searchedValue, replaceValue);

        Assert.Equal(container.Content, expectedOutput);
    }
}
