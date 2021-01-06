using System.Collections.Generic;
using System.Linq;
using BaseLibrary.UI;
using Microsoft.Xna.Framework;
using ModularTools.Core;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace ModularTools
{
	public class UpgradeStationUI : BaseState
	{
		private UIPanel panel;
		private UIPanel modulePanel;

		public UpgradeStationUI()
		{
			With(() =>
			{
				panel = new UIPanel
				{
					Settings =
					{
						Draggable = true,
						DragZones = new List<DragZone> { DragZone.TopBorder }
					},
					Width = { Pixels = 1000 },
					Height = { Pixels = 550 },
					X = { Percent = 50 },
					Y = { Percent = 50 }
				};
				Add(panel);

				panel.With(() =>
				{
					modulePanel = new UIPanel
					{
						Width = { Pixels = 1000 - 58 - 16 },
						Height = { Percent = 100 },
						X = { Pixels = 58 }
					};
					panel.Add(modulePanel);
				});
			});
		}

		public void Open()
		{
			int y = 0;
			foreach (Item item in Main.LocalPlayer.inventory.Concat(Main.LocalPlayer.armor))
			{
				if (item.IsAir) continue;

				if (item.modItem is ModularItem modularItem)
				{
					UIPanel mPanel = new UIPanel
					{
						Width = { Pixels = 50 },
						Height = { Pixels = 50 },
						Y = { Pixels = 58 * y++ },
						Padding = new Padding(8)
					};
					mPanel.OnClick += args =>
					{
						args.Handled = true;
						OpenItem(modularItem);
					};
					panel.Add(mPanel);

					Main.instance.LoadItem(modularItem.Type);
					UITexture image = new UITexture(TextureAssets.Item[modularItem.Type].Value)
					{
						Width = { Percent = 100 },
						Height = { Percent = 100 },
						Settings =
						{
							ScaleMode = ScaleMode.Zoom,
							ImageX = { Percent = 50 },
							ImageY = { Percent = 50 }
						}
					};
					mPanel.Add(image);
				}
			}
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
					Settings = { BorderColor = item.IsInstalled(module.Type) ? Color.LimeGreen : Color.Red },
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

					mPanel.Settings.BorderColor = item.IsInstalled(module.Type) ? Color.LimeGreen : Color.Red;

					args.Handled = true;
				};
				modulePanel.Add(mPanel);

				UITexture image = new UITexture(ModContent.GetTexture(module.Texture).Value)
				{
					Width = { Pixels = 48 },
					Height = { Pixels = 48 },
					Settings =
					{
						ScaleMode = ScaleMode.Stretch,
						ImageX = { Percent = 50 },
						ImageY = { Percent = 50 }
					}
				};
				mPanel.Add(image);

				UIText text = new UIText(module.DisplayName.GetDefault() + "\n" + module.Tooltip.GetDefault())
				{
					Width = { Pixels = -56, Percent = 100 },
					Height = { Percent = 100 },
					X = { Pixels = 56 }
				};
				mPanel.Add(text);
			}
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