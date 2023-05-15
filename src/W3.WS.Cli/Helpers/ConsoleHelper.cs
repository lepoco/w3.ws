// This Source Code Form is subject to the terms of the GNU GPL-3.0.
// If a copy of the GPL was not distributed with this file, You can obtain one at https://www.gnu.org/licenses/gpl-3.0.en.html.
// Copyright (C) 2022-2023 Leszek Pomianowski and W3.WS.CLI Contributors.
// All Rights Reserved.

using System;

namespace W3.WS.Cli.Helpers;

static class ConsoleHelper
{
    public static void WriteLog(string message) => WriteLog(message, true);

    public static void WriteLog(string message, bool appendDots)
    {
        Console.ResetColor();

        Console.Write("[");

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("--");
        Console.ResetColor();

        Console.Write("]");
        Console.Write(" " + message);

        if (appendDots)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("...");
            Console.ResetColor();
        }

        Console.WriteLine(String.Empty);
    }

    public static void WriteSuccess(string message)
    {
        Console.ResetColor();

        Console.Write("[");

        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("OK");
        Console.ResetColor();

        Console.Write("]");

        Console.WriteLine(" " + message);
    }

    public static void WriteError(string message)
    {
        Console.ResetColor();

        Console.Write("[");

        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("ER");
        Console.ResetColor();

        Console.Write("]");

        Console.WriteLine(" " + message);
    }
}
