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
			if (GUILayout.Button("Export Objects localization example"))
			{
				ExportObjectsLocalizationExample();
			}

		}

		// Обработчик процедуры экспорта файла дефолтной локали в ДЖСОН файл
		private void ExportUILocalizationExample()
		{
			// Получаем все объекты содержащие текстовую компоненту
			var textLabels = GetSceneTextComponents();

			// Подготовим словарик для дефолтной локализации, которую будем сериализовывать в ДЖСОН. Словарик автоматически удалит дубликаты ключей
			Dictionary<string, string> localizationUIDataFilter = new Dictionary<string, string>();

			// Обработаем полученные ранее геймобъекты содержащие текстовую компоненту
			foreach (Text textLabel in textLabels)
			{
				// Подготовим объект для получения компоненты локализации у геймобъекта с текстовой компонентой
				LocalizedText localizedText = textLabel.gameObject.GetComponent<LocalizedText>();
				if (localizedText == null) // Убедимся, что компонента действительно есть
				{
					// Если компонента не найдена, то сразу же добавим её и присвоим ключи
					localizedText = textLabel.gameObject.AddComponent<LocalizedText>();

					string currentKey = textLabel.gameObject.name;
					var currentParent = localizedText.gameObject.transform.parent;
					while (currentParent != null)
					{
						currentKey = currentKey + "_" + currentParent.name;
						currentParent = currentParent.parent;
					}
					localizedText.SetKey(currentKey);
					//Debug.LogError("Not found LocalizedTesx component. Added new component: " + localizedText.name); // TODO: Довести до ума текст записи в журнал.
				}

				// Теперь компонента точно готова к работе
				// Обработаем ошибки дубликатов ключей, чтобы на выходе получить словарь только с уникальными ключами
				try
				{
					localizationUIDataFilter.Add(localizedText.GetKey(), textLabel.text);
				}
				catch{
					//Debug.LogError("Cannot add item: " + localizedText.name + ". Possible doubled name in UI");
				}
			}

			LocalizationUIData localizationUIData = new LocalizationUIData
			{
				Items = new LocalizationUIItem[textLabels.Count]
			};

			// Словарь с уникальными значениями готов, пока в качестве костыля переведем словарь в ранее заготовленную структуру данных для локализаций
			var i = 0;
			foreach (var itemUI in localizationUIDataFilter)
			{
				LocalizationUIItem item = new LocalizationUIItem();
				item.Key = itemUI.Key;
				item.Value = itemUI.Value;
				localizationUIData.Items[i] = item;
				i++;
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

			var textLabels = GetSceneTextComponents();
			foreach (Text textLabel in textLabels)
			{
				string currentKey = textLabel.gameObject.name;
				if (textLabel.gameObject.GetComponent<LocalizedText>() == null)
				{

					textLabel.gameObject.AddComponent<LocalizedText>();
				}

				var currentParent = textLabel.gameObject.transform.parent;
				while (currentParent != null)
				{
					currentKey = currentKey + "_" + currentParent.name;
					currentParent = currentParent.parent;
				}



				textLabel.gameObject.GetComponent<LocalizedText>().SetKey(currentKey);
				EditorUtility.SetDirty(textLabel.gameObject.GetComponent<LocalizedText>());
			}

		}

		private void ExportItemLocalizationExample()
		{
			int index = 0;
			var objectsInScene = GetNonSceneItemPrefabs();
			var exportItemData = new Dictionary<string, ItemData>();

			var localizedItemsData = new LocalizedItemData
			{
				ItemsData = new Item[objectsInScene.Count]
			};

			foreach (Items.ItemAttributesV2 localizedText in objectsInScene)
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

					try
					{
						exportItemData.Add(item.ItemName, item.ItemData);
					}
					catch
					{
						//debug.logerror("cannot add item: " + localizedtext.name + ". possible doubled name in prefabs");
					}
				}
			}
			var itemDataExportStructure = new LocalizedItemData
			{
				ItemsData = new Item[objectsInScene.Count]
			};
			int i = 0;
			foreach(var exportItem in exportItemData)
			{
				Item item = new Item();
				item.ItemName = exportItem.Key;
				item.ItemData = exportItem.Value;
				itemDataExportStructure.ItemsData[i] = item;

				i++;
			}

			string fileNameItems = "English_items.json";
			string filePathItems = Path.Combine(Application.streamingAssetsPath, "Localizations", fileNameItems);

			if (File.Exists(filePathItems))
			{
				File.Delete(filePathItems);
			}
			File.WriteAllText(filePathItems, Newtonsoft.Json.JsonConvert.SerializeObject(itemDataExportStructure, Newtonsoft.Json.Formatting.Indented, Localization.Converter.Settings), System.Text.Encoding.UTF8);
		}

		// Экспорт примера локализации для Объектов (не предметов)
		private void ExportObjectsLocalizationExample()
		{
			int index = 0;
			var objectsInScene = GetNonSceneObjectsPrefabs();
			var exportObjectData = new Dictionary<string, ObjectData>();

			var localizedItemsData = new LocalizedObjectData
			{
				ObjectsData = new Object[objectsInScene.Count]
			};

			foreach (ObjectAttributes localizedText in objectsInScene)
			{
				var component = localizedText.gameObject.GetComponent<ObjectAttributes>();
				if (component.InitialName != "Unnamed" && component.InitialName != "")
				{
					Object obj = new Object();
					ObjectData objectDataForExport = new ObjectData();

					obj.ObjectName = localizedText.name;
					objectDataForExport.InitialObjectName = component.InitialName;
					objectDataForExport.InitialObjectDescription = component.InitialDescription;
					objectDataForExport.ExportName = component.ExportName;
					objectDataForExport.ExportMessage = component.ExportMessage;
					obj.ObjectData = objectDataForExport;
					localizedItemsData.ObjectsData[index] = obj;
					index++;

					try
					{
						exportObjectData.Add(obj.ObjectName, obj.ObjectData);
					}
					catch
					{
						//debug.logerror("cannot add item: " + localizedtext.name + ". possible doubled name in prefabs");
					}
				}
			}
			var objectDataExportStructure = new LocalizedObjectData
			{
				ObjectsData = new Object[objectsInScene.Count]
			};
			int i = 0;
			foreach (var exportObject in exportObjectData)
			{
				Object obj = new Object();
				obj.ObjectName = exportObject.Key;
				obj.ObjectData = exportObject.Value;
				objectDataExportStructure.ObjectsData[i] = obj;

				i++;
			}

			string fileNameObjects = "English_objects.json";
			string filePathObjects = Path.Combine(Application.streamingAssetsPath, "Localizations", fileNameObjects);

			if (File.Exists(filePathObjects))
			{
				File.Delete(filePathObjects);
			}
			File.WriteAllText(filePathObjects, Newtonsoft.Json.JsonConvert.SerializeObject(objectDataExportStructure, Newtonsoft.Json.Formatting.Indented, Localization.Converter.Settings), System.Text.Encoding.UTF8);
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

		private List<ObjectAttributes> GetNonSceneObjectsPrefabs()
		{
			List<ObjectAttributes> objectsInScene = new List<ObjectAttributes>();

			foreach (ObjectAttributes go in Resources.FindObjectsOfTypeAll(typeof(ObjectAttributes)) as ObjectAttributes[])
			{
				if (EditorUtility.IsPersistent(go.transform.root.gameObject) && !(go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave))
					objectsInScene.Add(go);
			}

			return objectsInScene;
		}

		private List<Text> GetSceneTextComponents()
		{
			List<Text> objectsInScene = new List<Text>();

			foreach (Text go in Resources.FindObjectsOfTypeAll(typeof(Text)) as Text[])
			{
				if (EditorUtility.IsPersistent(go.transform.root.gameObject) && !(go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave))
					objectsInScene.Add(go);
			}

			return objectsInScene;
		}
	}
}

