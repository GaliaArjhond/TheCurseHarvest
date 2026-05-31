using UnityEngine;

public class CaveRock : MonoBehaviour
{
    [Header("Rock")]
    [SerializeField] private int health = 3;

    [Header("Drop Item IDs")]
    [SerializeField] private int stoneID = 1;
    [SerializeField] private int coalID = 9;
    [SerializeField] private int ironID = 10;
    [SerializeField] private int goldID = 11;

    public void HitRock()
    {
        health--;

        Debug.Log("Rock hit. HP: " + health);

        if (health <= 0)
        {
            if (CaveManager.Instance != null)
            {
                CaveManager.Instance.TrySpawnLadder(
                    transform.position
                );
            }

            DropOre();

            Destroy(gameObject);
        }
    }

    void DropOre()
    {
        int level = 1;

        if (CaveManager.Instance != null)
            level = CaveManager.Instance.GetCurrentCaveLevel();

        int roll = Random.Range(0, 100);
        int itemToAdd = stoneID;

        if (level <= 5)
        {
            itemToAdd = roll < 80 ? stoneID : coalID;
        }
        else if (level <= 10)
        {
            if (roll < 50) itemToAdd = stoneID;
            else if (roll < 80) itemToAdd = coalID;
            else itemToAdd = ironID;
        }
        else
        {
            if (roll < 35) itemToAdd = stoneID;
            else if (roll < 60) itemToAdd = coalID;
            else if (roll < 90) itemToAdd = ironID;
            else itemToAdd = goldID;
        }

        if (InventoryController.Instance != null)
        {
            bool added =
                InventoryController.Instance.AddItem(itemToAdd, 1);

            if (added && PickupUI.Instance != null)
            {
                PickupUI.Instance.ShowPickup(itemToAdd, 1);
            }
        }

        Debug.Log("Dropped ore ID: " + itemToAdd);
    }
}