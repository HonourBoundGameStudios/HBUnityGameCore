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
    [SerializeField, Tooltip("Key used to look up the title in the localization table.")]
    public String titleLocalizationKey;

    [SerializeField, Tooltip("The localized string object that will be used to display the title of the item.")]
    public LocalizedString titleLocalized;

    public string Title
    {
        get;
        private set;
    }

    [Header("DESCRIPTION")]
    [SerializeField, Tooltip("Key used to look up the description in the localization table.")]
    public String descriptionLocalizationKey;

    [SerializeField, Tooltip("The localized string object that will be used to display the description of the item.")]
    public LocalizedString descriptionLocalized;

    public string Description
    {
        get;
        private set;
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

    [SerializeField, Tooltip("The value of this item in the game currency")]
    public float value;

    private void Start()
    {
        titleLocalized = new LocalizedString(localizationTableName, titleLocalizationKey);
        titleLocalized.TableEntryReference = titleLocalizationKey;
        titleLocalized.StringChanged += OnTitleStringChanged;

        descriptionLocalized = new LocalizedString(localizationTableName, descriptionLocalizationKey);
        descriptionLocalized.TableEntryReference = descriptionLocalizationKey;
        descriptionLocalized.StringChanged += OnDescriptionStringChanged;
    }

    private void OnDescriptionStringChanged(string updatedString)
    {
        Title = updatedString;
    }

    private void OnTitleStringChanged(string updatedString)
    {
        Description = updatedString;
    }
}
