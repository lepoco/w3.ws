// This Source Code Form is subject to the terms of the GNU GPL-3.0.
// If a copy of the GPL was not distributed with this file, You can obtain one at https://www.gnu.org/licenses/gpl-3.0.en.html.
// Copyright (C) 2022 Leszek Pomianowski and W3.WS.CLI Contributors.
// All Rights Reserved.

using System.Threading.Tasks;

namespace W3.WS.Cli;

internal class Program
{
    static async Task<int> Main(string[] args) => await new App().RunAsync(args);
}
