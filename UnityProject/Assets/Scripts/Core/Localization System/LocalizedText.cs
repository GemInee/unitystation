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
		//public string KeyString;


		private Component textForLocalize;
		//private Component itemForLocalize;

		void Start()
		{
			//this.gameObject.GetType();


			if (GetComponent<Text>() != null)
			{
				//textForLocalize = GetComponent<Text>();
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

		public void SetKey(string Key)
		{
			key = Key;
		}

		public void SetLocalizationText(string localizedText)
		{
			if(localizedText != null)
			{
				Text text = textForLocalize as Text;
				this.gameObject.GetComponent<Text>().text = localizedText;
			}

		}

		public void SetLocalizationItems(ItemData itemData)
		{
			if(itemData != null)
			{
				var itemForLocalize = gameObject.GetComponent<Items.ItemAttributesV2>();
				itemForLocalize.ServerSetArticleName(itemData.InitialItemName);
				itemForLocalize.ServerSetArticleDescription(itemData.InitialItemDescription);
			}
		}

		public void SetLocalizationStrings(Dictionary<string, string> localizedDictionary)
		{
			//присрать отправку набора переведенных текстов в скрипты чата, либо прировнять чат к айтему
		}
	}
}