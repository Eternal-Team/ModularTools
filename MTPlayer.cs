using System;
using BaseLibrary.Utility;
using ModularTools.Content.Items.Ingredients;
using ModularTools.Core;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace ModularTools
{
	public class MTPlayer : ModPlayer
	{
		public float AmbientTemperature = Utility.ToKelvin(50f);

		public HeatStorage PlayerHeat = new HeatStorage
		{
			Capacity = 80f * 3500f,
			Temperature = Utility.ToKelvin(37),
			Area = 1.9f,
			TransferCoefficient = 3f
		};

		// hell is 1000 C
		// 3°C per 100m

		// 731 meter

		private enum Layer
		{
			Space,
			Surface,
			Underground,
			Caverns,
			Underworld
		}

		private static Layer GetLayer(Player player, float surface, out int depth)
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

		public override void PreUpdate()
		{
			// 150 small (2400), 250 large (4800)

			float surface = (float)(Main.worldSurface * 2 - MathUtility.Lerp((Main.maxTilesX - 1200) / 1200f, 150, 250));
			Layer layer = GetLayer(player, surface, out int depth);

			Main.NewText("depth: " + depth);

			float safeZone = 200f;

			// safe zone 
			float temp = Utility.ToKelvin(20F);
			if (Math.Abs(depth) < safeZone) temp = Utility.ToKelvin(20f) + Math.Sign(depth) * MathUtility.Map(Math.Abs(depth), 0, safeZone, 0f, 10f);
			else
			{
				// bellow ground
				if (depth > 0) temp = MathUtility.Map(depth, safeZone, Main.maxTilesY * 2 - surface, Utility.ToKelvin(30f), Utility.ToKelvin(700f));
				// above ground
				else temp = MathUtility.Map(depth, -safeZone, -surface, Utility.ToKelvin(10f), Utility.ToKelvin(-50f));
			}

			float transfered = PlayerHeat.TransferToEnvironment(temp, 1f);

			if (transfered < 0) PlayerHeat.ModifyHeat(Math.Min(-transfered, 120));
			else PlayerHeat.ModifyHeat(-Math.Min(transfered, 120));

			Main.NewText($"Current ambient temperature {Utility.ToDegrees(temp):F1} C");
			Main.NewText($"Player temperature {Utility.ToDegrees(PlayerHeat.Temperature):F1} C");
		}

		public override void ProcessTriggers(TriggersSet triggersSet)
		{
			if (ModularTools.Instance.hotKey.JustPressed)
			{
				ref bool visible = ref UpgradeStationUISystem.Instance.upgradeState.Visible;
				if (!visible)
				{
					UpgradeStationUISystem.Instance.upgradeState.OnInitialize();
					visible = true;
				}
				else visible = false;
			}
		}

		public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
		{
			player.QuickSpawnItem(ModContent.ItemType<Brain>());
		}
	}
}