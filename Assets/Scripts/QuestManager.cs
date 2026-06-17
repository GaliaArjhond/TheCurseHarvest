using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    [Header("Quest States")]
    public bool quest1Accepted;
    public bool quest1Completed;

    public bool quest2Accepted;
    public bool quest2Completed;

    public bool quest3Accepted;
    public bool quest3Completed;

    public bool quest4Accepted;
    public bool quest4Completed;

    public bool quest5Accepted;
    public bool quest5Completed;

    [Header("Quest Progress")]
    public int woodCollected;
    public int stoneCollected;
    public int halimawsKilled;
    public int fishCaught;

    public void AcceptQuest1()
    {
        quest1Accepted = true;

        Debug.Log(
            "Quest Accepted: A Farmer's Beginning"
        );
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
        CheckQuest1();
        CheckQuest2();
        CheckQuest3();
        CheckQuest4();
        CheckQuest5();
    }

    void CheckQuest1()
    {
        if(
            quest1Accepted
            && !quest1Completed
            && woodCollected >= 10
        )
        {
            CompleteQuest1();
        }
    }

    void CompleteQuest1()
    {
        quest1Completed = true;

        Debug.Log(
            "Quest Complete!"
        );

        if(MoneyManager.Instance != null)
        {
            MoneyManager.Instance.AddMoney(
                100
            );
        }

        PlayerStatsManager player =
            FindFirstObjectByType<PlayerStatsManager>();

        if(player != null)
        {
            player.AddExp(
                25
            );
        }
    }

    void CheckQuest2()
    {
        if (
            quest2Accepted &&
            !quest2Completed &&
            stoneCollected >= 10
        )
        {
            quest2Completed = true;

            RewardPlayer(
                150,
                35
            );

            Debug.Log(
                "Quest 2 Complete!"
            );
        }
    }

    void CheckQuest3()
    {
        if (
            quest3Accepted &&
            !quest3Completed &&
            halimawsKilled >= 5
        )
        {
            quest3Completed = true;

            RewardPlayer(
                200,
                50
            );

            Debug.Log(
                "Quest 3 Complete!"
            );
        }
    }

    void CheckQuest4()
    {
        if (
            quest4Accepted &&
            !quest4Completed &&
            fishCaught >= 5
        )
        {
            quest4Completed = true;

            RewardPlayer(
                250,
                60
            );

            Debug.Log(
                "Quest 4 Complete!"
            );
        }
    }

    void CheckQuest5()
    {
        if (
            quest5Accepted &&
            !quest5Completed &&
            woodCollected >= 50 &&
            stoneCollected >= 25
        )
        {
            quest5Completed = true;

            RewardPlayer(
                500,
                100
            );

            Debug.Log(
                "Quest 5 Complete!"
            );
        }
    }

    void RewardPlayer(
        int gold,
        int exp)
    {
        if (MoneyManager.Instance != null)
        {
            MoneyManager.Instance.AddMoney(
                gold
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
    }
}