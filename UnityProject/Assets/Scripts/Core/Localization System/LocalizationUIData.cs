namespace Localization
{
	[System.Serializable]
	public class LocalizationUIData
	{
		public LocalizationUIItem[] items;
	}

	[System.Serializable]
	public class LocalizationUIItem
	{
		public string key;
		public string value;
	}
}