using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.UI;
using Unity.EditorCoroutines.Editor;
using System;

namespace Localization
{

	public class LocalizationEditor : EditorWindow
	{
		public LocalizationUIData localizationData;

		[MenuItem("Window/Localized text Editor")]
		static void ShowWindow() => GetWindow<LocalizationEditor>("Localization editor");


		private void OnGUI()
		{
			if (localizationData != null)
			{
				SerializedObject serializedObject = new SerializedObject(this);
				SerializedProperty serializedProperty = serializedObject.FindProperty("localizationData");
				EditorGUILayout.PropertyField(serializedProperty, true);
				serializedObject.ApplyModifiedProperties();

				if (GUILayout.Button("Save data"))
				{
					SaveGameData();
				}

			}

			if (GUILayout.Button("Load data"))
			{
				LoadingData();
			}

			if (GUILayout.Button("Create new data"))
			{
				CreateNewData();
			}
			if (GUILayout.Button("Add/Renew localization keys in all prefabs"))
			{
				AddRenewLocalizationInItemPrefabs();
			}
			if (GUILayout.Button("Add/Renew loc component in all Text Components"))
			{
				AddRenewLocalizationToUIObjects();
			}
			if (GUILayout.Button("Export Items JSON Example"))
			{
				ExportItemLocalizationExample();
			}
			if(GUILayout.Button("Export UI localization example"))
			{
				ExportUILocalizationExample();
			}

		}

		private void ExportUILocalizationExample()
		{
			Text[] textLabels = FindObjectsOfType<Text>();

			LocalizationUIData localizationUIData = new LocalizationUIData
			{
				items = new LocalizationUIItem[textLabels.Length]
			};
			int index = 0;
			foreach (Text textLabel in textLabels)
			{

				LocalizedText localizedText = textLabel.gameObject.GetComponent<LocalizedText>();
				if (localizedText == null)
				{
					localizedText = textLabel.gameObject.AddComponent<LocalizedText>();

					string currentKey = textLabel.gameObject.name;
					var currentParent = localizedText.gameObject.transform.parent;
					while (currentParent != null)
					{
						currentKey = currentKey + "_" + currentParent.name;
						currentParent = currentParent.parent;
					}
					localizedText.SetKey(currentKey);
				}

				localizationUIData.items[index].key = localizedText.GetKey();
				localizationUIData.items[index].value = textLabel.text;
				index++;
			}
			string fileNameItems = "English.json";
			string filePathItems = Path.Combine(Application.streamingAssetsPath, "Localizations", fileNameItems);

			if (File.Exists(filePathItems))
			{
				File.Delete(filePathItems);
			}
			File.WriteAllText(filePathItems, Newtonsoft.Json.JsonConvert.SerializeObject(localizationUIData, Newtonsoft.Json.Formatting.Indented, Localization.Converter.Settings), System.Text.Encoding.UTF8);


		}

		private void AddRenewLocalizationToUIObjects()
		{
			Text[] textLabels = FindObjectsOfType<Text>();
			foreach (Text textLabel in textLabels)
			{
				string currentKey = textLabel.gameObject.name;
				LocalizedText localizedText = textLabel.gameObject.GetComponent<LocalizedText>();
				if (localizedText == null)
				{
					localizedText = textLabel.gameObject.AddComponent<LocalizedText>();
				}

				var currentParent = localizedText.gameObject.transform.parent;
				while (currentParent != null)
				{
					currentKey = currentKey + "_" + currentParent.name;
					currentParent = currentParent.parent;
				}

				localizedText.SetKey(currentKey);
				EditorUtility.SetDirty(localizedText.gameObject);
			}

		}

