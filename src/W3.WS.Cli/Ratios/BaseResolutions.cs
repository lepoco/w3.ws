// This Source Code Form is subject to the terms of the GNU GPL-3.0.
// If a copy of the GPL was not distributed with this file, You can obtain one at https://www.gnu.org/licenses/gpl-3.0.en.html.
// Copyright (C) 2022 Leszek Pomianowski and W3.WS.CLI Contributors.
// All Rights Reserved.

namespace W3.WS.Cli.Ratios;

internal static class BaseResolutions
{
    public static readonly byte[] BakedSequence = new byte[] { 0x39, 0x8E, 0xE3, 0x3F };

    public static readonly Resolution[] Common = new Resolution[]
    {
        new () { Horizontal = 2560, Vertical = 1080 },
        new () { Horizontal = 3440, Vertical = 1440 },
        new () { Horizontal = 3840, Vertical = 1200 },
        new () { Horizontal = 3840, Vertical = 1600 },
        new () { Horizontal = 5120, Vertical = 1440 },
        new () { Horizontal = 5120, Vertical = 2160 },
        new () { Horizontal = 6880, Vertical = 2880 }
    };
}
