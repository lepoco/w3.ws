// This Source Code Form is subject to the terms of the GNU GPL-3.0.
// If a copy of the GPL was not distributed with this file, You can obtain one at https://www.gnu.org/licenses/gpl-3.0.en.html.
// Copyright (C) 2022 Leszek Pomianowski and W3.WS.CLI Contributors.
// All Rights Reserved.

using System;

namespace W3.WS.Cli.Binary;

public class BinaryContainer
{
    public byte[] Content { get; }

    public BinaryContainer(byte[] content)
    {
        Content = content;
    }

    public int ReplaceAll(byte[] search, byte[] replace)
    {
        var occurences = 0;

        if (search.Length != replace.Length)
            throw new Exception("Binary arrays must be of the same length.");

        for (int i = 0; i < Content.Length; i++)
        {
            // If first element matches
            if (Content[i] != search[0])
                continue;

            if (Content.Length < i + search.Length)
                continue;

            var match = true;

            for (int j = 0; j < search.Length; j++)
            {
                if (Content[i + j] != search[j])
                    match = false;
            }

            if (!match)
                continue;

            for (int j = 0; j < replace.Length; j++)
                Content[i + j] = replace[j];

            occurences++;
        }

        return occurences;
    }
}
