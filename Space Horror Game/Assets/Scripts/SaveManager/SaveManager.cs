using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace SaveManagement
{
	public class SaveManager : MonoBehaviour
	{
        //[SerializeField] private AnalyticsEvent sessionEndEvent;

        private Dictionary<string, object> parameters = new Dictionary<string, object>();

        [SerializeField] private bool developmentWipe = false;

		private static Dictionary<string, SaveData> savedItems = new Dictionary<string, SaveData>();

		private static string nameOfSavingFile { get { return Application.persistentDataPath + Path.DirectorySeparatorChar + "SaveData.dat"; } } //Name of the text file that will be retrived
		private static bool canWrite; //bool that determines if the savemanager can write to the file 
		private static bool hasBeenInstantiated = false;
		private static bool hasFileBeenRead = false;

		private void Awake() //If it's a Development build wipes the save, reads from a file, and cancels any active notification
		{
			if (hasBeenInstantiated) Destroy(this);
			transform.parent = null;
			DontDestroyOnLoad(gameObject);
			hasBeenInstantiated = true;

			#if UNITY_EDITOR
			if (developmentWipe) WipeFile();
			#else
			if (Debug.isDebugBuild) WipeFile();
			#endif
			ReadFromFile();

			if (GetData("SaveDataRefresh") == null)
			{
				WipeFile();
				SetData("SaveDataRefresh", 1f);
			}
			ReadFromFile();

			canWrite = true;
        }

		//Writes to the file when the application pauses
		private void OnApplicationPause(bool pause) => WriteToFile();
		private void OnDestroy() => WriteToFile();
        private void OnApplicationQuit() => WriteToFile();

        #region Variable Retrieval
        public static object GetData(string key)
		{
			ReadFromFile();
			return savedItems.ContainsKey(key) ? savedItems[key].Value : null;
		}
		public static void SetData(string key, object value)
		{
			ReadFromFile();
			if (savedItems.ContainsKey(key)) savedItems[key].Value = value;
			else savedItems.Add(key, new SaveData(key, value));
		}
		public static void RemoveData(string key)
		{
			ReadFromFile();
			if (savedItems.ContainsKey(key)) savedItems.Remove(key);
		}
		#endregion

		#region File Manipulation
		private static void DataToDictionaries(Data data)
		{
			foreach (SaveData item in data.savedObjectsArray) savedItems.Add(item.Key, item);
		}
		private static void ReadFromFile()
		{
			if (hasFileBeenRead) return;

			Console.Log("Reading from file.");
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream fileStream = File.Open(nameOfSavingFile, FileMode.OpenOrCreate);

			if (fileStream.Length > 0)
			{
				Data savedData = (Data)formatter.Deserialize(fileStream);
				DataToDictionaries(savedData);
			}

			fileStream.Close();
			hasFileBeenRead = true;
		}
		private static void WriteToFile()
		{
			if (canWrite)
			{
				Console.Log("Writing to file.");
				BinaryFormatter formatter = new BinaryFormatter();
				FileStream fileStream = File.Open(nameOfSavingFile, FileMode.OpenOrCreate);
				formatter.Serialize(fileStream, new Data(savedItems));
				fileStream.Close();
			}
			else Console.LogWarning("Writing disabled");
		}
		public static void WipeFile()
		{
			Console.Log("Wiping file.");

			savedItems.Clear();

			File.Delete(nameOfSavingFile);
			hasFileBeenRead = false;
		}
		#endregion
	}
}