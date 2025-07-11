using System.Collections.Generic;
using Godot;

namespace NewArcana;

public class Settlement
{
    public Dictionary<Vector2I, House> Houses = new Dictionary<Vector2I, House>();
    private TopLayer _topLayer;

    public Settlement(TopLayer topLayer)
    {
        _topLayer = topLayer;
    }

    public void SpawnSettlement(Vector2I position)
    {
        for (int i = 0; i < 5; i++)
        {
            int x = GD.RandRange(-3, 3);
            int y = GD.RandRange(-3, 3);
            CreateHouse(position + new Vector2I(x, y));
        }
        ConnectRoads();
    }

    private void CreateHouse(Vector2I position)
    {
        if (Houses.ContainsKey(position))
            return;
        House house = new House();
        Houses.Add(position, house);
        _topLayer.SetCell(position, TopLayerTiles.House);
    }

    private void ConnectRoads()
    {
        List<Vector2I> housePositions = new List<Vector2I>(Houses.Keys);
        HashSet<Vector2I> visited = new HashSet<Vector2I>();
        PriorityQueue<(Vector2I from, Vector2I to, int distance), int> priorityQueue = new PriorityQueue<(Vector2I from, Vector2I to, int distance), int>();

        visited.Add(housePositions[0]);

        foreach (Vector2I house in housePositions)
        {
            if (house != housePositions[0])
            {
                int distance = ManhattanDistance(housePositions[0], house);
                priorityQueue.Enqueue((housePositions[0], house, distance), distance);
            }
        }

        while (visited.Count < housePositions.Count && priorityQueue.Count > 0)
        {
            (Vector2I from, Vector2I to, int distance) = priorityQueue.Dequeue();

            if (visited.Contains(to))
                continue;

            visited.Add(to);
            CreateRoadBetween(from, to);

            foreach (Vector2I house in housePositions)
            {
                if (!visited.Contains(house))
                {
                    int newDistance = ManhattanDistance(to, house);
                    priorityQueue.Enqueue((to, house, newDistance), newDistance);
                }
            }
        }
    }

    private int ManhattanDistance(Vector2I a, Vector2I b)
    {
        return Mathf.Abs(a.X - b.X) + Mathf.Abs(a.Y - b.Y);
    }

    private void CreateRoadBetween(Vector2I start, Vector2I end)
    {
        Vector2I current = start;

        while (current.X != end.X)
        {
            current.X += current.X < end.X ? 1 : -1;

            if (!Houses.ContainsKey(current))
            {
                _topLayer.SetCell(current, TopLayerTiles.Street);
            }
        }

        while (current.Y != end.Y)
        {
            current.Y += current.Y < end.Y ? 1 : -1;

            if (!Houses.ContainsKey(current))
            {
                _topLayer.SetCell(current, TopLayerTiles.Street);
            }
        }
    }
}