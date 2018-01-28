using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelRendererController : MonoBehaviour
{
	public LevelTheme Theme;
	public Tilemap Map;

	public void Start()
	{
		LevelImporter importer = new LevelImporter ();
		LevelsCollection levels = importer.loadMetadata ( Resources.Load<TextAsset> ( "levels" ).text );

		List<LevelObject> objects = importer.loadLevel ( Resources.Load<TextAsset> ( PlayerPrefs.GetInt ( "CurrentLevel", 0 ).ToString () ).text );

		Map.ClearAllTiles ();
		LoadLevel ( levels.levels[PlayerPrefs.GetInt ( "CurrentLevel", 0 )], objects, Theme );
	}



	public void LoadLevel(LevelsCollection.Level levelData, List<LevelObject> levelEntities, LevelTheme theme)
	{
		Vector2Int max = new Vector2Int ( levelData.gridSizeX, levelData.gridSizeY );


		for ( int x = -1; x <= max.x + 2; x++ )
		{
			for ( int y = -1; y <= max.y + 2; y++ )
			{
				Map.SetTile ( new Vector3Int ( x, y, 0 ), theme.Hole );
			}
		}
		for ( int x = 0; x <= max.x; x++ )
		{
			for ( int y = 0; y <= max.y; y++ )
			{
				Map.SetTile ( new Vector3Int ( x, y, 0 ), theme.Ground );
			}
		}

		foreach ( var item in levelEntities )
		{
			switch ( item.LevelObjectType )
			{
				//case ELevelObjectType.NONE:
				//	break;
				case ELevelObjectType.WALL:
					Map.SetTile ( new Vector3Int ( item.Position.x, levelData.gridSizeY - item.Position.y, 0 ), theme.Wall );
					break;
				case ELevelObjectType.HOLE:
					Map.SetTile ( new Vector3Int ( item.Position.x, levelData.gridSizeY - item.Position.y, 0 ), theme.Hole );
					break;
			}
		}

		//Map.transform.position = new Vector3 ();
		//var center = Map.GetCellCenterWorld ( new Vector3Int ( max.x / 2, max.y / 2, 0 ) );
		//Map.transform.position = -center;
	}
}
