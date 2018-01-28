using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

[Serializable]
public class LevelsCollection
{
    [Serializable]
    public struct Level
    {
        public int id;
        public string title;
        public string introText;
        public int rewindCount;
        public int timeLimit;
        public int gridSizeX;
        public int gridSizeY;
        public string[] background;
        public string theme;
    }
    public Level[] levels;
}

public class LevelImporter {
    public LevelImporter()
    {

    }

    public LevelsCollection loadMetadata(string metadata)
    {
        return JsonUtility.FromJson<LevelsCollection>( metadata );
    }

    public List<LevelObject> loadLevel(string levelData)
    {
		string content = levelData;
        string[] lines = content.Split('\n');

        List<LevelObject> objects = new List<LevelObject>();

        Dictionary<int, List<LevelObject>> triggers = new Dictionary<int, List<LevelObject>>();
        Dictionary<int, List<LevelObject>> targets = new Dictionary<int, List<LevelObject>>();

        for (int y = 0; y<lines.Length; y++)
        {
            string[] entries = lines[y].Split(';');
            for (int x = 0; x < entries.Length; x++)
            {
                string[] posEntry = entries[x].Split('@');
                string[] element = posEntry[0].Split('-');

                string position = posEntry.Length > 1 ? posEntry[1] : null;
                string elementType = element[0].Trim();
                string elementId = element.Length > 1 ? element[1] : null;

                if (elementType.Length > 0)
                {
                    LevelObject item = createLevelObject(x, y, elementType, elementId, position);
                    objects.Add(item);

                    if (elementId != null)
                    {
                        int idx = int.Parse(elementId);
                        switch (item.LevelObjectType)
                        {
                            case ELevelObjectType.BUTTON:
                            case ELevelObjectType.SWITCH:
                            case ELevelObjectType.PORTAL:
                                if (!triggers.ContainsKey(idx))
                                {
                                    triggers.Add(idx, new List<LevelObject>());
                                }
                                triggers[idx].Add(item);
                                break;

                            case ELevelObjectType.DOOR:
                            case ELevelObjectType.LIGHT:
                            case ELevelObjectType.GATE:
                            case ELevelObjectType.PORTALTARGET:
                                if (!targets.ContainsKey(idx))
                                {
                                    targets.Add(idx, new List<LevelObject>());
                                }
                                targets[idx].Add(item);
                                break;
                        }
                    }
                }
            }
        }

        foreach (int sourceKey in triggers.Keys)
            {
                foreach (LevelObject sourceItem in triggers[sourceKey])
                {
                    foreach (LevelObject targetItem in targets[sourceKey])
                    {
                        sourceItem.addTargetReference(targetItem);
                    }
                }
            }

        return objects;
    }

    private LevelObject createLevelObject(int x, int y, string elementType, string elementId, string position)
    {
        ELevelObjectType type = (ELevelObjectType) Enum.Parse(typeof(ELevelObjectType), elementType.ToUpper());
        return new LevelObject(new Vector2Int(x, y), type);
    }

    public static string loadFile(string fileName)
    {
        StreamReader reader = new StreamReader(fileName);
        string content = reader.ReadToEnd();
        reader.Close();
        return content;
    }
}