		private void ExportItemLocalizationExample()
		{
			int index = 0;
			var objectsInScene = GetNonSceneLocalizedTextComponents();
			var localizedItemsData = new LocalizedItemData
			{
				ItemsData = new Item[objectsInScene.Count]
			};
			foreach (LocalizedText localizedText in objectsInScene)
			{
				var component = localizedText.gameObject.GetComponent<Items.ItemAttributesV2>();
				if (component.InitialName != "Unnamed" && component.InitialName != "")
				{
					Item item = new Item();
					ItemData itemDataForExport = new ItemData();

					item.ItemName = localizedText.name;
					itemDataForExport.InitialItemName = component.InitialName;
					itemDataForExport.InitialItemDescription = component.InitialDescription;
					itemDataForExport.ExportName = component.ExportName;
					itemDataForExport.ExportMessage = component.ExportMessage;
					item.ItemData = itemDataForExport;
					localizedItemsData.ItemsData[index] = item;
					index++;
				}

				//try
				//{
				//	localizedItemsData.Add(localizedText.name, itemDataForExport);
				//}
				//catch
				//{
				//	Debug.LogError("Cannot add item: " + localizedText.name + ". Possible doubled name in prefabs");
				//}

			}
			string fileNameItems = "English_items.json";
			string filePathItems = Path.Combine(Application.streamingAssetsPath, "Localizations", fileNameItems);

			if (File.Exists(filePathItems))
			{
				File.Delete(filePathItems);
			}
			File.WriteAllText(filePathItems, Newtonsoft.Json.JsonConvert.SerializeObject(localizedItemsData, Newtonsoft.Json.Formatting.Indented, Localization.Converter.Settings), System.Text.Encoding.UTF8);
		}

		//Добавить процедуру проверки наличия компоненты локализации и добавления, если её нет с созданием ключа
		private void AddRenewLocalizationInItemPrefabs()
		{

			var itemComponentsInScene = GetNonSceneItemPrefabs();
			
			foreach (Items.ItemAttributesV2 item in itemComponentsInScene)
			{
				if(item.gameObject.GetComponent<LocalizedText>() == null)
				{
					item.gameObject.AddComponent<LocalizedText>();
				}

				if(item.gameObject.name != item.gameObject.GetComponent<LocalizedText>().GetKey())
				{
					item.gameObject.GetComponent<LocalizedText>().SetKey(item.gameObject.name);
				}
				EditorUtility.SetDirty(item.gameObject);
			}
		}

		private void LoadingData()
		{
			string filePath = EditorUtility.OpenFilePanel("Select localozation data file", Application.streamingAssetsPath, "json");

			if (!string.IsNullOrEmpty(filePath))
			{
				string dataAsJson = File.ReadAllText(filePath);

				localizationData = JsonUtility.FromJson<LocalizationUIData>(dataAsJson);
			}

		}

		private void SaveGameData()
		{
			string filePath = EditorUtility.SaveFilePanel("Save localization data file", Application.streamingAssetsPath, "", "json");

			if (!string.IsNullOrEmpty(filePath))
			{
				string dataAsJson = JsonUtility.ToJson(localizationData);
				File.WriteAllText(filePath, dataAsJson);
			}
		}

		private void CreateNewData()
		{
			localizationData = new LocalizationUIData();
		}

		// РЕФАКТОРИНГ: Переделать получение объектов по запрашиваемому типу, а строко по одному
		private List<LocalizedText> GetNonSceneLocalizedTextComponents()
		{
			List<LocalizedText> objectsInScene = new List<LocalizedText>();

			foreach (LocalizedText go in Resources.FindObjectsOfTypeAll(typeof(LocalizedText)) as LocalizedText[])
			{
				if (EditorUtility.IsPersistent(go.transform.root.gameObject) && !(go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave))
					objectsInScene.Add(go);
			}

			return objectsInScene;
		}

		private List<Items.ItemAttributesV2> GetNonSceneItemPrefabs()
		{
			List<Items.ItemAttributesV2> objectsInScene = new List<Items.ItemAttributesV2>();

			foreach (Items.ItemAttributesV2 go in Resources.FindObjectsOfTypeAll(typeof(Items.ItemAttributesV2)) as Items.ItemAttributesV2[])
			{
				if (EditorUtility.IsPersistent(go.transform.root.gameObject) && !(go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave))
					objectsInScene.Add(go);
			}

			return objectsInScene;
		}
	}
}

