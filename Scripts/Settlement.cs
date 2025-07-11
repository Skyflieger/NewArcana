using System;
using Godot;
using System.Collections.Generic;
using System.Linq;

namespace NewArcana.Scripts;

public class Settlement
{
    private readonly TopLayer _topLayer;
    private Vector2I centerPosition;
    public Settlement(TopLayer topLayer)
    {
        _topLayer = topLayer;
    }

    public void SpawnSettlement(Vector2I position)
    {
        centerPosition = position;
        int maxDistance = 3;
        HashSet<Vector2I> streetPositions = new HashSet<Vector2I>();

        for (int distance = 0; distance <= maxDistance; distance++)
        {
            Vector2I[] directions =
            {
                new Vector2I(-distance, 0),
                new Vector2I(distance, 0), 
                new Vector2I(0, -distance),
                new Vector2I(0, distance)  
            };

            foreach (var direction in directions)
            {
                var roadPosition = position + direction;
                _topLayer.SetCell(roadPosition, TopLayerTiles.Street);
                streetPositions.Add(roadPosition);
            }
        }

        for (int i = 0; i < 5; i++)
        {
            EvolveHouse();
        }
    }

    public void EvolveHouse()
    {
        
    }

    public void EvolveStreet(Vector2I target)
    {
        foreach (var vector2I in FindPathToTarget(target))
        {
            if (_topLayer.GetCellTileType(vector2I) == null)
            {
                _topLayer.SetCell(vector2I, TopLayerTiles.Street);
                break;
            }
        }
    }

    public List<Vector2I> FindPathToTarget(Vector2I target)
    {
        // Priority Queue for A* (sorted by the lowest fScore)
        var openSet = new PriorityQueue<Vector2I, int>();
        openSet.Enqueue(centerPosition, 0);

        // Dictionaries to keep track of scores and paths
        var cameFrom = new Dictionary<Vector2I, Vector2I>();
        var gScore = new Dictionary<Vector2I, int> { [centerPosition] = 0 };
        var fScore = new Dictionary<Vector2I, int> { [centerPosition] = Heuristic(centerPosition, target) };

        // While there are tiles to explore
        while (openSet.Count > 0)
        {
            Vector2I current = openSet.Dequeue();

            // If we reached the target, reconstruct the path
            if (current == target)
            {
                return ReconstructPath(cameFrom, current);
            }

            foreach (var neighbor in GetNeighbors(current))
            {
                if (!IsValidTile(neighbor)) continue; // Check if neighbor is a valid tile in the grid

                int tileCost = GetTileCost(neighbor);
                if (tileCost == int.MaxValue) continue; // Skip invalid tiles (not "empty" or "street")

                // Calculate tentative gScore
                int tentativeGScore = gScore.GetValueOrDefault(current, int.MaxValue) + tileCost;

                if (tentativeGScore < gScore.GetValueOrDefault(neighbor, int.MaxValue))
                {
                    // Update path and scores for this neighbor
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = tentativeGScore + Heuristic(neighbor, target);

                    // Add neighbor to openSet if not already present
                    if (!openSet.UnorderedItems.Any(item => item.Element == neighbor))
                    {
                        openSet.Enqueue(neighbor, fScore[neighbor]);
                    }
                }
            }
        }

        // No path was found
        return null;
    }

    private bool IsValidTile(Vector2I tile)
    {
        return _topLayer.GetCellTileType(tile) == null || _topLayer.GetCellTileType(tile) == TopLayerTiles.Street;
    }
    
    private int GetTileCost(Vector2I tile)
    {
        switch (_topLayer.GetCellTileType(tile))
        {
            case null:
                return 100;
            case TopLayerTiles.Street:
                return 1;
            default:
                return int.MaxValue;
        }
    }

    private IEnumerable<Vector2I> GetNeighbors(Vector2I position)
    {
        // Assuming 4-directional movement (up, down, left, right)
        return new List<Vector2I>
        {
            position + new Vector2I(0, -1), // Up
            position + new Vector2I(0, 1),  // Down
            position + new Vector2I(-1, 0), // Left
            position + new Vector2I(1, 0)   // Right
        };
    }

    private int Heuristic(Vector2I a, Vector2I b)
    {
        // Use Manhattan distance as the heuristic for grid-based movement
        return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
    }

    private List<Vector2I> ReconstructPath(Dictionary<Vector2I, Vector2I> cameFrom, Vector2I current)
    {
        var path = new List<Vector2I>();
        while (cameFrom.ContainsKey(current))
        {
            path.Add(current);
            current = cameFrom[current];
        }
        path.Reverse();
        return path;
    }
}