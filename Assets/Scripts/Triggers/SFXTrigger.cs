using UnityEngine;

public class SFXTrigger : MonoBehaviour
{
    [SerializeField] private AudioClip sfx;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.TryGetComponent<P_Controller>(out var _)) return;

        AudioManager.Instance.PlaySFX(sfx);
        this.enabled = false;
    }
}
