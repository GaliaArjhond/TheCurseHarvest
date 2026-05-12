using UnityEngine;

public class HarvestableProp : MonoBehaviour
{
    public string propId;

    public enum ToolType
    {
        Axe,
        Pickaxe
    }

    [Header("Tool Requirement")]
    public ToolType requiredTool;

    [SerializeField] private int hitsToBreak = 3;
    [SerializeField] private GameObject dropItemPrefab;
    [SerializeField] private int dropAmount = 2;

    private int currentHits = 0;
    private PropsSpawner ownerSpawner;

    void DropItems()
    {
        if (dropItemPrefab == null) return;

        for (int i = 0; i < dropAmount; i++)
        {
            Vector3 spawnPos = transform.position + new Vector3(0f, -1f, 0f);

            Instantiate(dropItemPrefab, spawnPos, Quaternion.identity);
        }
    }

    public void Init(string id, PropsSpawner spawner)
    {
        propId = id;
        ownerSpawner = spawner;
    }

    public void HitProp(string toolName)
    {
        if (requiredTool == ToolType.Axe && toolName != "Axe")
        {
            Debug.Log("Need Axe");
            return;
        }

        if (requiredTool == ToolType.Pickaxe && toolName != "Pickaxe")
        {
            Debug.Log("Need Pickaxe");
            return;
        }

        currentHits++;

        Debug.Log(name + " hits: " + currentHits + " / " + hitsToBreak);

        if (currentHits >= hitsToBreak)
        {
            DestroyProp();
        }
    }

    public void DestroyProp()
    {
        if (ownerSpawner != null)
            ownerSpawner.MarkDestroyed(propId);

        DropItems();

        Destroy(gameObject);
    }
}