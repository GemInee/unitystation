namespace Localization
{
	using System.Collections.Generic;
	using Newtonsoft.Json;

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
}