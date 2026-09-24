using System;
using UnityEngine;

public class G_DetectionController : MonoBehaviour
{
    public Action<Transform> OnPlayerDetected;
    public Action OnPlayerLost;

    [SerializeField] private float minDetectionTime;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private LayerMask hitMask;

    private Transform _player;
    private float _detectionTimer;
    private bool _playerDetected;

    private void OnTriggerEnter(Collider other)
    {
        if (!IsInLayerMask(other.gameObject, playerMask)) return;

        if (other.TryGetComponent<P_Controller>(out var playerController))
        {
            _player = playerController.transform;
            _detectionTimer = 0f;
            _playerDetected = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsInLayerMask(other.gameObject, playerMask)) return;

        if (_player != null)
        {
            _player = null;
            _detectionTimer = 0f;
            _playerDetected = false;

            OnPlayerLost?.Invoke();
        }
    }

    private void Update()
    {
        if (_player == null) return;

        if (HasLineOfSight(_player))
        {
            _detectionTimer += Time.deltaTime;

            if (!_playerDetected && _detectionTimer >= minDetectionTime)
            {
                _playerDetected = true;
                OnPlayerDetected?.Invoke(_player);
            }
        }
        else
        {
            _detectionTimer = 0f;

            if (_playerDetected)
            {
                _playerDetected = false;
                OnPlayerLost?.Invoke();
            }
        }
    }

    private bool HasLineOfSight(Transform player)
    {
        Vector3 origin = transform.position;
        Vector3 target = player.position;

        Vector3 direction = target - origin;

        if (Physics.Raycast(origin, direction.normalized, out RaycastHit hit, direction.magnitude, hitMask))
        {
            return IsInLayerMask(hit.collider.gameObject, playerMask);
        }

        return false;
    }

    private bool IsInLayerMask(GameObject gameObject, LayerMask layerMask)
    {
        return ((1 << gameObject.layer) & layerMask) != 0;
    }
}