// This Source Code Form is subject to the terms of the GNU GPL-3.0.
// If a copy of the GPL was not distributed with this file, You can obtain one at https://www.gnu.org/licenses/gpl-3.0.en.html.
// Copyright (C) 2022 Leszek Pomianowski and W3.WS.CLI Contributors.
// All Rights Reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using W3.WS.Cli.Binary;
using W3.WS.Cli.Helpers;
using W3.WS.Cli.Ratios;

namespace W3.WS.Cli;

internal class App
{
    private const string AppName = "WITCHER 3 WIDE SCREEN PATCHER";

    private int _selectedResolution = 0;

    private IDictionary<string, BinaryContainer> _executables = new Dictionary<string, BinaryContainer>();

    public async Task<int> RunAsync(string[] args)
    {
        Console.ResetColor();

        WriteHello();

        if (!SelectResolution())
            return 1;

        Console.Write("Are you sure you want to continue? [Y]es / [N]o");
        Console.WriteLine(String.Empty);
        var readedKey = Console.ReadKey();
        Console.WriteLine(String.Empty);

        if (readedKey.Key != ConsoleKey.Y)
            return 0;

        ConsoleHelper.WriteLog($"Reading game files");

        if (!await ReadGamesAsync())
            return 1;

        if (!_executables.Any())
        {
            ConsoleHelper.WriteError("Error! Game files not found.");

            return 1;
        }

        if (!ReplaceFileContents())
            return 1;

        ConsoleHelper.WriteSuccess("Done.");

        Console.WriteLine(String.Empty);
        Console.Write("Press any key to exit...");
        Console.ReadKey();

        return 0;
    }

    private static void WriteHello()
    {
        var consoleWidth = Console.WindowWidth;

        Console.WriteLine(String.Empty);
        if (consoleWidth - AppName.Length > 4)
        {
            var header = "-- " + AppName + " ";
            Console.WriteLine(header + new string('-', consoleWidth - header.Length));
        }
        else
        {
            Console.WriteLine(AppName);
        }

        Console.WriteLine("For Witcher 3 Complete Edition - 4.0.1.427");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("https://nexusmods.com/witcher3/mods/7324");
        Console.ResetColor();
        Console.WriteLine(String.Empty);
    }

    private async Task<bool> ReadGamesAsync()
    {
        var executables = Game.ExecutableLocator.GetLocations();

        foreach (var item in executables)
            ConsoleHelper.WriteLog($"Found: {item}", false);

        foreach (var item in executables)
        {
            if (!File.Exists(item))
                return false;

            var executableContents = await BinaryParser.ReadAsync(item);

            if ((executableContents?.Content?.Length ?? 0) > 0)
                _executables.Add(item, executableContents!);
        }

        return true;
    }

    private bool SelectResolution()
    {
        Console.WriteLine("Select your resolution: ");

        Console.ForegroundColor = ConsoleColor.Yellow;

        for (int i = 0; i < BaseResolutions.Custom.Length; i++)
            Console.WriteLine($"    {i + 1} ) {BaseResolutions.Custom[i].Horizontal}x{BaseResolutions.Custom[i].Vertical}");

        Console.ResetColor();
        Console.WriteLine(String.Empty);

        Console.Write("Select resolution or press Q to exit: ");
        var readedKey = Console.ReadKey();

        if (readedKey.Key == ConsoleKey.Escape || readedKey.Key == ConsoleKey.Q)
        {
            Console.WriteLine("Exiting...");

            return false;
        }

        Console.WriteLine(String.Empty);

        Int32.TryParse(readedKey.KeyChar.ToString(), out var parsedKeyNumber);

        if (parsedKeyNumber < 1 || parsedKeyNumber > BaseResolutions.Custom.Length + 1)
        {
            ConsoleHelper.WriteError("Error! Invalid selection.");

            return false;
        }

        ConsoleHelper.WriteLog($"Selected: {BaseResolutions.Custom[parsedKeyNumber - 1].Horizontal}x{BaseResolutions.Custom[parsedKeyNumber - 1].Vertical}", false);

        _selectedResolution = parsedKeyNumber;

        return true;
    }

    private bool ReplaceFileContents()
    {
        ConsoleHelper.WriteLog($"Validating resolution");
        var selectedSequence = BaseResolutions.Custom[_selectedResolution - 1].Seq;

        foreach (var executable in _executables)
        {
            ConsoleHelper.WriteLog($"Replacing contents in {executable.Key}");

            var replaces = executable.Value.ReplaceAll(BaseResolutions.BakedSequence, selectedSequence);

            ConsoleHelper.WriteSuccess($"{replaces} reps have been replaced.");

            if (replaces > 0)
            {
                ConsoleHelper.WriteLog($"Saving file");

                File.WriteAllBytes(executable.Key, executable.Value.Content);

                ConsoleHelper.WriteSuccess($"File saved!");
            }
            else
            {
                ConsoleHelper.WriteLog($"No changes, skipping");
            }
        }

        return true;
    }
}
