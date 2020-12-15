using System;
using System.IO;
using BaseLibrary.Utility;
using Terraria.ModLoader.IO;

namespace ModularTools.Core
{
	public class HeatStorage
	{
		public float Heat;

		public float Temperature // K
		{
			get => Heat / Capacity;
			set => Heat = Capacity * value;
		}

		public float Capacity; // J/K
		public float TransferCoefficient; //  W/m2*K
		public float Area; // m2

		public float CoolingCoefficient => TransferCoefficient * Area / Capacity;

		internal HeatStorage()
		{
		}

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

		public TagCompound Save() => new TagCompound
		{
			["Heat"] = Heat,
			["Capacity"] = Capacity,
			["TransferCoefficient"] = TransferCoefficient,
			["Area"] = Area
		};

		public void Load(TagCompound tag)
		{
			Heat = tag.Get<float>("Heat");
			Capacity = tag.Get<float>("Capacity");
			TransferCoefficient = tag.Get<float>("TransferCoefficient");
			Area = tag.Get<float>("Area");
		}

		public void Write(BinaryWriter writer)
		{
			writer.Write(Heat);
			writer.Write(Capacity);
			writer.Write(TransferCoefficient);
			writer.Write(Area);
		}

		public void Read(BinaryReader reader)
		{
			Heat = reader.ReadSingle();
			Capacity = reader.ReadSingle();
			TransferCoefficient = reader.ReadSingle();
			Area = reader.ReadSingle();
		}

		public override string ToString() => $"Heat: {Heat}/{Capacity}";

		public float TransferTo(HeatStorage storage, float insulation)
		{
			float canAccept = storage.Area * storage.TransferCoefficient;
			float canSend = Area * TransferCoefficient;
			float transfer = MathUtility.Min(canAccept, canSend, storage.Heat, Heat);

			float delta = storage.Temperature - Temperature;

			float transfered = transfer * -delta;

			if (transfer > 0)
			{
				transfered -= insulation;
				if (transfered < 0f) transfered = 0f;
			}
			else
			{
				transfered += insulation;
				if (transfered > 0f) transfered = 0f;
			}

			storage.Heat += transfered;
			Heat -= transfered;

			return transfered;
		}
	}

	public interface IHeatStorage
	{
		HeatStorage GetHeatStorage();
	}
}