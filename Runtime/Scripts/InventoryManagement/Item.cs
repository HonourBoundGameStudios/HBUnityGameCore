using System;
using UnityEngine;
using UnityEngine.Localization;

[Serializable]
[CreateAssetMenu(fileName = "New Basic Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    [Header("VISUAL REPRESENTATION")]
    [SerializeField]
    public Sprite icon; // The icon of the item that will be displayed in the inspector ui.

    [SerializeField]
    public GameObject pickupObject; // 2D or 3D object to display in the world

    [SerializeField]
    public GameObject displayObject; // 2D or 3D object to display in the UI

    [Header("DETAILS (Localized Fields)")]
    [SerializeField]
    public String localizationTableName; // The name of the Localisation table.

    [Header("TITLE")]
    [SerializeField]
    public String titleLocalizationKey; // Key used to look up the description in the localization table

    [SerializeField]
    public LocalizedString titleLocalized;

    private String title; // Autofill from the localized string. Not persisted.

    [Header("DESCRIPTION")]
    [SerializeField]
    public String descriptionLocalizationKey; // Key used to look up the description in the localization table

    [SerializeField]
    public LocalizedString descriptionLocalized;

    private String description; // Autofill from the localized string. Not persisted

    [SerializeField]
    public bool unique; // A single instance of this item can exist in the game

    [SerializeField]
    public bool equipable; // The item can be equipped by the player

    [SerializeField]
    public bool upgradeable; // The item can be upgraded using crafting or any other gameplay means

    [SerializeField]
    public bool consumable; // The item can be consumed by the player

    [SerializeField]
    public float weight; // The weight of the item in the inventory

    [SerializeField]
    public float value; // The value of the item in the currency of the game

    [SerializeField]
    public bool markAsImplemented; // This is a flag for the developers to know what is ready to be used

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
        title = updatedString;
    }

    private void OnTitleStringChanged(string updatedString)
    {
        description = updatedString;
    }
}
