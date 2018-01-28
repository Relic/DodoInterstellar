using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseTriggerListener : MonoBehaviour
{
    protected LevelObject _levelObject = null;

    public LevelObject LevelObjectReference
    {
        get
        {
            return _levelObject;
        }
        set
        {
            _levelObject = value;
            InitializeLevelObject();
        }
    }

    public abstract void OnStateChanged(bool state, GameObject triggeringObject);
    public abstract void InitializeLevelObject();
}
