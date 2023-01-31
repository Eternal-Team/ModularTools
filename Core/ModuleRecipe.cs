using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Exceptions;

namespace ModularTools.Core;

public sealed class ModuleRecipe
{
	public readonly BaseModule module;
	public readonly List<Item> requiredItems = new();
	public readonly List<(int, int)> requiredGroups = new();

	public static readonly Dictionary<int, ModuleRecipe> recipes = new();

	public ModuleRecipe(BaseModule module)
	{
		this.module = module;
	}

	/// <summary>
	/// Adds an ingredient to this recipe with the given item type and stack size. Ex:
	/// <c>recipe.AddIngredient(ItemID.IronAxe)</c>
	/// </summary>
	/// <param name="itemID">The item identifier.</param>
	/// <param name="stack">The stack.</param>
	public ModuleRecipe AddIngredient(int itemID, int stack = 1)
	{
		requiredItems.Add(new Item(itemID) { stack = stack });

		return this;
	}

	/// <summary>
	/// Adds an ingredient to this recipe with the given item name from the given mod, and with the given stack stack. If the
	/// mod parameter is null, then it will automatically use an item from the mod creating this recipe.
	/// </summary>
	/// <param name="mod">The mod.</param>
	/// <param name="itemName">Name of the item.</param>
	/// <param name="stack">The stack.</param>
	/// <exception cref="RecipeException">
	/// The item " + itemName + " does not exist in mod " + mod.Name + ". If you are trying
	/// to use a vanilla item, try removing the first argument.
	/// </exception>
	public ModuleRecipe AddIngredient(Mod mod, string itemName, int stack = 1)
	{
		if (!ModContent.TryFind(mod.Name, itemName, out ModItem item))
			throw new RecipeException($"The item {itemName} does not exist in the mod {mod.Name}.\r\nIf you are trying to use a vanilla item, try removing the first argument.");

		return AddIngredient(item, stack);
	}

	/// <summary>
	/// Adds an ingredient to this recipe of the given type of item and stack size.
	/// </summary>
	/// <param name="item">The item.</param>
	/// <param name="stack">The stack.</param>
	public ModuleRecipe AddIngredient(ModItem item, int stack = 1) => AddIngredient(item.Type, stack);

	/// <summary>
	/// Adds an ingredient to this recipe of the given type of item and stack size.
	/// </summary>
	/// <param name="item">The item.</param>
	/// <param name="stack">The stack.</param>
	public ModuleRecipe AddIngredient<T>(int stack = 1) where T : ModItem
		=> AddIngredient(ModContent.ItemType<T>(), stack);

	/// <summary>
	/// Adds a recipe group ingredient to this recipe with the given RecipeGroup name and stack size. Vanilla recipe groups
	/// consist of "Wood", "IronBar", "PresurePlate", "Sand", and "Fragment".
	/// </summary>
	/// <param name="name">The name.</param>
	/// <param name="stack">The stack.</param>
	/// <exception cref="RecipeException">A recipe group with the name " + name + " does not exist.</exception>
	public ModuleRecipe AddRecipeGroup(string name, int stack = 1)
	{
		if (!RecipeGroup.recipeGroupIDs.ContainsKey(name))
			throw new RecipeException($"A recipe group with the name {name} does not exist.");

		int id = RecipeGroup.recipeGroupIDs[name];
		requiredGroups.Add((id, stack));

		return this;
	}

	/// <summary>
	/// Adds a recipe group ingredient to this recipe with the given RecipeGroupID and stack size. Vanilla recipe group IDs can
	/// be found in Terraria.ID.RecipeGroupID and modded recipe group IDs will be returned from RecipeGroup.RegisterGroup.
	/// </summary>
	/// <param name="recipeGroupId">The RecipeGroupID.</param>
	/// <param name="stack">The stack.</param>
	/// <exception cref="RecipeException">A recipe group with the ID " + recipeGroupID + " does not exist.</exception>
	public ModuleRecipe AddRecipeGroup(int recipeGroupId, int stack = 1)
	{
		if (!RecipeGroup.recipeGroups.ContainsKey(recipeGroupId))
			throw new RecipeException($"A recipe group with the ID {recipeGroupId} does not exist.");

		requiredGroups.Add((recipeGroupId, stack));

		return this;
	}

	/// <summary>
	/// Adds a recipe group ingredient to this recipe with the given RecipeGroup.
	/// </summary>
	/// <param name="recipeGroup">The RecipeGroup.</param>
	public ModuleRecipe AddRecipeGroup(RecipeGroup recipeGroup, int stack = 1)
	{
		requiredGroups.Add((recipeGroup.ID, stack));

		return this;
	}

	/// <summary>
	/// Adds this recipe to the game. Call this after you have finished setting the result, ingredients, etc.
	/// </summary>
	/// <exception cref="RecipeException">A recipe without any result has been added.</exception>
	public void Register()
	{
		if (module == null)
			throw new RecipeException("A recipe without any result has been added.");

		if (recipes.ContainsKey(module.Type))
			throw new RecipeException("Recipe for module has been already added.");

		recipes.Add(module.Type, this);
	}
}