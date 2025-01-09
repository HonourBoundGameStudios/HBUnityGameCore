using System;
using UnityEngine;
using UnityEngine.Localization;

[Serializable]
[CreateAssetMenu(fileName = "New Basic Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    // The icon of the item that will be displayed inspector ui.
    [Header("VISUAL REPRESENTATION")]

    [SerializeField]
    public Sprite icon;

    // The prefab of the item that will be spawned in the world when the player picks it up.
    [SerializeField]
    public GameObject pickupObject;

    // The prefab of the item that will be displayed in the ui (Typically a 3D model).
    [SerializeField]
    public GameObject displayObject;

    [Header("DETAILS (Localized Fields)")]
    [SerializeField]
    public String localizationTableName;

    [Header("TITLE")]
    [SerializeField]
    public String titleLocalizationKey;

    [SerializeField]
    public LocalizedString titleLocalized;

    private String title;

    [Header("DESCRIPTION")]
    [SerializeField]
    public String descriptionLocalizationKey;

    [SerializeField]
    public LocalizedString descriptionLocalized;

    private String description;

    [SerializeField]
    public bool unique; // A single instance of this item can exist in the game

    [SerializeField]
    public bool equipable; // The item can be equipped by the player

    [SerializeField]
    public bool upgradeable; // The item can be upgraded using crafting or any other gameplay means

    [SerializeField]
    public bool consumable; // The item can be consumed by the player

    // All steps of implementation this item a done and
    // the item is ready to be added to the world (Ready to be used in game)
    // This is a flag for the developers to know what is ready to be used
    [SerializeField]
    public bool markAsImplemented;

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

    // CRAFTING TODO
    //
    // [SerializeField, Range(1, 50)]
    // public int craftedQty = 1;
    //
    // public InventoryByQty craftingMaterials;
    //
    // public bool CanBeCrafted()
    // {
    //     return PlayerInventory.current.CombinedInventory().ContainsItems(craftingMaterials);
    // }
    //
    // public bool CanBeCrafted(int multiplier)
    // {
    //     return PlayerInventory.current.CombinedInventory().ContainsItems(craftingMaterials, multiplier);
    // }
    //
    // public int GetItemCraftingOrder()
    // {
    //     if (CanBeCrafted())
    //     {
    //         switch (GetItemType())
    //         {
    //             case EItemType.DrillAttachment:
    //                 return 0;
    //             case EItemType.Gun:
    //             case EItemType.GloveAttachment:
    //                 return 1;
    //             case EItemType.ClunkyUpgrade:
    //             case EItemType.Melee:
    //             case EItemType.ScannerAttachment:
    //                 return 2;
    //         }
    //         return 3;
    //     }
    //     else
    //     {
    //         return 4;
    //     }
    // }
    // public ItemBlueprint RequiredBlueprint()
    // {
    //     return craftingMaterials.GetItem(0) as ItemBlueprint;
    // }


    // ┌───────────────────────────────────────────────────────────────────────────────────────────────────────────┐
    // │ EVENTS ON COLLECTED                                                                                       │
    // └───────────────────────────────────────────────────────────────────────────────────────────────────────────┘

    // [SerializeField]
    // public ItemGuide givenGuide;

    // [SerializeField]
    // public ObjectiveScriptable objectiveCompleted;

    // [SerializeField]
    // public ScriptableObjects.SoundEffectSO defaultPickupSound;

    // // ┌───────────────────────────────────────────────────────────────────────────────────────────────────────────┐
    // // │ ITEM SORTING                                                                                              │
    // // └───────────────────────────────────────────────────────────────────────────────────────────────────────────┘
    // public static void SortItemsByType(ref List<Item> items)
    // {
    //     items.Sort((a, b) =>
    //     {
    //         int typeCompare = a.GetItemTypeDefaultOrder().CompareTo(b.GetItemTypeDefaultOrder());
    //
    //         if (typeCompare == 0)
    //             typeCompare = a.GetDisplayName().CompareTo(b.GetDisplayName());
    //
    //         return typeCompare;
    //     });
    // }

}
