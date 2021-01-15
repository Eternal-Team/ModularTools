using System.Collections.Generic;
using System.Linq;
using BaseLibrary.UI;
using BaseLibrary.Utility;
using Microsoft.Xna.Framework;
using ModularTools.Core;
using Terraria;
using Terraria.ModLoader;

namespace ModularTools.UI
{
	public class UpgradeStationUI : BaseState
	{
		private UIGrid<UIModularItem> gridItems;
		private UIGrid<UIModule> gridModules;
		private Ref<string> search = new Ref<string>("");
		private UIPanel panelInfo;
		private ModularItem selectedItem;
		private BaseModule selectedModule;

		public UpgradeStationUI()
		{
			UIPanel panel = new UIPanel
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

				UIPanel itemPanel = new UIPanel
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

				gridItems = new UIGrid<UIModularItem>
				{
					Width = { Percent = 100 },
					Height = { Percent = 100 }
				};
				itemPanel.Add(gridItems);

				UIPanel modulePanel = new UIPanel
				{
					Width = { Percent = 40, Pixels = -64 },
					Height = { Percent = 100, Pixels = -28 },
					X = { Pixels = 64 },
					Y = { Pixels = 28 },
					Settings =
					{
						BorderColor = Color.Transparent,
						BackgroundColor = DrawingUtility.Colors.PanelSelected * 0.75f
					}
				};
				panel.Add(modulePanel);

				UIPanel inputBG = new UIPanel
				{
					Width = { Percent = 100 },
					Height = { Pixels = 36 },
					Settings = { BorderColor = Color.Transparent }
				};
				modulePanel.Add(inputBG);

				UITextInput input = new UITextInput(ref search)
				{
					Width = { Percent = 100 },
					Height = { Percent = 100 },
					OnTextChange = () => gridModules.Search(),
					Settings = { HintText = "<Search>" }
				};
				inputBG.Add(input);

				// todo: category buttons

				gridModules = new UIGrid<UIModule>
				{
					Width = { Percent = 100 },
					Height = { Percent = 100, Pixels = -44 },
					Y = { Pixels = 44 },
					SearchSelector = item => string.IsNullOrWhiteSpace(search.Value) || TextUtility.Search(item.module.DisplayName.Get().ToLower(), search.Value.ToLower()).Any()
				};
				modulePanel.Add(gridModules);

				panelInfo = new UIPanel
				{
					Width = { Percent = 60 },
					Height = { Percent = 100, Pixels = -28 },
					X = { Percent = 100 },
					Y = { Pixels = 28 },
					Settings =
					{
						BorderColor = Color.Transparent,
						BackgroundColor = DrawingUtility.Colors.PanelSelected * 0.75f
					}
				};
				panel.Add(panelInfo);
			});
		}

		public void Open()
		{
			selectedItem = null;
			selectedModule = null;

			gridItems.Clear();

			foreach (Item item in InventoryUtility.InvArmorEquips(Main.LocalPlayer))
			{
				if (item.IsAir) continue;

				if (item.ModItem is ModularItem modularItem)
				{
					UIModularItem uiModularItem = new UIModularItem(modularItem)
					{
						Width = { Pixels = 48 },
						Height = { Pixels = 48 }
					};
					uiModularItem.OnClick += args =>
					{
						args.Handled = true;

						foreach (UIModularItem m in gridItems.Children.OfType<UIModularItem>()) m.selected = false;
						uiModularItem.selected = true;
						OpenItem(modularItem);
					};

					gridItems.Add(uiModularItem);
				}
			}
		}

		private void OpenItem(ModularItem item)
		{
			selectedItem = item;

			gridModules.Clear();

			foreach (int type in ModuleLoader.validModulesForItem[item.Type])
			{
				BaseModule module = ModuleLoader.GetModule(type);

				UIModule uiModule = new UIModule(module)
				{
					Color = item.IsInstalled(module.Type) ? Color.LimeGreen : Color.Red,
					Width = { Percent = 100 },
					Height = { Pixels = 64 }
				};
				uiModule.OnClick += args =>
				{
					args.Handled = true;

					OpenModule(module);
				};
				gridModules.Add(uiModule);
			}
		}

		private void OpenModule(BaseModule module)
		{
			selectedModule = module;
			panelInfo.Clear();

			UIText textModule = new UIText(module.DisplayName.Get())
			{
				Width = { Percent = 100 },
				Height = { Pixels = 20 }
			};
			panelInfo.Add(textModule);

			UIDivider divider = new UIDivider
			{
				Width = { Percent = 100 },
				Y = { Pixels = 28 }
			};
			panelInfo.Add(divider);

			UIText text = new UIText(module.Tooltip.Get())
			{
				Width = { Percent = 100 },
				Height = { Percent = 100 },
				Y = { Pixels = 36 }
			};
			panelInfo.Add(text);

			string name = ModuleLoader.GetRequirements(module.Type).Aggregate("Requirements", (current, type) => current + "\n\t" + ModuleLoader.GetModule(type).DisplayName.Get());
			UIText textRequirements = new UIText(name)
			{
				Width = { Percent = 50 },
				Height = { Pixels = 20 },
				Y = { Pixels = 100 + 36 }
			};
			panelInfo.Add(textRequirements);

			name = ModuleLoader.GetIncompatibleModules(module.Type).Aggregate("Incompatible", (current, type) => current + "\n\t" + ModuleLoader.GetModule(type).DisplayName.Get());
			UIText textBlacklist = new UIText(name)
			{
				Width = { Percent = 50 },
				Height = { Pixels = 20 },
				X = { Percent = 100 },
				Y = { Pixels = 100 + 36 }
			};
			panelInfo.Add(textBlacklist);

			UITextButton buttonInstall = new UITextButton("Install")
			{
				Width = { Percent = 50, Pixels = -4 },
				Height = { Pixels = 30 },
				Y = { Percent = 100 },
				Settings =
				{
					VerticalAlignment = VerticalAlignment.Center,
					HorizontalAlignment = HorizontalAlignment.Center,
					Disabled = !selectedItem.CanInstall(module.Type)
				}
			};

			UITextButton buttonUninstall = new UITextButton("Uninstall")
			{
				Width = { Percent = 50, Pixels = -4 },
				Height = { Pixels = 30 },
				X = { Percent = 100 },
				Y = { Percent = 100 },
				Settings =
				{
					VerticalAlignment = VerticalAlignment.Center,
					HorizontalAlignment = HorizontalAlignment.Center,
					Disabled = !selectedItem.CanUninstall(module.Type)
				}
			};

			buttonInstall.OnClick += args =>
			{
				args.Handled = true;

				if (selectedItem.CanInstall(module.Type))
				{
					BaseModule clone = module.Clone();
					clone.InternalInstall(selectedItem);

					gridModules.Children.OfType<UIModule>().FirstOrDefault(x => x.module == module).Color = Color.LimeGreen;
					buttonInstall.Settings.Disabled = true;
					buttonUninstall.Settings.Disabled = false;
				}
			};
			panelInfo.Add(buttonInstall);

			buttonUninstall.OnClick += args =>
			{
				args.Handled = true;

				if (selectedItem.CanUninstall(module.Type))
				{
					BaseModule clone = selectedItem.InstalledModules.First(x => x.Type == module.Type);
					clone.InternalRemove(selectedItem);

					gridModules.Children.OfType<UIModule>().FirstOrDefault(x => x.module == module).Color = Color.Red;
					buttonInstall.Settings.Disabled = false;
					buttonUninstall.Settings.Disabled = true;
				}
			};
			panelInfo.Add(buttonUninstall);
		}
	}

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

		public void HandleUI()
		{
			ref UpgradeStationUI ui = ref upgradeState;
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
}