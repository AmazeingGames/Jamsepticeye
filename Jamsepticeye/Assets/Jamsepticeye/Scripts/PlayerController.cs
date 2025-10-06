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

    public float speed = 3.0f;

    public Sprite capeSprite;
    public Sprite noCapeSprite;

    public GameObject spawnPoint;

    public DynamicMovement dynamicMover;
    void Start()
    {
        DialogueManager.GetInstance();
        moveDirection = new Vector2(1, 0);
        openMenuAction.Enable();
        moveAction.Enable();
        teleport.Enable();

        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        // dynamicMover = GetComponentInParent<DynamicMovement>();
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        if (SpawnPointHandler.shouldTeleport)
            transform.position = SpawnPointHandler.targetPosition;
        else
            transform.position = spawnPoint.transform.position;
    }
    void FixedUpdate()
    {
        if (!DialogueManager.GetInstance().IsDialoguePlaying)
        {
            Vector2 position = rigidbody2d.position + movement * speed * Time.deltaTime;
            rigidbody2d.MovePosition(position);
        }
    }

    void Update()
    {
        // In case there is no animation running, we need to show the correct sprite
        if (spriteRenderer != null)
            spriteRenderer.sprite = GameStateScript.Instance.Is(GameState.PLACED_HAMMOCK) ? noCapeSprite : capeSprite;

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
            }

            // Handle animation variables
            animator.SetFloat("LookX", moveDirection.x);
            animator.SetFloat("LookY", moveDirection.y);
            animator.SetFloat("Speed", movement.magnitude);
            animator.SetBool("HasCape", !GameStateScript.Instance.Is(GameState.PLACED_HAMMOCK));
        }
    }
}
