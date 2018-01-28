using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RewindButtonEvents : MonoBehaviour
{
    public LevelManager LevelManager = null;

    public void PointerEnter(BaseEventData bed)
    {
        LevelManager.SetFrozen(true);
    }

    public void PointerExit(BaseEventData bed)
    {
        LevelManager.SetFrozen(false);
    }
}
