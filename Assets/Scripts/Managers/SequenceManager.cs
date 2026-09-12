using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SequenceManager : MonoBehaviour
{
    [SerializeField] private P_Controller playerController;
    [SerializeField] private P_CameraController cameraController;

    [Header("Transition")]
    [SerializeField] private float transitionDelay;
    [SerializeField] private float transitionTime;
    [Range(0f, 1f)]
    [SerializeField] private float beepStart;

    [Header("Audio")]
    [SerializeField] private AudioClip noSeeAudioClip;
    [SerializeField] private AudioClip noHearAudioClip;
    [SerializeField] private AudioClip noSpeakAudioClip;

    private SequenceTrigger _activeSequenceTrigger;
    private bool _sequenceActive;

    private InputActions _inputActions;

    private Coroutine _choiceCoroutine;

    private void Awake()
    {
        _inputActions = new InputActions();
    }

    private void OnEnable()
    {
        GameEventsBus.OnSequenceStarted += HandleSequenceStarted;

        _inputActions.Player.Choice1.performed += HandleChoice1;
        _inputActions.Player.Choice2.performed += HandleChoice2;
        _inputActions.Player.Choice3.performed += HandleChoice3;

        _inputActions.Enable();
    }

    private void OnDisable()
    {
        GameEventsBus.OnSequenceStarted -= HandleSequenceStarted;

        _inputActions.Player.Choice1.performed -= HandleChoice1;
        _inputActions.Player.Choice2.performed -= HandleChoice2;
        _inputActions.Player.Choice3.performed -= HandleChoice3;

        _inputActions.Disable();
    }

    private void HandleChoice1(InputAction.CallbackContext context)
    {
        if (!_sequenceActive) return;

        HandleChoice(1);
    }

    private void HandleChoice2(InputAction.CallbackContext context)
    {
        if (!_sequenceActive) return;

        HandleChoice(2);
    }

    private void HandleChoice3(InputAction.CallbackContext context)
    {
        if (!_sequenceActive) return;

        HandleChoice(3);
    }

    private void HandleChoice(int choice)
    {
        if (_activeSequenceTrigger.IsChoiceAvailable(choice))
        {
            _activeSequenceTrigger.OnChoice(choice);

            switch (choice)
            {
                case 1:
                    AudioManager.Instance.PlaySFX(noSeeAudioClip);
                    break;
                case 2:
                    AudioManager.Instance.PlaySFX(noHearAudioClip);
                    break;
                case 3:
                    AudioManager.Instance.PlaySFX(noSpeakAudioClip);
                    break;
            }

            if (_choiceCoroutine != null)
            {
                StopCoroutine(_choiceCoroutine);
            }

            _choiceCoroutine = StartCoroutine(HandleSequenceFinished(choice));
        }
    }

    private void HandleSequenceStarted(SequenceTrigger activeSequenceTrigger)
    {
        _sequenceActive = true;
        _activeSequenceTrigger = activeSequenceTrigger;

        playerController.SetInput(false);
        cameraController.SetTarget(_activeSequenceTrigger.CameraViewTransform, transitionTime);

        UIManager.Instance.Beep(transitionTime * beepStart, transitionTime * (1f - beepStart));
    }

    private IEnumerator HandleSequenceFinished(int choice)
    {
        yield return new WaitForSeconds(transitionDelay);

        _sequenceActive = false;
        
        cameraController.SetTarget(null, transitionTime);
        playerController.SetInput(true);

        UIManager.Instance.Beep(0f, transitionTime * (1f - beepStart));

        _activeSequenceTrigger.OnSequenceFinished(choice);
    }
}