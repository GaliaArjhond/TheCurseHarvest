using UnityEngine;

public class MenuInputController : MonoBehaviour
{
    [SerializeField] private GameObject menuRoot;
    [SerializeField] private GameObject chestWindow;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            bool menuOpen = menuRoot.activeSelf;

            // CLOSE EVERYTHING
            if (menuOpen)
            {
                if (chestWindow != null)
                    chestWindow.SetActive(false);

                menuRoot.SetActive(false);
            }
            // OPEN MENU
            else
            {
                menuRoot.SetActive(true);
            }
        }
    }
}