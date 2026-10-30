/*****************************************************************************
// File Name : ITimeline.cs
// Author : Arcadia Koederitz
// Creation Date : 10/23/2026
// Last Modified : 10/23/2026
//
// Brief Description : Generic interface for any timeline class.
*****************************************************************************/
using System;

public interface ITimeline
{
    /// <summary>
    /// Plays the timeline from the current playhead position.
    /// </summary>
    void Play();

    /// <summary>
    /// Plays the timeline from the start.
    /// </summary>
    void PlayFromStart();

    /// <summary>
    /// Stops the timeline.
    /// </summary>
    void Stop();

    /// <summary>
    /// Plays the timeline starting from the playhead in the reverse direction.
    /// </summary>
    void Reverse();

    /// <summary>
    /// Plays the timeline in the reverse direction from the end.
    /// </summary>
    void ReverseFromEnd();

    /// <summary>
    /// Sets the current playhead position.
    /// </summary>
    /// <param name="time">The time to set the playhead.</param>
    void SetTime(float time);

    #region Events
    event Action<float> OnUpdateEvent;
    event Action OnFinishedEvent;
    #endregion

    public float Duration
    {
        get;
        set;
    }

    public bool IsPlaying { get; }
}
