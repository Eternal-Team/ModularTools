using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ModularTools.DataTags;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ModularTools.Core;

public abstract class BaseModule : ModTexturedType
{
	public override string Texture => BaseLibrary.BaseLibrary.PlaceholderTexture;

	public ModTranslation DisplayName { get; internal set; }
	public ModTranslation Tooltip { get; internal set; }
	public int Type { get; internal set; }

	#region Loading
	protected override void Register()
	{
		ModuleLoader.RegisterModule(this);

		ModTypeLookup<BaseModule>.Register(this);

		DisplayName = LocalizationLoader.GetOrCreateTranslation(Mod, "ModuleName." + Name);
		Tooltip = LocalizationLoader.GetOrCreateTranslation(Mod, "ModuleTooltip." + Name);
	}

	public override void SetupContent()
	{
		SetStaticDefaults();

		if (DisplayName.IsDefault())
			DisplayName.SetDefault(Regex.Replace(Name, "([A-Z])", " $1").Trim());

		// SetDefaults();
	}

	public sealed override void Load()
	{
		base.Load();
	}

	public sealed override void Unload()
	{
		base.Unload();
	}

	public virtual BaseModule Clone() => (BaseModule)MemberwiseClone();
	#endregion

	#region Requirements
	protected void AddRequiredModule<T>() where T : BaseModule
	{
		if (GetType() == typeof(T)) throw new Exception("Module can't require itself");

		BaseModule dependency = ModContent.GetInstance<T>();
		if (GetRequirements(dependency).Contains(Type)) throw new Exception($"Adding '{typeof(T).FullName}' as requirement to '{GetType().FullName}' produces a circular dependency");

		ModuleLoader.GetRequiredModules(Type).Add(dependency.Type);

		IEnumerable<int> GetRequirements(BaseModule root)
		{
			var nodes = new Stack<int>(new[] { root.Type });
			while (nodes.Any())
			{
				int node = nodes.Pop();
				yield return node;
				foreach (var n in ModuleLoader.GetRequiredModules(node)) nodes.Push(n);
			}
		}
	}

	protected void AddIncompatibleModules(ModuleGroup tag)
	{
		ModuleLoader.GetIncompatibleGroups(Type).Add(tag);
	}

	protected void AddIncompatibleModule<T>() where T : BaseModule
	{
		if (GetType() == typeof(T)) throw new Exception("Module can't be incompatible to itself");

		ModuleLoader.GetIncompatibleModules(Type).Add(ModuleLoader.ModuleType<T>());
	}

	protected void AddValidModularItems(DataTagData<bool> tag)
	{
		foreach (int type in tag.GetEntries())
		{
			ModuleLoader.GetValidModulesForItem(type).Add(Type);
		}
	}

	protected void AddValidModularItem<T>() where T : ModularItem
	{
		ModuleLoader.GetValidModulesForItem(ModContent.ItemType<T>()).Add(Type);
	}
	#endregion

	public virtual void AddRecipes()
	{
	}

	internal void OnInstalledInternal(ModularItem item)
	{
		item.EnergyStorage.ModifyCapacity(ModuleData.EnergyCapacity.Get(Type));

		ulong max = 0;
		foreach (BaseModule module in item.InstalledModules)
		{
			if (ModuleData.EnergyTransfer.TryGet(module.Type, out ulong val) && val > max) max = val;
		}

		item.EnergyStorage.SetMaxTransfer(max);
		
		OnInstalled(item);
	}
	
	public virtual void OnInstalled(ModularItem item)
	{
	}

	internal void OnRemovedInternal(ModularItem item)
	{
		item.EnergyStorage.ModifyCapacity(-ModuleData.EnergyCapacity.Get(Type));

		ulong max = 0;
		foreach (BaseModule module in item.InstalledModules)
		{
			if (ModuleData.EnergyTransfer.TryGet(module.Type, out ulong val) && val > max) max = val;
		}

		item.EnergyStorage.SetMaxTransfer(max);
		
		OnRemoved(item);
	}
	
	public virtual void OnRemoved(ModularItem item)
	{
	}

	public virtual void OnUpdate(ModularItem item, Player player)
	{
	}

	public virtual void SaveData(TagCompound tag)
	{
	}

	public virtual void LoadData(TagCompound tag)
	{
	}

	protected ModuleRecipe CreateRecipe()
	{
		return ModuleRecipe.Create(Mod, this);
	}
}