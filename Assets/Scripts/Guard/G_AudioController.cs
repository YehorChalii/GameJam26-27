using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class G_AudioController : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip detectAudioClip;
    [SerializeField] private AudioClip spotAudioClip;

    private G_AlertController alertController;

    private AudioSource _audioSource;

    private bool _wasDetecting;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        alertController = GetComponent<G_AlertController>();
    }

    private void OnEnable()
    {
        alertController.OnAlertLevelChanged += HandleAlertLevelChanged;
        alertController.OnPlayerSpotted += HandlePlayerSpotted;
    }

    private void OnDisable()
    {
        alertController.OnAlertLevelChanged -= HandleAlertLevelChanged;
        alertController.OnPlayerSpotted -= HandlePlayerSpotted;
    }

    private void HandleAlertLevelChanged(float alertLevel)
    {
        bool playerDetected = alertLevel > 0f;

        if (playerDetected && !_wasDetecting)
        {
            _audioSource.PlayOneShot(detectAudioClip);
        }

        _wasDetecting = playerDetected;
    }
    private void HandlePlayerSpotted()
    {
        _audioSource.PlayOneShot(spotAudioClip);
    }
}