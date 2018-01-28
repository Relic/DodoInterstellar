using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu ( fileName = "BlockTile", menuName = "Rewinde/BlockTile", order = 1 )]
public class BlockTile : Tile
{
	// This refreshes itself and other RoadTiles that are orthogonally and diagonally adjacent
	public override void RefreshTile(Vector3Int location, ITilemap tilemap)
	{
		for ( int yd = -1; yd <= 1; yd++ )
			for ( int xd = -1; xd <= 1; xd++ )
			{
				Vector3Int position = new Vector3Int ( location.x + xd, location.y + yd, location.z );
				if ( tilemap.GetTile ( position ) is GroundTile || tilemap.GetTile ( position ) == this )
					tilemap.RefreshTile ( position );
			}
	}
}
