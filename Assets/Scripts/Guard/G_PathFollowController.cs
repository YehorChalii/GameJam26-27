using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class G_PathFollowController : MonoBehaviour
{
    [Header("Path")]
    [SerializeField] private WaypointsPath waypointsPath;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float waypointReachThreshold;
    [SerializeField] private float waitTimeAtEnd;

    [Space]
    [SerializeField] private bool isDebugging;

    private List<Transform> _waypoints;

    private int _currentWaypointIndex;
    private int _direction = 1;
    private float _waitTimer;

    [HideInInspector] public bool Stop;
    [HideInInspector] public Vector3 FollowPosition;

    private void Start()
    {
        InitializeWaypoints();
    }

    private void InitializeWaypoints()
    {
        _waypoints = waypointsPath.Waypoints.ToList();

        if (_waypoints.Count == 0) return;

        FollowPosition = _waypoints[0].position;
    }

    private void Update()
    {
        float dt = Time.deltaTime;

        HandleMovement(dt);
    }

    private void HandleMovement(float dt)
    {
        if (_waypoints == null || _waypoints.Count == 0) return;

        if (Stop) return;

        if (_waitTimer > 0f)
        {
            _waitTimer -= dt;
            return;
        }

        Transform targetWaypoint = _waypoints[_currentWaypointIndex];

        FollowPosition = Vector3.MoveTowards(FollowPosition, targetWaypoint.position, movementSpeed * dt);

        if (Vector3.Distance(FollowPosition, targetWaypoint.position) <= waypointReachThreshold)
        {
            AdvanceToNextWaypoint();
        }
    }

    private void AdvanceToNextWaypoint()
    {
        if (_waypoints.Count <= 1) return;

        int nextIndex = _currentWaypointIndex + _direction;

        if (nextIndex >= _waypoints.Count || nextIndex < 0)
        {
            _direction *= -1;
            _currentWaypointIndex += _direction;

            _waitTimer = waitTimeAtEnd;
        }
        else
        {
            _currentWaypointIndex = nextIndex;
        }
    }

    private void OnDrawGizmos()
    {
        if(!isDebugging) return;

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(FollowPosition, 0.5f);
    }
}