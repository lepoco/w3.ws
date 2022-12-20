// This Source Code Form is subject to the terms of the GNU GPL-3.0.
// If a copy of the GPL was not distributed with this file, You can obtain one at https://www.gnu.org/licenses/gpl-3.0.en.html.
// Copyright (C) 2022 Leszek Pomianowski and W3.WS.CLI Contributors.
// All Rights Reserved.

using System.IO;
using System.Threading.Tasks;

namespace W3.WS.Cli.Binary;

internal static class BinaryParser
{
    public static async Task<BinaryContainer?> ReadAsync(string path)
    {
        if (!File.Exists(path))
            return null;

        byte[] contents;

        using FileStream stream = File.Open(path, FileMode.Open);

        contents = new byte[stream.Length];
        await stream.ReadAsync(contents, 0, (int)stream.Length);

        return new BinaryContainer(contents);
    }
}
