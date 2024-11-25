using System;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;



public class TileMapVisualizer : MonoBehaviour
{
    /// <summary>
    /// Storage of the tilemap, tiles and graphical customizers
    /// </summary>
    [SerializeField]
    private Tilemap floorTilemap, wallTilemap;
    [SerializeField]
    private TileBase floorTile, wallTop, wallTile;
    [SerializeField]
    private List<TileBase> greenFloor, redFloor, purpleFloor;
    [SerializeField]
    private List<TileBase> greenWall, redWall, purpleWall;
    [SerializeField]
    private List<TileBase> decorationFloor, decorationWall;
    [SerializeField]
    [Range(0f, 1f)]
    private float chanceOfDecorationFloor = 0.2f;
    [SerializeField]
    [Range(0f, 1f)]
    private float chanceOfDecorationWall = 0.4f;
    [SerializeField]
    [Range(0f, 1f)]
    private float chanceOfColorFloors = 0.1f;
    [SerializeField]
    [Range(0f, 1f)]
    private float chanceOfColorWalls = 0.2f;

    /// <summary>
    /// Just calls the Paint Tiles with floortilemap
    /// </summary>
    /// <param name="floorPositions">all the coordinates for the floor tiles</param>
    public void PaintFloorandPerspectiveWallTiles(IEnumerable<Vector2Int> floorPositions)
    {
        PaintTiles(floorPositions, floorTilemap);
    }
    /// <summary>
    /// For each floor tile it checks if it should be a perspective wall and then by chance places a normal or a more special tile
    /// </summary>
    /// <param name="tilepositions">all the coordinates we want to add a tile to</param>
    /// <param name="tilemap">the tilemap we will be placing the tiles into</param>
    private void PaintTiles(IEnumerable<Vector2Int> tilepositions, Tilemap tilemap)
    {
        foreach (var tilepos in tilepositions)
        {
            TileBase tile;
            if (!tilepositions.Contains(new Vector2Int(tilepos.x, tilepos.y + 1)))
            {
                tile = wallTile;
                if (UnityEngine.Random.value < chanceOfDecorationWall)
                {
                    tile = decorationWall[UnityEngine.Random.Range(0, decorationWall.Count)];
                }
                PaintSingleTile(tilemap, tile, tilepos);
            }
            else
            {
                tile = floorTile;
                if (UnityEngine.Random.value < chanceOfDecorationFloor)
                {
                    tile = decorationFloor[UnityEngine.Random.Range(0, decorationFloor.Count)];
                }
                PaintSingleTile(tilemap, tile, tilepos);
            }
        }
    }
    /// <summary>
    /// Just sets up the tile
    /// </summary>
    /// <param name="tilemap">the tilemap we will be placing the tiles into</param>
    /// <param name="tile">the tile sprite we will be placing</param>
    /// <param name="tilepos">the tiles spot in the tilemap</param>
    private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int tilepos)
    {
        tilemap.SetTile((Vector3Int)tilepos, tile);
    }
    /// <summary>
    /// Clears all the tilemaps
    /// </summary>
    public void Clear()
    {
        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();
    }
    /// <summary>
    /// Calls Paint single with a wallTilemap
    /// </summary>
    internal void paintSingleBasickWall(Vector2Int position)
    {
        PaintSingleTile(wallTilemap, wallTop, position);
    }
    /// <summary>
    /// Just configurates a bit and calls the main coloring function
    /// </summary>
    /// <param name="room">the room we will be painting</param>
    /// <param name="floorpositions">all the coordinates of the floor</param>
    /// /// <param name="color">the color for this room, coresponding to one of the enemy types</param>
    public void ColourPaintRoom(BoundsInt room, IEnumerable<Vector2Int> floorpositions, ColorEnemy color)
    {
        List<TileBase> walls = new List<TileBase>();
        List<TileBase> floors = new List<TileBase>();
        switch (color)
        {
            case ColorEnemy.None:
                return;
            case ColorEnemy.Red:
                walls = redWall;
                floors = redFloor;
                break;
            case ColorEnemy.Green:
                walls = greenWall;
                floors = greenFloor;
                break;
            case ColorEnemy.Purple:
                walls = purpleWall;
                floors = purpleFloor;
                break;

        }
        ColorASpecificRoom(room, floorpositions, walls, floors, floorTilemap);
    }
    /// <summary>
    /// Goas through each tile of the room and by chance repaints them to the color
    /// </summary>
    /// <param name="room">the room we will be painting</param>
    /// <param name="tilepositions">all the coordinates of the floor</param>
    /// <param name="walls">colored wall tiles not actual wall but the one that adds perspecitve</param>
    /// <param name="floors">colored floor tiles that will be painted</param>
    /// <param name="tilemap">The tilemap we will be painting on</param>
    private void ColorASpecificRoom(BoundsInt room, IEnumerable<Vector2Int> tilepositions, List<TileBase> walls, List<TileBase> floors, Tilemap tilemap)
    {
        for (int x = room.min.x+1; x < room.max.x-1; x++)
        {
            for (int y = room.min.y+1; y < room.max.y-1; y++)
            {
                TileBase tile;
                if (!tilepositions.Contains(new Vector2Int(x, y + 1)))
                {
                    if (UnityEngine.Random.value < chanceOfColorWalls)
                    {
                        tile = walls[UnityEngine.Random.Range(0, walls.Count)];

                        PaintSingleTile(tilemap, tile, new Vector2Int(x, y));
                    }
                }
                else
                {
                    if (UnityEngine.Random.value < chanceOfColorFloors)
                    {
                        tile = floors[UnityEngine.Random.Range(0, floors.Count)];

                        PaintSingleTile(tilemap, tile, new Vector2Int(x, y));
                    }
                }
            }
        }
    }
    /// <summary>
    /// Just gets the coordinates in the game from the tile coordinates, helps with placement in relation to tilemap
    /// </summary>
    public Vector3 GetRealCoordsFromFloorTileMap(Vector2Int tilecoords)
    {
        return floorTilemap.CellToWorld(new Vector3Int(tilecoords.x, tilecoords.y, 0));
    }

    public float GetTilemapCellSize()
    {
        return floorTilemap.layoutGrid.cellSize.x; 
    }
}

