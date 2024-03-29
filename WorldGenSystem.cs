﻿using System.Collections.Generic;
using System.Linq;
using BaseLibrary.Utility;
using Microsoft.Xna.Framework;
using ModularTools.Content.Tiles;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace ModularTools;

public class WorldGenSystem : ModSystem
{
	private class GenerateSulfurAction : GenAction
	{
		public override bool Apply(Point origin, int x, int y, params object[] args)
		{
			x = MathUtility.Clamp(x, 1, Main.maxTilesX - 1);
			y = MathUtility.Clamp(y, 1, Main.maxTilesY - 1);

			if (!Main.tile[x, y].HasTile) return false;

			if (!Sulfur.GetNeighbors(x,y).Any()) return false;

			int found = 0;
			const int checkSize = 3;
			for (int i = x - checkSize; i <= x + checkSize; i++)
			{
				for (int j = y - checkSize; j <= y + checkSize; j++)
				{
					if (i < 1 || i > Main.maxTilesX - 1 || j < 1 || j > Main.maxTilesY - 1) continue;

					if (Main.tile[i, j] != null && Main.tile[i, j].HasTile && Main.tile[i, j].TileType == ModContent.TileType<Sulfur>()) found++;
				}
			}

			if (found < 2)
			{
				WorldGen.PlaceTile(x, y, ModContent.TileType<Sulfur>(), true);
				Tile tile = Main.tile[x, y];
				int style = WorldGen._genRand.Next(4);
				tile.TileFrameX = (short)(style * 18);
			}

			return true;
		}
	}

	public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
	{
		int index = tasks.FindLastIndex(x => x.Name == "Settle Liquids Again");
		if (index != -1)
		{
			tasks.Insert(index + 1, new PassLegacy("ModularTools:Sulfur", (progress, configuration) =>
			{
				progress.Message = "Crystallizing sulfur";

				// foreach (Point poolLocation in Hooking.PoolLocations)
				// {
				// 	if (poolLocation.Y > WorldGen.lavaLine) WorldUtils.Gen(poolLocation, new Shapes.Circle(10), new GenerateSulfurAction());
				// }
			}));
		}
	}
}