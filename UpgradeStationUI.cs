using System.Linq;
using BaseLibrary.UI;
using Microsoft.Xna.Framework;
using ModularTools.Core;
using Terraria;
using Terraria.ModLoader;

namespace ModularTools
{
	public class UpgradeStationUI : BaseState
	{
		private UIPanel modulePanel;

		public UpgradeStationUI()
		{
			UIDraggablePanel panel = new UIDraggablePanel
			{
				Width = { Pixels = 1000 },
				Height = { Pixels = 550 },
				X = { Percent = 50 },
				Y = { Percent = 50 }
			};
			Add(panel);

			modulePanel = new UIPanel
			{
				Width = { Pixels = 1000 - 58 - 16 },
				Height = { Percent = 100 },
				X = { Pixels = 58 }
			};
			panel.Add(modulePanel);

			// int y = 0;
			// foreach (Item item in Main.LocalPlayer.inventory.Concat(Main.LocalPlayer.armor))
			// {
			// 	if (item.IsAir) continue;
			//
			// 	if (item.modItem is ModularItem modularItem)
			// 	{
			// 		UIPanel mPanel = new UIPanel();
			// 		mPanel.Width.Set(50f, 0f);
			// 		mPanel.Height.Set(50f, 0f);
			// 		mPanel.Top.Set(50f * y++, 0f);
			// 		mPanel.SetPadding(8);
			// 		mPanel.OnClick += (evt, element) => OpenItem(modularItem);
			// 		panel.Append(mPanel);
			//
			// 		Main.instance.LoadItem(modularItem.Type);
			// 		UIImage image = new UIImage(TextureAssets.Item[modularItem.Type]) { ScaleToFit = true };
			// 		image.Width.Set(0f, 100f);
			// 		image.Height.Set(0f, 100f);
			// 		mPanel.Append(image);
			// 	}
			// }
		}

		private void OpenItem(ModularItem item)
		{
			modulePanel.Clear();

			int y = 0;
			foreach (int type in ModuleLoader.validModulesForItem[item.Type])
			{
				BaseModule module = ModuleLoader.modules[type];

				UIPanel mPanel = new UIPanel
				{
					BorderColor = item.IsInstalled(module.Type) ? Color.LimeGreen : Color.Red,
					Width = { Pixels = 400 },
					Height = { Pixels = 64 },
					Y = { Pixels = 72 * y++ }
				};
				mPanel.OnClick += args =>
				{
					if (item.IsInstalled(module.Type))
					{
						BaseModule clone = item.InstalledModules.First(x => x.Type == module.Type);
						item.InstalledModules.Remove(clone);
						clone.OnRemoved(item);
					}
					else
					{
						BaseModule clone = module.Clone();
						item.InstalledModules.Add(clone);
						clone.OnInstalled(item);
					}

					mPanel.BorderColor = item.IsInstalled(module.Type) ? Color.LimeGreen : Color.Red;

					args.Handled = true;
				};
				modulePanel.Add(mPanel);

				// UIImage image = new UIImage(ModContent.GetTexture(module.Texture));
				// image.ScaleToFit = true;
				// image.Width.Set(48f, 0f);
				// image.Height.Set(48f, 0f);
				// mPanel.Append(image);
				//
				// UIText text = new UIText(module.DisplayName.GetDefault() + "\n" + module.Tooltip.GetDefault()) { TextOriginX = 0f };
				// text.Left.Set(56f, 0f);
				// text.Width.Set(-56f, 100f);
				// text.Height.Set(0f, 100f);
				// mPanel.Append(text);
			}
		}

		public void Open()
		{
			
		}
	}

	// todo: add upgrade station
	public class UpgradeStationUISystem : ModSystem
	{
		public static UpgradeStationUISystem Instance => ModContent.GetInstance<UpgradeStationUISystem>();

		public UpgradeStationUI upgradeState;

		public override void Load()
		{
			if (!Main.dedServ)
			{
				upgradeState = new UpgradeStationUI { Display = Display.None };
				UILayer.Instance.Add(upgradeState);
			}
		}
	}
}