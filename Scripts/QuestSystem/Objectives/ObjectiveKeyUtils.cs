using UnityEditor;
using UnityEngine;

namespace QuestSystem
{
    public static class ObjectiveKeyUtils
    {
        /// <summary>
        /// Creates an ObjectiveKey asset with the given ID in the specified folder path if it does not already exist.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        public static ObjectiveKey CreateObjectiveKeyIfNotExists(string id, string folderPath)
        {
            string assetPath = $"{folderPath}/{id}.asset";

            // Check if the asset already exists
            ObjectiveKey existing = AssetDatabase.LoadAssetAtPath<ObjectiveKey>(assetPath);

            if (existing != null)
            {
                // It already exists — return it
                return existing;
            }

            // Otherwise create a new one
            ObjectiveKey newKey = ScriptableObject.CreateInstance<ObjectiveKey>();
            newKey.id = id;

            AssetDatabase.CreateAsset(newKey, assetPath);
            AssetDatabase.SaveAssets();

            return newKey;
        }
    }
}
