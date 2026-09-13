using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private P_Controller playerController;

    [Header("Game Over")]
    [SerializeField] private float delayAfterPlayerSpotted;
    [SerializeField] private float blackScreenDuration;

    private Coroutine _playerSpottedCoroutine;

    private void OnEnable()
    {
        GameEventsBus.OnPlayerSpotted += HandlePlayerSpotted;
    }

    private void OnDisable()
    {
        GameEventsBus.OnPlayerSpotted -= HandlePlayerSpotted;
    }

    private void HandlePlayerSpotted()
    {
        if (_playerSpottedCoroutine != null) return;

        _playerSpottedCoroutine = StartCoroutine(PlayerSpottedRoutine());
    }

    private IEnumerator PlayerSpottedRoutine()
    {
        playerController.SetInput(false);

        AudioManager.Instance.StopBackground(delayAfterPlayerSpotted);
        AudioManager.Instance.StopSecondary(delayAfterPlayerSpotted);

        UIManager.Instance.Beep(delayAfterPlayerSpotted, blackScreenDuration);

        yield return new WaitForSeconds(delayAfterPlayerSpotted + blackScreenDuration);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}