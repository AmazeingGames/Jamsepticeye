using DG.Tweening;
using MoreMountains.Tools;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

// This could potentially be a transform extension class
public class AnimationBank : MMSingleton<AnimationBank>
{
    [Header("Slide-In Tween")]
    [SerializeField] float moveInDuration = .45f;
    [SerializeField] float moveOutDuration = .6f;
    [SerializeField] float inPosition = 0;
    [SerializeField] float outPosition = 870;
    [SerializeField] Ease moveEase = Ease.OutBack;

    [field: Header("Text Grow Tween")]
    [field: SerializeField] public float ButtonLerpSpeed { get; private set; } = 8;
    [field: SerializeField] public float UnderlineLerpSpeed { get; private set; } = 8;
    [field: SerializeField] public AnimationCurve ButtonLerpCurve { get; private set; }
    [field: SerializeField] public AnimationCurve UnderlineLerpCurve { get; private set; }

    Sequence menuAnimationSequence;
    bool isMenuOpen;

    // This needs to be able to kill animations properly
    public void SlideIn(RectTransform transform, bool boolSlideIn, Action onComplete)
    {
        isMenuOpen = boolSlideIn;

        menuAnimationSequence?.Kill();
        menuAnimationSequence = DOTween.Sequence();

        if (boolSlideIn)
        {
            menuAnimationSequence.Append(transform.DOLocalMoveY(inPosition, moveInDuration).SetEase(moveEase));
        }
        else
        {
            menuAnimationSequence.Append(transform.DOLocalMoveY(outPosition, moveOutDuration).SetEase(moveEase));
            menuAnimationSequence.OnComplete(() => onComplete());
        }
    }

    public IEnumerator AnimateButton_Co(bool isSelected, TextMeshProUGUI text_TMP, float regularScale, float hoverScale, float hoverOpacity, float regularOpacity)
    {
        float time = 0;

        float startingScale = text_TMP.transform.localScale.x;
        float targetScale = isSelected ? hoverScale : regularScale;

        float startingOpacity = text_TMP.alpha;
        float targetOpacity = isSelected ? hoverOpacity : regularOpacity;

        while (time < 1)
        {
            var lerpCurve = ButtonLerpCurve;

            float newScale = Mathf.Lerp(startingScale, targetScale, lerpCurve.Evaluate(time));
            text_TMP.transform.localScale = new Vector3(newScale, newScale, text_TMP.transform.localScale.z);

            float newOpacity = Mathf.Lerp(startingOpacity, targetOpacity, lerpCurve.Evaluate(time));
            text_TMP.alpha = newOpacity;

            time += Time.deltaTime * ButtonLerpSpeed;
            yield return null;
        }
    }
}
