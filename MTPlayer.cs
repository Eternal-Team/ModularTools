using BaseLibrary.UI;
using ModularTools.Content.Items.Ingredients;
using ModularTools.Core;
using ModularTools.UI;
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
				ref UpgradeStationUI ui = ref UpgradeStationUISystem.Instance.upgradeState;
				ref Display display = ref ui.Display;

				if (display == Display.None)
				{
					UILayer.Instance.Remove(ui);
					ui = new UpgradeStationUI { Display = Display.Visible };
					ui.Recalculate();
					UILayer.Instance.Add(ui);
					ui.Open();
				}
				else display = Display.None;
			}
		}

		public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
		{
			player.QuickSpawnItem(ModContent.ItemType<Brain>());
		}
	}
}