using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreditsAnimator : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] TextMeshProUGUI gameBy;
    [SerializeField] VerticalLayoutGroup namesHolder;

    [Header("Game By")]
    [SerializeField] float gameByPadding = 2f;

    [Header("Fade Properties")]
    [SerializeField] float intervalTime = 2f;
    [SerializeField] float fadeDuration = .5f;
    [SerializeField] float timeTillFadeOut = 4f;

    [Header("Continue Properties")]
    [SerializeField] float timeBetweenDisappears = .5f;

    Sequence namesAppearSequence;
    Sequence continueSequence;

    bool canContinue;

    List<TextMeshProUGUI> Names_TMP
    {
        get
        {
            List<TextMeshProUGUI> names = new();
            for (int i = 0; i < namesHolder.transform.childCount; i++)
            {
                TextMeshProUGUI name_TMP = namesHolder.transform.GetChild(i).GetComponent<TextMeshProUGUI>();
                names.Add(name_TMP);
            }
            return names;
        }
    }

    private void OnEnable()
    {
        GameFlow.EndedGameEventHandler += GameFlow_EndedGame;
    }

    private void OnDisable()
    {
        GameFlow.EndedGameEventHandler -= GameFlow_EndedGame;
    }

    private void Start()
    {
        ReadyCanvas(false);
    }

    private void Update()
    {
# if DEBUG
        if (Input.GetKeyDown(KeyCode.V))
            ReadyCanvas(false);

        if (Input.GetKeyDown(KeyCode.B))
            ReadyCanvas(true);

        if (Input.GetKeyDown(KeyCode.N))
            GameFlow_EndedGame(null, null);

        if (Input.GetButtonDown("Continue") && canContinue)
        {
            Debug.Log("Did continue");
            Continue();
        }
#endif
    }

    void GameFlow_EndedGame(object sender, GameFlow.EndedGameEventArgs e)
    {
        ReadyCanvas(true);

        namesAppearSequence = DOTween.Sequence();

        // Fade "A Game By" in and out
        for (int i = 1; i > -1; i--)
        {
            namesAppearSequence.Append(gameBy.DOFade(i, fadeDuration).SetDelay(gameByPadding));
            namesAppearSequence.AppendInterval(gameByPadding);
        }
        
        // Names appear sequential and disappear after a certain amount of time
        foreach (TextMeshProUGUI name_TMP in Names_TMP)
        {
            namesAppearSequence.AppendCallback(() => name_TMP.gameObject.SetActive(true));
            namesAppearSequence.AppendInterval(intervalTime);
            namesAppearSequence.AppendCallback(() => canContinue = true);
        }
    }

    void Continue()
    {
        Debug.Log("Continue sa");
        gameBy.text = "Thanks for playing.";

        continueSequence = DOTween.Sequence();

        foreach (TextMeshProUGUI name_TMP in Names_TMP)
        {
            Debug.Log($"null : {name_TMP == null}");
            continueSequence.AppendCallback(() => name_TMP.gameObject.SetActive(false));
            continueSequence.AppendInterval(timeBetweenDisappears);
        }
    }

    void ReadyCanvas(bool setReady)
    {
        canContinue = false;

        namesAppearSequence?.Kill();
        continueSequence?.Kill();
        canvas.gameObject.SetActive(setReady);

        foreach (TextMeshProUGUI name_TMP in Names_TMP)
            name_TMP.gameObject.SetActive(false);

        gameBy.alpha = 0;
        gameBy.text = "A game by";
    }
}
