using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTriggerListener : BaseTriggerListener
{
    public Sprite ClosedSprite;
    public Sprite OpenedSprite;

    public override void OnStateChanged(bool state, GameObject triggeringObject)
    {
        GetComponent<SpriteRenderer>().sprite = state ? OpenedSprite : ClosedSprite;
        GetComponent<Collider2D>().enabled = !state;
    }

    public override void InitializeLevelObject()
    {
        LevelObjectReference.OnTriggerValueChanged += OnStateChanged;
    }
}
