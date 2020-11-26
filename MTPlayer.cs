using ModularTools.Content.Items.Ingredients;
using ModularTools.Core;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace ModularTools
{
	public partial class MTPlayer : ModPlayer
	{
		public HeatStorage PlayerHeat = new HeatStorage
		{
			Capacity = 80f * 3500f,
			Temperature = Utility.ToKelvin(37),
			Area = 1.9f,
			TransferCoefficient = 3f
		};

		public override void PreUpdate()
		{
			UpdateEnvironment();
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