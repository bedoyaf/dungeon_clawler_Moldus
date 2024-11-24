using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Algorithms like bfs and spawner random spots that calculate info for the generator
/// </summary>
public static class DungeonContentGeneratorAlgorithms
{
    /// <summary>
    /// Bit randomly gets coordinates in a circular fashion for placing spawners
    /// This way their placement looks natural and they concentrate more on the center of the room
    /// </summary>
    /// <param name="room">The room in question</param>
    /// <param name="numberOfObjects">How many spawners will be placed there</param>
    /// <returns>List of coordinates to place</returns>
    public static List<Vector2Int> PlaceCoordinatesCircularPatternInRoom(BoundsInt room,int numberOfObjects)
    {
        List<Vector2Int> positions = new List<Vector2Int>();

        Vector2Int roomCenter = new Vector2Int(
        room.xMin + room.size.x / 2,
        room.yMin + room.size.y / 2
        );

        int radius = Mathf.Min(room.size.x-1, room.size.y-1) / 2;



        for (int i = 0; i < numberOfObjects; i++)
        {
            float angle = (i / (float)numberOfObjects) * Mathf.PI * 2;

            int randomRadius = UnityEngine.Random.Range(0, radius);

            int x = Mathf.RoundToInt(roomCenter.x + Mathf.Cos(angle) * randomRadius);
            int y = Mathf.RoundToInt(roomCenter.y + Mathf.Sin(angle) * randomRadius);

            if (x >= room.xMin && x < room.xMax && y >= room.yMin && y < room.yMax)
            {
                positions.Add(new Vector2Int(x, y));
            }
        }

        return positions;
    }
    /// <summary>
    /// Just a BFS algorithm that for each room finds the one furthest away and then sends back the most distant rooms
    /// </summary>
    /// <param name="roomcenters">All the center positions of rooms</param>
    /// <param name="floor">All positions of where the floor is</param>
    /// <returns>Tuple of two coordinates one being the start of the level the other the end, they signify the most distant rooms from each other</returns>
    public static Tuple<Vector2Int, Vector2Int> GetTwoRoomsFurthestFromEachOther(List<Vector2Int> roomcenters, IEnumerable<Vector2Int> floor)
    {
        int longestDistance = 0;
        Vector2Int start=roomcenters.First();
        Vector2Int end = roomcenters.Last();
        foreach(var room in roomcenters)
        {
            int currentLongest = 0;
            Vector2Int currentMostDistantRoom=room;
            HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
            Queue<Tuple<Vector2Int,int>> queue = new Queue<Tuple<Vector2Int, int>>();
            queue.Enqueue(new Tuple<Vector2Int, int>(room,0));

            while(queue.Count > 0)
            {
                var currentPoint = queue.Dequeue();
                if(roomcenters.Contains(currentPoint.Item1) && currentLongest<currentPoint.Item2)
                {
                    currentMostDistantRoom = currentPoint.Item1;
                    currentLongest = currentPoint.Item2;
                }
                foreach(var direction in DirectionManager.directions)
                {
                    var neighbour = currentPoint.Item1+ direction;
                    if(floor.Contains(neighbour) && !visited.Contains(neighbour))
                    {
                        queue.Enqueue(new Tuple<Vector2Int, int>(neighbour, currentPoint.Item2+1));
                        visited.Add(neighbour);
                    }
                }
            }
            if(currentLongest>longestDistance)
            {
                longestDistance = currentLongest;
                start = room;
                end = currentMostDistantRoom;
            }
        }
        return new Tuple<Vector2Int,Vector2Int>(start, end);
    }



    public static List<Vector2Int> PlacePillars(BoundsInt room, int minDistanceFromCenter, int minDistanceFromWalls)
    {
        if (room.size.x < minDistanceFromWalls * 3 || room.size.y < minDistanceFromWalls * 3)
        {
            Debug.LogWarning("Room is too small for pillar placement. Returning empty list.");
            return new List<Vector2Int>();
        }
        int randomChoice = Random.Range(0, 3); 
        switch (randomChoice)
        {
            case 0:
                return PlacePillarsInCornersOfRoom(room, minDistanceFromCenter, minDistanceFromWalls);
            case 1:
                return PlacePillarsInRoomRandomly(room, minDistanceFromCenter, minDistanceFromWalls);
            default:
                return new List<Vector2Int>();
        }
    }

    private static List<Vector2Int> PlacePillarsInRoomRandomly(BoundsInt room, int minDistanceFromCenter, int minDistanceFromWalls)
    {
        List<Vector2Int> pillars = new List<Vector2Int>();

        for (int i = 0; i < 4; i++) // Generate 4 random pillars
        {
            int x = Random.Range(room.xMin + minDistanceFromWalls, room.xMax - minDistanceFromWalls);
            int y = Random.Range(room.yMin + minDistanceFromWalls, room.yMax - minDistanceFromWalls);
            pillars.Add(new Vector2Int(x, y));
        }
        return pillars;
    }
    public static List<Vector2Int> PlacePillarsInCornersOfRoom(BoundsInt room,int minDistanceFromCenter, int minDistanceFromWalls)
    {
        List<Vector2Int> pillarCords = new List<Vector2Int>();
        Vector3Int roomCenter = new Vector3Int(
        room.xMin + room.size.x / 2,
        room.yMin + room.size.y / 2,
        0
        );

        int maxRadius = Mathf.Min(
            room.size.x / 2 - minDistanceFromWalls,
            room.size.y / 2 - minDistanceFromWalls
        );

        int radius = Random.Range(minDistanceFromCenter, maxRadius);

        Vector3Int[] offsets = new Vector3Int[]
        {
        new Vector3Int(radius, radius, 0),   // Top-right
        new Vector3Int(-radius, radius, 0), // Top-left
        new Vector3Int(radius, -radius, 0), // Bottom-right
        new Vector3Int(-radius, -radius, 0) // Bottom-left
        };

        foreach (Vector3Int offset in offsets)
        {
            Vector3Int pillarPosition = roomCenter + offset;

    //        if (room.Contains(pillarPosition))
     //       {
                pillarCords.Add(new Vector2Int(pillarPosition[0], pillarPosition[1])); // Custom method to place a pillar (instantiate object, etc.)
       //     }
        }
        return pillarCords;
    }
}
