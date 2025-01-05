using System.Collections.Generic;
using UnityEngine;

public static class BasicDungeonGenerationAlgorithms
{
    public static BoundsInt GenerateSimpleRoom(Vector2Int size, Vector2Int offset)
    {
        Vector3Int roomOrigin = new Vector3Int(offset.x, offset.y, 0);

        BoundsInt roomBounds = new BoundsInt(roomOrigin, new Vector3Int(size.x, size.y, 1));

        return  roomBounds ;
    }

    public static  HashSet<Vector2Int> CreateRoomFloor(BoundsInt room) //Todo customize
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();

        for (int i = 0; i < room.size.x - 0; i++)
        {
            for (int j = 0; j < room.size.y - 0; j++)
            {
                Vector2Int position = (Vector2Int)room.min + new Vector2Int(i, j);
                floor.Add(position);
            }
        }
        return floor;
    }
}
