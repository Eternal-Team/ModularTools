using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using BaseLibrary.Utility;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ModLoader.Tags;

namespace ModularTools.Core
{
	public abstract class BaseModule : ModTexturedType
	{
		public override string Texture => BaseLibrary.BaseLibrary.PlaceholderTexture;

		public ModTranslation DisplayName { get; internal set; }
		public ModTranslation Tooltip { get; internal set; }
		public int Type { get; internal set; }

		#region Loading
		private static MethodInfo GetOrCreateTranslation = typeof(Mod).GetMethod("GetOrCreateTranslation", ReflectionUtility.DefaultFlags);

		protected override void Register()
		{
			ModuleLoader.RegisterModule(this);

			ModTypeLookup<BaseModule>.Register(this);

			DisplayName = GetOrCreateTranslation.Invoke<ModTranslation>(Mod, $"Mods.{Mod.Name}.ModuleName.{Name}", false);
			Tooltip = GetOrCreateTranslation.Invoke<ModTranslation>(Mod, $"Mods.{Mod.Name}.ModuleTooltip.{Name}", true);
		}

		public override void SetupContent()
		{
			SetStaticDefaults();

			if (DisplayName.IsDefault())
				DisplayName.SetDefault(Regex.Replace(Name, "([A-Z])", " $1").Trim());

			SetDefaults();
		}

		public sealed override void Load()
		{
			base.Load();
		}

		public sealed override void Unload()
		{
			base.Unload();
		}
		#endregion

		public int EnergyCapacity;
		public int HeatCapacity;

		public virtual BaseModule Clone() => (BaseModule)MemberwiseClone();

		protected void AddRequiredModule<T>() where T : BaseModule
		{
			if (GetType() == typeof(T)) throw new Exception("Module can't require itself");

			BaseModule dependency = ModContent.GetInstance<T>();
			if (GetRequirements(dependency).Contains(Type)) throw new Exception($"Adding '{typeof(T).FullName}' as requirement produces a circular dependency");

			ModuleLoader.requirements[Type].Add(dependency.Type);

			IEnumerable<int> GetRequirements(BaseModule root)
			{
				var nodes = new Stack<int>(new[] { root.Type });
				while (nodes.Any())
				{
					int node = nodes.Pop();
					yield return node;
					foreach (var n in ModuleLoader.requirements[node]) nodes.Push(n);
				}
			}
		}

		protected void AddIncompatibleModules(TagData tag)
		{
			ModuleLoader.blacklistGroups[Type].Add(tag);
		}

		protected void AddIncompatibleModule<T>() where T : BaseModule
		{
			if (GetType() == typeof(T)) throw new Exception("Module can't be incompatible to itself");

			BaseModule dependency = ModContent.GetInstance<T>();
			ModuleLoader.blacklistTypes[Type].Add(dependency.Type);
		}

		protected void AddValidModularItems(TagData tag)
		{
			foreach (int type in tag.GetEntries())
			{
				ModuleLoader.validItemsForModule[Type].Add(type);
				ModuleLoader.validModulesForItem[type].Add(Type);
			}
		}

		protected void AddValidModularItem<T>() where T : ModularItem
		{
			int type = ModContent.ItemType<T>();

			ModuleLoader.validItemsForModule[Type].Add(type);
			ModuleLoader.validModulesForItem[type].Add(Type);
		}

		public virtual void SetStaticDefaults()
		{
		}

		public virtual void SetDefaults()
		{
		}

		public virtual void AddRecipes()
		{
		}

		internal void InternalInstall(ModularItem item)
		{
			item.InstalledModules.Add(this);

			item.HeatStorage.ModifyCapacity(HeatCapacity);
			item.EnergyStorage.ModifyCapacity(EnergyCapacity);
		}

		internal void InternalRemove(ModularItem item)
		{
			item.InstalledModules.Remove(this);

			item.HeatStorage.ModifyCapacity(-HeatCapacity);
			item.EnergyStorage.ModifyCapacity(-EnergyCapacity);
		}

		public virtual void OnInstalled(ModularItem item)
		{
		}

		public virtual void OnRemoved(ModularItem item)
		{
		}

		public virtual void OnUpdate(ModularItem item, Player player)
		{
		}

		public virtual TagCompound Save() => null;

		public virtual void Load(TagCompound tag)
		{
		}

		protected ModuleRecipe Create()
		{
			return ModuleRecipe.Create(Mod, this);
		}
	}
}