using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    void Start()
    {
        if (SaveController.Instance != null && SaveController.Instance.HasSave())
            SaveController.Instance.LoadGame();
        else
            Debug.Log("No save file — starting fresh, inventory untouched");
    }

    void LoadAfterDelay()
    {
        if (SaveController.Instance != null)
            SaveController.Instance.LoadGame();
    }
}