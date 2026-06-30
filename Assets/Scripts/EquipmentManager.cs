using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager Instance;

    public Item helmet;
    public Item armor;
    public Item boots;

    public Item charm1;
    public Item charm2;
    public Item charm3;

    void Awake()
    {
        Instance = this;
    }

    public void Equip(
    Item item,
    Slot slot)
    {
        if (item == null || slot == null)
            return;

        switch(slot.slotType)
        {
            case Slot.SlotType.Helmet:
                helmet = item;
                break;

            case Slot.SlotType.Armor:
                armor = item;
                break;

            case Slot.SlotType.Boots:
                boots = item;
                break;

            case Slot.SlotType.Charm:

                if(charm1 == null)
                    charm1 = item;
                else if(charm2 == null)
                    charm2 = item;
                else
                    charm3 = item;

                break;
        }

        ApplyStats();
    }

    public void Unequip(Item.ItemType type)
    {
        switch(type)
        {
            case Item.ItemType.Helmet:
                helmet = null;
                break;

            case Item.ItemType.Armor:
                armor = null;
                break;

            case Item.ItemType.Boots:
                boots = null;
                break;
        }

        ApplyStats();
    }

    void ApplyStats()
    {
        PlayerStatsManager player =
            FindFirstObjectByType<PlayerStatsManager>();

        if(player == null)
            return;

        int hp = 0;
        int atk = 0;
        int def = 0;

        Item[] equipped =
        {
            helmet,
            armor,
            boots,
            charm1,
            charm2,
            charm3
        };

        foreach(Item item in equipped)
        {
            if(item == null)
                continue;

            hp += item.hpBonus;
            atk += item.attackBonus;
            def += item.defenseBonus;
        }

        Debug.Log(
            "Equipment Stats\n" +
            "HP: " + hp +
            "\nATK: " + atk +
            "\nDEF: " + def
        );
    }
}