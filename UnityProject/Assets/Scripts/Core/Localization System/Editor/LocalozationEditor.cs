using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using Unity.EditorCoroutines.Editor;

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
				AddRenewLocalizationKeyInPrefabs();
			}
			if (GUILayout.Button("Export Items JSON Example"))
			{
				ExportLocalizationExample();
			}

		}

		private void ExportLocalizationExample()
		{
			int index = 0;
			var objectsInScene = GetNonSceneObjects();
			var localizedItemsData = new LocalizedItemData();
			localizedItemsData.ItemsData = new Item[objectsInScene.Count];
			foreach (LocalizedText localizedText in objectsInScene)
			{
				var component = localizedText.gameObject.GetComponent<Items.ItemAttributesV2>();
				if (component.InitialName != "Unnamed" && component.InitialName != "")
				{
					Item item = new Item();
					ItemData itemDataForExport = new ItemData();

					item.ItemName = component.InitialName;
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

			File.WriteAllText(filePathItems, Newtonsoft.Json.JsonConvert.SerializeObject(localizedItemsData, Newtonsoft.Json.Formatting.Indented, Localization.Converter.Settings), System.Text.Encoding.UTF8);
		}

		private void AddRenewLocalizationKeyInPrefabs()
		{
			var objectsInScene = GetNonSceneObjects();

			foreach (LocalizedText localizedText in objectsInScene)
			{
				EditorUtility.SetDirty(localizedText);
				localizedText.SetKey(localizedText.gameObject.name);
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

		private List<LocalizedText> GetNonSceneObjects()
		{
			List<LocalizedText> objectsInScene = new List<LocalizedText>();

			foreach (LocalizedText go in Resources.FindObjectsOfTypeAll(typeof(LocalizedText)) as LocalizedText[])
			{
				if (EditorUtility.IsPersistent(go.transform.root.gameObject) && !(go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave))
					objectsInScene.Add(go);
			}

			return objectsInScene;
		}

	}
}

