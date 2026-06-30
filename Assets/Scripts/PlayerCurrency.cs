using UnityEngine;
using TMPro;

public class PlayerCurrency : MonoBehaviour
{
    public static PlayerCurrency Instance;

    public int pesos = 0;
    public int experience = 0;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI pesosText;
    [SerializeField] private TextMeshProUGUI expText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (PickupUI.Instance == null)
        {
            Debug.LogWarning("PlayerCurrency: PickupUI not found in scene. Reward popup will not show.");
        }

        UpdateUI();
    }

    public void AddGold(int amount)
    {
        AddPesos(amount);
    }

    public void AddPesos(int amount)
    {
        pesos += amount;
        Debug.Log($"Pesos +{amount}  Total: {pesos}");
        UpdateUI();
        ShowRewardPopup(amount, 0);
    }

    public void AddExperience(int amount)
    {
        experience += amount;
        Debug.Log($"EXP +{amount}  Total: {experience}");
        UpdateUI();
        ShowRewardPopup(0, amount);
    }

    private void UpdateUI()
    {
        if (pesosText != null)
            pesosText.text = "₱ " + pesos.ToString();

        if (expText != null)
            expText.text = "EXP: " + experience.ToString();
    }

    private void ShowRewardPopup(int pesosAmount, int expAmount)
    {
        if (PickupUI.Instance == null)
            return;

        string message = string.Empty;

        if (pesosAmount > 0)
            message += "+" + pesosAmount + " Pesos";

        if (expAmount > 0)
        {
            if (message.Length > 0)
                message += "\n";

            message += "+" + expAmount + " EXP";
        }

        if (message.Length > 0)
            PickupUI.Instance.ShowReward(message);
    }
}