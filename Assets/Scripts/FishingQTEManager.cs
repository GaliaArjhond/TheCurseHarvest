using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FishingQTEManager : MonoBehaviour
{
    public static FishingQTEManager Instance;
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
        if (!isFishing) return;

        currentAngle += pointerSpeed * Time.deltaTime;

        if (currentAngle >= 360f)
            currentAngle -= 360f;

        pointer.localRotation = Quaternion.Euler(0f, 0f, -currentAngle + visualOffset);

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
            resultText.text = "Fish biting...";

        if (fishingPanel != null)
            fishingPanel.SetActive(true);
    }

    void RandomizeTargetZone()
    {
        targetAngle = Random.Range(0f, 360f);

        yellowZone.localRotation = Quaternion.Euler(0f, 0f, -targetAngle + visualOffset);

        greenZone.localRotation = Quaternion.Euler(0f, 0f, -targetAngle + visualOffset);
    }

    void CheckQTE()
    {
        float distance =
            Mathf.Abs(Mathf.DeltaAngle(currentAngle, targetAngle));

        if (distance <= 10f)
        {
            progress += 35f;

            if (resultText != null)
                resultText.text = "Perfect!";

            RandomizeTargetZone();
        }
        else if (distance <= 35f)
        {
            progress += 15f;

            if (resultText != null)
                resultText.text = "Good!";

            RandomizeTargetZone();
        }
        else
        {
            progress -= 20f;
            missCount++;

            if (resultText != null)
                resultText.text =
                    "Miss! " + missCount + "/" + maxMisses;
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

    void CatchFish()
    {
        isFishing = false;

        int fishID = GetRandomFish();

        bool added = false;

        if (InventoryController.Instance != null)
        {
            added = InventoryController.Instance.AddItem(
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
            switch (fishID)
            {
                case 17:
                    resultText.text = "Caught Carp!";
                    break;

                case 18:
                    resultText.text = "Caught Tilapia!";
                    break;

                case 19:
                    resultText.text = "Caught Catfish!";
                    break;

                case 20:
                    resultText.text = "Caught Golden Fish!";
                    break;

                case 21:
                    resultText.text = "Caught Trash...";
                    break;

                default:
                    resultText.text = "Caught Something!";
                    break;
            }
        }

        Invoke(nameof(CloseFishingPanel), 1f);
    }

    void FailFishing()
    {
        isFishing = false;

        if (resultText != null)
            resultText.text = "Fish escaped!";

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