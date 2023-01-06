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
using W3.WS.Cli.Game;
using W3.WS.Cli.Helpers;
using W3.WS.Cli.Ratios;

namespace W3.WS.Cli;

// 89 83 30 01 00 00 C7 83 38 01 00 00 > 39 8E E3 3F > SELECTED
// AC 65 E8 3F B4 C2 E6 3F 98 25 E5 3F > 39 8E E3 3F > SELECTED
// 00 00 E0 3F 9A 99 99 99 99 99 E1 3F > 39 8E E3 3F > SELECTED

// This class is a mess, please don't blame me, it was just supposed to work somehow
internal class App
{
    private const string AppName = "WITCHER 3 WIDESCREEN PATCHER";

    // For build 4.0.0.66291 | 4.0.1.755
    private static readonly byte[] PreBakedValue = new byte[]
    {
        0x9A,
        0x99,
        0x99,
        0x99,
        0x99,
        0x99,
        0xE1,
        0x3F
    }; // There's a good chance that will change in the future

    private Ratios.Resolution _selectedResolution = new Resolution { Horizontal = 0, Vertical = 0 };

    private IList<GameInstance>? _gameExecutables;

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

        if (!_gameExecutables.Any())
        {
            ConsoleHelper.WriteError("Error! Game files not found.");

            return 1;
        }

        Console.Write("Are you sure you want to continue? [Y]es / [N]o");
        Console.WriteLine(String.Empty);
        readedKey = Console.ReadKey();
        Console.WriteLine(String.Empty);

        if (readedKey.Key != ConsoleKey.Y)
            return 0;

        CreateBackups();

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

        Console.WriteLine("For Witcher 3 Complete Edition - 4.0.1.755");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("https://nexusmods.com/witcher3/mods/7324");
        Console.ResetColor();
        Console.WriteLine(String.Empty);
    }

    private async Task<bool> ReadGamesAsync()
    {
        var executables = Game.ExecutableLocator.GetLocations();

        foreach (var item in executables)
            ConsoleHelper.WriteLog($"Found: {new Uri(item.Path).AbsolutePath}", false);

        foreach (var item in executables)
        {
            if (!File.Exists(item.Path))
                return false;

            var executableContents = await BinaryParser.ReadAsync(item.Path);

            item.Container = executableContents;
        }

        _gameExecutables = executables as List<GameInstance>;

        return true;
    }

    private void CreateBackups()
    {
        if (_gameExecutables == null)
            return;

        var backupPath = Path.Combine(
            Environment.CurrentDirectory,
            $"backup_{DateTime.Now.ToString("dd_MM_yyyy_HH_mm")}"
        );

        foreach (var item in _gameExecutables)
        {
            if (!File.Exists(item.Path))
                continue;

            string backupFilePath = item.Type switch
            {
                GameInstanceType.DX12
                    => backupFilePath = Path.Combine(backupPath, "x64_dx12/witcher3.exe"),
                _ => backupFilePath = Path.Combine(backupPath, "x64/witcher3.exe")
            };

            var backupFileDirectory = Path.GetDirectoryName(backupFilePath);

            if (!Directory.Exists(backupFileDirectory))
                Directory.CreateDirectory(backupFileDirectory);

            if (!File.Exists(backupFilePath))
                File.Copy(item.Path, backupFilePath);

            ConsoleHelper.WriteLog(
                $"Created backup in: {new Uri(backupFilePath).AbsolutePath}",
                false
            );
        }
    }

    private bool SelectResolution()
    {
        Console.WriteLine("Select your resolution: ");

        Console.ForegroundColor = ConsoleColor.Yellow;

        Console.WriteLine($"    0 ) Custom");

        for (int i = 0; i < BaseResolutions.Common.Length; i++)
            Console.WriteLine(
                $"    {i + 1} ) {BaseResolutions.Common[i].Horizontal}x{BaseResolutions.Common[i].Vertical}"
            );

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

        if (parsedKeyNumber == 0)
            return ManuallyReadResolution();

        if (parsedKeyNumber > 0 && parsedKeyNumber < BaseResolutions.Common.Length + 1)
        {
            _selectedResolution = BaseResolutions.Common[parsedKeyNumber - 1];

            ConsoleHelper.WriteLog(
                $"Selected: {_selectedResolution.Horizontal}x{_selectedResolution.Vertical}",
                false
            );

            return true;
        }

        ConsoleHelper.WriteError("Error! Invalid selection.");

        return false;
    }

    private bool ManuallyReadResolution()
    {
        int horizontalResolution,
            verticalResolution;

        Console.WriteLine(String.Empty);
        Console.Write("    Type your horizontal resolution: ");
        var currentLine = Console.ReadLine();

        Int32.TryParse(currentLine, out horizontalResolution);

        if (horizontalResolution < 1 || horizontalResolution > 99999)
        {
            ConsoleHelper.WriteError("Error! Invalid value.");

            return false;
        }

        Console.Write("    Type your horizontal resolution: ");
        currentLine = Console.ReadLine();

        Int32.TryParse(currentLine, out verticalResolution);

        if (verticalResolution < 1 || verticalResolution > 99999)
        {
            ConsoleHelper.WriteError("Error! Invalid value.");

            return false;
        }

        _selectedResolution = new Resolution
        {
            Horizontal = horizontalResolution,
            Vertical = verticalResolution
        };

        Console.WriteLine(String.Empty);
        ConsoleHelper.WriteLog(
            $"Selected: {_selectedResolution.Horizontal}x{_selectedResolution.Vertical}",
            false
        );

        return true;
    }

    private bool ReplaceFileContents()
    {
        if (_gameExecutables == null || _selectedResolution.Horizontal < 1)
            return false;

        ConsoleHelper.WriteLog($"Validating resolution");
        var selectedSequence = _selectedResolution.GetRatio();

        foreach (var executable in _gameExecutables)
        {
            if (executable.Container == null)
                continue;

            ConsoleHelper.WriteLog($"Replacing contents in {executable.Path}");

            //var replaces = executable.Container?.ReplaceAll(BaseResolutions.BakedResolution.GetRatio(), selectedSequence);
            var replaces = executable.Container?.ReplaceAfter(PreBakedValue, selectedSequence);

            ConsoleHelper.WriteSuccess($"{replaces} reps have been replaced.");

            if (replaces > 0)
            {
                ConsoleHelper.WriteLog($"Saving file");

                File.WriteAllBytes(executable.Path, executable.Container!.Content);

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
