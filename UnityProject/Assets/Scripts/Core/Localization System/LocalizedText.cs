using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizedText : MonoBehaviour
{
	
	public string key;

	void Start()
	{

		LocalizedText localizedGameObjectComponent = GetComponent<LocalizedText>();
		LocalizationManager.OnWakeGameObjectCacheForLocalization(localizedGameObjectComponent);


	}

	public void SetLocalizationText(string localizedText)
	{

		Text text = GetComponent<Text>();
		text.text = localizedText;

	}
}
