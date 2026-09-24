using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class G_AudioController : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip detectAudioClip;
    [SerializeField] private AudioClip spotAudioClip;

    private G_AlertController _alertController;
    private G_DetectionController _detectionController;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        _detectionController = GetComponentInChildren<G_DetectionController>();
        _alertController = GetComponent<G_AlertController>();
    }

    private void OnEnable()
    {
        _detectionController.OnPlayerDetected += HandlePlayerDetected;
        _alertController.OnPlayerSpotted += HandlePlayerSpotted;
    }

    private void OnDisable()
    {
        _detectionController.OnPlayerDetected -= HandlePlayerDetected;
        _alertController.OnPlayerSpotted -= HandlePlayerSpotted;
    }

    private void HandlePlayerDetected(Transform playerTransform) => _audioSource.PlayOneShot(detectAudioClip);

    private void HandlePlayerSpotted() => _audioSource.PlayOneShot(spotAudioClip);
}