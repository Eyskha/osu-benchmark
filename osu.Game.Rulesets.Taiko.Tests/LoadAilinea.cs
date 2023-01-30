// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Game.Ailinea;

namespace osu.Game.Rulesets.Taiko.Tests
{
    [SetUpFixture]
    public class LoadAilinea
    {
        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            ModLoader.Load("Test");
        }
    }
}
