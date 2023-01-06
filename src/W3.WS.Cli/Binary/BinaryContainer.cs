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

    public int ReplaceAfter(byte[] search, byte[] replacement)
    {
        var occurrences = 0;

        if (search.Length < 1 || replacement.Length < 1)
            throw new Exception("Binary arrays must be non negative.");

        for (int i = 0; i < Content.Length; i++)
        {
            // If first element matches
            if (Content[i] != search[0])
                continue;

            // If enough left
            if (Content.Length < i + search.Length)
                continue;

            var match = true;

            // Whether is matched
            for (int j = 0; j < search.Length; j++)
            {
                if (Content[i + j] != search[j])
                    match = false;
            }

            if (!match)
                continue;

            // i as content position plus search length
            var contentStartPosition = i + search.Length;

            // Replace
            for (int j = 0; j < replacement.Length; j++)
                Content[contentStartPosition + j] = replacement[j];

            // Counter
            occurrences++;
        }

        return occurrences;
    }

    public int ReplaceAll(byte[] search, byte[] replacement)
    {
        var occurrences = 0;

        if (search.Length < 1 || replacement.Length < 1)
            throw new Exception("Binary arrays must be non negative.");

        if (search.Length != replacement.Length)
            throw new Exception("Binary arrays must be of the same length.");

        for (int i = 0; i < Content.Length; i++)
        {
            // If first element matches
            if (Content[i] != search[0])
                continue;

            // If enough left
            if (Content.Length < i + search.Length)
                continue;

            var match = true;

            // Whether is matched
            for (int j = 0; j < search.Length; j++)
            {
                if (Content[i + j] != search[j])
                    match = false;
            }

            if (!match)
                continue;

            // Replace
            for (int j = 0; j < replacement.Length; j++)
                Content[i + j] = replacement[j];

            // Counter
            occurrences++;
        }

        return occurrences;
    }
}
