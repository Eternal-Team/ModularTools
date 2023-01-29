using System.Collections.Generic;
using System.Linq;
using BaseLibrary;
using BaseLibrary.UI;
using BaseLibrary.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ModularTools.Core;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ModularTools.UI;

internal class InfoPanel : UIPanel
{
	private UIText textModule;
	private UIText textModuleDescription;
	private UIModuleList textRequirements;
	private UIModuleList textIncompatible;
	private UIText buttonInstall;
	private UIText buttonUninstall;

	public InfoPanel()
	{
		#region Panels
		UIPanelSettings settings = new()
		{
			BorderColor = Color.Transparent,
			BackgroundColor = DrawingUtility.Colors.PanelSelected * 1.2f
			// 61, 78, 143
		};

		UIPanel panelDescription = new()
		{
			Width = { Percent = 100 },
			Height = { Percent = 10 },
			PositionIncludeDims = false,
			Settings = settings
		};
		Add(panelDescription);

		UIPanel panelStats = new()
		{
			Width = { Percent = 50 },
			Height = { Percent = 65 },
			Y = { Percent = 10 },
			PositionIncludeDims = false,
			Settings = settings
		};
		Add(panelStats);

		BaseElement holder = new()
		{
			Width = { Percent = 50 },
			Height = { Percent = 65 },
			X = { Percent = 50 },
			Y = { Percent = 10 },
			PositionIncludeDims = false
		};
		Add(holder);
		
		UIPanel panelRequirements = new()
		{
			Width = { Percent = 100 },
			Height = { Percent = 50 },
			Settings = settings
		};
		holder.Add(panelRequirements);

		UIPanel panelIncompatible = new()
		{
			Width = { Percent = 100 },
			Height = { Percent = 50 },
			Y = { Percent = 100 },
			Settings = settings
		};
		holder.Add(panelIncompatible);
		
		UIPanel panelRecipe = new()
		{
			Width = { Percent = 100 },
			Height = { Percent = 20 },
			Y = { Percent = 75 },
			PositionIncludeDims = false,
			Settings = settings
		};
		Add(panelRecipe);

		UIPanel panelInstall = new()
		{
			Width = { Percent = 100 },
			Height = { Percent = 5 },
			Y = { Percent = 100 },
			Settings = settings
		};
		Add(panelInstall);
		#endregion

		textModule = new UIText("")
		{
			Width = { Percent = 100 },
			Height = { Pixels = 20 }
		};
		panelDescription.Add(textModule);

		textModuleDescription = new UIText("")
		{
			Width = { Percent = 100 },
			Height = { Percent = 20 },
			Y = { Pixels = 28 }
		};
		panelDescription.Add(textModuleDescription);

		UIText textStats = new("Stats")
		{
			Width = { Percent = 100 },
			Height = { Pixels = 20 }
		};
		panelStats.Add(textStats);

		UIText textRequirements = new("Requirements")
		{
			Width = { Percent = 100 },
			Height = { Pixels = 20 }
		};
		panelRequirements.Add(textRequirements);

		UIText textIncompatible = new("Incompatible")
		{
			Width = { Percent = 100 },
			Height = { Pixels = 20 }
		};
		panelIncompatible.Add(textIncompatible);

		UIText textRecipe = new("Recipe")
		{
			Width = { Percent = 100 },
			Height = { Pixels = 20 }
		};
		panelRecipe.Add(textRecipe);

		// textRequirements = new UIModuleList(null)
		// {
		// 	Width = { Pixels = 48 },
		// 	Height = { Pixels = 48 },
		// 	Y = { Pixels = 100 + 36 }
		// };
		// Add(textRequirements);
		// 	
		// textIncompatible = new UIModuleList(null)
		// {
		// 	Width = { Pixels = 48 },
		// 	Height = { Pixels = 48 },
		// 	X = { Percent = 100 },
		// 	Y = { Pixels = 100 + 36 }
		// };
		// Add(textIncompatible);

		buttonInstall = new UIText("Install")
		{
			Width = { Percent = 50, Pixels = -4 },
			Height = { Percent = 100 },
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
		panelInstall.Add(buttonInstall);

		buttonUninstall = new UIText("Uninstall")
		{
			Width = { Percent = 50, Pixels = -4 },
			Height = { Percent = 100 },
			X = { Percent = 100 },
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
		panelInstall.Add(buttonUninstall);
	}

	public void SetModule(BaseModule module)
	{
		textModule.Text = module.DisplayName.Get();
		textModuleDescription.Text = module.Tooltip.Get();

		// string text = Core.DataTags.GetGroup<ModuleDataGroup>().GetDataFor(module.Type)
		// 	.Aggregate("", (current, pair) => current + pair.GetText(module.Type) + "\n");
		// text += module.Tooltip.Get();


		// textRequirements.modules = ModuleLoader.GetRequiredModules(module.Type);
		// textIncompatible.modules = ModuleLoader.GetIncompatibleModules(module.Type);

		// textRequirements.Text = ModuleLoader.GetRequiredModules(module.Type).Aggregate("Requirements", (current, type) => current + "\n\t" + ModuleLoader.GetModule(type).DisplayName.Get());
		// textIncompatible.Text = 
		// ModuleLoader.GetIncompatibleModules(module.Type).Aggregate("Incompatible", (current, type) => current + "\n\t" + ModuleLoader.GetModule(type).DisplayName.Get())
		// + ModuleLoader.GetIncompatibleGroups(module.Type).Aggregate("", (current, type) => current + "\n\tGroup: " +type.DisplayName.GetDefault());
		// buttonInstall.Settings.Disabled = !selectedItem.CanInstall(module.Type);
		// buttonUninstall.Settings.Disabled = !selectedItem.CanUninstall(module.Type);
	}
}

public class UpgradeStationUI : BaseState
{
	private UIPanel panelMain;

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
		selectedModule = null;

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

				selectedModule = module;
				infoPanel.SetModule(module);
			};
			gridModules.Add(uiModule);
		}
	}
}

public class UIModuleList : BaseElement
{
	public List<int> modules;
	private int currentIndex;
	private int timer;

	public UIModuleList(List<int> modules)
	{
		this.modules = modules;
	}

	protected override void Draw(SpriteBatch spriteBatch)
	{
		if (modules == null || modules.Count == 0) return;

		spriteBatch.Draw(ModContent.Request<Texture2D>(ModuleLoader.GetModule(modules[currentIndex]).Texture).Value, Dimensions);


		if (++timer >= 30)
		{
			currentIndex++;
			if (currentIndex >= modules.Count) currentIndex = 0;
			timer = 0;
		}
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