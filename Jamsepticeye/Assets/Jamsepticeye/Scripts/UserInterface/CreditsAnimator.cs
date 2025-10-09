using DG.Tweening;
using EasyTextEffects;
using Sirenix.OdinInspector;
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
    [SerializeField] TextMeshProUGUI thanks;
    [SerializeField] VerticalLayoutGroup namesHolder;

    [Header("Game By")]
    [SerializeField] float gameByPadding = 2f;

    [Header("Fade Properties")]
    [SerializeField] float intervalTime = 2f;
    [SerializeField] float fadeDuration = .5f;
    [SerializeField] float timeTillFadeOut = 4f;

    [Header("Continue Properties")]
    [SerializeField] float timeBetweenDisappears = .5f;
    [SerializeField] float timeForDisappearAnimation = 1f;
    [SerializeField] float continueTime = 5;
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

    List<TextEffect> TextEffects
    {
        get
        {
            List<TextEffect> textEffects = new();
            foreach (var effect in Names_TMP)
                textEffects.Add(effect.GetComponent<TextEffect>());
            return textEffects;
        }

    }
    /*
    private void OnEnable()
    {
        GameFlow.EndingGameEventHandler += GameFlow_EndedGame;
    }

    private void OnDisable()
    {
        GameFlow.EndingGameEventHandler -= GameFlow_EndedGame;
    }*/

    private void Start()
    {
        ReadyCanvas(false);
        Debug.Log("end gae");
        GameFlow_EndedGame(null, null);
    }

    [Button]
    public void Show()
    {
        ReadyCanvas(true);
    }

    [Button]
    public void Hide()
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

#endif
        // Quick solution 
        if (!hasPlayedCredits)
            GameFlow_EndedGame(null, null);

        if (Input.GetButtonDown("Continue") && canContinue)
        {
            Debug.Log("Did continue");
            StartCoroutine(Continue());
        }
    }

    bool hasPlayedCredits = false;
    void GameFlow_EndedGame(object sender, GameFlow.EndingGameEventArgs e)
    {
        hasPlayedCredits = true;
        Debug.Log("ending game");

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

    IEnumerator Continue()
    {
        canContinue = false;
        thanks.text = "Thank you for playing.";

        continueSequence = DOTween.Sequence();

        foreach (TextEffect textEffect in TextEffects)
        {
            textEffect.gameObject.SetActive(true);
            continueSequence.AppendCallback(() => textEffect.StartManualEffects());
            continueSequence.AppendInterval(timeBetweenDisappears);
        }

        yield return new WaitForSeconds(Names_TMP.Count * timeBetweenDisappears + gameByPadding);
        thanks.DOFade(1, fadeDuration).SetDelay(gameByPadding);
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
