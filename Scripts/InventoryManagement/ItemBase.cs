using System;
using UnityEngine;
using UnityEngine.Localization;

namespace HBUnityGameCore
{
    [Serializable]
    [CreateAssetMenu(fileName = "Base Object", menuName = "HBUnityGameCore/BaseObject")]
    public class BaseObjectScriptableObject : ScriptableObject
    {
        [Header("VISUAL REPRESENTATION")]
        [SerializeField, Tooltip("The icon of the item that will be displayed in the inspector ui.")]
        public Sprite icon;

        [Header("DEVELOPER PROPERTIES")]
        [SerializeField, Tooltip("This is a flag for the developers to know that this object is ready to be used.")]
        public bool markAsImplemented;

        [SerializeField, Tooltip("2D or 3D object to spawn in the world and interacted with.")]
        public GameObject pickupObject;

        [SerializeField, Tooltip("2D or 3D object to display in the UI.")]
        public GameObject displayObject;

        [Header("DETAILS (Localized Fields)")]
        [SerializeField, Tooltip("The name of the Localisation table. It is advisable to use one table for all game objects.")]
        public String localizationTableName;

        [Header("TITLE")]
        [SerializeField, Tooltip("The title not using localization. Leave empty if using localization.")]
        public String titleWithoutLocalization;

        [SerializeField, Tooltip("The localized title. Unused if titleWithoutLocalization is set.")]
        public LocalizedString titleLocalized;

        public string Title()
        {
            if (titleWithoutLocalization is { Length: > 0 })
            {
                return titleWithoutLocalization;
            }
            return titleLocalized.GetLocalizedString();
        }

        [Header("DESCRIPTION")]
        [SerializeField, Tooltip("The description not using localization. Leave empty if using localization.")]
        public String descriptionWithoutLocalization;

        [SerializeField, Tooltip("The localized description. Unused if descriptionWithoutLocalization is set.")]
        public LocalizedString descriptionLocalized;

        public string Description()
        {
            if (descriptionWithoutLocalization is { Length: > 0 })
            {
                return descriptionLocalized.GetLocalizedString();
            }

            return descriptionWithoutLocalization;
        }
    }
}
