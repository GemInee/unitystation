using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Localization
{
	public class LocalizedText : MonoBehaviour
	{

		[SerializeField]
		private string key;
		public string Key => key;

		private Component textForLocalize;
		//private Component itemForLocalize;

		void Start()
		{



			if (GetComponent<Text>() != null)
			{
				textForLocalize = GetComponent<Text>();
				LocalizationManager.OnWakeGameObjectUICacheForLocalization(GetComponent<LocalizedText>());
			}
			else if (GetComponent<Items.ItemAttributesV2>() != null)
			{
				// тут надо получить дикшинари со списком всего необходимого к локализации текста
				//itemForLocalize = GetComponent<Items.ItemAttributesV2>
				LocalizationManager.OnWakeItemsCacheForLocalization(GetComponent<LocalizedText>());
			}
			//GetComponent<Strings.ChatTemplates>() != null || GetComponent<Strings.ReportTemplates>() != null с этим надо что то делать
			//else if (null != null)
			//{
			//	LocalizationManager.OnWakeStringsCacheForLocalization(localizedGameObjectComponent);
			//}
			else
			{
				Debug.LogError("ERROR: Scrip in " + gameObject.name + " not found components for localizations!", gameObject);
			}

		}

		public string GetKey()
		{
			return key;
		}

		public void SetLocalizationText(string localizedText)
		{
			Text text = textForLocalize as Text;
			text.text = localizedText;
		}

		public void SetLocalizationItems(ItemData itemData)
		{
			var itemForLocalize = gameObject.GetComponent<Items.ItemAttributesV2>();
			itemForLocalize.ServerSetArticleName(itemData.InitialItemName);
			itemForLocalize.ServerSetArticleDescription(itemData.InitialItemDescription);

		}

		public void SetLocalizationStrings(Dictionary<string, string> localizedDictionary)
		{
			//присрать отправку набора переведенных текстов в скрипты чата, либо прировнять чат к айтему
		}
	}
}