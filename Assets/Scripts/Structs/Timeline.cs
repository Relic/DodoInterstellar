using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Create a new timeline of player inputs for the rewind mechanic
/// </summary>
[Serializable]
public class Timeline
{
    /// <summary>
    /// The player's position at the beginning of the timeline
    /// </summary>
    public Vector3 StartPosition = Vector3.zero;
    public bool Playing = false;

    /// <summary>
    /// The history of inputs in the timeline
    /// </summary>
    public List<InputFrame> History { get; private set; }

    /// <summary>
    /// The game object controller by a timeline replay
    /// </summary>
    public PlayerController Phantom { get; private set; }

    private float _startTime = 0.0f;
    private int _head = 0;
    private float _replayTime = 0.0f;

    public Timeline(float time, Vector3 position)
    {
        _startTime = time;
        StartPosition = position;

        History = new List<InputFrame>();
        Phantom = null;
    }

    /// <summary>
    /// Record a frame in the timeline
    /// </summary>
    /// <param name="input"></param>
    public void RecordFrame(InputFrame input)
    {
        History.Add(input);
    }

    /// <summary>
    /// Start the replay with a new phantom
    /// </summary>
    /// <param name="time">The time the replay starts from</param>
    /// <param name="phantom">The instantiated phantom</param>
    public void StartReplay(float time, PlayerController phantom)
    {
        DestroyPhantom();

        _replayTime = time;
        Phantom = phantom;
        Phantom.transform.position = StartPosition;
        Phantom.Killed += OnPhantomKilled;
        _head = 0;
        Playing = true;
    }

    /// <summary>
    /// Stop the current replay freezing the phantom in place
    /// </summary>
    public void StopReplay()
    {
        if (Phantom)
        {
            Phantom.SetVelocity(Vector2.zero);
            _head = 0;
            Playing = false;
        }
    }

    /// <summary>
    /// Destroy this timeline's phantom if it exists
    /// </summary>
    public void DestroyPhantom()
    {
        Playing = false;
        if (Phantom != null)
        {
            Phantom.Killed -= OnPhantomKilled;
            GameObject.Destroy(Phantom.gameObject);
        }
        Phantom = null;
    }

    /// <summary>
    /// Play the next frame if this timeline is actively playing
    /// and we're not at the end of the history
    /// </summary>
    /// <param name="fixedTime">The elapsed fixedTime of the level</param>
    /// <returns></returns>
    public bool TryPlayNextFrame(float fixedTime)
    {
        if (Playing && Phantom != null)
        {
            if (_head >= History.Count)
            {
                StopReplay();
            }
            else if (fixedTime - _replayTime >= History[_head].Timestamp)
            {
                Phantom.Move(History[_head++]);
            }
        }
        else if (Phantom == null)
        {
            StopReplay();
        }
        return Playing;
    }

    private void OnPhantomKilled()
    {
        DestroyPhantom();
    }
}
