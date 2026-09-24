using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class G_PathFollowController : MonoBehaviour
{
    public Action<Vector3> OnPathFollowPositionUpdated;
    private Vector3 _pathFollowPosition;

    [Header("Path")]
    [SerializeField] private WaypointsPath waypointsPath;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float waypointReachThreshold;
    [SerializeField] private float waitTimeAtEnd;

    private List<Transform> _waypoints;

    private int _currentWaypointIndex;
    private int _direction = 1;
    private float _waitTimer;
    private bool _hasStoped;

    private G_DetectionController _detectionController;

    private void Awake()
    {
        _detectionController = GetComponentInChildren<G_DetectionController>();
    }

    private void OnEnable()
    {
        _detectionController.OnPlayerDetected += HandlePlayerDetected;
        _detectionController.OnPlayerLost += HandlePlayerLost;
    }

    private void OnDisable()
    {
        _detectionController.OnPlayerDetected -= HandlePlayerDetected;
        _detectionController.OnPlayerLost -= HandlePlayerLost;
    }

    private void HandlePlayerDetected(Transform playerTransform) => _hasStoped = true;
    private void HandlePlayerLost() => _hasStoped = false;

    private void Start()
    {
        InitializeWaypoints();
    }

    private void InitializeWaypoints()
    {
        _waypoints = waypointsPath.Waypoints.ToList();

        if (_waypoints.Count == 0) return;

        _pathFollowPosition = _waypoints[0].position;
    }

    private void Update()
    {
        float dt = Time.deltaTime;

        HandleMovement(dt);

        OnPathFollowPositionUpdated?.Invoke(_pathFollowPosition);
    }

    private void HandleMovement(float dt)
    {
        if (_waypoints == null || _waypoints.Count == 0) return;

        if (_hasStoped) return;

        if (_waitTimer > 0f)
        {
            _waitTimer -= dt;
            return;
        }

        Transform targetWaypoint = _waypoints[_currentWaypointIndex];

        _pathFollowPosition = Vector3.MoveTowards(_pathFollowPosition, targetWaypoint.position, movementSpeed * dt);

        if (Vector3.Distance(_pathFollowPosition, targetWaypoint.position) <= waypointReachThreshold)
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
}