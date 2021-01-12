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
	}

	public void LoadLocalizedText()
	{
		Dropdown dropdown = dropDown.GetComponent("Dropdown") as Dropdown;
		int choicedLanguage = dropdown.value;

		string fileName = choicedLanguage.ToString() + ".json";

		localizedText = new Dictionary<string, string>();
		string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

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

	public static void OnWakeGameObjectCachForLocalization(LocalizedText component)
	{
		cacheLocalizedGameObjectsComponents.Add(component);
	}

	public bool GetIsReady()
	{
		return isReady;
	}
}
