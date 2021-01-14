using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

namespace Localization
{
	public class LocalizationManager : MonoBehaviour
	{
		public delegate void LanguageIsChangeEventHandler();
		public event LanguageIsChangeEventHandler OnLanguageChanged;
		public static LocalizationManager instance;
		public GameObject dropDown;
		private Dictionary<string, string> localizedText;
		private Dictionary<string, LocalizedItemData> localizedItemData;
		private static List<LocalizedText> cacheLocalizedGameObjectsUIComponents;
		private static List<LocalizedText> cacheLocalizedItems;
		private static List<LocalizedText> cacheLocalizedStrings;
		private FileInfo[] LocalizedFiles;
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
			int choicedLanguage = dropdown.value;

			//Грузим локализацию для UI
			string fileNameUI = LocalizedFiles[dropdown.value].Name;

			localizedText = new Dictionary<string, string>();
			string filePathUI = Path.Combine(Application.streamingAssetsPath, "Localizations", fileNameUI);

			if (File.Exists(filePathUI))
			{
				string dataJson = File.ReadAllText(filePathUI);
				LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataJson);

				for (int i = 0; i < loadedData.items.Length; i++)
				{
					localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
				}

			}
			else
			{
				Debug.LogError("Cannot find file");
			}

			isReady = true;
			//Применяем локализацию для UI
			foreach (LocalizedText component in cacheLocalizedGameObjectsUIComponents)
			{

				component.SetLocalizationText((GetLocalizedValue(component.GetKey())));

			}

			//Грузим локализацию для Items
			string fileNameItems = LocalizedFiles[dropdown.value].Name + "_items";

			//Готовим словарик приёмник для локализаций
			localizedItemData = new Dictionary<string, LocalizedItemData>();
			string filePathItems = Path.Combine(Application.streamingAssetsPath, "Localizations", fileNameItems);

			if (File.Exists(filePathItems))
			{
				string dataJson = File.ReadAllText(filePathItems);
				LocalizedItemData loadedData = JsonUtility.FromJson<LocalizedItemData>(dataJson);

				for (int i = 0; i < loadedData.it .Length; i++)
				{
					localizedItemData.Add(loadedData.items[i].key, loadedData.items[i].value);
				}

			}
			else
			{
				Debug.LogError("Cannot find file");
			}





		}

		public string GetLocalizedValue(string key)
		{
			string result = "Text not found";
			if (localizedText.ContainsKey(key))
			{
				result = localizedText[key];
			}
			else
			{
				result = key;
				Debug.LogError("ERROR: Scrip in " + gameObject.name + " not found text for localaizeing with KEY: " + key +"!", gameObject);
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
			LocalizedFiles = dir.GetFiles("*.json");

			Dropdown.OptionData optionData;
			foreach (FileInfo f in LocalizedFiles)
			{
				optionData = new Dropdown.OptionData(f.Name.Remove(f.Name.Length - f.Extension.Length), null);
				ddOptionsList.options.Add(optionData);
			}
			dropdown.ClearOptions();
			dropdown.options = ddOptionsList.options;
		}

		public bool GetIsReady()
		{
			return isReady;
		}
	}
}
