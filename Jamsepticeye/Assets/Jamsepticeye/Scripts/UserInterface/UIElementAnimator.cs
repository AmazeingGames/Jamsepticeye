using DG.Tweening;
using UnityEngine;

public class UIElementAnimator : MonoBehaviour
{
    [Header("List Toggle Tween")]
    [SerializeField] float moveInDuration = .45f;
    [SerializeField] float moveOutDuration = .6f;
    [SerializeField] float inPosition = 0;
    [SerializeField] float outPosition = 870;
    [SerializeField] Ease moveEase = Ease.OutBack;

    [Header("List Canvas")]
    [SerializeField] Canvas menu_CANVAS;
    [SerializeField] RectTransform menu;

    Sequence menuAnimationSequence;
    bool isMenuOpen;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ToggleList(false); 
    }

    // Update is called once per frame
    void Update()
    {
# if DEBUG
        if (Input.GetKeyDown(KeyCode.Tab))
            ToggleList(!isMenuOpen);
#endif
    }

    void ToggleList(bool isOpening)
    {
        isMenuOpen = isOpening;
        // ToggleNotesHandler?.HandleToggleNotes(new(isOpening));

        menuAnimationSequence?.Kill();
        menuAnimationSequence = DOTween.Sequence();

        if (isOpening)
        {
            menu_CANVAS.gameObject.SetActive(isOpening);
            menuAnimationSequence.Append(menu.DOLocalMoveY(inPosition, moveInDuration).SetEase(moveEase));
        }
        else
        {
            menuAnimationSequence.Append(menu.DOLocalMoveY(outPosition, moveOutDuration).SetEase(moveEase));
            menuAnimationSequence.OnComplete(() => menu_CANVAS.gameObject.SetActive(isOpening));
        }
    }
}
