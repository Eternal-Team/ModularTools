using System;
using Terraria.ModLoader.IO;

namespace ModularTools.Core
{
	public class HeatStorage
	{
		public float Heat;

		public float Temperature
		{
			get => Heat / Capacity;
			set => Heat = Capacity * value;
		}

		public float Capacity; // J/K
		public float TransferCoefficient; //  W/m2*K
		public float Area; // m2

		public float CoolingCoefficient => TransferCoefficient * Area / Capacity;

		// public ulong MaxExtract { get; private set; }
		// public ulong MaxReceive { get; private set; }

		internal HeatStorage()
		{
		}

		// public HeatStorage(ulong capacity)
		// {
		// 	Capacity = capacity;
		// 	// MaxReceive = capacity;
		// 	// MaxExtract = capacity;
		// }
		//
		// public HeatStorage(ulong capacity, ulong maxTransfer)
		// {
		// 	Capacity = capacity;
		// 	// MaxReceive = maxTransfer;
		// 	// MaxExtract = maxTransfer;
		// }
		//
		// public HeatStorage(ulong capacity, ulong maxReceive, ulong maxExtract)
		// {
		// 	Capacity = capacity;
		// 	// MaxReceive = maxReceive;
		// 	// MaxExtract = maxExtract;
		// }

		public HeatStorage Clone()
		{
			HeatStorage storage = (HeatStorage)MemberwiseClone();
			return storage;
		}

		public void SetCapacity(ulong capacity)
		{
			Capacity = capacity;
			if (Heat > capacity) Heat = Capacity;
		}

		public void ModifyCapacity(long capacity)
		{
			if (capacity < 0) Capacity -= Math.Min(Capacity, (ulong)capacity);
			else Capacity += (ulong)capacity;
		}

		public float TransferToEnvironment(float environmentTemp, float timeStep)
		{
			float oldTemperature = Temperature;
			Temperature = (float)(environmentTemp + (Temperature - environmentTemp) * Math.Exp(-CoolingCoefficient * timeStep));
			return (Temperature - oldTemperature) * Capacity;
		}

		public void ModifyHeat(float joules)
		{
			Heat += joules;
			if (Heat < 0) Heat = 0;
		}

		// public void SetMaxTransfer(ulong maxTransfer)
		// {
		// SetMaxReceive(maxTransfer);
		// SetMaxExtract(maxTransfer);
		// }

		// public void SetMaxReceive(ulong maxReceive)
		// {
		// MaxReceive = maxReceive;
		// }

		// public void SetMaxExtract(ulong maxExtract)
		// {
		// MaxExtract = maxExtract;
		// }

		// public ulong InsertHeat(ulong amount)
		// {
		// 	ulong CurrentDelta = Math.Min(Capacity - Heat, amount);
		// 	Heat += CurrentDelta;
		//
		// 	// DeltaBuffer.Enqueue(CurrentDelta);
		// 	//
		// 	// if (DeltaBuffer.Count > ModContent.GetInstance<EnergyLibraryConfig>().DeltaCacheSize)
		// 	// {
		// 	// 	DeltaBuffer.Dequeue();
		// 	// 	AverageDelta = (long)DeltaBuffer.Average(i => i);
		// 	// }
		// 	// else AverageDelta = CurrentDelta;
		// 	//
		// 	// OnChanged?.Invoke();
		//
		// 	return CurrentDelta;
		// }
		//
		// public ulong ExtractHeat(ulong amount)
		// {
		// 	ulong CurrentDelta = Math.Min(Heat, amount);
		// 	Heat -= CurrentDelta;
		//
		// 	// DeltaBuffer.Enqueue(CurrentDelta);
		// 	//
		// 	// if (DeltaBuffer.Count > ModContent.GetInstance<EnergyLibraryConfig>().DeltaCacheSize)
		// 	// {
		// 	// 	DeltaBuffer.Dequeue();
		// 	// 	AverageDelta = (long)DeltaBuffer.Average(i => i);
		// 	// }
		// 	// else AverageDelta = CurrentDelta;
		// 	//
		// 	// OnChanged?.Invoke();
		//
		// 	return CurrentDelta;
		// }

		public TagCompound Save() => new TagCompound
		{
			["Heat"] = Heat,
			["Capacity"] = Capacity
			// ["MaxExtract"] = MaxExtract,
			// ["MaxReceive"] = MaxReceive
		};

		public void Load(TagCompound tag)
		{
			Heat = tag.Get<float>("Heat");
			Capacity = tag.Get<float>("Capacity");
			// MaxExtract = tag.Get<ulong>("MaxExtract");
			// MaxReceive = tag.Get<ulong>("MaxReceive");
		}

		// public void Write(BinaryWriter writer)
		// {
		// 	writer.Write(Heat);
		// 	writer.Write(Capacity);
		// 	// writer.Write(MaxExtract);
		// 	// writer.Write(MaxReceive);
		// }
		//
		// public void Read(BinaryReader reader)
		// {
		// 	Heat = reader.ReadUInt64();
		// 	Capacity = reader.ReadUInt64();
		// 	// MaxExtract = reader.ReadUInt64();
		// 	// MaxReceive = reader.ReadUInt64();
		// }

		public override string ToString() => $"Heat: {Heat}/{Capacity}";
	}

	public interface IHeatStorage
	{
		HeatStorage GetHeatStorage();
	}
}