using System;
using System.Collections.Generic;
using ModularTools.Core;
using Terraria.ModLoader;

namespace ModularTools.DataTags
{
	public class ModuleDataGroup : DataTagGroup
	{
		public override int TypeCount => ModuleLoader.Count;
	}

	public static class ModuleData
	{
		public static readonly DataTagData<int> Defense = DataTags.Get<ModuleDataGroup, int>(nameof(Defense));
	}

	public static class DataTags
	{
		internal static readonly Dictionary<Type, DataTagGroup> TagGroupsByType = new Dictionary<Type, DataTagGroup>();

		public static DataTagData<K> Get<T, K>(string tagName) where T : DataTagGroup => GetGroup<T>().GetTag<K>(tagName);

		public static T GetGroup<T>() where T : DataTagGroup => ModContent.GetInstance<T>();
	}

	[Autoload]
	public abstract class DataTagGroup : ModType
	{
		public abstract int TypeCount { get; }

		internal Dictionary<string, DataTagData> TagNameToData = new Dictionary<string, DataTagData>(StringComparer.InvariantCultureIgnoreCase);

		protected sealed override void Register() => ModTypeLookup<DataTagGroup>.Register(this);

		public sealed override void Unload()
		{
			TagNameToData.Clear();

			TagNameToData = null;
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
		protected readonly IReadOnlyList<int> readonlyEntryList;

		internal DataTagData()
		{
			entryList = new List<int>();
			readonlyEntryList = entryList.AsReadOnly();
		}

		public bool Has(int id) => entryList.Contains(id);
		
		public IReadOnlyList<int> GetEntries() => readonlyEntryList;
	}

	public sealed class DataTagData<T> : DataTagData
	{
		private readonly T[] idToValue;

		internal DataTagData(int maxEntries)
		{
			idToValue = new T[maxEntries];
			// entryList = new List<int>();
			// readonlyEntryList = entryList.AsReadOnly();
		}

		public T Get(int id) => idToValue[id];

		public void Set(int id, T value)
		{
			idToValue[id] = value;

			if (!entryList.Contains(id)) entryList.Add(id);
		}
	}
}