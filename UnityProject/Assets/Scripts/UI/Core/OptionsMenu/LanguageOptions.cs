using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Unitystation.Options
{
	public class LanguageOptions : MonoBehaviour
	{
		public GameObject dropDown;
		Dropdown languageDropDownm_Dropdown;

		void Start()
		{
			//Fetch the Dropdown GameObject
			languageDropDownm_Dropdown = dropDown.GetComponent("Dropdown") as Dropdown;
			//Add listener for when the value of the Dropdown changes, to take action
			languageDropDownm_Dropdown.onValueChanged.AddListener(delegate
			{
				DropdownValueChanged(languageDropDownm_Dropdown);
			});
		}

		//Ouput the new value of the Dropdown into Text
		void DropdownValueChanged(Dropdown change)
		{
			Localization.LocalizationManager localizationManager = Localization.LocalizationManager.GetLocalizationManager();
			localizationManager.LoadLocalizedText();
		}
	}
}