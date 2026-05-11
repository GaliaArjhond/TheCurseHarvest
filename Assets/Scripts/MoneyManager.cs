using UnityEngine;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance;

    [SerializeField] private int money = 100;
    [SerializeField] private TextMeshProUGUI moneyText;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateUI();
    }

    public bool SpendMoney(int amount)
    {
        if (money < amount)
        {
            Debug.Log("Not enough money");
            return false;
        }

        money -= amount;
        UpdateUI();
        return true;
    }

    public void AddMoney(int amount)
    {
        money += amount;
        UpdateUI();
    }

    public int GetMoney()
    {
        return money;
    }

    void UpdateUI()
    {
        if (moneyText != null)
            moneyText.text = money.ToString();
    }
}