using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using NewArcana.Scripts.Cards;
using NewArcana.Scripts.World.Tiles;

namespace NewArcana.Scripts.World;

public partial class Map : Node2D
{
    [Export] private GroundLayer _groudLayer;
    [Export] private BuildingLayer _buildingLayer;
    [Export] private CardHand _cardHand;
    [Export] private TimeManager _timeManager;

    private readonly Dictionary<Vector2I, Tiles.Tile> _tiles = new Dictionary<Vector2I, Tiles.Tile>();
    [Export] private PackedScene _cityScene;
    [Export] private PackedScene _baseScene;
    [Export] private PackedScene _farmScene;

    public void InitializeMap()
    {
        for (int x = -0; x < 10; x++)
        {
            for (int y = -0; y < 10; y++)
            {
                _groudLayer.SetCell(new Vector2I(x, y), GroundLayerTileType.Grass);
                _buildingLayer.SetCell(new Vector2I(x, y), BuildingLayerTileType.Forest);
            }
        }

        float time = 1f;
        Tween tweenBase = CreateTween();
        tweenBase.TweenInterval(time);
        tweenBase.TweenCallback(Callable.From(AddBase));
        time++;
        Tween tweenFarm = CreateTween();
        tweenFarm.TweenInterval(time);
        tweenFarm.TweenCallback(Callable.From(AddFarm));
        time++;

        for(int i = 0; i < 3; i++)
        {
            Tween tweenCity = CreateTween();
            tweenCity.TweenInterval(time + i);
            tweenCity.TweenCallback(Callable.From(AddNewCity));
        }
    }

    public override void _Process(double delta)
    {
        if (Input.IsMouseButtonPressed(MouseButton.Left))
        {
            Vector2 mousePos = GetGlobalMousePosition();
            Vector2I tilePos = _groudLayer.LocalToMap(_groudLayer.ToLocal(mousePos));
            if (_tiles.TryGetValue(tilePos, out Tiles.Tile tile))
            {
                _cardHand.ExecuteCard(tile);
            }
        }
    }

    private Tile AddTile<T>(Vector2I position, BuildingLayerTileType tileType, PackedScene scene) where T : Tiles.Tile
    {
        _buildingLayer.EraseCell(position);
        _buildingLayer.SetCell(position, tileType);
    
        var tile = scene.Instantiate<T>();
        tile.Position = position;
        _tiles.Add(position, tile);
        
        AddChild(tile);

        return tile;
    }

    public void AddBase()
    {
        var position = new Vector2I(4, 4);
        var tile = AddTile<Tiles.Base>(position, BuildingLayerTileType.Base, _baseScene);
        _timeManager.BaseMonthPassed += tile.OnMonthPassed;
        _timeManager.BaseYearPassed += tile.OnYearPassed;
    }

    public void AddNewCity()
    {
        var position = GetRandomForestPosition();
        if (position.HasValue)
        {
            var tile = AddTile<Tiles.City>(position.Value, BuildingLayerTileType.City1, _cityScene);
            
            _timeManager.CityMonthPassed += tile.OnMonthPassed;
            _timeManager.CityYearPassed += tile.OnYearPassed;
        }
    }
    
    public void AddFarm()
    {
        var position = new Vector2I(2, 2);
        var tile = AddTile<Farm>(position, BuildingLayerTileType.Farm, _farmScene);
        _timeManager.FarmMonthPassed += tile.OnMonthPassed;
        _timeManager.FarmYearPassed += tile.OnYearPassed;
    }

    public Vector2I? GetRandomForestPosition()
    {
        List<Vector2I> forestCells = _buildingLayer.GetUsedCells()
            .Where(cell => _buildingLayer.IsCellOfType(cell, BuildingLayerTileType.Forest))
            .ToList();

        if (forestCells.Count == 0)
            return null;
        
        Random random = new Random();
        int randomIndex = random.Next(forestCells.Count);
        return forestCells[randomIndex];
    }

    public Vector2I GetMapPosition(Vector2 position)
    {
        return _groudLayer.LocalToMap(position);
    }

    public Vector2 GetGlobalPosition(Vector2I position)
    {
        return _groudLayer.MapToLocal(position);
    }

    public void ExpandBorders(int expansionSize)
    {
        List<Vector2I> usedCells = _groudLayer.GetUsedCells().ToList();

        if (usedCells.Count == 0) return;

        int minX = usedCells.Min(cell => cell.X);
        int maxX = usedCells.Max(cell => cell.X);
        int minY = usedCells.Min(cell => cell.Y);
        int maxY = usedCells.Max(cell => cell.Y);

        for (int x = minX - expansionSize; x <= maxX + expansionSize; x++)
        {
            for (int y = minY - expansionSize; y <= maxY + expansionSize; y++)
            {
                Vector2I position = new Vector2I(x, y);
                if (_groudLayer.GetCellSourceId(position) == -1)
                {
                    _groudLayer.SetCell(position, GroundLayerTileType.Grass);
                    _buildingLayer.SetCell(position, BuildingLayerTileType.Forest);
                }
            }
        }
    }

    
    public void OnMonthPassed(int month, int year)
    {
        if (month % 3 == 0)
        {
            AddNewCity();
        }
    }

    public void OnYearPassed(int year)
    {
        if (year % 6 == 0)
        {
            ExpandBorders(1);
        }
    }
}