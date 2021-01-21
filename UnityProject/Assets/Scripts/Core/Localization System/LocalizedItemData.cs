using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Localization
{
	[System.Serializable]
	public class ItemData : MonoBehaviour
	{
		public string InitialItemName { get; set; }
		public string InitialItemDescription { get; set; }
		public string ExportName { get; set; }
		public string ExportDescription { get; set; }
		public string ExportMessage { get; set; }
	}

	[System.Serializable]
	public class Item : MonoBehaviour
	{
		public string itemName { get; set; }
		public ItemData itemData { get; set; }
	}

	[System.Serializable]
	public class Root
	{
		public Item[] items;
	}

}
