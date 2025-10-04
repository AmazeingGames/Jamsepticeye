using MoreMountains.Tools;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuScript : MonoBehaviour
{
    public static MenuScript instance { get; private set; }

    [SerializeField]
    private VisualElement root;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UIDocument uiDocument = GetComponent<UIDocument>();
        if (uiDocument == null)
            return;

        root = uiDocument.rootVisualElement;
        var resumeButton = root.Q<Button>("ResumeButton");
        if (resumeButton != null)
            resumeButton.RegisterCallback<ClickEvent>(Resume);

        Hide();
    }

    public void Toggle()
    {
        if (root.visible)
            Hide();
        else
            Show();
    }

    private void Hide()
    {
        Debug.Log("Hide");
        root.visible = false;
    }

    private void Show()
    {
        Debug.Log("Show");
        root.visible = true;
    }
    private void Resume(ClickEvent e)
    {
        if (!root.visible)
            return;

        if (e.button == 0)
        {
            // Left click
            Hide();
        }
    }
}
