using System;
using System.Collections.Generic;
using Godot;

namespace NewArcana.Scripts.World;

public abstract partial class LayerBase<T> : TileMapLayer where T : struct, Enum
{
	private ShaderMaterial _fadeMaterial;
	private Dictionary<Vector2I, Sprite2D> _fadeSprites = new Dictionary<Vector2I, Sprite2D>();
	public override void _Ready()
	{
		_fadeMaterial = new ShaderMaterial
		{
			Shader = GD.Load<Shader>("res://tile_fade_shader.gdshader")
		};
	}

	public void SetCell(Vector2I position, T tile)
	{
		(int sourceId, Vector2I coords) = GetTileInfo(tile);

		Sprite2D sprite = new Sprite2D();
		Vector2I tileSize = new Vector2I(32, 32);
		TileSetAtlasSource sourceTexture = TileSet.GetSource(sourceId) as TileSetAtlasSource;
		Rect2I regionRect = new Rect2I(coords * tileSize, tileSize);
		Image image = sourceTexture.Texture.GetImage();
		Image tileImage = image.GetRegion(regionRect);
		sprite.Texture = ImageTexture.CreateFromImage(tileImage);

		
		ShaderMaterial spriteMaterial = _fadeMaterial.Duplicate() as ShaderMaterial;
		spriteMaterial.SetShaderParameter("alpha", 0.0);
		sprite.Material = spriteMaterial;
		
		sprite.Offset = -(tileSize / 2);
		sprite.Position = MapToLocal(position) + (tileSize / 2);

		sprite.Visible = true;

		AddChild(sprite);
		
		if (_fadeSprites.ContainsKey(position))
		{
			_fadeSprites[position].QueueFree();
		}
		_fadeSprites[position] = sprite;

		Tween tween = CreateTween();
		tween.Play();
		
		tween.TweenProperty(sprite.Material, "shader_parameter/alpha", 1.0, 0.3f)
			.From(0.0)
			.SetTrans(Tween.TransitionType.Sine)
			.SetEase(Tween.EaseType.Out);
		
		tween.TweenCallback(Callable.From(() =>
		{
			sprite.QueueFree();
			_fadeSprites.Remove(position);
			SetCell(position, sourceId, coords);
		}));
	}
	
	public override void _ExitTree()
	{
		base._ExitTree();
		// Clean up any remaining sprites
		foreach (Sprite2D sprite in _fadeSprites.Values)
		{
			sprite.QueueFree();
		}
		_fadeSprites.Clear();
	}

	public T? GetCellTileType(Vector2I position)
	{
		int cellSourceId = GetCellSourceId(position);
		Vector2I cellTileSetPosition = GetCellAtlasCoords(position);

		foreach (T tile in Enum.GetValues(typeof(T)))
		{
			(int id, Vector2I tileSetPosition) = GetTileInfo(tile);

			if (cellSourceId == id && cellTileSetPosition == tileSetPosition) return tile;
		}

		return null;
	}

	protected abstract (int, Vector2I) GetTileInfo(T tile);

	public bool IsCellOfType(Vector2I position, T tileType)
	{
		(int id, Vector2I tileSetPosition) = GetTileInfo(tileType);
		return GetCellSourceId(position) == id && GetCellAtlasCoords(position) == tileSetPosition;
	}
}
