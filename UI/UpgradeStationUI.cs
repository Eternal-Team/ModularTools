using System.Collections.Generic;
using System.Linq;
using BaseLibrary;
using BaseLibrary.UI;
using BaseLibrary.Utility;
using Microsoft.Xna.Framework;
using ModularTools.Core;
using ModularTools.DataTags;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ModularTools.UI;

public class UpgradeStationUI : BaseState
{
	private UIPanel panelMain;
	private UIGrid<UIModularItem> gridItems;
	private UIGrid<UIModule> gridModules;
	private Ref<string> search = new Ref<string>("");
	private ModularItem selectedItem;
	private BaseModule selectedModule;
	private UIText textModule;
	private UIText textModuleDescription;
	private UIText textRequirements;
	private UIText textIncompatible;
	private UIText buttonInstall;
	private UIText buttonUninstall;

	public UpgradeStationUI()
	{
		CreateMainPanel();
		CreateItemsPanel();
		CreateModulesPanel();
		CreateInfoPanel();
	}

	private void CreateMainPanel()
	{
		panelMain = new UIPanel
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
		Add(panelMain);
		
		UIText title = new UIText("Upgrade Station")
		{
			Height = { Pixels = 20 }
		};
		panelMain.Add(title);

		UIText buttonClose = new UIText("X")
		{
			Height = { Pixels = 20 },
			Width = { Pixels = 20 },
			X = { Percent = 100 },
			HoverText = Language.GetText("Mods.BaseLibrary.UI.Close")
		};
		buttonClose.OnMouseDown += args =>
		{
			if (args.Button != MouseButton.Left) return;
			args.Handled = true;

			Display = Display.None;
		};
		buttonClose.OnMouseEnter += _ => buttonClose.Settings.TextColor = Color.Red;
		buttonClose.OnMouseLeave += _ => buttonClose.Settings.TextColor = Color.White;
		panelMain.Add(buttonClose);
	}

	private void CreateItemsPanel()
	{
		UIPanel panelItems = new UIPanel
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
		panelMain.Add(panelItems);
		gridItems = new UIGrid<UIModularItem>
		{
			Width = { Percent = 100 },
			Height = { Percent = 100 },
			Settings = { MaxSelectedItems = 1 }
		};
		panelItems.Add(gridItems);
	}

	// todo: dont show info panel when no module is selected, extended module panel with more information instead
	
	private void CreateModulesPanel()
	{
		UIPanel panelModules = new UIPanel
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
		panelMain.Add(panelModules);
		
		UIPanel panelInput = new UIPanel
		{
			Width = { Percent = 100 },
			Height = { Pixels = 36 },
			Settings = { BorderColor = Color.Transparent }
		};
		panelModules.Add(panelInput);
		
		UITextInput input = new UITextInput(ref search)
		{
			Width = { Percent = 100 },
			Height = { Percent = 100 },
			OnTextChange = () => gridModules.Search(),
			Settings = { HintText = "<Search>" }
		};
		panelInput.Add(input);
		
		// todo: category buttons
		
		gridModules = new UIGrid<UIModule>
		{
			Width = { Percent = 100 },
			Height = { Percent = 100, Pixels = -44 },
			Y = { Pixels = 44 },
			SearchSelector = item => string.IsNullOrWhiteSpace(search.Value) || TextUtility.Search(item.Module.DisplayName.Get().ToLower(), search.Value.ToLower()).Any()
		};
		panelModules.Add(gridModules);
	}

