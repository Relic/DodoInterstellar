using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class LevelsImporterTest {

	[Test]
	public void BasicImporterTest() {
        LevelImporter importer = new LevelImporter();
        LevelsCollection levels = importer.loadMetadata( LevelImporter.loadFile("Assets/Editor/testlevels.json") );
        Assert.NotNull(levels);
        Assert.AreEqual(1,  levels.levels.Length);
        Assert.AreEqual(1, levels.levels[0].id);
        Assert.AreEqual("test", levels.levels[0].title);
    }

    [Test]
    public void LevelReaderTest()
    {
        LevelImporter importer = new LevelImporter();
        LevelsCollection levels = importer.loadMetadata( LevelImporter.loadFile("Assets/Editor/testlevels.json"));
        List<LevelObject> objects = importer.loadLevel( LevelImporter.loadFile ( "Assets/Editor/"+ levels.levels[0].id + ".csv" ) );

        Assert.AreEqual(30, objects.Count);
        Assert.AreEqual(new Vector2Int(7, 0), objects[0].Position);
        Assert.AreEqual(ELevelObjectType.PORTAL, objects[0].LevelObjectType);
        Assert.IsEmpty(objects[0].targetReferences);
    }

    [Test]
    public void LevelReaderSwitchDoorTest()
    {
        LevelImporter importer = new LevelImporter();
        LevelsCollection levels = importer.loadMetadata( LevelImporter.loadFile("Assets/Editor/testlevels.json") );
        List<LevelObject> objects = importer.loadLevel( LevelImporter.loadFile ( "Assets/Editor/" + levels.levels[0].id + ".csv" ) );

        LevelObject theswitch = objects[29];
        LevelObject thedoor = objects[10];

        Assert.AreEqual(1, theswitch.targetReferences.Count);
        Assert.AreEqual(thedoor, theswitch.targetReferences[0]);
    }
}
