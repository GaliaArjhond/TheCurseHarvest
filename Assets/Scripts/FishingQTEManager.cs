using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FishingQTEManager : MonoBehaviour
{
    public static FishingQTEManager Instance;

    [Header("Visual Offset")]
    [SerializeField] private float visualOffset = 90f;

    [Header("UI")]
    [SerializeField] private GameObject fishingPanel;
    [SerializeField] private RectTransform pointer;
    [SerializeField] private RectTransform yellowZone;
    [SerializeField] private RectTransform greenZone;
    [SerializeField] private Slider progressBar;
    [SerializeField] private TextMeshProUGUI resultText;

    [Header("QTE Settings")]
    [SerializeField] private float pointerSpeed = 180f;
    [SerializeField] private int maxMisses = 3;

    [Header("Rewards")]
    [SerializeField] private int carpID = 17;
    [SerializeField] private int tilapiaID = 18;
    [SerializeField] private int catfishID = 19;
    [SerializeField] private int goldenFishID = 20;
    [SerializeField] private int trashID = 21;

    private bool isFishing = false;
    private float currentAngle = 0f;
    private float targetAngle = 0f;
    private float progress = 0f;
    private int missCount = 0;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        if (fishingPanel != null)
            fishingPanel.SetActive(false);
    }

    void Update()
    {
        if (!isFishing)
            return;

        currentAngle += pointerSpeed * Time.deltaTime;

        if (currentAngle >= 360f)
            currentAngle -= 360f;

        if (pointer != null)
        {
            pointer.localRotation =
                Quaternion.Euler(
                    0f,
                    0f,
                    -currentAngle + visualOffset
                );
        }

        if (Input.GetKeyDown(KeyCode.Space))
            CheckQTE();
    }

    public void StartFishing()
    {
        if (isFishing)
            return;

        isFishing = true;
        progress = 0f;
        missCount = 0;

        currentAngle = Random.Range(0f, 360f);

        RandomizeTargetZone();

        if (progressBar != null)
            progressBar.value = 0f;

        if (resultText != null)
        {
            resultText.color = Color.white;
            resultText.text = "Fish biting...";
        }

        if (fishingPanel != null)
            fishingPanel.SetActive(true);
    }

    void RandomizeTargetZone()
    {
        targetAngle = Random.Range(0f, 360f);

        if (yellowZone != null)
        {
            yellowZone.localRotation =
                Quaternion.Euler(
                    0f,
                    0f,
                    -targetAngle + visualOffset
                );
        }

        if (greenZone != null)
        {
            greenZone.localRotation =
                Quaternion.Euler(
                    0f,
                    0f,
                    -targetAngle + visualOffset
                );
        }
    }

    void CheckQTE()
    {
        float distance =
            Mathf.Abs(
                Mathf.DeltaAngle(
                    currentAngle,
                    targetAngle
                )
            );

        if (distance <= 10f)
        {
            progress += 45f;

            if (resultText != null)
            {
                resultText.color = Color.green;
                resultText.text = "Perfect!";
            }

            RandomizeTargetZone();
        }
        else if (distance <= 35f)
        {
            progress += 25f;

            if (resultText != null)
            {
                resultText.color = Color.yellow;
                resultText.text = "Good!";
            }

            RandomizeTargetZone();
        }
        else
        {
            progress -= 20f;
            missCount++;

            if (resultText != null)
            {
                resultText.color = Color.red;
                resultText.text =
                    "Miss! " + missCount + "/" + maxMisses;
            }
        }

        progress = Mathf.Clamp(progress, 0f, 100f);

        if (progressBar != null)
            progressBar.value = progress / 100f;

        if (progress >= 100f)
        {
            CatchFish();
            return;
        }

        if (missCount >= maxMisses)
        {
            FailFishing();
            return;
        }
    }

    int GetRandomFish()
    {
        int roll = Random.Range(0, 100);

        if (roll < 40)
            return carpID;

        if (roll < 70)
            return tilapiaID;

        if (roll < 90)
            return catfishID;

        if (roll < 97)
            return trashID;

        return goldenFishID;
    }

    string GetFishName(int fishID)
    {
        if (fishID == carpID)
            return "Carp";

        if (fishID == tilapiaID)
            return "Tilapia";

        if (fishID == catfishID)
            return "Catfish";

        if (fishID == goldenFishID)
            return "Golden Fish";

        if (fishID == trashID)
            return "Trash";

        return "Unknown";
    }

    string GetFishRarity(int fishID)
    {
        if (fishID == carpID)
            return "Common";

        if (fishID == tilapiaID)
            return "Uncommon";

        if (fishID == catfishID)
            return "Rare";

        if (fishID == goldenFishID)
            return "Legendary";

        if (fishID == trashID)
            return "Junk";

        return "Unknown";
    }

    Color GetRarityColor(int fishID)
    {
        if (fishID == carpID)
            return Color.white;

        if (fishID == tilapiaID)
            return Color.green;

        if (fishID == catfishID)
            return Color.cyan;

        if (fishID == goldenFishID)
            return Color.yellow;

        if (fishID == trashID)
            return Color.gray;

        return Color.white;
    }

    void CatchFish()
    {
        isFishing = false;

        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.fishCaught++;
        }

        int fishID = GetRandomFish();

        bool added = false;

        if (InventoryController.Instance != null)
        {
            added =
                InventoryController.Instance.AddItem(
                    fishID,
                    1
                );
        }

        if (added &&
            PickupUI.Instance != null)
        {
            PickupUI.Instance.ShowPickup(
                fishID,
                1
            );
        }

        if (resultText != null)
        {
            string fishName = GetFishName(fishID);
            string rarity = GetFishRarity(fishID);

            resultText.color = GetRarityColor(fishID);

            resultText.text =
                "Caught " +
                fishName +
                "!\n[" +
                rarity +
                "]";
        }

        Invoke(nameof(CloseFishingPanel), 1f);
    }

    void FailFishing()
    {
        isFishing = false;

        if (resultText != null)
        {
            resultText.color = Color.red;
            resultText.text = "Fish escaped!";
        }

        Invoke(nameof(CloseFishingPanel), 0.7f);
    }

    void CloseFishingPanel()
    {
        if (fishingPanel != null)
            fishingPanel.SetActive(false);
    }

    public bool IsFishing()
    {
        return isFishing;
    }
}