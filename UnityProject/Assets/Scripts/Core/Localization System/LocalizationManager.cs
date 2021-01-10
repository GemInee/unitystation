using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class LocalizationManager : MonoBehaviour
{
	public static LocalizationManager instance;
	private Dictionary<string, string> localizedText;
	private bool isReady = false;


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
	}

	public void LoadLocalizedText()
	{
		Dropdown dropdown = this.gameObject.GetComponent("Dropdown") as Dropdown;
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

	public bool GetIsReady()
	{
		return isReady;
	}
}
