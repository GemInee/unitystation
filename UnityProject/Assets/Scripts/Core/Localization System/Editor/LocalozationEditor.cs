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

			}
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

