using System.Collections.Generic;
using System.Linq;
using BaseLibrary.UI;
using BaseLibrary.Utility;
using Microsoft.Xna.Framework;
using ModularTools.Core;
using Terraria;
using Terraria.ModLoader;

namespace ModularTools
{
	public class UpgradeStationUI : BaseState
	{
		private UIPanel panel;

		private UIPanel itemPanel;
		private UIPanel modulePanel;

		public UpgradeStationUI()
		{
			With(() =>
			{
				panel = new UIPanel
				{
					Width = { Pixels = 1000 },
					Height = { Pixels = 550 + 28 },
					X = { Percent = 50 },
					Y = { Percent = 50 },
					Settings =
					{
						Draggable = true,
						DragZones = new List<DragZone> { new DragZone { Width = { Percent = 100 }, Height = { Pixels = 28 } } }
					}
				};
				Add(panel);

				panel.With(() =>
				{
					UIText title = new UIText("Upgrade Station")
					{
						Height = { Pixels = 20 }
					};
					panel.Add(title);

					UIText closeButton = new UIText("X")
					{
						Height = { Pixels = 20 },
						Width = { Pixels = 20 },
						X = { Percent = 100 }
					};
					closeButton.OnClick += args =>
					{
						args.Handled = true;
						Display = Display.None;
					};
					closeButton.OnMouseEnter += args => closeButton.Settings.TextColor = Color.Red;
					closeButton.OnMouseLeave += args => closeButton.Settings.TextColor = Color.White;
					panel.Add(closeButton);

					itemPanel = new UIPanel
					{
						Width = { Pixels = 64 },
						Height = { Percent = 100, Pixels = -28 },
						Y = { Pixels = 28 },
						Settings =
						{
							BorderColor = Color.Transparent,
							BackgroundColor = DrawingUtility.Colors.PanelSelected * 0.75f
						}
					};
					panel.Add(itemPanel);

					modulePanel = new UIPanel
					{
						Width = { Percent = 40 },
						Height = { Percent = 100, Pixels = -28 },
						X = { Pixels = 72 },
						Y = { Pixels = 28 }
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
					UIModularItem uiModularItem = new UIModularItem(modularItem)
					{
						Width = { Pixels = 48 },
						Height = { Pixels = 48 },
						Y = { Pixels = 58 * y++ }
					};
					uiModularItem.OnClick += args =>
					{
						args.Handled = true;
						foreach (UIModularItem m in itemPanel.Children.OfType<UIModularItem>()) m.selected = false;
						uiModularItem.selected = true;
						OpenItem(modularItem);
					};
					itemPanel.Add(uiModularItem);
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
					Width = { Percent = 100},
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