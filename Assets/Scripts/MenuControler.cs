using UnityEngine;
using UnityEngine.InputSystem;

public class MenuControler : MonoBehaviour
{
    public GameObject menuCanvas;

    private PlayerInput playerInput;

    void Start()
    {
        menuCanvas.SetActive(false);

        playerInput = FindFirstObjectByType<PlayerInput>();
    }

    void Update()
    {
        if (playerInput.actions["Inventory"].WasPressedThisFrame())
        {
            bool open = !menuCanvas.activeSelf;

            menuCanvas.SetActive(menuCanvas.activeSelf == false);

            if (PauseManager.Instance != null)
                PauseManager.Instance.SetPaused(menuCanvas.activeSelf);
        }
    }
}