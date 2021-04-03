//    var LocalizedObjectData = LocalizedObjectData.FromJson(jsonString);

namespace Localization
{
	using System.Globalization;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Converters;

	public partial class LocalizedObjectData
	{
		[JsonProperty("objects")]
		public Object[] ObjectsData { get; set; }
	}

	public partial class Object
	{
		[JsonProperty("objectName")]
		public string ObjectName { get; set; }

		[JsonProperty("objectData")]
		public ObjectData ObjectData { get; set; }
	}

	public partial class ObjectData
	{
		[JsonProperty("InitialObjectName")]
		public string InitialObjectName { get; set; }

		[JsonProperty("InitialObjectDescription")]
		public string InitialObjectDescription { get; set; }

		[JsonProperty("ExportName")]
		public string ExportName { get; set; }

		[JsonProperty("ExportMessage")]
		public string ExportMessage { get; set; }
	}

	public partial class LocalizedObjectData
	{
		public static LocalizedObjectData FromJson(string json) => JsonConvert.DeserializeObject<LocalizedObjectData>(json, Localization.Converter.Settings);
	}

	public static class SerializeObjectData
	{
		public static string ToJson(this LocalizedObjectData self) => JsonConvert.SerializeObject(self, Localization.Converter.Settings);
	}

}
