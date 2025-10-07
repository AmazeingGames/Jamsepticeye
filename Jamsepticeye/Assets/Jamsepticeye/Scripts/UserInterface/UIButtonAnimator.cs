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

    AnimationBank Animations => AnimationBank.Instance;

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

    // This needs to also be able to stop the animation on enter/exit
    public void OnPointerEnter(PointerEventData pointerEventData)
        => StartCoroutine(Animations.AnimateButton_Co(true, text_TMP, RegularScale, HoverScale, HoverOpacity, RegularOpacity));

    public void OnPointerExit(PointerEventData pointerEventData)
        => StartCoroutine(Animations.AnimateButton_Co(false, text_TMP, RegularScale, HoverScale, HoverOpacity, RegularOpacity));
}
