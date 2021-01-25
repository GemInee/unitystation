//    var LocalizedItemData = LocalizedItemData.FromJson(jsonString);

namespace Localization
{
	using System.Globalization;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Converters;

	public partial class LocalizedItemData
	{
		[JsonProperty("items")]
		public Item[] Items { get; set; }
	}

	public partial class Item
	{
		[JsonProperty("itemName")]
		public string ItemName { get; set; }

		[JsonProperty("itemData")]
		public ItemData ItemData { get; set; }
	}

	public partial class ItemData
	{
		[JsonProperty("InitialItemName")]
		public string InitialItemName { get; set; }

		[JsonProperty("InitialItemDescription")]
		public string InitialItemDescription { get; set; }

		[JsonProperty("ExportName")]
		public string ExportName { get; set; }

		[JsonProperty("ExportDescription")]
		public string ExportDescription { get; set; }

		[JsonProperty("ExportMessage")]
		public string ExportMessage { get; set; }
	}

	public partial class LocalizedItemData
	{
		public static LocalizedItemData FromJson(string json) => JsonConvert.DeserializeObject<LocalizedItemData>(json, Localization.Converter.Settings);
	}

	public static class Serialize
	{
		public static string ToJson(this LocalizedItemData self) => JsonConvert.SerializeObject(self, Localization.Converter.Settings);
	}

	internal static class Converter
	{
		public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
		{
			MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
			DateParseHandling = DateParseHandling.None,
			Converters =
			{
				new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
			},
		};
	}
}
