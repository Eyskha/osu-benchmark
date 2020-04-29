﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Textures;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Graphics.Containers;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Taiko.UI
{
    public class DrawableTaikoMascot : BeatSyncedContainer
    {
        public IBindable<TaikoMascotAnimationState> State => state;

        private readonly Bindable<TaikoMascotAnimationState> state;
        private readonly Dictionary<TaikoMascotAnimationState, TaikoMascotAnimation> animations;
        private TaikoMascotAnimation currentAnimation;

        private bool lastHitMissed;
        private bool kiaiMode;

        public DrawableTaikoMascot(TaikoMascotAnimationState startingState = TaikoMascotAnimationState.Idle)
        {
            RelativeSizeAxes = Axes.Both;

            state = new Bindable<TaikoMascotAnimationState>(startingState);
            animations = new Dictionary<TaikoMascotAnimationState, TaikoMascotAnimation>();
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            InternalChildren = new[]
            {
                animations[TaikoMascotAnimationState.Idle] = new TaikoMascotAnimation(TaikoMascotAnimationState.Idle),
                animations[TaikoMascotAnimationState.Clear] = new TaikoMascotAnimation(TaikoMascotAnimationState.Clear),
                animations[TaikoMascotAnimationState.Kiai] = new TaikoMascotAnimation(TaikoMascotAnimationState.Kiai),
                animations[TaikoMascotAnimationState.Fail] = new TaikoMascotAnimation(TaikoMascotAnimationState.Fail),
            };
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            animations.Values.ForEach(animation => animation.Hide());
            state.BindValueChanged(mascotStateChanged, true);
        }

        public void OnNewResult(JudgementResult result)
        {
            lastHitMissed = result.Type == HitResult.Miss && result.Judgement.AffectsCombo;
        }

        protected override void OnNewBeat(int beatIndex, TimingControlPoint timingPoint, EffectControlPoint effectPoint, TrackAmplitudes amplitudes)
        {
            kiaiMode = effectPoint.KiaiMode;
        }

        protected override void Update()
        {
            base.Update();
            state.Value = getNextState();
        }

        private TaikoMascotAnimationState getNextState()
        {
            // don't change state if current animation is playing
            // (used for clear state - others are manually animated on new beats)
            if (currentAnimation != null && !currentAnimation.Completed)
                return state.Value;

            if (lastHitMissed)
                return TaikoMascotAnimationState.Fail;

            return kiaiMode ? TaikoMascotAnimationState.Kiai : TaikoMascotAnimationState.Idle;
        }

        private void mascotStateChanged(ValueChangedEvent<TaikoMascotAnimationState> state)
        {
            currentAnimation?.Hide();
            currentAnimation = animations[state.NewValue];
            currentAnimation.Show();
        }
    }
}
