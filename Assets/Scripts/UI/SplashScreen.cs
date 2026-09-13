using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    [SerializeField] private float duration;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(duration);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}