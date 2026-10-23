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

        /// <summary>
        /// Plays the timeline from it's current position.
        /// </summary>
        public void Play()
        {
            _ = PlayAsync();
        }

        /// <summary>
        /// Plays the timeline asyncronously.
        /// </summary>
        /// <returns></returns>
        public async Awaitable PlayAsync()
        {
           await PlayTimeline(1);
        }

        /// <summary>
        /// Plays the timeline from the starting position.
        /// </summary>
        public void PlayFromStart()
        {
            time = 0;
            Play();
        }

        /// <summary>
        /// Plays the timeline from the starting position asyncronously.
        /// </summary>
        /// <returns></returns>
        public async Awaitable PlayFromStartAsync()
        {
            time = 0;
            await PlayAsync();
        }

        /// <summary>
        /// Plays the timeline in reverse from it's current time.
        /// </summary>
        public void Reverse()
        {
            _ = ReverseAsync();
        }
        /// <summary>
        /// Plays the timeline in reverse from it's current time asyncronously.
        /// </summary>
        /// <returns></returns>
        public async Awaitable ReverseAsync()
        {
            await PlayTimeline(-1);
        }

        /// <summary>
        /// Plays the timeline in reverse from the end.
        /// </summary>
        public void ReverseFromEnd()
        {
            time = duration;
            Reverse();
        }

        /// <summary>
        /// Plats the timleine in reverse from the end asyncronously.
        /// </summary>
        /// <returns></returns>
        public async Awaitable ReverseFromEndAsync()
        {
            time = duration;
            await ReverseAsync();
        }

        /// <summary>
        /// Sets the current time of the playhead.
        /// </summary>
        /// <param name="time"></param>
        public void SetTime(float time)
        {
            this.time = Mathf.Clamp(time, 0, duration);
        }

        /// <summary>
        /// Stops the timeline.
        /// </summary>
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