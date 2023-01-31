using BaseLibrary.UI;
using Terraria;
using Terraria.ModLoader;

namespace ModularTools.UI;

public class UpgradeStationUISystem : ModSystem
{
	public static UpgradeStationUISystem Instance => ModContent.GetInstance<UpgradeStationUISystem>();

	private UpgradeStationUI ui;

	public override void Load()
	{
		if (!Main.dedServ)
		{
			ui = new UpgradeStationUI { Display = Display.None };
			UILayer.Instance.Add(ui);
		}
	}

	public void HandleUI()
	{
		ref Display display = ref ui.Display;

		if (display == Display.None)
		{
			UILayer.Instance.Remove(ui);
			ui = new UpgradeStationUI { Display = Display.Visible };
			UILayer.Instance.Add(ui);
			ui.Open();
		}
		else display = Display.None;
	}
}