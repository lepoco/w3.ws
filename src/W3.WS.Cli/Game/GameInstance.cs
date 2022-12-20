// This Source Code Form is subject to the terms of the GNU GPL-3.0.
// If a copy of the GPL was not distributed with this file, You can obtain one at https://www.gnu.org/licenses/gpl-3.0.en.html.
// Copyright (C) 2022 Leszek Pomianowski and W3.WS.CLI Contributors.
// All Rights Reserved.

using W3.WS.Cli.Binary;

namespace W3.WS.Cli.Game;

internal class GameInstance
{
    public GameInstanceType Type { get; set; }

    public string Path { get; set; }

    public BinaryContainer? Container { get; set; }

    public GameInstance(GameInstanceType type, string path)
    {
        Type = type;
        Path = path;
    }
}
