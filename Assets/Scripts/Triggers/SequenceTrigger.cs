using System.Collections.Generic;
using UnityEngine;

public class SequenceTrigger : MonoBehaviour
{
    [SerializeField] private List<GameObject> show;
    [SerializeField] private List<GameObject> seeChoiceHide;
    [SerializeField] private List<GameObject> hearChoiceHide;
    [SerializeField] private List<GameObject> speakChoiceHide;

    [Space]
    public bool FinalTrigger;

    [Header("View")]
    public Transform CameraViewTransform;

    [Header("HUD")]
    [SerializeField] private UI_Button noSeeImage;
    [SerializeField] private UI_Button noHearImage;
    [SerializeField] private UI_Button noSpeakImage;

    [Header("Audio")]
    [SerializeField] private AudioClip bgAudioClip;
    [SerializeField] private AudioClip secondaryAudioClip;
    [SerializeField] private bool returnToMainBGAudio;

    private bool _seeChoiceAvailable;
    private bool _hearChoiceAvailable;
    private bool _speakChoiceAvailable;

    public bool IsChoiceAvailable(int choice)
    {
        return choice switch
        {
            1 => _seeChoiceAvailable,
            2 => _hearChoiceAvailable,
            3 => _speakChoiceAvailable,
            _ => false
        };
    }

    private void Awake()
    {
        _seeChoiceAvailable = noSeeImage != null;
        _hearChoiceAvailable = noHearImage != null;
        _speakChoiceAvailable = noSpeakImage != null;

        SetGameObjects(show, false);
    }

    public void OnChoice(int choice)
    {
        switch (choice)
        {
            case 1:
                noSeeImage.Beep();
                _seeChoiceAvailable = false;
                break;
            case 2:
                noHearImage.Beep();
                _hearChoiceAvailable = false;
                break;
            case 3:
                noSpeakImage.Beep();
                _speakChoiceAvailable = false;
                break;
        }
    }

    public void OnSequenceFinished(int choice)
    {
        switch (choice)
        {
            case 1:
                HandleSeeChoice();
                break;
            case 2:
                HandleHearChoice();
                break;
            case 3:
                HandleSpeakChoice();
                break ;
        }

        SetGameObjects(show, false);

        AudioManager.Instance.StopSecondary(2f);

        Destroy(gameObject);
    }

    private void HandleSeeChoice()
    {
        SetGameObjects(seeChoiceHide, false);

        if (returnToMainBGAudio)
        {
            AudioManager.Instance.ReturnToMainBackground(2f);
        }
    }

    private void HandleHearChoice()
    {
        SetGameObjects(hearChoiceHide, false);

        if (!FinalTrigger)
        {
            AudioManager.Instance.ReturnToMainBackground(2f);
        }
    }

    private void HandleSpeakChoice()
    {
        SetGameObjects(speakChoiceHide, false);

        if (returnToMainBGAudio)
        {
            AudioManager.Instance.ReturnToMainBackground(2f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<P_Controller>(out _)) return;

        SetGameObjects(show, true);

        if(bgAudioClip != null)
        {
            AudioManager.Instance.PlayBackground(bgAudioClip);
        }
        if(secondaryAudioClip != null)
        {
            AudioManager.Instance.PlaySecondary(secondaryAudioClip);
        }

        GameEventsBus.OnSequenceStarted?.Invoke(this);
    }

    private void SetGameObjects(List<GameObject> gameObjects, bool active)
    {
        foreach (GameObject gameObject in gameObjects)
        {
            gameObject.SetActive(active);
        }
    }
}