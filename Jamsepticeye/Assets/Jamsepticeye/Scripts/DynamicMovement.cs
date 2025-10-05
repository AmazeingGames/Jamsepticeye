using UnityEngine;

public class DynamicMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float reachThreshold = 0.1f;
    public bool updateAnimatorAutomatically = true;
    public bool moveHorizontalFirst = true; // If false, moves vertical first
    private System.Action onCompleteCallback;

    [Header("References")]
    public Animator animator;

    public Vector2 finalLookDirection;
    public bool finalLookDirectionSet = false;
    private Vector2 finalDestination;
    private Vector2 currentWaypoint;
    public bool isMoving = false;
    private bool onSecondLeg = false; // True when moving on the second axis

    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isMoving)
        {
            MoveToDestination();
        }
    }

    public void MoveTo(Vector2 targetPosition, Vector2 finalLookDirection_, System.Action onComplete = null)
    {
        finalDestination = targetPosition;
        finalLookDirection = finalLookDirection_;
        finalLookDirectionSet = true;
        isMoving = true;
        onSecondLeg = false;
        onCompleteCallback = onComplete;

        // Calculate the waypoint (corner point)
        CalculateCurrentWaypoint();

        if (updateAnimatorAutomatically)
        {
            UpdateAnimatorFromDirection();
        }

    }
    public void MoveTo(Vector2 targetPosition, System.Action onComplete = null)
    {
        finalDestination = targetPosition;
        onCompleteCallback = onComplete;
        isMoving = true;
        finalLookDirectionSet = false;
        onSecondLeg = false;

        // Calculate the waypoint (corner point)
        CalculateCurrentWaypoint();

        if (updateAnimatorAutomatically)
        {
            UpdateAnimatorFromDirection();
        }
    }

    void CalculateCurrentWaypoint()
    {
        Vector2 currentPos = transform.position;

        if (moveHorizontalFirst)
        {
            // Move horizontal first, then vertical
            if (!onSecondLeg)
            {
                // First leg: match X, keep current Y
                currentWaypoint = new Vector2(finalDestination.x, currentPos.y);
            }
            else
            {
                // Second leg: go to final destination
                currentWaypoint = finalDestination;
            }
        }
        else
        {
            // Move vertical first, then horizontal
            if (!onSecondLeg)
            {
                // First leg: match Y, keep current X
                currentWaypoint = new Vector2(currentPos.x, finalDestination.y);
            }
            else
            {
                // Second leg: go to final destination
                currentWaypoint = finalDestination;
            }
        }
    }

    void MoveToDestination()
    {
        Vector2 currentPos = transform.position;

        // Move towards current waypoint
        Vector2 newPos = Vector2.MoveTowards(
            currentPos,
            currentWaypoint,
            moveSpeed * Time.deltaTime
        );

        transform.position = newPos;

        // Check if reached current waypoint
        float distance = Vector2.Distance(newPos, currentWaypoint);
        if (distance <= reachThreshold)
        {
            if (!onSecondLeg)
            {
                // Reached first waypoint, now move to second leg
                onSecondLeg = true;
                CalculateCurrentWaypoint();

                if (updateAnimatorAutomatically)
                {
                    UpdateAnimatorFromDirection();
                }
            }
            else
            {
                // Reached final destination
                OnReachDestination();
            }
        }
    }

    void UpdateAnimatorFromDirection()
    {
        if (animator == null)
            return;

        Vector2 currentPos = transform.position;
        Vector2 direction = (currentWaypoint - currentPos).normalized;

        // Calculate look direction based on movement
        float lookX = direction.x;
        float lookY = direction.y;

        // Since we're moving in right angles, one should be ~0
        // Normalize to ensure clean cardinal directions
        if (Mathf.Abs(lookX) > Mathf.Abs(lookY))
        {
            lookX = Mathf.Sign(lookX);
            lookY = 0f;
        }
        else
        {
            lookX = 0f;
            lookY = Mathf.Sign(lookY);
        }


        SetAnimatorValues(lookX, lookY, moveSpeed);
    }

    void SetAnimatorValues(float lookX, float lookY, float speed)
    {
        if (animator != null)
        {
            animator.SetFloat("LookX", lookX);
            animator.SetFloat("LookY", lookY);
            animator.SetFloat("Speed", speed);
        }
    }

    void OnReachDestination()
    {
        isMoving = false;
        onSecondLeg = false;

        if (updateAnimatorAutomatically && animator != null)
        {
            animator.SetFloat("Speed", 0f);
            if (finalLookDirection != null && finalLookDirectionSet)
            {
                animator.SetFloat("LookX", finalLookDirection.x);
                animator.SetFloat("LookY", finalLookDirection.y);
            }
        }

        // Invoke callback if provided
        onCompleteCallback?.Invoke();
        onCompleteCallback = null;
    }


    // Stop current movement
    public void Stop()
    {
        isMoving = false;
        if (updateAnimatorAutomatically && animator != null)
        {
            animator.SetFloat("Speed", 0f);
            if (finalLookDirection != null && finalLookDirectionSet)
            {
                animator.SetFloat("LookX", finalLookDirection.x);
                animator.SetFloat("LookY", finalLookDirection.y);
            }
        }
    }

    // Check if currently moving
    public bool IsMoving()
    {
        return isMoving;
    }

    // Get current destination
    public Vector2 GetDestination()
    {
        return finalDestination;
    }

    // Visualize destination in editor
    void OnDrawGizmos()
    {
        if (isMoving)
        {
            Vector2 currentPos = transform.position;

            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(finalDestination, 0.3f);

            // Draw the L-shaped path
            Gizmos.color = Color.cyan;
            if (moveHorizontalFirst)
            {
                Vector2 corner = new Vector2(finalDestination.x, currentPos.y);
                Gizmos.DrawLine(currentPos, corner);
                Gizmos.DrawLine(corner, finalDestination);
                Gizmos.DrawSphere(corner, 0.2f);
            }
            else
            {
                Vector2 corner = new Vector2(currentPos.x, finalDestination.y);
                Gizmos.DrawLine(currentPos, corner);
                Gizmos.DrawLine(corner, finalDestination);
                Gizmos.DrawSphere(corner, 0.2f);
            }
        }
    }
}