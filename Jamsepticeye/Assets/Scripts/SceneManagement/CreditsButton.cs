using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreditsButton : MonoBehaviour
{
    public GameObject creditPage;
    public Button credsButton;
    public Button backButton;
    public Button playButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (credsButton != null)
        { credsButton.onClick.AddListener(TaskOnClick); }

        if (creditPage != null)
        { creditPage.SetActive(false); }

        if (backButton != null)
        {
            backButton.onClick.AddListener(OnBackButtonClick);
            backButton.gameObject.SetActive(false);
        }

        if (playButton != null)
        { playButton.onClick.AddListener(Play); }

    }

    void Play()
    { SceneManager.LoadScene("TownScene"); }

    void TaskOnClick()
    {
        Debug.Log("You have clicked the button!");

        if (creditPage != null)
        {
            creditPage.SetActive(true);
            backButton.gameObject.SetActive(true);
            credsButton.gameObject.SetActive(false);
            playButton.gameObject.SetActive(false);

        }


    }

    void OnBackButtonClick()
    {
        if (creditPage != null)
        {
            creditPage.SetActive(false);
            backButton.gameObject.SetActive(false);
            credsButton.gameObject.SetActive(true);
            playButton.gameObject.SetActive (true);
        }
    }
}
