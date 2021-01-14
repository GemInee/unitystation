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

		void Start()
		{

			LocalizedText localizedGameObjectComponent = GetComponent<LocalizedText>();

			if (GetComponent<Text>() != null)
			{
				textForLocalize = GetComponent<Text>();
				LocalizationManager.OnWakeGameObjectUICacheForLocalization(localizedGameObjectComponent);
			}
			else if (GetComponent<Items.ItemAttributesV2>() != null)
			{
				// тут надо получить дикшинари со списком всего необходимого к локализации текста
				LocalizationManager.OnWakeItemsCacheForLocalization(localizedGameObjectComponent);
			}
			//GetComponent<Strings.ChatTemplates>() != null || GetComponent<Strings.ReportTemplates>() != null с этим надо что то делать
			else if (null != null)
			{
				LocalizationManager.OnWakeStringsCacheForLocalization(localizedGameObjectComponent);
			}
			else
			{
				Debug.LogError("ERROR: Scrip in " + gameObject + " not found components for localizations!");
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

		public void SetLocalizationItems(Dictionary<string, string> localizedDictionary)
		{
			//присрать отправку набора переведенных текстов в префаб item'ов
		}

		public void SetLocalizationStrings(Dictionary<string, string> localizedDictionary)
		{
			//присрать отправку набора переведенных текстов в скрипты чата, либо прировнять чат к айтему
		}
	}
}