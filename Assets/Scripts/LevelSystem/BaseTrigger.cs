using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseTrigger : MonoBehaviour
{
    public int TriggerId { get; set; }
    protected LevelObject _levelObject = null;
    protected List<PlayerController> _triggeredPlayers = new List<PlayerController>();

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

    public abstract void InitializeLevelObject();

}
