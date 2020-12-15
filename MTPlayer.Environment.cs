using System;
using BaseLibrary.Utility;
using ModularTools.Content.Items.Armor;
using Terraria;
using Terraria.ID;

namespace ModularTools
{
	public partial class MTPlayer
	{
		public bool WearingModularSet =>
			!player.armor[0].IsAir && player.armor[0].modItem is ModularHelmet &&
			!player.armor[1].IsAir && player.armor[1].modItem is ModularChestplate &&
			!player.armor[2].IsAir && player.armor[2].modItem is ModularLeggings;

		public float AmbientTemperature;
		public float heatBalancing;

		public void UpdateEnvironment()
		{
			float surface = (float)(Main.worldSurface * 2 - MathUtility.Lerp((Main.maxTilesY - 1200) / 1200f, 150, 250));
			Layer layer = GetLayer(player, surface, out int depth);

			float safeZone = 200f;

			// safe zone 
			AmbientTemperature = Utility.ToKelvin(20f);
			if (Math.Abs(depth) < safeZone) AmbientTemperature = Utility.ToKelvin(20f) + Math.Sign(depth) * MathUtility.Map(Math.Abs(depth), 0, safeZone, 0f, 10f);
			else
			{
				// bellow ground
				if (depth > 0) AmbientTemperature = MathUtility.Map(depth, safeZone, Main.maxTilesY * 2 - surface, Utility.ToKelvin(30f), Utility.ToKelvin(700f));
				// above ground
				else AmbientTemperature = MathUtility.Map(depth, -safeZone, -surface, Utility.ToKelvin(10f), Utility.ToKelvin(-50f));
			}

			if (player.ZoneSnow)
			{
				AmbientTemperature -= 50f;
			}
			else if (player.ZoneDesert || player.ZoneBeach)
			{
				AmbientTemperature += 20f;
			}
			else if (player.ZoneJungle)
			{
				AmbientTemperature += 10f;
			}

			float transfered;
			if (WearingModularSet)
			{
				ModularHelmet helmet = (ModularHelmet)player.armor[0].modItem;
				ModularChestplate chestplate = (ModularChestplate)player.armor[1].modItem;
				ModularLeggings leggings = (ModularLeggings)player.armor[2].modItem;

				helmet.HeatStorage.TransferToEnvironment(AmbientTemperature, 1f);
				chestplate.HeatStorage.TransferToEnvironment(AmbientTemperature, 1f);
				leggings.HeatStorage.TransferToEnvironment(AmbientTemperature, 1f);

				transfered = helmet.HeatStorage.TransferTo(PlayerHeat, helmet.insulation);
				transfered += chestplate.HeatStorage.TransferTo(PlayerHeat, chestplate.insulation);
				transfered += leggings.HeatStorage.TransferTo(PlayerHeat, leggings.insulation);
			}
			else
			{
				transfered = PlayerHeat.TransferToEnvironment(AmbientTemperature, 1f);
			}

			// heating
			heatBalancing = 120f;
			if (transfered < 0)
			{
				if (!player.HeldItem.IsAir && ItemID.Sets.Torches[player.HeldItem.type] || player.adjTile[TileID.Torches])
					heatBalancing += 200f;
				if (player.adjTile[TileID.Candles] || player.adjTile[TileID.Candelabras] || player.adjTile[TileID.Chandeliers])
					heatBalancing += 5f;
				if (player.HasBuff(BuffID.Warmth) || player.HasBuff(BuffID.Campfire))
					heatBalancing += 100f;
				PlayerHeat.ModifyHeat(Math.Min(-transfered, heatBalancing));
			}
			// cooling
			else PlayerHeat.ModifyHeat(-Math.Min(transfered, heatBalancing));
		}

		internal enum Layer
		{
			Space,
			Surface,
			Underground,
			Caverns,
			Underworld
		}

		internal static Layer GetLayer(Player player, float surface, out int depth)
		{
			int signedDepth = (int)((player.position.Y + player.height) * 2f / 16f - surface);
			int normalizedWidth = Main.maxTilesX / 4200;
			normalizedWidth *= normalizedWidth;

			Layer layer = Layer.Surface;

			float num25 = (float)((player.Center.Y / 16f - (65f + 10f * normalizedWidth)) / (Main.worldSurface / 5.0));
			if (player.position.Y > (Main.maxTilesY - 204) * 16) layer = Layer.Underworld;
			else if (player.position.Y > Main.rockLayer * 16.0 + 600 + 16.0) layer = Layer.Caverns;
			else if (signedDepth > 0) layer = Layer.Underground;
			else if (!(num25 >= 1f)) layer = Layer.Space;

			depth = signedDepth;
			return layer;
		}
	}
}