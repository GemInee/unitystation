using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Localization
{
	[System.Serializable]
	public class LocalizedItem : MonoBehaviour
	{
		public string InitialItemName { get; set; }
		public string InitialItemDescription { get; set; }
		public string ExportName { get; set; }
		public string ExportDescription { get; set; }
		public string ExportMessage { get; set; }
	}

	[System.Serializable]
	public class LocalizedItemDataSet : MonoBehaviour
	{
		public string ItemName { get; set; }
		public LocalizedItem localizedItem { get; set; }
	}

	[System.Serializable]
	public class LocalizationItemData
	{
		public LocalizedItemDataSet[] items;
	}

}
