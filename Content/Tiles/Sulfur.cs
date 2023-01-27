using System.Collections.Generic;
using System.Linq;
using BaseLibrary.Utility;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModularTools.Content.Tiles;

public class Sulfur : ModTile
{
	public override string Texture => ModularTools.TexturePath + "Tiles/Sulfur";

	public override void SetStaticDefaults()
	{
		Main.tileShine[Type] = 300;
		Main.tileNoFail[Type] = true;
		Main.tileLighted[Type] = true;
		Main.tileFrameImportant[Type] = true;

		AddMapEntry(Color.Yellow);
	}

	public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
	{
		//MathUtility.IsBitSet(Main.tile[i, j].sTileHeader, 15) ? 1 : Main.rand.Next(1, 4)
		Item.NewItem(new EntitySource_TileBreak(i,j),i * 16, j * 16, 16, 16, ModContent.ItemType<Items.Ingredients.Sulfur>(), 1);
	}

	internal static bool IsValid(int i, int j)
	{
		Tile tile = Main.tile[i, j];
		return i >= 0 && i <= Main.maxTilesX && j >= 0 && j <= Main.maxTilesY && tile != null && tile is { HasTile: true, IsActuated: false, IsHalfBlock: false } && Main.tileSolid[tile.TileType];
	}

	internal static IEnumerable<(Direction, Tile)> GetNeighbors(int i, int j)
	{
		if (IsValid(i, j - 1))
		{
			Tile tile = Main.tile[i, j - 1];
			if (tile.Slope != SlopeType.SlopeUpLeft && tile.Slope != SlopeType.SlopeUpRight) yield return (Direction.Up, tile);
		}

		if (IsValid(i, j + 1))
		{
			Tile tile = Main.tile[i, j + 1];
			if (tile.Slope != SlopeType.SlopeDownLeft && tile.Slope != SlopeType.SlopeDownRight) yield return (Direction.Down, tile);
		}

		if (IsValid(i - 1, j))
		{
			Tile tile = Main.tile[i - 1, j];
			if (tile.Slope != SlopeType.SlopeDownLeft && tile.Slope != SlopeType.SlopeUpLeft) yield return (Direction.Left, tile);
		}

		if (IsValid(i + 1, j))
		{
			Tile tile = Main.tile[i + 1, j];
			if (tile.Slope != SlopeType.SlopeDownRight && tile.Slope != SlopeType.SlopeUpRight) yield return (Direction.Right, tile);
		}
	}

	public override void PlaceInWorld(int i, int j, Item item)
	{
		Tile tile = Main.tile[i, j];
		int style = Main.rand.Next(4);
		tile.TileFrameX = (short)(style * 18);

		// MathUtility.SetBit(ref tile.sTileHeader, 15);
	}

	public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
	{
		Tile tile = Main.tile[i, j];

		var neighbors = GetNeighbors(i, j).ToList();
		if (neighbors.Count == 0)
		{
			WorldGen.KillTile(i, j);
			return false;
		}

		foreach (var (direction, neighbor) in neighbors)
		{
			if (direction == Direction.Up)
			{
				tile.TileFrameY = 18;
				break;
			}

			if (direction == Direction.Down)
			{
				tile.TileFrameY = 0;
				break;
			}

			if (direction == Direction.Left)
			{
				tile.TileFrameY = 54;
				break;
			}

			if (direction == Direction.Right)
			{
				tile.TileFrameY = 36;
				break;
			}
		}

		return false;
	}

	public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
	{
		r = 0.3529412f;
		g = 0.3529412f;
		b = 0f;
	}
}