using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New Basic Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    [SerializeField]
    public Sprite icon;

    [SerializeField]
    public GameObject pickup;

    [SerializeField]
    public GameObject display;

    // NAME & DESCRIPTION
    [SerializeField]
    public String displayName;

    [Header("DESCRIPTION")]
    [SerializeField]
    public String itemDescription;

    // MARK UNIQUE
    [SerializeField]
    public bool unique;

    // MARK AS IMPLEMENTED (Ready to be used in game)
    [SerializeField]
    public bool markAsImplemented;

    public virtual bool UseItem() => false;
    public virtual bool CanBeEquipped() => false;
    public virtual bool CanBeUpgraded() => true;

    // CRAFTING
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
