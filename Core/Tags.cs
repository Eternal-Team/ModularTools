using System;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace ModularTools.Core;

#region Data Tags
public static class DataTags
{
	internal static readonly Dictionary<Type, DataTagGroup> TagGroupsByType = new();

	public static DataTagData<K> Get<T, K>(string tagName) where T : DataTagGroup => GetGroup<T>().GetTag<K>(tagName);

	public static T GetGroup<T>() where T : DataTagGroup => ModContent.GetInstance<T>();
}

[Autoload]
public abstract class DataTagGroup : ModType
{
	public abstract int TypeCount { get; }

	internal Dictionary<string, DataTagData> TagNameToData = new(StringComparer.InvariantCultureIgnoreCase);

	protected sealed override void Register() => ModTypeLookup<DataTagGroup>.Register(this);

	public sealed override void Unload()
	{
		TagNameToData.Clear();

		TagNameToData = null;
	}

	public IEnumerable<DataTagData> GetDataFor(int type)
	{
		foreach (var (key, value) in TagNameToData)
		{
			if (value.Has(type)) yield return value;
		}
	}
	
	public DataTagData<T> GetTag<T>(string tagName)
	{
		if (!TagNameToData.TryGetValue(tagName, out var data))
		{
			TagNameToData[tagName] = data = new DataTagData<T>(TypeCount);
		}

		return (DataTagData<T>)data;
	}
}

public abstract class DataTagData
{
	protected readonly List<int> entryList;
	// protected readonly IReadOnlyList<int> readonlyEntryList;

	internal DataTagData()
	{
		entryList = new List<int>();
		// readonlyEntryList = entryList.AsReadOnly();
	}

	public bool Has(int id) => entryList.Contains(id);

	public IEnumerable<int> GetEntries() => entryList.AsReadOnly();

	public abstract string GetText(int id);
}

public class DataTagData<T> : DataTagData
{
	private readonly T[] idToValue;
	private Func<T, string> localization;

	internal DataTagData(int maxEntries)
	{
		idToValue = new T[maxEntries];
	}

	public T Get(int id) => idToValue[id];

	public bool TryGet(int id, out T value)
	{
		if (entryList.Contains(id))
		{
			value = idToValue[id];
			return true;
		}

		value = default;
		return false;
	}

	public void Set(int id, T value)
	{
		idToValue[id] = value;

		if (!entryList.Contains(id)) entryList.Add(id);
	}

	public override string GetText(int id)
	{
		return localization != null ? localization(idToValue[id]) : GetType().Name;
	}

	public DataTagData<T> AddLocalization(Func<T, string> func)
	{
		localization = func;
		return this;
	}
}
#endregion