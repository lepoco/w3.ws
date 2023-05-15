// This Source Code Form is subject to the terms of the GNU GPL-3.0.
// If a copy of the GPL was not distributed with this file, You can obtain one at https://www.gnu.org/licenses/gpl-3.0.en.html.
// Copyright (C) 2022-2023 Leszek Pomianowski and W3.WS.CLI Contributors.
// All Rights Reserved.

namespace W3.WS.Cli.Ratios;

static class BaseResolutions
{
    public static readonly Resolution BakedResolution = new Resolution(1920, 1080);

    public static readonly Resolution[] Common = new Resolution[]
    {
        new(2560, 1080),
        new(3440, 1440),
        new(3840, 1200),
        new(3840, 1600),
        new(5120, 1440),
        new(5120, 2160),
        new(6880, 2880)
    };
}
