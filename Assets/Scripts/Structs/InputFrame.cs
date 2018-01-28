using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains the input of a fixed update frame for the rewinding mechanic.
/// </summary>
[Serializable]
public struct InputFrame
{
    public static InputFrame Empty { get { return new InputFrame(-1.0f, 0.0f, 0.0f, false, Vector3.zero); } }

    public float Timestamp;
    public float HorizontalInput;
    public float VerticalInput;

    public bool MouseDown;
    public Vector3 MousePosition;

    public InputFrame(float timestamp, float horizontal, float vertical, bool mouseDown, Vector3 mousePosition)
    {
        Timestamp = timestamp;
        HorizontalInput = horizontal;
        VerticalInput = vertical;
        MouseDown = mouseDown;
        MousePosition = mousePosition;
    }

    /// <summary>
    /// Equality operator overload checking inputs only (ignore timestamp)
    /// </summary>
    /// <param name="i1"></param>
    /// <param name="i2"></param>
    /// <returns></returns>
    public static bool operator ==(InputFrame i1, InputFrame i2)
    {
        // Null & reference check (i1 and i2 are the same if they're at the same location in memory obviously)
        if (ReferenceEquals(i1, i2)) return true;
        else if (ReferenceEquals(i1, null) || ReferenceEquals(i2, null)) return false;

        return i1.HorizontalInput == i2.HorizontalInput && i1.VerticalInput == i2.VerticalInput &&
            i1.MouseDown == i2.MouseDown && i1.MousePosition == i2.MousePosition;
    }

    /// <summary>
    /// Inequality operator overload checking inputs only (ignore timestamp)
    /// </summary>
    /// <param name="i1"></param>
    /// <param name="i2"></param>
    /// <returns></returns>
    public static bool operator !=(InputFrame i1, InputFrame i2)
    {
        // Null & reference check (i1 and i2 are the same if they're at the same location in memory obviously)
        if (ReferenceEquals(i1, i2)) return false;
        else if (ReferenceEquals(i1, null) || ReferenceEquals(i2, null)) return true;

        return i1.HorizontalInput != i2.HorizontalInput || i1.VerticalInput != i2.VerticalInput ||
            i1.MouseDown != i2.MouseDown || i1.MousePosition != i2.MousePosition;
    }

    /// <summary>
    /// Equality operator overload checking inputs only (ignore timestamp)
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>

    public override bool Equals(object obj)
    {
        // Null & reference check (i1 and i2 are the same if they're at the same location in memory obviously)
        if (ReferenceEquals(this, obj)) return true;
        else if (ReferenceEquals(this, null) || ReferenceEquals(obj, null)) return false;

        return HorizontalInput == ((InputFrame)obj).HorizontalInput && VerticalInput == ((InputFrame)obj).VerticalInput &&
            MouseDown == ((InputFrame)obj).MouseDown && MousePosition == ((InputFrame)obj).MousePosition;
    }

    /// <summary>
    /// GetHashCode() should correspond to Equals() and the equality operator
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        return HorizontalInput.GetHashCode() ^ VerticalInput.GetHashCode() ^ MouseDown.GetHashCode() ^ MousePosition.GetHashCode();
    }
}
