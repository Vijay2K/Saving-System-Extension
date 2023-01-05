using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;

namespace Extension.SavingSystem
{
    public class SavingSystem : MonoBehaviour
    {
        public static SavingSystem Instance { get; private set; }

        private const string lastSceneIndexId = "lastSceneIndex";
        private const string fileName = "GameData";

        private void Awake() 
        {
            if(Instance != null)
            {
                Debug.LogError($"More than one instance for {this}");
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public void Save()
        {
            Dictionary<string, object> state = LoadFile(fileName);
            CaptureState(state);
            SaveFile(fileName, state);
        }

        public void Load()
        {
            RestoreState(LoadFile(fileName));
        }

        public IEnumerator LoadLastScene(string fileName)
        {
            Dictionary<string, object> state = LoadFile(fileName);
            if(state.ContainsKey(lastSceneIndexId))
            {
                int sceneIndex = (int)state[lastSceneIndexId];
                if(SceneManager.GetActiveScene().buildIndex != sceneIndex)
                {
                    yield return SceneManager.LoadSceneAsync(sceneIndex);
                }
            }

            RestoreState(state);
        }

        private void SaveFile(string fileName, object state)
        {
            string path = GetFilePath(fileName);
            Debug.Log("Saving to the path : " + path);
            using(FileStream stream = File.Open(path, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, state);
            }
        }

        private Dictionary<string, object> LoadFile(string fileName)
        {
            string path = GetFilePath(fileName);

            if(!File.Exists(path))
            {
                return new Dictionary<string, object>();
            }

            Debug.Log("Loading from the path : " + path);
            using(FileStream stream = File.Open(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return formatter.Deserialize(stream) as Dictionary<string, object>;
            }
        }

        private void CaptureState(Dictionary<string, object> state)
        {
            foreach(SaveableEntity saveableEntity in FindObjectsOfType<SaveableEntity>())
            {
                state[saveableEntity.GetUniqueIdentifier()] = saveableEntity.CaptureState();
            }

            state[lastSceneIndexId] = SceneManager.GetActiveScene().buildIndex;
        }

        private void RestoreState(Dictionary<string, object> state)
        {
            foreach(SaveableEntity saveableEntity in FindObjectsOfType<SaveableEntity>())
            {
                string id = saveableEntity.GetUniqueIdentifier();
                if(state.ContainsKey(id))
                {
                    saveableEntity.RestoreState(state[id]);
                }
            }
        }

        private string GetFilePath(string fileName)
        {
            return Path.Combine(Application.persistentDataPath, fileName + ".sav");
        }
    }
}
