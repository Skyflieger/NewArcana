using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using NewArcana;

public partial class TopLayer : TileMapLayer
{
    public List<Settlement> Settlements { get; set; } = new List<Settlement>();
    public List<Sanctuary> Sanctuaries { get; set; } = new List<Sanctuary>();

    private const float MinDistance = 20.0f;

    public override void _Ready()
    {
        CreateSettlement();
        CreateSettlement();
        CreateSettlement();
        CreateSanctuary();
        CreateSanctuary();
        CreateSanctuary();

        ConnectSettlementsWithRoads();
    }

    public Settlement CreateSettlement()
    {
        Settlement settlement = new Settlement(this);
        Vector2I position;

        do
        {
            position = new Vector2I(GD.RandRange(-30, 30), GD.RandRange(-30, 30));
        } while (!IsPositionValidForSettlement(position));

        settlement.SpawnSettlement(position);
        Settlements.Add(settlement);

        return settlement;
    }

    public Sanctuary CreateSanctuary()
    {
        Sanctuary sanctuary = new Sanctuary(this);
        Vector2I position;

        do
        {
            position = new Vector2I(GD.RandRange(-30, 30), GD.RandRange(-30, 30));
        } while (!IsPositionValidForSanctuary(position));

        sanctuary.SpawnSanctuary(position);
        Sanctuaries.Add(sanctuary);

        return sanctuary;
    }

    private bool IsPositionValidForSettlement(Vector2I position)
    {
        foreach (Settlement settlement in Settlements)
        {
            if (position.DistanceTo(GetSettlementCenter(settlement)) < MinDistance)
                return false;
        }

        foreach (Sanctuary sanctuary in Sanctuaries)
        {
            if (position.DistanceTo(GetSanctuaryCenter(sanctuary)) < MinDistance)
                return false;
        }

        return true;
    }

    private bool IsPositionValidForSanctuary(Vector2I position)
    {
        foreach (Sanctuary sanctuary in Sanctuaries)
        {
            if (position.DistanceTo(GetSanctuaryCenter(sanctuary)) < MinDistance)
                return false;
        }

        foreach (Settlement settlement in Settlements)
        {
            if (position.DistanceTo(GetSettlementCenter(settlement)) < MinDistance)
                return false;
        }

        return true;
    }

    private Vector2I GetSettlementCenter(Settlement settlement)
    {
        return settlement.Houses.Count > 0 ? new Vector2I(settlement.Houses.Keys.First().X, settlement.Houses.Keys.First().Y) : Vector2I.Zero;
    }

    private Vector2I GetSanctuaryCenter(Sanctuary sanctuary)
    {
        return sanctuary.Trees.Count > 0 ? new Vector2I(sanctuary.Trees.Keys.First().X, sanctuary.Trees.Keys.First().Y) : Vector2I.Zero;
    }

    public void SetCell(Vector2I position, TopLayerTiles tile)
    {
        int sourceId = 0;
        Vector2I tileSetPosition = Vector2I.Zero;
        
        switch (tile)
        {
            case TopLayerTiles.House:
                sourceId = 0;
                tileSetPosition = new Vector2I(0, 0);
                break;
            case TopLayerTiles.Street:
                sourceId = 0;
                tileSetPosition = new Vector2I(0, 1);
                break;
            case TopLayerTiles.Tree:
                sourceId = 1;
                tileSetPosition = new Vector2I(0, 0);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(tile), tile, null);
        }
        
        SetCell(position, sourceId, tileSetPosition);
    }

    private void ConnectSettlementsWithRoads()
    {
        if (Settlements.Count < 2)
            return;

        List<Vector2I> settlementCenters = Settlements
            .Select(GetSettlementCenter)
            .Where(center => center != Vector2I.Zero)
            .ToList();

        HashSet<Vector2I> visited = new HashSet<Vector2I>();
        PriorityQueue<(Vector2I from, Vector2I to, int distance), int> priorityQueue = new PriorityQueue<(Vector2I from, Vector2I to, int distance), int>();

        visited.Add(settlementCenters[0]);

        foreach (Vector2I center in settlementCenters)
        {
            if (center != settlementCenters[0])
            {
                int distance = ManhattanDistance(settlementCenters[0], center);
                priorityQueue.Enqueue((settlementCenters[0], center, distance), distance);
            }
        }

        while (visited.Count < settlementCenters.Count && priorityQueue.Count > 0)
        {
            (Vector2I from, Vector2I to, int distance) = priorityQueue.Dequeue();

            if (visited.Contains(to))
                continue;

            visited.Add(to);
            CreateRoadBetween(from, to);

            foreach (Vector2I center in settlementCenters)
            {
                if (!visited.Contains(center))
                {
                    int newDistance = ManhattanDistance(to, center);
                    priorityQueue.Enqueue((to, center, newDistance), newDistance);
                }
            }
        }
    }

    private void CreateRoadBetween(Vector2I start, Vector2I end)
    {
        Vector2I current = start;

        while (current.X != end.X)
        {
            current.X += current.X < end.X ? 1 : -1;
            SetCell(current, TopLayerTiles.Street);
        }

        while (current.Y != end.Y)
        {
            current.Y += current.Y < end.Y ? 1 : -1;
            SetCell(current, TopLayerTiles.Street);
        }
    }

    private int ManhattanDistance(Vector2I a, Vector2I b)
    {
        return Mathf.Abs(a.X - b.X) + Mathf.Abs(a.Y - b.Y);
    }
}