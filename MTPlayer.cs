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
	}
}