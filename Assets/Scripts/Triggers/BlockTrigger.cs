using UnityEngine;

public class BlockTrigger : MonoBehaviour
{
    [SerializeField] private Block block;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.TryGetComponent<P_Controller>(out var _)) return;

        block.Raise();
        this.enabled = false;
    }
}
