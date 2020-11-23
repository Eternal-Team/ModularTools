using ModularTools.Content.Items.Ingredients;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace ModularTools
{
	public class MTPlayer : ModPlayer
	{
		public override void ProcessTriggers(TriggersSet triggersSet)
		{
			if (ModularTools.Instance.hotKey.JustPressed)
			{
				UpgradeStationUISystem.Instance.upgradeState.OnInitialize();
				UpgradeStationUISystem.Instance.upgradeState.Visible = true;
			}
		}

		public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
		{
			player.QuickSpawnItem(ModContent.ItemType<Brain>());
		}
	}
}