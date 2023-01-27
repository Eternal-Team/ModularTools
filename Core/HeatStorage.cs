using System;
using System.IO;
using Terraria.ModLoader.IO;

namespace ModularTools.Core;

public class HeatStorage
{
	public float Heat;

	public float Temperature // K
	{
		get => Heat / Capacity;
		set => Heat = Capacity * value;
	}

	public float Capacity; // J/K
	public float ThermalConductivity; //  W/(m*K)
	public float Area; // m2

	public float CoolingCoefficient => ThermalConductivity * Area / Capacity;

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

	public void ModifyCapacity(float capacity)
	{
		float temperature = Temperature;

		Capacity += capacity;
		if (Capacity < 0) Capacity = 0;

		Heat = Capacity * temperature;
	}

	public float TransferToEnvironment(float environmentTemp, float timeStep)
	{
		// note: here I'm assuming the material is 1 meter thick
		float energyMoved = -ThermalConductivity * Area * (Temperature - environmentTemp) * timeStep;
		ModifyHeat(energyMoved);

		return energyMoved;
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
		["TransferCoefficient"] = ThermalConductivity,
		["Area"] = Area
	};

	public void Load(TagCompound tag)
	{
		Heat = tag.Get<float>("Heat");
		Capacity = tag.Get<float>("Capacity");
		ThermalConductivity = tag.Get<float>("TransferCoefficient");
		Area = tag.Get<float>("Area");
	}

	public void Write(BinaryWriter writer)
	{
		writer.Write(Heat);
		writer.Write(Capacity);
		writer.Write(ThermalConductivity);
		writer.Write(Area);
	}

	public void Read(BinaryReader reader)
	{
		Heat = reader.ReadSingle();
		Capacity = reader.ReadSingle();
		ThermalConductivity = reader.ReadSingle();
		Area = reader.ReadSingle();
	}

	public override string ToString() => $"Heat: {Heat}/{Capacity}";

	public float TransferTo(HeatStorage storage, float timeStep)
	{
		float area = MathF.Min(Area, storage.Area);
		float conductivity = MathF.Min(ThermalConductivity, storage.ThermalConductivity);
		float energyMoved = -conductivity * area * (Temperature - storage.Temperature) * timeStep;
		ModifyHeat(energyMoved);
		storage.ModifyHeat(energyMoved);

		return energyMoved;
	}
}

public interface IHeatStorage
{
	HeatStorage GetHeatStorage();
}