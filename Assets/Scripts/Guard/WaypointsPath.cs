using System.Collections.Generic;
using UnityEngine;

public class WaypointsPath : MonoBehaviour
{
    private List<Transform> _waypoints;
    public IReadOnlyList<Transform> Waypoints => _waypoints;

    private void Awake()
    {
        _waypoints = new List<Transform>();

        foreach (Transform child in transform)
        {
            _waypoints.Add(child);
        }
    }
}