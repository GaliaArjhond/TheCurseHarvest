using UnityEngine;

public class Item : MonoBehaviour
{
    public int ID;
    public string Name;
    public virtual void UseItem()
    {
        Debug.Log("Using item: " + Name);
    }
}
