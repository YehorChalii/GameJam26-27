using UnityEngine;

public class G_DetectionController : MonoBehaviour
{
    [SerializeField] private G_BehaviorController behaviorController;
    [SerializeField] private LayerMask playerMask;

    private PlayerController _player;

    private void OnTriggerEnter(Collider other)
    {
        if (!IsInLayerMask(other.gameObject, playerMask)) return;

        if (other.TryGetComponent<PlayerController>(out var playerController))
        {
            _player = playerController;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsInLayerMask(other.gameObject, playerMask)) return;

        if (_player != null)
        {
            _player = null;
            behaviorController.OnPlayerExit();
        }
    }

    private void Update()
    {
        if (_player == null) return;

        if (HasLineOfSight(_player))
        {
            behaviorController.OnPlayerEnter(_player);
        }
        else
        {
            behaviorController.OnPlayerExit();
        }
    }

    private bool HasLineOfSight(PlayerController player)
    {
        Vector3 origin = transform.position;
        Vector3 target = player.transform.position;

        Vector3 direction = target - origin;

        if (Physics.Raycast(origin, direction.normalized, out RaycastHit hit))
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