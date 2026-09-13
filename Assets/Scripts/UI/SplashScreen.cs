using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    [SerializeField] private float duration;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(duration);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}