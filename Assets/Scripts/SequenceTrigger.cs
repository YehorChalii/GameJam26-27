using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SequenceTrigger : MonoBehaviour
{
    [SerializeField] private List<GameObject> show;
    [SerializeField] private List<GameObject> seeChoiceHide;
    [SerializeField] private List<GameObject> hearChoiceHide;
    [SerializeField] private List<GameObject> speakChoiceHide;

    [Header("View")]
    public Transform CameraViewTransform;

    [Header("HUD")]
    [SerializeField] private Image noSeeImage;
    [SerializeField] private Image noHearImage;
    [SerializeField] private Image noSpeakImage;

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
        Destroy(gameObject);
    }

    private void HandleSeeChoice()
    {
        SetGameObjects(seeChoiceHide, false);
    }

    private void HandleHearChoice()
    {
        SetGameObjects(hearChoiceHide, false);

        AudioManager.Instance.StopSecondary();
    }

    private void HandleSpeakChoice()
    {
        SetGameObjects(speakChoiceHide, false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<P_Controller>(out _)) return;

        SetGameObjects(show, true);

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