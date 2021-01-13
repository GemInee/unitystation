using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class LocalizationManager : MonoBehaviour
{
	public delegate void LanguageIsChangeEventHandler();
	public event LanguageIsChangeEventHandler OnLanguageChanged;
	public static LocalizationManager instance;
	public GameObject dropDown;
	private Dictionary<string, string> localizedText;
	private static List<LocalizedText> cacheLocalizedGameObjectsComponents;
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

		cacheLocalizedGameObjectsComponents = new List<LocalizedText>();
		DontDestroyOnLoad(gameObject);
		FillDropDown();

	}

	public void LoadLocalizedText()
	{
		Dropdown dropdown = dropDown.GetComponent("Dropdown") as Dropdown;
		int choicedLanguage = dropdown.value;

		string fileName = LocalizedFiles[dropdown.value].Name;

		localizedText = new Dictionary<string, string>();
		string filePath = Path.Combine(Application.streamingAssetsPath, "Localizations", fileName);

		if (File.Exists(filePath))
		{
			string dataJson = File.ReadAllText(filePath);
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

		foreach (LocalizedText component in cacheLocalizedGameObjectsComponents)
		{

			component.SetLocalizationText((GetLocalizedValue(component.key)));

		}
	}

	public string GetLocalizedValue(string key)
	{
		string result = "Text not found";
		if (localizedText.ContainsKey(key))
		{
			result = localizedText[key];
		}

		return result;
	}

	public static void OnWakeGameObjectCacheForLocalization(LocalizedText component)
	{
		cacheLocalizedGameObjectsComponents.Add(component);
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
