using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Extension.SavingSystem
{
    [ExecuteInEditMode]
    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField] private string uniqueIdentifier = "";

        private static Dictionary<string, SaveableEntity> globalLookUp = new Dictionary<string, SaveableEntity>();

    #if UNITY_EDITOR
        private void Update() 
        {
            if(Application.IsPlaying(this.gameObject)) return;

            if(string.IsNullOrEmpty(gameObject.scene.path)) return;

            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty serializedProperty = serializedObject.FindProperty("uniqueIdentifier");
        
            if(string.IsNullOrEmpty(serializedProperty.stringValue) || !IsUnique(serializedProperty.stringValue))
            {
                serializedProperty.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }

            globalLookUp[serializedProperty.stringValue] = this;
        }
    #endif

        private bool IsUnique(string candidate)
        {
            if(!globalLookUp.ContainsKey(candidate))
                return true;

            if(globalLookUp[candidate] == this)
                return true;

            if(globalLookUp[candidate] == null)
            {
                globalLookUp.Remove(candidate);
                return true;
            }

            if(globalLookUp[candidate].GetUniqueIdentifier() != candidate)
            {
                globalLookUp.Remove(candidate);
                return true;
            }

            return false;
        }

        public object CaptureState()
        {
            Dictionary<string, object> state = new Dictionary<string, object>();

            ISavable[] savables = GetComponents<ISavable>();

            foreach(ISavable savable in savables)
            {
                Debug.Log("Capturing the state of " + savable.GetType().ToString());
                state[savable.GetType().ToString()] = savable.CaptureState();
            }

            return state;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, object> stateDict = (Dictionary<string, object>)state;
            ISavable[] savables = GetComponents<ISavable>();
            foreach(ISavable saveable in savables)
            {
                Debug.Log("Restoring the state of " + saveable.GetType().ToString());
                string id = saveable.GetType().ToString();
                if(stateDict.ContainsKey(id))
                {
                    saveable.RestoreState(stateDict[id]);
                }
            }
        }

        public string GetUniqueIdentifier() => uniqueIdentifier;
    }
}
