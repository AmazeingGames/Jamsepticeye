using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UIButton;

public class UIButtonAnimator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Components")]
    [SerializeField] TextMeshProUGUI text_TMP;
    [SerializeField] Image underline;

    [field: Header("Button")]
    [field: SerializeField] public float RegularScale { get; private set; } = 1.0f;
    [field: SerializeField] public float HoverScale { get; private set; } = 1.1f;
    [field: Range(0, 1)][field: SerializeField] public float RegularOpacity { get; private set; } = .66f;
    [field: Range(0, 1)][field: SerializeField] public float HoverOpacity { get; private set; } = 1;

    [field: Header("Button Lerp")]
    [field: SerializeField] public float ButtonLerpSpeed { get; private set; } = 8;
    [field: SerializeField] public float UnderlineLerpSpeed { get; private set; } = 8;
    [field: SerializeField] public AnimationCurve ButtonLerpCurve { get; private set; }
    [field: SerializeField] public AnimationCurve UnderlineLerpCurve { get; private set; }

    Coroutine hoverAnimationCoroutine = null;

    private void Start()
    {
        text_TMP.alpha = RegularOpacity;
        text_TMP.gameObject.SetActive(true);

        var regularScale = RegularScale;
        text_TMP.transform.localScale = new Vector3(regularScale, regularScale, text_TMP.transform.localScale.z);
    }

    private void OnDisable()
    {
        text_TMP.transform.localScale = new(RegularScale, RegularScale);
        text_TMP.alpha = RegularOpacity;
    }

    IEnumerator AnimateButton_Co(bool isSelected)
    {
        float time = 0;

        float startingScale = text_TMP.transform.localScale.x;
        float targetScale = isSelected ? HoverScale : RegularScale;

        float startingOpacity = text_TMP.alpha;
        float targetOpacity = isSelected ? HoverOpacity : RegularOpacity;

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

    public void OnPointerEnter(PointerEventData pointerEventData)
        => HoverAnimation(true);

    public void OnPointerExit(PointerEventData pointerEventData)
        => HoverAnimation(false);

    void HoverAnimation(bool isSelected)
    {
        if (!gameObject.activeInHierarchy)
            return;

        if (hoverAnimationCoroutine != null)
            StopCoroutine(hoverAnimationCoroutine);

        hoverAnimationCoroutine = StartCoroutine(AnimateButton_Co(isSelected));
    }
}
