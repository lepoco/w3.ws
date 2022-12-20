// This Source Code Form is subject to the terms of the GNU GPL-3.0.
// If a copy of the GPL was not distributed with this file, You can obtain one at https://www.gnu.org/licenses/gpl-3.0.en.html.
// Copyright (C) 2022 Leszek Pomianowski and W3.WS.CLI Contributors.
// All Rights Reserved.

using System;
using System.Collections.Generic;
using System.IO;

namespace W3.WS.Cli.Game;

internal static class ExecutableLocator
{
    public static IEnumerable<string> GetLocations()
    {
        var workingDirectory = Environment.CurrentDirectory;

        if (File.Exists(Path.Combine(workingDirectory, @"bin\x64\witcher3.exe")))
            return FetchExecutables(workingDirectory);

        if (File.Exists(Path.Combine(workingDirectory, @"witcher3.exe")))
            return FetchExecutables(Path.Combine(workingDirectory, @"..\..\"));

        var locations = new string[]
        {
            GameLocations.BaseGogLocation,
            GameLocations.WildGogLocation,
            GameLocations.GotyGogLocation,
            GameLocations.BaseSteamLocation,
            GameLocations.WildSteamLocation,
            GameLocations.GotySteamLocation
        };

        foreach (var location in locations)
            if (Directory.Exists(location))
                return FetchExecutables(location);

        return Array.Empty<string>();
    }

    private static IEnumerable<string> FetchExecutables(string basePath)
    {
        var x64Path = Path.Combine(basePath, @"bin\x64\witcher3.exe");
        var x64Dx12Path = Path.Combine(basePath, @"bin\x64_dx12\witcher3.exe");

        if (!File.Exists(x64Path))
            return Array.Empty<string>();

        if (File.Exists(x64Dx12Path))
            return new string[]
            {
                x64Path,
                x64Dx12Path
            };

        return new string[]
        {
            x64Path
        };
    }
}
