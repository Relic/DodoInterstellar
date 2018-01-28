using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu ( fileName = "GroundTile", menuName = "Rewinde/GroundTile", order = 1 )]
public class GroundTile : Tile
{
	const int TOP = 1 << 0;
	const int RIGHT = 1 << 1;
	const int BOTTOM = 1 << 2;
	const int LEFT = 1 << 3;

	[Space]
	public Sprite[] m_Sprites;
	public Sprite m_Preview;

	// This refreshes itself and other RoadTiles that are orthogonally and diagonally adjacent
	public override void RefreshTile(Vector3Int location, ITilemap tilemap)
	{
		for ( int yd = -1; yd <= 1; yd++ )
			for ( int xd = -1; xd <= 1; xd++ )
			{
				Vector3Int position = new Vector3Int ( location.x + xd, location.y + yd, location.z );
				if ( HasBlockTile ( tilemap, position ) || tilemap.GetTile ( position ) == this )
					tilemap.RefreshTile ( position );
			}
	}
	// This determines which sprite is used based on the RoadTiles that are adjacent to it and rotates it to fit the other tiles.
	// As the rotation is determined by the RoadTile, the TileFlags.OverrideTransform is set for the tile.
	public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
	{
		int mask = HasBlockTile ( tilemap, location + new Vector3Int ( 0, 1, 0 ) ) ? TOP : 0;
		mask |= HasBlockTile ( tilemap, location + new Vector3Int ( 1, 0, 0 ) ) ? RIGHT : 0;
		mask |= HasBlockTile ( tilemap, location + new Vector3Int ( 0, -1, 0 ) ) ? BOTTOM : 0;
		mask |= HasBlockTile ( tilemap, location + new Vector3Int ( -1, 0, 0 ) ) ? LEFT : 0;
		int index = GetIndex ( (byte)mask );
		if ( index >= 0 && index < m_Sprites.Length )
		{
			tileData.sprite = m_Sprites[index];
			tileData.color = Color.white;
			tileData.colliderType = ColliderType.Sprite;
		}
		else
		{
			tileData.sprite = m_Preview;
			tileData.color = Color.white;
			tileData.colliderType = ColliderType.None;
		}
		tileData.flags = TileFlags.None;
	}
	// This determines if the Tile at the position is the same RoadTile.
	private bool HasBlockTile(ITilemap tilemap, Vector3Int position)
	{
		var tile = tilemap.GetTile ( position );
		return tile is BlockTile;
	}
	// The following determines which sprite to use based on the number of adjacent RoadTiles
	private int GetIndex(byte mask)
	{
		switch ( mask )
		{
			//case 0:
			//	return -2;
			case TOP | RIGHT:
				return 5;
			case RIGHT | BOTTOM:
				return 6;
			case TOP | LEFT:
				return 4;
			case BOTTOM | LEFT:
				return 7;
			case TOP:
				return 0;
			case RIGHT:
				return 1;
			case BOTTOM:
				return 2;
			case TOP | BOTTOM:
			case RIGHT | LEFT:
			case LEFT:
				return 3;
			//case TOP | RIGHT | BOTTOM:
			//case TOP | RIGHT | LEFT:
			//case TOP | BOTTOM | LEFT:
			//case RIGHT | BOTTOM | LEFT:
			//case TOP | RIGHT | BOTTOM | LEFT:
			//	return -2;
		}
		return -1;
	}
	// The following determines which rotation to use based on the positions of adjacent RoadTiles
	private Quaternion GetRotation(byte mask)
	{
		switch ( mask )
		{
			case 9:
			case 10:
			case 7:
			case 2:
			case 8:
				return Quaternion.Euler ( 0f, 0f, -90f );
			case 3:
			case 14:
				return Quaternion.Euler ( 0f, 0f, -180f );
			case 6:
			case 13:
				return Quaternion.Euler ( 0f, 0f, -270f );
		}
		return Quaternion.Euler ( 0f, 0f, 0f );
	}
}