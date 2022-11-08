// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Chloe.Mod;
using System;
using System.Collections.Generic;

namespace osu.Game.ModChloe
{
    public class ModLoader
    {
        public static void Load(string executionEnvironment)
        {
            ModLogExecutionData.SetModParameters(
                "Osu",
                @"C:\Users\chloe\Documents\Chloe\Master-Thesis\Benchmarks\osu-benchmark\osu.Desktop\bin\Debug\net6.0\osu.Game.Rulesets.Mania.dll",
                new string[] { "osu.Game.Rulesets.Mania.Replays" },
                executionEnvironment,
                new Type[] { },
                new List<(string, string)> { });
            ModLogExecutionData.Patch();
        }
    }
}
