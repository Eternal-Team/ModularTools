using System.Collections.Generic;
using BaseLibrary.Utility;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;

namespace ModularTools
{
	public class DebugUISystem : ModSystem
	{
		public static DebugUISystem Instance => ModContent.GetInstance<DebugUISystem>();

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
			if (mouseTextIndex != -1)
			{
				layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
					"ModularTools: DebugUI",
					delegate
					{
						MTPlayer player = Main.LocalPlayer.GetModPlayer<MTPlayer>();

						float x = 20f;
						float y = 100f;
						DebugPrint($"Ambient temperature: {Utility.ToDegrees(player.AmbientTemperature):F1} C");
						DebugPrint($"Player temperature: {Utility.ToDegrees(player.PlayerHeat.Temperature):F1} C");
						DebugPrint($"Player temp balancing: {player.heatBalancing:F1} W");

						float surface = (float)(Main.worldSurface * 2 - MathUtility.Lerp((Main.maxTilesY - 1200) / 1200f, 150, 250));
						MTPlayer.Layer layer = MTPlayer.GetLayer(player.player, surface, out int depth);
						DebugPrint($"Depth: {depth} ft");

						void DebugPrint(string text)
						{
							ChatManager.DrawColorCodedString(Main.spriteBatch, FontAssets.MouseText.Value, text, new Vector2(x, y += 20f), Color.White, 0f, Vector2.Zero, Vector2.One);
						}

						return true;
					},
					InterfaceScaleType.UI)
				);
			}
		}
	}
}