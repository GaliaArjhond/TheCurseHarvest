using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    [Header("Quest States")]
    // current quest id mapping:
    // 0 = Talk to Maria
    // 1 = Collect 10 Wood
    // 2 = Collect 5 Stone
    // 3 = Plant 5 Carrots
    // 4 = Harvest 5 Carrots
    // 5 = Defeat 3 Halimaws
    // 6 = Complete
    public int currentQuest = 0;

    [Header("Quest Progress")]
    public int woodCollected;
    public int stoneCollected;
    public int halimawsKilled;
    public int fishCaught;
    public int carrotsPlanted;
    public int carrotsHarvested;
    // (use existing `halimawsKilled` counter)

    public void AcceptQuest1()
    {
        // Player accepted the first quest — advance to quest 1
        if (currentQuest == 0)
        {
            currentQuest = 1;
            Debug.Log("Quest Accepted: A Farmer's Beginning");
        }
    }
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Update()
    {
        CheckCurrentQuest();
    }

    void CheckCurrentQuest()
    {
        switch (currentQuest)
        {
            case 1: // Collect 10 Wood
                if (woodCollected >= 10)
                {
                    Debug.Log("Quest 1 Complete!");
                    RewardPlayer(100, 25);
                    currentQuest = 2;
                }
                break;
            case 2: // Collect 5 Stone
                if (stoneCollected >= 5)
                {
                    Debug.Log("Quest 2 Complete!");
                    RewardPlayer(150, 35);
                    currentQuest = 3;
                }
                break;
            case 3: // Plant 5 Carrots
                if (carrotsPlanted >= 5)
                {
                    Debug.Log("Quest 3 Complete!");
                    RewardPlayer(200, 50);
                    currentQuest = 4;
                }
                break;
            case 4: // Harvest 5 Carrots
                if (carrotsHarvested >= 5)
                {
                    Debug.Log("Quest 4 Complete!");
                    RewardPlayer(250, 60);
                    currentQuest = 5;
                }
                break;
            case 5: // Defeat 3 Halimaws
                if (halimawsKilled >= 3)
                {
                    Debug.Log("Quest 5 Complete!");
                    RewardPlayer(500, 100);
                    currentQuest = 6;
                }
                break;
            case 6: // All complete
                // nothing to check
                break;
            default:
                break;
        }
    }

    void RewardPlayer(
        int pesos,
        int exp)
    {
        if (MoneyManager.Instance != null)
        {
            MoneyManager.Instance.AddMoney(
                pesos
            );
        }

        PlayerStatsManager player =
            FindFirstObjectByType<PlayerStatsManager>();

        if (player != null)
        {
            player.AddExp(
                exp
            );
        }
    }

    public void ResetQuestProgress()
    {
        woodCollected = 0;
        stoneCollected = 0;
        halimawsKilled = 0;
        fishCaught = 0;
        carrotsPlanted = 0;
        carrotsHarvested = 0;

    }
}