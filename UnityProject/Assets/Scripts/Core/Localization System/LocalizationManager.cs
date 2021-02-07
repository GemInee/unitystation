using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Localization
{
	public class LocalizationManager : MonoBehaviour
	{
		public delegate void LanguageIsChangeEventHandler();
		public event LanguageIsChangeEventHandler OnLanguageChanged;
		public static LocalizationManager instance;
		public GameObject dropDown;
		private Dictionary<string, string> localizedText;
		private Dictionary<string, ItemData> localizedItemsData;
		private static List<LocalizedText> cacheLocalizedGameObjectsUIComponents;
		private static List<LocalizedText> cacheLocalizedItems;
		private static List<LocalizedText> cacheLocalizedStrings;
		private FileInfo[] LocalizedFilesCache;
		private bool isReady = false;

		//public delegate void LanguageIsChangeEventHandler();
		//public event LanguageIsChangeEventHandler LanguageIsChanged;

		//initialization
		void Awake()
		{
			if (instance == null)
			{
				instance = this;
			}
			else if (instance != this)
			{
				Destroy(gameObject);
			}
			DontDestroyOnLoad(gameObject);

			cacheLocalizedGameObjectsUIComponents = new List<LocalizedText>();
			cacheLocalizedItems = new List<LocalizedText>();
			cacheLocalizedStrings = new List<LocalizedText>();
			FillDropDown();
		}

		public void LoadLocalizedText()
		{
			//Выбор локализации
			Dropdown dropdown = dropDown.GetComponent("Dropdown") as Dropdown;
			//int choicedLanguage = dropdown.value;

			//Грузим локализацию для UI
			string fileNameUI = dropdown.options[dropdown.value].text + ".json";
				//currentAvailableLocalizationFileNames.;
				//LocalizedFilesCache[dropdown.value].Name;

			localizedText = new Dictionary<string, string>();
			string filePathUI = Path.Combine(Application.streamingAssetsPath, "Localizations", fileNameUI);

			if (File.Exists(filePathUI))
			{
				string dataJson = File.ReadAllText(filePathUI);
				LocalizationUIData loadedData = JsonUtility.FromJson<LocalizationUIData>(dataJson);

				for (int i = 0; i < loadedData.Items.Length; i++)
				{
					localizedText.Add(loadedData.Items[i].Key, loadedData.Items[i].Value);
				}

			}
			else
			{
				Debug.LogError("Cannot find file");
			}


			//Применяем локализацию для UI
			foreach (LocalizedText component in cacheLocalizedGameObjectsUIComponents)
			{

				component.SetLocalizationText((GetLocalizedValue(component.GetKey())));

			}
			//f.Name.Remove(f.Name.Length - f.Extension.Length)
			//Грузим локализацию для Items
			string fileNameItems = dropdown.options[dropdown.value].text + "_items.json";

			//Готовим словарик приёмник для локализаций
			localizedItemsData = new Dictionary<string, ItemData>();
			string filePathItems = Path.Combine(Application.streamingAssetsPath, "Localizations", fileNameItems);

			if (File.Exists(filePathItems))
			{
				string jsonString = File.ReadAllText(filePathItems);

				var loadedLocalizedItemData = LocalizedItemData.FromJson(jsonString);

				for (int i = 0; i < loadedLocalizedItemData.ItemsData.Length; i++)
				{
					localizedItemsData.Add(loadedLocalizedItemData.ItemsData[i].ItemName, loadedLocalizedItemData.ItemsData[i].ItemData);
				}

			}
			else
			{
				Debug.LogError("Cannot find file");
			}

			foreach(LocalizedText component in cacheLocalizedItems)
			{

				component.SetLocalizationItems(GetLocalizedValueForItem(component.GetKey()));
			}



			isReady = true;
		}

		public string GetLocalizedValue(string key)
		{
			string result;
			if (localizedText.ContainsKey(key))
			{
				result = localizedText[key];
			}

			else
			{
				result = key;
				Debug.LogError("ERROR: Scrip in " + gameObject.name + " not found text for localaizeing with KEY: " + key + "!", gameObject);
			}

			return result;
		}

		public ItemData GetLocalizedValueForItem(string key)
		{
			ItemData result = null;

			if (localizedItemsData.ContainsKey(key))
			{
				result = localizedItemsData[key];
			}
			else
			{
				//result = key;
				Debug.LogError("ERROR: Scrip in " + gameObject.name + " not found text for localaizeing with KEY: " + key + "!", gameObject);
			}

			return result;
		}

		public static void OnWakeGameObjectUICacheForLocalization(LocalizedText component)
		{
			cacheLocalizedGameObjectsUIComponents.Add(component);
		}

		public static void OnWakeItemsCacheForLocalization(LocalizedText component)
		{
			cacheLocalizedItems.Add(component);
		}

		public static void OnWakeStringsCacheForLocalization(LocalizedText component)
		{
			cacheLocalizedStrings.Add(component);
		}

		public void FillDropDown()
		{
			Dropdown dropdown = dropDown.GetComponent("Dropdown") as Dropdown;
			string filePath = Path.Combine(Application.streamingAssetsPath, "Localizations");
			Dropdown.OptionDataList ddOptionsList = new Dropdown.OptionDataList();
			DirectoryInfo dir = new DirectoryInfo(filePath);
			LocalizedFilesCache = dir.GetFiles("*.json"); //Возможно потом будем пересобирать перечень локалей так, чтобы однозначно сопоставлять номер в списке и номер в дропдауне. Если будет косячить.

			Dropdown.OptionData optionData;
			foreach (FileInfo f in LocalizedFilesCache)
			{
				//Check for current file is a base Localisation file
				if (!f.Name.Contains("_items"))
				{
					//If true - add file name in dropdown list
					optionData = new Dropdown.OptionData(f.Name.Remove(f.Name.Length - f.Extension.Length), null);
					ddOptionsList.options.Add(optionData);
				}

			}
			dropdown.ClearOptions();
			dropdown.options = ddOptionsList.options;
		}

		public bool GetIsReady()
		{
			return isReady;
		}

		public void ExportLocalizationExample()
		{

		}
	}
}
