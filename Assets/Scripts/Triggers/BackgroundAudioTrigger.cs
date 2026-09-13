using UnityEngine;

public class BackgroundAudioTrigger : MonoBehaviour
{
    [SerializeField] private AudioClip BGAudioClip;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.TryGetComponent<P_Controller>(out var _)) return;

        AudioManager.Instance.PlayBackground(BGAudioClip, 2f);
        this.enabled = false;
    }
}