	private void CreateInfoPanel()
	{
		UIPanel panelInfo = new UIPanel
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
		panelMain.Add(panelInfo);

		textModule = new UIText("")
		{
			Width = { Percent = 100 },
			Height = { Pixels = 20 }
		};
		panelInfo.Add(textModule);
			
		// UIDivider divider = new UIDivider
		// {
		// 	Width = { Percent = 100 },
		// 	Y = { Pixels = 28 }
		// };
		// panelInfo.Add(divider);
			
		textModuleDescription = new UIText("")
		{
			Width = { Percent = 100 },
			Height = { Percent = 100 },
			Y = { Pixels = 36 }
		};
		panelInfo.Add(textModuleDescription);
			
		textRequirements = new UIText("")
		{
			Width = { Percent = 50 },
			Height = { Pixels = 20 },
			Y = { Pixels = 100 + 36 }
		};
		panelInfo.Add(textRequirements);
			
		textIncompatible = new UIText("")
		{
			Width = { Percent = 50 },
			Height = { Pixels = 20 },
			X = { Percent = 100 },
			Y = { Pixels = 100 + 36 }
		};
		panelInfo.Add(textIncompatible);
			
		buttonInstall = new UIText("Install")
		{
			Width = { Percent = 50, Pixels = -4 },
			Height = { Pixels = 30 },
			Y = { Percent = 100 },
			Settings =
			{
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Center
			}
		};
		buttonInstall.OnClick += args =>
		{
			args.Handled = true;
			
			// if (selectedItem.CanInstall(selectedModule.Type))
			// {
			// 	BaseModule clone = selectedModule.Clone();
			// 	clone.InternalInstall(selectedItem);
			//
			// 	gridModules.Children.OfType<UIModule>().FirstOrDefault(x => x.Module == selectedModule).Color = Color.LimeGreen;
			// 	buttonInstall.Settings.Disabled = true;
			// 	buttonUninstall.Settings.Disabled = false;
			// }
		};
		panelInfo.Add(buttonInstall);
			
		buttonUninstall = new UIText("Uninstall")
		{
			Width = { Percent = 50, Pixels = -4 },
			Height = { Pixels = 30 },
			X = { Percent = 100 },
			Y = { Percent = 100 },
			Settings =
			{
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Center
			}
		};
		buttonUninstall.OnClick += args =>
		{
			args.Handled = true;
			
			// if (selectedItem.CanUninstall(selectedModule.Type))
			// {
			// 	BaseModule clone = selectedItem.InstalledModules.First(x => x.Type == selectedModule.Type);
			// 	clone.InternalRemove(selectedItem);
			//
			// 	gridModules.Children.OfType<UIModule>().FirstOrDefault(x => x.Module == selectedModule).Color = Color.Red;
			// 	buttonInstall.Settings.Disabled = false;
			// 	buttonUninstall.Settings.Disabled = true;
			// }
		};
		panelInfo.Add(buttonUninstall);
	}

	public void Open()
	{
		selectedItem = null;
		selectedModule = null;

		gridItems.Clear();

		foreach (Item item in InventoryUtility.InvArmor(Main.LocalPlayer))
		{
			if (item.IsAir || item.ModItem is not ModularItem modularItem) continue;

			UIModularItem uiModularItem = new UIModularItem(modularItem)
			{
				Width = { Pixels = 48 },
				Height = { Pixels = 48 }
			};
			uiModularItem.OnClick += args =>
			{
				if (args.Button != MouseButton.Left) return;
				args.Handled = true;

				OpenItem(modularItem);
			};

			gridItems.Add(uiModularItem);
		}
	}

	private void OpenItem(ModularItem item)
	{
		selectedItem = item;

		gridModules.Clear();

		foreach (int type in ModuleLoader.GetValidModulesForItem(item.Type))
		{
			BaseModule module = ModuleLoader.GetModule(type);
		
			UIModule uiModule = new UIModule(module)
			{
				// todo: grey out when can't install
				// todo: icons for (un)installed
				Color = item.IsInstalled(module.Type) ? Color.LimeGreen : Color.Red,
				Width = { Percent = 100 },
				Height = { Pixels = 64 }
			};
			uiModule.OnClick += args =>
			{
				if (args.Button != MouseButton.Left) return;
				args.Handled = true;
		
				OpenModule(module);
			};
			gridModules.Add(uiModule);
		}
	}

	private void OpenModule(BaseModule module)
	{
		selectedModule = module;
		
		textModule.Text = module.DisplayName.Get();
		
		// todo: better group localization
		// Language.GetTextValue($"Mods.ModularTools.ModuleData.{pair.Key}", pair.Value.Invoke<object>("Get", args: new object[] { module.Type })
		string text = Core.DataTags.GetGroup<ModuleDataGroup>().GetDataFor(module.Type)
			.Aggregate("", (current, pair) => current + pair.GetText(module.Type) + "\n");
		text += module.Tooltip.Get();
		
		textModuleDescription.Text = text;
		
		textRequirements.Text = ModuleLoader.GetRequiredModules(module.Type).Aggregate("Requirements", (current, type) => current + "\n\t" + ModuleLoader.GetModule(type).DisplayName.Get());
		textIncompatible.Text = 
			ModuleLoader.GetIncompatibleModules(module.Type).Aggregate("Incompatible", (current, type) => current + "\n\t" + ModuleLoader.GetModule(type).DisplayName.Get())
			+ ModuleLoader.GetIncompatibleGroups(module.Type).Aggregate("", (current, type) => current + "\n\tGroup: " +type.DisplayName.GetDefault());
		// buttonInstall.Settings.Disabled = !selectedItem.CanInstall(module.Type);
		// buttonUninstall.Settings.Disabled = !selectedItem.CanUninstall(module.Type);
	}
}

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