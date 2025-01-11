using System;
using UnityEngine;
using UnityEngine.Localization;

[Serializable]
[CreateAssetMenu(fileName = "New Basic Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    [Header("DEVELOPER PROPERTIES")]
    [SerializeField, Tooltip("This is a flag for the developers to know what is ready to be used.")]
    public bool markAsImplemented;

    [Header("VISUAL REPRESENTATION")]
    [SerializeField, Tooltip("The icon of the item that will be displayed in the inspector ui.")]
    public Sprite icon;

    [SerializeField, Tooltip("2D or 3D object to spawn in the world and interacted with.")]
    public GameObject pickupObject;

    [SerializeField, Tooltip("2D or 3D object to display in the UI.")]
    public GameObject displayObject;

    [Header("DETAILS (Localized Fields)")]
    [SerializeField, Tooltip("The name of the Localisation table. It is advisable to use one table for all game objects.")]
    public String localizationTableName;

    [Header("TITLE")]
    [SerializeField, Tooltip("The title of the item not using localization. Leave empty if using localization.")]
    public String titleWithoutLocalization;

    [SerializeField, Tooltip("The localized string object that will be used to display the title of the item. Unused if titleWithoutLocalization is set.")]
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
    [SerializeField, Tooltip("The description of the item not using localization. Leave empty if using localization.")]
    public String descriptionWithoutLocalization;

    [SerializeField, Tooltip("The localized string object that will be used to display the description of the item. Unused if descriptionWithoutLocalization is set.")]
    public LocalizedString descriptionLocalized;

    public string Description()
    {
        if (descriptionWithoutLocalization is { Length: > 0 })
        {
            return descriptionLocalized.GetLocalizedString();
        }

        return descriptionWithoutLocalization;
    }

    [Header("OTHER PROPERTIES")]
    [SerializeField, Tooltip("A single instance of this item can exist in the game.")]
    public bool unique;

    [SerializeField, Tooltip("A single instance of this item can exist on a player.")]
    public bool uniquePerPlayer;

    [SerializeField, Tooltip("The item can be equipped by the player.")]
    public bool equipable;

    [SerializeField, Tooltip("The item can be upgraded using crafting or any other gameplay means")]
    public bool upgradeable;

    [SerializeField, Tooltip("The item can be consumed by the player")]
    public bool consumable;

    [SerializeField, Tooltip("The weight of this item in game units (kg or lbs)")]
    public float weight;

    [SerializeField, Tooltip("The value of this item in the game's currency")]
    public float value;
}
