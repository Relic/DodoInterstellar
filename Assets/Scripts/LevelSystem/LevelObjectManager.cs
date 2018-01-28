using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelObjectManager : MonoBehaviour
{
    private List<BaseTrigger> _trigger = new List<BaseTrigger>();
    private const float TILE_SIZE = 1f;
    private const string PREFAB_PATH = "LevelObjects/";
    private string _currentTheme = "SPACE";

    public Vector2Int GridSize { get; internal set; }

    public void SetTheme(string theme)
    {
        _currentTheme = theme;
    }

    public void InstantiateLevelObject(LevelObject levelObject)
    {
        ELevelObjectType type = levelObject.LevelObjectType;
        GameObject levelGameObject = InstantiateObject(_currentTheme, type.ToString());
        BaseTrigger trigger = null;
        BaseTriggerListener listener = null;
        if (levelGameObject != null)
        {
            switch (type)
            {
                case ELevelObjectType.BUTTON:
                case ELevelObjectType.PORTAL:
                case ELevelObjectType.START:
                case ELevelObjectType.TRAP:
                case ELevelObjectType.EXIT:
                    trigger = levelGameObject.GetComponent<BaseTrigger>();
                    levelGameObject.transform.position = new Vector3(levelObject.Position.x, GridSize.y - levelObject.Position.y, 1);
                    trigger.LevelObjectReference = levelObject;
                    break;
                case ELevelObjectType.DOOR:
                case ELevelObjectType.PORTALTARGET:
                    listener = levelGameObject.GetComponent<BaseTriggerListener>();
                    levelGameObject.transform.position = new Vector3(levelObject.Position.x, GridSize.y - levelObject.Position.y, 1);
                    listener.LevelObjectReference = levelObject;
                    break;

            }
        }
    }

    private GameObject InstantiateObject(string currentTheme, string type)
    {
        string prefabPath = PREFAB_PATH + _currentTheme + "_" + type;
        GameObject instance = Resources.Load<GameObject>(prefabPath);
        if (instance != null)
        {
            instance = Instantiate(instance);
            return instance;
        }
        return null;

    }
}
