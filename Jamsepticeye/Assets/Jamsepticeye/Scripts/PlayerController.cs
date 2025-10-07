using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private InputAction openMenuAction;

    [SerializeField]
    private InputAction moveAction;

    [SerializeField]
    private InputAction teleport;

    [SerializeField]
    private Vector2 movement;

    [SerializeField]
    private Vector2 moveDirection = new Vector2(1, 0);

    private Animator animator;
    private Rigidbody2D rigidbody2d;
    private SpriteRenderer spriteRenderer;

    [SerializeField] float defaultSpeed = 3.0f;
    float currentMoveSpeed;

    [Header("Debug")]
    [SerializeField] float cheatSpeed = 12;

    public Sprite capeSprite;
    public Sprite noCapeSprite;

    public GameObject spawnPoint;

    public DynamicMovement dynamicMover;
    
    public static EventHandler<SteppedEventArgs> SteppedEventHandler;

    public class SteppedEventArgs : EventArgs { public SteppedEventArgs() { } }

    Vector2 mostRecentDirectionKeyPress = Vector2.zero;
    Vector2 rawAxisLastFrame;
    Vector2 rawAxisThisFrame;
    Vector2 mostRecentAxisPress;

    void Start()
    {
        // Force cookies if left bakery without

        
        if (GameStateScript.Instance.Is(GameState.BAKER_DEAD))
            GameStateScript.Instance.Set(GameState.HAS_COOKIES);

        /*
        if (GameStateScript.Instance.Is(GameState.HAS_COOKIES))
            ServiceLocator.GetInventoryService().CollectItem(ItemData.ItemType.Cookies);
        if (GameStateScript.Instance.Is(GameState.HAS_SUGAR))
            ServiceLocator.GetInventoryService().CollectItem(ItemData.ItemType.Sugar);
        if (GameStateScript.Instance.Is(GameState.HAS_STICKS))
            ServiceLocator.GetInventoryService().CollectItem(ItemData.ItemType.Stick);
        if (GameStateScript.Instance.Is(GameState.HAS_ROCKS))
            ServiceLocator.GetInventoryService().CollectItem(ItemData.ItemType.Rocks);
        if (GameStateScript.Instance.Is(GameState.HAS_EGGS))
            ServiceLocator.GetInventoryService().CollectItem(ItemData.ItemType.Eggs);
        if (GameStateScript.Instance.Is(GameState.HAS_COFFEE))
            ServiceLocator.GetInventoryService().CollectItem(ItemData.ItemType.Coffee);
        */

        DialogueManager.GetInstance();
        moveDirection = new Vector2(1, 0);
        openMenuAction.Enable();
        moveAction.Enable();
        teleport.Enable();

        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        SpawnPlayer();
    }

    void FixedUpdate()
    {
        if (!DialogueManager.GetInstance().IsDialoguePlaying)
        {
            Vector2 position = rigidbody2d.position + movement * currentMoveSpeed * Time.deltaTime;
            rigidbody2d.MovePosition(position);
        }
    }

    void Update()
    {
        /*
        rawAxisLastFrame = rawAxisThisFrame;
        rawAxisThisFrame.x = Input.GetAxisRaw("Horizontal");
        rawAxisThisFrame.y = Input.GetAxisRaw("Vertical");

        if (rawAxisLastFrame != rawAxisThisFrame)
        {
            bool newXPress = (rawAxisThisFrame.x != rawAxisLastFrame.x) && (rawAxisThisFrame.x != 0);
            bool newYPress = (rawAxisThisFrame.y != rawAxisLastFrame.y) && (rawAxisThisFrame.y != 0);

            bool pressedDirectionalKey = newXPress || newYPress;
            if (pressedDirectionalKey)
            {
                mostRecentAxisPress = rawAxisThisFrame - rawAxisLastFrame;
                Debug.Log($"New axis pressed: ({mostRecentAxisPress.x}, {mostRecentAxisPress.y})");
            }
        }*/


        currentMoveSpeed = defaultSpeed;
#if DEBUG
        if (Input.GetKey(KeyCode.LeftShift))
            currentMoveSpeed = cheatSpeed;
#endif
        // In case there is no animation running, we need to show the correct sprite
        if (spriteRenderer != null)
            spriteRenderer.sprite = GameStateScript.Instance.Is(GameState.PLACED_HAMMOCK) ? noCapeSprite : capeSprite;

        float speed = 0f;
        if (!GameStateScript.Instance.Is(GameState.KID_CHOKING))
        {
            if (!DialogueManager.GetInstance().IsDialoguePlaying)
            {
                if (dynamicMover != null && dynamicMover.isMoving)
                {
                    if (dynamicMover.finalLookDirectionSet)
                        moveDirection = dynamicMover.finalLookDirection; // Update the move direction to the final direction so this class doesn't override the player look direction
                    return;
                }

                // Handle movement
                movement = moveAction.ReadValue<Vector2>();
                if (!Mathf.Approximately(movement.x, 0.0f) || !Mathf.Approximately(movement.y, 0.0f))
                {
                    // He do be schmoving
                    moveDirection.Set(movement.x, movement.y);
                    moveDirection.Normalize();
                }
                speed = movement.magnitude;
            }
        }


        // Handle animation variables

        Vector2 animatorMoveDirection = Vector2.zero;
        if (moveDirection.x > 0.1 || moveDirection.x < -0.1)
        {
            animatorMoveDirection.y = 0;
            animatorMoveDirection.x = Mathf.Sign(moveDirection.x); // 1 or -1
        }
        else
        {
            animatorMoveDirection.y = Mathf.Sign(moveDirection.y);
        }

        animator.SetFloat("LookX", animatorMoveDirection.x);
        animator.SetFloat("LookY", animatorMoveDirection.y);

        animator.SetFloat("Speed", speed);
        animator.SetBool("HasCape", !GameStateScript.Instance.Is(GameState.PLACED_HAMMOCK));
    }

    void SpawnPlayer()
    {
        if (SpawnPointHandler.shouldTeleport)
            transform.position = SpawnPointHandler.targetPosition;
        else
            transform.position = spawnPoint.transform.position;
    }
}
