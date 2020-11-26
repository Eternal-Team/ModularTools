namespace ModularTools
{
	public static class Utility
	{
		public static float ToKelvin(float deg) => 273.15f + deg;

		public static float ToDegrees(float kelvin) => kelvin - 273.15f;
	}
}