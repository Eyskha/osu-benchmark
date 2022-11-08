// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

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
                @"C:\Users\chloe\Documents\Chloe\Master-Thesis\Benchmarks\osu-2022.1022.0\osu.Game.Rulesets.Mania\bin\Debug\netstandard2.1\osu.Game.Rulesets.Mania.dll",
                new string[] { "osu.Game.Rulesets.Mania.Replays" },
                executionEnvironment,
                new Type[] { },
                new List<(string, string)> { });
            ModLogExecutionData.Patch();
        }
    }
}
