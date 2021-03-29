namespace Localization
{
	using System;
	using System.Collections.Generic;

	using System.Globalization;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Converters;


	public partial class LocalizationUIData
	{
		[JsonProperty("items")]
		public LocalizationUIItem[] Items { get; set; }
	}

	public partial class LocalizationUIItem
	{
		[JsonProperty("key")]
		public string Key { get; set; }

		[JsonProperty("value")]
		public string Value { get; set; }
	}

	public partial class LocalizationUIData
	{
		public static LocalizationUIData FromJson(string json) => JsonConvert.DeserializeObject<LocalizationUIData>(json, Localization.Converter.Settings);
	}

	public static class Serialize
	{
		public static string ToJson(this LocalizationUIData self) => JsonConvert.SerializeObject(self, Localization.Converter.Settings);
	}
	
}