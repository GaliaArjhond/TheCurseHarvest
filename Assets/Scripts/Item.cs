using UnityEditor.ShaderGraph;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int ID;
    public string Name;

    public ItemType itemType = ItemType.Misc;

    public enum ItemType
    {
        Misc,
        Tool,
        Seed,
        Food,
        Resource,
        Weapon
    }

    // for seeds — drag the CropData asset here
    public CropData cropData;

    public virtual void UseItem()
    {
        Debug.Log("Using item: " + Name);
    }
}