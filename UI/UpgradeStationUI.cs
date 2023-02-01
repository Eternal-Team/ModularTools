using System.Collections.Generic;
using System.Linq;
using BaseLibrary;
using BaseLibrary.UI;
using BaseLibrary.Utility;
using Microsoft.Xna.Framework;
using ModularTools.Core;
using Terraria;
using Terraria.Localization;

namespace ModularTools.UI;

public class UpgradeStationUI : BaseState
{
	private UIPanel panelMain;
	
	// todo: improve module list

	private UIGrid<UIModularItem> gridItems;
	private UIGrid<UIModule> gridModules;
	private Ref<string> search = new("");
	private ModularItem selectedItem;
	private BaseModule selectedModule;

	private InfoPanel infoPanel;

	public UpgradeStationUI()
	{
		CreateMainPanel();
		CreateItemsPanel();
		CreateModulesPanel();

		infoPanel = new InfoPanel
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
		panelMain.Add(infoPanel);
	}

	private void CreateMainPanel()
	{
		panelMain = new UIPanel
		{
			Width = { Pixels = 1000 },
			Height = { Pixels = 700 + 28 },
			X = { Percent = 50 },
			Y = { Percent = 50 },
			Settings =
			{
				Draggable = true,
				DragZones = new List<DragZone> { new() { Width = { Percent = 100 }, Height = { Pixels = 28 } } }
			}
		};
		Add(panelMain);

		UIText title = new("Upgrade Station")
		{
			Height = { Pixels = 20 }
		};
		panelMain.Add(title);

		UIText buttonClose = new("X")
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
		UIPanel panelItems = new()
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
		UIPanel panelModules = new()
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

		UIPanel panelInput = new()
		{
			Width = { Percent = 100 },
			Height = { Pixels = 36 },
			Settings = { BorderColor = Color.Transparent }
		};
		panelModules.Add(panelInput);

		UITextInput input = new(ref search)
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

	public void Open()
	{
		selectedItem = null;

		gridItems.Clear();

		foreach (Item item in InventoryUtility.InvArmor(Main.LocalPlayer))
		{
			if (item.IsAir || item.ModItem is not ModularItem modularItem) continue;

			UIModularItem uiModularItem = new(modularItem)
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

			UIModule uiModule = new(module)
			{
				// todo: grey out when can't install, white-ish when selected
				// todo: icons for (un)installed
				Color = item.IsInstalled(module.Type) ? Color.LimeGreen : Color.Red,
				Width = { Percent = 100 },
				Height = { Pixels = 64 }
			};
			uiModule.OnClick += args =>
			{
				if (args.Button != MouseButton.Left) return;
				args.Handled = true;

				if (selectedModule == module) return;
				
				selectedModule = module;
				infoPanel.SetModule(selectedItem, uiModule);
			};
			gridModules.Add(uiModule);
		}
	}
}