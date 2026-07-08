using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [System.Serializable]
    public class TutorialPage
    {
        public string title;

        [TextArea(4, 10)]
        public string body;

        public Sprite image;
    }

    [Header("UI")]
    public GameObject tutorialPanel;
    public Image tutorialImage;

    public TMP_Text titleText;
    public TMP_Text bodyText;
    public TMP_Text pageIndicator;

    public Button previousButton;
    public Button nextButton;
    public Button skipButton;

    [Header("Tutorial Pages")]
    public TutorialPage[] pages;

    [Header("Player")]
    public MonoBehaviour playerMovement;

    private int currentPage = 0;

    void Start()
    {
        SaveData save = SaveController.Instance.CurrentSaveData;

        if (save != null && save.tutorialCompleted)
        {
            tutorialPanel.SetActive(false);

            if (playerMovement != null)
                playerMovement.enabled = true;

            return;
        }

        tutorialPanel.SetActive(true);

        if (playerMovement != null)
            playerMovement.enabled = false;

        ShowPage();
    }

    void ShowPage()
    {
        titleText.text = pages[currentPage].title;
        bodyText.text = pages[currentPage].body;
        if (pages[currentPage].image != null)
        {
            tutorialImage.gameObject.SetActive(true);
            tutorialImage.sprite = pages[currentPage].image;
        }
        else
        {
            tutorialImage.gameObject.SetActive(false);
        }

        // Page Indicator
        pageIndicator.text = (currentPage + 1) + " / " + pages.Length;

        // Previous button
        previousButton.interactable = currentPage > 0;

        // Change Next button text
        TMP_Text nextButtonText = nextButton.GetComponentInChildren<TMP_Text>();

        if (currentPage == pages.Length - 1)
            nextButtonText.text = "Start";
        else
            nextButtonText.text = "Next";
    }

    public void NextPage()
    {
        if (currentPage < pages.Length - 1)
        {
            currentPage++;
            ShowPage();
        }
        else
        {
            FinishTutorial();
        }
    }

    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            ShowPage();
        }
    }

    public void SkipTutorial()
    {
        FinishTutorial();
    }

    public void OpenTutorial()
    {
        currentPage = 0;

        tutorialPanel.SetActive(true);

        if (playerMovement != null)
            playerMovement.enabled = false;

        ShowPage();
    }

    void FinishTutorial()
    {
        // Only save completion the first time
        if (!SaveController.Instance.CurrentSaveData.tutorialCompleted)
        {
            SaveController.Instance.CurrentSaveData.tutorialCompleted = true;
            SaveController.Instance.SaveGame();
        }

        tutorialPanel.SetActive(false);

        if (playerMovement != null)
            playerMovement.enabled = true;
    }

    // Optional: Reset tutorial for testing
    [ContextMenu("Reset Tutorial")]
    void ResetTutorial()
    {
        PlayerPrefs.DeleteKey("TutorialCompleted");
        Debug.Log("Tutorial reset.");
    }
}