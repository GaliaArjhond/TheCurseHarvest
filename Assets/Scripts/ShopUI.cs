using UnityEngine;

public class ShopUI : MonoBehaviour
{
    public void CloseShop()
    {
        gameObject.SetActive(false);
    }

    public void OpenShop()
    {
        gameObject.SetActive(true);
    }
}