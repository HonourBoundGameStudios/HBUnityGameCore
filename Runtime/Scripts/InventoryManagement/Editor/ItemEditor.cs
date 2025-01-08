using UnityEngine;
using UnityEditor;

namespace HBUnityGameCore.InventoryManagement
{
    [CustomEditor(typeof(Item))]
    public class ItemEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            Item item = (Item)target;

            EditorGUILayout.LabelField("Basic Info", EditorStyles.boldLabel);
            item.icon = (Sprite)EditorGUILayout.ObjectField("Icon", item.icon, typeof(Sprite), false);
            item.pickup = (GameObject)EditorGUILayout.ObjectField("Pickup", item.pickup, typeof(GameObject), false);
            item.display = (GameObject)EditorGUILayout.ObjectField("Display", item.display, typeof(GameObject), false);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Details", EditorStyles.boldLabel);
            item.displayName = EditorGUILayout.TextField("Display Name", item.displayName);
            item.itemDescription = EditorGUILayout.TextField("Description", item.itemDescription);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
            item.unique = EditorGUILayout.Toggle("Unique", item.unique);
            item.markAsImplemented = EditorGUILayout.Toggle("Implemented", item.markAsImplemented);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Methods", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Use Item", item.UseItem().ToString());
            EditorGUILayout.LabelField("Can Be Equipped", item.CanBeEquipped().ToString());
            EditorGUILayout.LabelField("Can Be Upgraded", item.CanBeUpgraded().ToString());

            EditorGUILayout.Space();
            if (GUI.changed)
            {
                EditorUtility.SetDirty(item);
            }
        }
    }

}
