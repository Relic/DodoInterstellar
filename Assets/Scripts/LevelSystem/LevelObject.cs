using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class LevelObject
{
    public Vector2Int Position;
    public ELevelObjectType LevelObjectType;
    public List<LevelObject> targetReferences = new List<LevelObject>();
    private int _orTriggerCounter = 0;
    private bool _currentState = false;

    public int OrTriggerCounter
    {
        get
        {
            return _orTriggerCounter;
        }
    }

    public LevelObject(Vector2Int vector2Int, ELevelObjectType type)
    {
        Position = vector2Int;
        LevelObjectType = type;
        targetReferences = new List<LevelObject>();
    }

    public event System.Action<bool, GameObject> OnTriggerValueChanged;

    public void addTargetReference(LevelObject targetReference)
    {
        targetReferences.Add(targetReference);
    }

    /// <summary>
    /// Called if the MonoBehaviour's OnTriggerEnter2D Event is called
    /// </summary>
    public virtual void OnTriggerEnter(GameObject triggeringObject = null)
    {
        for (int i = 0; i < targetReferences.Count; i++)
        {
            targetReferences[i].TriggerObject(triggeringObject);
        }
    }

    /// <summary>
    /// Called if the MonoBehaviour's OnTriggerExit2D Event is called
    /// </summary>
    public virtual void OnTriggerExit(GameObject triggeringObject = null)
    {
        for (int i = 0; i < targetReferences.Count; i++)
        {
            targetReferences[i].UnTriggerObject(triggeringObject);
        }
    }

    /// <summary>
    /// adds 1 to the current trigger counter and updates the level object state
    /// </summary>
    public virtual void TriggerObject(GameObject triggeringObject = null)
    {
        _orTriggerCounter++;
        UpdateLevelObjectState(triggeringObject);
    }

    /// <summary>
    /// subracts 1 from the current trigger counter and updates the level object state
    /// </summary>
    public virtual void UnTriggerObject(GameObject triggeringObject = null)
    {
        _orTriggerCounter--;
        UpdateLevelObjectState(triggeringObject);
    }

    /// <summary>
    /// Updates the current level object state (default counter handling => or trigger listener)
    /// </summary>
    protected virtual void UpdateLevelObjectState(GameObject triggeringObject = null)
    {
        _currentState = _orTriggerCounter > 0;
        if (OnTriggerValueChanged != null)
        {
            OnTriggerValueChanged(_currentState, triggeringObject);
        }

    }


}

