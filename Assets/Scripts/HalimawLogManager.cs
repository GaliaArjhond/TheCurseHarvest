using System.Collections.Generic;
using UnityEngine;

public class HalimawLogManager : MonoBehaviour
{
    public static HalimawLogManager Instance;

    private List<HalimawEntry> discoveredHalimaw = new List<HalimawEntry>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void Unlock(HalimawEntry entry)
    {
        if (entry == null)
            return;

        if (discoveredHalimaw.Contains(entry))
            return;

        discoveredHalimaw.Add(entry);

        Debug.Log("New Halimaw Discovered: " + entry.monsterName);

        if (HalimawPopupUI.Instance != null)
            HalimawPopupUI.Instance.Show(entry);
    }

    public bool IsUnlocked(HalimawEntry entry)
    {
        return discoveredHalimaw.Contains(entry);
    }
}