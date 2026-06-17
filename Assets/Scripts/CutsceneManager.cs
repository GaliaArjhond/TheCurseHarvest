using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private Image cutsceneImage;

    [SerializeField]
    private Sprite[] cutsceneSprites;

    [TextArea]
    [SerializeField]
    private string[] storyTexts;

    [SerializeField]
    private TextMeshProUGUI storyText;

    private int currentIndex = 0;

    void Start()
    {
        ShowCurrentSlide();
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            NextSlide();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Game");
        }
    }

    void NextSlide()
    {
        currentIndex++;

        if (currentIndex >= cutsceneSprites.Length)
        {
            StartCoroutine(StartGame());
            return;
        }

        ShowCurrentSlide();
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene("Game");
    }

    
    

    void ShowCurrentSlide()
    {
        cutsceneImage.sprite =
            cutsceneSprites[currentIndex];

        storyText.text =
            storyTexts[currentIndex];
    }

    
}