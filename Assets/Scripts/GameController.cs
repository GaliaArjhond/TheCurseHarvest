using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    void Start()
    {
        // load game automatically on start
        if (SaveController.Instance != null)
            SaveController.Instance.LoadGame();
    }
}