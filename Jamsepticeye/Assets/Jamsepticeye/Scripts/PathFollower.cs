using UnityEngine;
using System.Collections.Generic;

public class PathFollower : MonoBehaviour
{
    [System.Serializable]
    public class PathPoint
    {
        public GameObject position;
        public float lookX;
        public float lookY;
        public bool moving = true;
    }

    [Header("Path Settings")]
    public List<PathPoint> pathPoints = new List<PathPoint>();
    public float moveSpeed = 5f;
    public float reachThreshold = 0.1f;
    public bool loopPath = false;

    [Header("References")]
    public Animator animator;

    private int currentPointIndex = 0;
    public bool pathComplete = false;
    public bool pathStarted = false;
    public void StartPath()
    {
        pathStarted = true;

        if (pathPoints.Count > 0)
        {
            UpdateAnimatorValues(pathPoints[0]);
        }
    }
    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!pathStarted || pathComplete || pathPoints.Count == 0)
            return;

        FollowPath();
    }

    void FollowPath()
    {
        PathPoint targetPoint = pathPoints[currentPointIndex];

        // Move towards target
        Vector3 direction = (targetPoint.position.transform.position - transform.position).normalized;
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPoint.position.transform.position,
            moveSpeed * Time.deltaTime
        );

        // Check if reached checkpoint
        float distance = Vector3.Distance(transform.position, targetPoint.position.transform.position);
        if (distance <= reachThreshold)
        {
            OnReachCheckpoint();
        }
    }

    void OnReachCheckpoint()
    {
        currentPointIndex++;

        // Check if path is complete
        if (currentPointIndex >= pathPoints.Count)
        {
            if (loopPath)
            {
                currentPointIndex = 0;
            }
            else
            {
                pathComplete = true;
                return;
            }
        }

        // Update animator with new values
        UpdateAnimatorValues(pathPoints[currentPointIndex]);
    }

    void UpdateAnimatorValues(PathPoint point)
    {
        if (animator != null)
        {
            animator.SetFloat("LookX", point.lookX);
            animator.SetFloat("LookY", point.lookY);
            animator.SetFloat("Speed", point.moving ? moveSpeed : 0.0f);
        }
    }

    // Visualize path in editor
    void OnDrawGizmos()
    {
        if (pathPoints.Count == 0)
            return;

        Gizmos.color = Color.green;
        for (int i = 0; i < pathPoints.Count; i++)
        {
            Gizmos.DrawSphere(pathPoints[i].position.transform.position, 0.2f);

            if (i < pathPoints.Count - 1)
            {
                Gizmos.DrawLine(pathPoints[i].position.transform.position, pathPoints[i + 1].position.transform.position);
            }
            else if (loopPath && pathPoints.Count > 1)
            {
                Gizmos.DrawLine(pathPoints[i].position.transform.position, pathPoints[0].position.transform.position);
            }
        }
    }
}