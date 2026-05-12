using UnityEngine;
using UnityEngine.InputSystem;

public class MenuControler : MonoBehaviour
{
    public GameObject menuCanvas;

    void Start()
    {
        menuCanvas.SetActive(false);
    }

    void Update()
    {
        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            bool open = !menuCanvas.activeSelf;

            menuCanvas.SetActive(open);

            if (PauseManager.Instance != null)
                PauseManager.Instance.SetPaused(open);
        }
    }
}