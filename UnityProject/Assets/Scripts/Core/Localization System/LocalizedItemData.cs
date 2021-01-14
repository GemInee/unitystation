using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Localization
{
	public class LocalizedItem : MonoBehaviour
	{
		public string InitialItemName { get; set; }
		public string InitialItemDescription { get; set; }
		public string ExportName { get; set; }
		public string ExportDescription { get; set; }
		public string ExportMessage { get; set; }
	}

	public class LocalizedItemData : MonoBehaviour
	{
		public string ItemName { get; set; }
		public Dictionary<string, LocalizedItem> items { get; set; }
	}
}
