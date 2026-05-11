using UnityEngine;
using UnityEngine.EventSystems;

public class PreviewSlotBlocker : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Cannot drop items into crafting preview.");
    }
}