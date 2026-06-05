using UnityEngine;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance;

    [SerializeField] private float money = 100f;
    [SerializeField] private TextMeshProUGUI moneyText;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        UpdateUI();
    }

    public bool SpendMoney(float amount)
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

    public void AddMoney(float amount)
    {
        money += amount;
        UpdateUI();
    }

    public float GetMoney()
    {
        return money;
    }

    void UpdateUI()
    {
        if (moneyText != null)
            moneyText.text = "₱ " + money.ToString("N2");
    }
}