using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Temporary level loading test to setup triggers for testing
/// </summary>
public class LoadTestLevel : MonoBehaviour
{
    [Tooltip("A list of positions where buttons should be placed.")]
    public List<Vector2> TriggerPositions;
    public LevelObjectManager LevelObjects;

    private int _nextId = 1;

    void Start()
    {
        LevelObject start = new LevelObject(new Vector2Int(4, 9), ELevelObjectType.START);
        LevelObjects.InstantiateLevelObject(start);

        LevelObject levelObject = new LevelObject(new Vector2Int(8, 6), ELevelObjectType.BUTTON);
        LevelObject doorObject = new LevelObject(new Vector2Int(9, 5), ELevelObjectType.DOOR);
        levelObject.addTargetReference(doorObject);
        LevelObjects.InstantiateLevelObject(levelObject);
        LevelObjects.InstantiateLevelObject(doorObject);

        LevelObject portal = new LevelObject(new Vector2Int(10, 7), ELevelObjectType.PORTAL);
        LevelObject portalTarget = new LevelObject(new Vector2Int(3, 3), ELevelObjectType.PORTALTARGET);
        portal.addTargetReference(portalTarget);
        LevelObjects.InstantiateLevelObject(portal);
        LevelObjects.InstantiateLevelObject(portalTarget);

        LevelObject trap = new LevelObject(new Vector2Int(11, 11), ELevelObjectType.TRAP);
        LevelObjects.InstantiateLevelObject(trap);

        LevelObject exit = new LevelObject(new Vector2Int(1, 1), ELevelObjectType.EXIT);
        LevelObjects.InstantiateLevelObject(exit);

        SmoothCamera sc = FindObjectOfType<SmoothCamera>();
        sc.AddBackground("bg_planet_ring");
        sc.AddBackground("bg_planet_round");
        sc.AddBackground("bg_planet_ring");
    }
}
