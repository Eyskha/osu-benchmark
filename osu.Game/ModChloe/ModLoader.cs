// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Chloe.Mod;
using Chloe.ProjectMetrics;
using System;
using System.Collections.Generic;
using System.IO;

namespace osu.Game.ModChloe
{
    public class ModLoader
    {
        public static void Load(string executionEnvironment)
        {
            string path = @"C:\Users\chloe\Documents\Chloe\Master-Thesis\Benchmarks\osu-benchmark\osu.Desktop\bin\Debug\net6.0";
            string[] targetedNamespaces = new string[] { "osu.Game.Rulesets.Mania.Replays" };

            ModProjectMetrics.SetModParameters(
                "Osu",
                new string[] {
                    Path.Combine(path,"osu.Game.dll"),
                    Path.Combine(path,"osu.Game.Rulesets.Catch.dll"),
                    Path.Combine(path,"osu.Game.Rulesets.Mania.dll"),
                    Path.Combine(path,"osu.Game.Rulesets.Osu.dll"),
                    Path.Combine(path,"osu.Game.Rulesets.Taiko.dll"),
                    Path.Combine(path,"osu.Game.Tournament.dll"),
                },
                targetedNamespaces,
                new Type[] { }
            );
            ModProjectMetrics.ComputeAndLogProjectMetrics();

            ModLogExecutionData.SetModParameters(
                "Osu",
                Path.Combine(path, "osu.Game.Rulesets.Mania.dll"),
                targetedNamespaces,
                executionEnvironment,
                new Type[] { },
                new List<(string, string)> { },
                new Type[] { },
                @"C:\Users\chloe\Documents\Chloe\Master-Thesis\Benchmarks\osu-benchmark\osu.Game.Rulesets.Mania.Tests\bin\Debug\net6.0\osu.Game.Rulesets.Mania.Tests.dll"
            );
            ModLogExecutionData.Patch();
        }
    }
}
