/*****************************************************************************
// File Name : AsyncTimeline.cs
// Author : Arcadia Koederitz
// Creation Date : 7/22/2026
// Last Modified : 7/22/2026
//
// Brief Description : Utility class that can animate a value over time using a curve.  Backed by an async method.
*****************************************************************************/
using System;
using System.Threading;
using UnityEngine;

namespace AriUtils
{
    [System.Serializable]
    public class AsyncTimeline : ITimeline
    {
        [SerializeField] private float duration;
        [SerializeField] private bool useUnscaledTime;
        [SerializeField] private AnimationCurve curve;
        [SerializeField] private bool normalizeCurveTime = true;
        [SerializeField, ReadOnly] private float time;

        private CancellationTokenSource cts;
        private bool isPlaying;

        public float Duration { get => duration; set => duration = value; }
        public bool IsPlaying => isPlaying;

        public event Action<float> OnUpdateEvent;
        public event Action OnFinishedEvent;

        public void Play()
        {
            _ = PlayAsync();
        }
        public async Awaitable PlayAsync()
        {
           await PlayTimeline(1);
        }

        public void PlayFromStart()
        {
            time = 0;
            Play();
        }

        public async Awaitable PlayFromStartAsync()
        {
            time = 0;
            await PlayAsync();
        }

        public void Reverse()
        {
            _ = ReverseAsync();
        }
        public async Awaitable ReverseAsync()
        {
            await PlayTimeline(-1);
        }

        public void ReverseFromEnd()
        {
            time = duration;
            Reverse();
        }

        public async Awaitable ReverseFromEndAsync()
        {
            time = duration;
            await ReverseAsync();
        }

        public void SetTime(float time)
        {
            this.time = Mathf.Clamp(time, 0, duration);
        }

        public void Stop()
        {
            if (isPlaying)
            {
                cts.Cancel();
                isPlaying = false;
            }
        }

        private async Awaitable PlayTimeline(int timeStep)
        {
            Stop();
            cts = new CancellationTokenSource();
            await TimelineAsync(timeStep, duration, cts.Token);
        }

        private async Awaitable TimelineAsync(int timeStep, float duration, CancellationToken ct)
        {
            try
            {
                isPlaying = true;
                while (time <= duration && time >= 0)
                {
                    time += timeStep * (useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime);
                    OnUpdateEvent?.Invoke(curve.Evaluate(normalizeCurveTime ? time / duration : time));
                    await Awaitable.NextFrameAsync(ct);
                }
                SetTime(time);
                OnUpdateEvent?.Invoke(curve.Evaluate(normalizeCurveTime ? time / duration : time));
                OnFinishedEvent?.Invoke();
            }
            catch (Exception e)
            {
                if (e is not OperationCanceledException)
                {
                    Debug.LogException(e);
                }
            }
            finally
            {
                isPlaying = false;
            }
            
        }
    }
}