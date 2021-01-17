using System.Collections.Generic;
using System.Linq;
using BaseLibrary.Utility;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModularTools.Content.Tiles
{
	public class Sulfur : ModTile
	{
		public override string Texture => ModularTools.TexturePath + "Tiles/Sulfur";

		public override void SetDefaults()
		{
			Main.tileShine[Type] = 300;
			Main.tileNoFail[Type] = true;
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;

			AddMapEntry(Color.Yellow);
		}

		// bug: player places tiles still drop >1 items
		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			Item.NewItem(i * 16, j * 16, 16, 16, ModContent.ItemType<Items.Ingredients.Sulfur>(), Main.rand.Next(1, 4));
		}

		internal static bool IsValid(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			return i >= 0 && i <= Main.maxTilesX && j >= 0 && j <= Main.maxTilesY && tile != null && !tile.IsAir && !tile.IsActuated && !tile.IsHalfBrick && Main.tileSolid[tile.type];
		}

		internal static IEnumerable<(Direction, Tile)> GetNeighbors(int i, int j)
		{
			if (IsValid(i, j - 1))
			{
				Tile tile = Main.tile[i, j - 1];
				if (tile.Slope != SlopeID.SlopeUpLeft && tile.Slope != SlopeID.SlopeUpRight) yield return (Direction.Up, tile);
			}

			if (IsValid(i, j + 1))
			{
				Tile tile = Main.tile[i, j + 1];
				if (tile.Slope != SlopeID.SlopeDownLeft && tile.Slope != SlopeID.SlopeDownRight) yield return (Direction.Down, tile);
			}

			if (IsValid(i - 1, j))
			{
				Tile tile = Main.tile[i - 1, j];
				if (tile.Slope != SlopeID.SlopeDownLeft && tile.Slope != SlopeID.SlopeUpLeft) yield return (Direction.Left, tile);
			}

			if (IsValid(i + 1, j))
			{
				Tile tile = Main.tile[i + 1, j];
				if (tile.Slope != SlopeID.SlopeDownRight && tile.Slope != SlopeID.SlopeUpRight) yield return (Direction.Right, tile);
			}
		}

		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
		{
			Tile tile = Main.tile[i, j];
			int style = WorldGen._genRand.Next(4);
			tile.frameX = (short)(style * 18);

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
					tile.frameY = 18;
					break;
				}

				if (direction == Direction.Down)
				{
					tile.frameY = 0;
					break;
				}

				if (direction == Direction.Left)
				{
					tile.frameY = 54;
					break;
				}

				if (direction == Direction.Right)
				{
					tile.frameY = 36;
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
}