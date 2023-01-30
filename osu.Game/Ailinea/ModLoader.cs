// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.IO;
using Ailinea;
using Ailinea.Inputs;

namespace osu.Game.Ailinea
{
    public class ModLoader
    {
        public static void Load(string executionEnvironment)
        {
            string path = @"C:\Users\chloe\Documents\Benchmarks\osu-benchmark\osu.Desktop\bin\Debug\net6.0";
            string[] targetedNamespaces = new string[] {
                "osu.Game.Rulesets.Mods",
            };
            // osu.Game.Rulesets.Osu.Mods also?

            AilineaMod.CreateMod(
                new ProjectInformation()
                {
                    ProjectName = "Osu",
                    ExecutionEnvironment = executionEnvironment
                },
                new Targets()
                {
                    PathAssemblyToInspect = Path.Combine(path, "osu.Game.dll"),
                    NamespacesToInspect = targetedNamespaces,
                },
                new TestTargets()
                {
                    PathTestAssembly = @"C:\Users\chloe\Documents\Benchmarks\osu-benchmark\osu.Game.Tests\bin\Debug\net6.0\osu.Game.Tests.dll"
                }
            );

            //ModProjectMetrics.SetModParameters(
            //    "Osu",
            //    new string[] {
            //        Path.Combine(path,"osu.Game.dll"),
            //        Path.Combine(path,"osu.Game.Rulesets.Catch.dll"),
            //        Path.Combine(path,"osu.Game.Rulesets.Mania.dll"),
            //        Path.Combine(path,"osu.Game.Rulesets.Osu.dll"),
            //        Path.Combine(path,"osu.Game.Rulesets.Taiko.dll"),
            //        Path.Combine(path,"osu.Game.Tournament.dll"),
            //    },
            //    targetedNamespaces,
            //    new Type[] { }
            //);
            //ModProjectMetrics.ComputeAndLogProjectMetrics();
        }
    }
}
