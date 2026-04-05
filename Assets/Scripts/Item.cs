using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Item : MonoBehaviour
{
    public int ID;
    public string itemName;
    public Sprite icon;
    public virtual void UseItem() 
    {
        Debug.Log("Using item: " + itemName);
    }
}