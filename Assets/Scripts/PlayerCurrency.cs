using UnityEngine;

public class PlayerCurrency : MonoBehaviour
{
    public static PlayerCurrency Instance;

    public int pesos = 0;
    public int experience = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddGold(int amount)
    {
        AddPesos(amount);
    }

    public void AddPesos(int amount)
    {
        pesos += amount;
        Debug.Log($"Pesos +{amount}  Total: {pesos}");

        ShowRewardPopup(amount, 0);
    }

    public void AddExperience(int amount)
    {
        experience += amount;
        Debug.Log($"EXP +{amount}  Total: {experience}");

        ShowRewardPopup(0, amount);
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