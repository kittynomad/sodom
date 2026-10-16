/*****************************************************************************
// File Name : CoroutineTimeline.cs
// Author : Arcadia Koederitz
// Creation Date : 7/22/2026
// Last Modified : 7/22/2026
//
// Brief Description : Utility class that can animate a value over time using a curve.  Backed by a Coroutine.
*****************************************************************************/
using System;
using System.Collections;
using UnityEngine;

namespace AriUtils
{
    [System.Serializable]
    public class CoroutineTimeline
    {
        [SerializeField] private float duration;
        [SerializeField] private bool useUnscaledTime;
        [SerializeField] private AnimationCurve curve;
        [SerializeField] private bool normalizeCurveTime = true;

        [SerializeField] private MonoBehaviour _parentBehavior;

        private Coroutine timelineRoutine;
        [SerializeField] private float time;

        #region Events
        public event Action<float> OnUpdateEvent;
        public event Action OnFinishedEvent;
        #endregion

        /// <summary>
        /// Plays the timeline from the current playhead position.
        /// </summary>
        public void Play()
        {
            PlayTimeline(1);
        }

        /// <summary>
        /// Plays the timeline from the start.
        /// </summary>
        public void PlayFromStart()
        {
            time = 0;
            Play();
        }

        /// <summary>
        /// Stops the timeline.
        /// </summary>
        public void Stop()
        {
            if (timelineRoutine != null)
            {
                _parentBehavior.StopCoroutine(timelineRoutine);
                timelineRoutine = null;
            }
        }

        /// <summary>
        /// Plays the timeline starting from the playhead in the reverse direction.
        /// </summary>
        public void Reverse()
        {
            PlayTimeline(-1);
        }

        /// <summary>
        /// Plays the timeline in the reverse direction from the end.
        /// </summary>
        public void ReverseFromEnd()
        {
            time = duration;
            Reverse();
        }

        /// <summary>
        /// Sets the current playhead position.
        /// </summary>
        /// <param name="time">The time to set the playhead.</param>
        public void SetTime(float time)
        {
            this.time = Mathf.Clamp(time, 0, duration);
        }

        private void PlayTimeline(int timeStep)
        {
            Stop();
            timelineRoutine = _parentBehavior.StartCoroutine(TimelineRoutine(timeStep));
        }
        private IEnumerator TimelineRoutine(int timeStep)
        {
            while (time <= duration && time >= 0)
            {
                time += timeStep * (useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime);
                OnUpdateEvent?.Invoke(curve.Evaluate(normalizeCurveTime ? time / duration : time));
                yield return null;
            }
            SetTime(time);
            OnUpdateEvent?.Invoke(curve.Evaluate(normalizeCurveTime ? time / duration : time));
            OnFinishedEvent?.Invoke();
            timelineRoutine = null;
        }
    }

}