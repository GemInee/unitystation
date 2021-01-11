using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizedText : MonoBehaviour
{
	
	public string key;

	void Start()
	{

		LocalizedText localizedGemaObjectComponent = GetComponent<LocalizedText>();
		LocalizationManager.OnWakeGameObjectCachForLocalization(localizedGemaObjectComponent);


	}

	public void SetLocalizationText(string localizedText)
	{

		Text text = GetComponent<Text>();
		text.text = localizedText;

	}
}
