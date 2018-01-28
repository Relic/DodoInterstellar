using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;

[CreateAssetMenu ( fileName = "ThemeTilmap", menuName = "Rewinde/Theme Tilemap", order = 1 )]
public class LevelTheme : ScriptableObject
{
	public TileBase Ground;
	public TileBase Wall;
	public TileBase Hole;
}
