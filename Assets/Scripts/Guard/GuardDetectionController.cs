using UnityEngine;

public class GuardDetectionController : MonoBehaviour
{
    [SerializeField] private GuardBehaviorController behaviorController;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && other.TryGetComponent<PlayerController>(out var playerController))
        {
            behaviorController.OnPlayerEnter(playerController);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent<PlayerController>(out var playerController))
        {
            behaviorController.OnPlayerExit();
        }
    }
}
