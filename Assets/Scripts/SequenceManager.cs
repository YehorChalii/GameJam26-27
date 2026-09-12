using UnityEngine;
using UnityEngine.InputSystem;

public class SequenceManager : MonoBehaviour
{
    [SerializeField] private P_Controller playerController;
    [SerializeField] private P_CameraController cameraController;

    [Header("Transition")]
    [SerializeField] private float transitionTime;
    [Range(0f, 1f)]
    [SerializeField] private float beepStart;

    [Header("Audio")]
    [SerializeField] private AudioClip choiceAudioClip;

    private SequenceTrigger _activeSequenceTrigger;
    private bool _sequenceActive;

    private InputActions _inputActions;

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

        int choice = 1;

        if (_activeSequenceTrigger.IsChoiceAvailable(choice))
        {
            HandleSequenceFinished(choice);
        }
    }

    private void HandleChoice2(InputAction.CallbackContext context)
    {
        if (!_sequenceActive) return;

        int choice = 2;

        if (_activeSequenceTrigger.IsChoiceAvailable(choice))
        {
            HandleSequenceFinished(choice);
        }
    }

    private void HandleChoice3(InputAction.CallbackContext context)
    {
        if (!_sequenceActive) return;

        int choice = 3;

        if (_activeSequenceTrigger.IsChoiceAvailable(choice))
        {
            HandleSequenceFinished(choice);
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

    private void HandleSequenceFinished(int choice)
    {
        _sequenceActive = false;
        
        cameraController.SetTarget(null, transitionTime);
        playerController.SetInput(true);

        UIManager.Instance.Beep(0f, transitionTime * (1f - beepStart));
        AudioManager.Instance.PlaySFX(choiceAudioClip);

        _activeSequenceTrigger.OnSequenceFinished(choice);
    }
}