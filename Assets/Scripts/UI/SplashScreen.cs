using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class SplashScreen : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Application.targetFrameRate = 120;
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = Application.streamingAssetsPath + "/splash.mp4";
        videoPlayer.playOnAwake = false;
        videoPlayer.isLooping = false;

        videoPlayer.audioOutputMode = VideoAudioOutputMode.Direct;
        videoPlayer.prepareCompleted += HandleVideoPrepared;
        videoPlayer.loopPointReached += HandleVideoFinished;
    }

    private void Start()
    {
        videoPlayer.Prepare();
    }

    private void HandleVideoPrepared(VideoPlayer videoPlayer)
    {
        videoPlayer.Play();
    }

    private void HandleVideoFinished(VideoPlayer videoPlayer)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void OnDestroy() 
    {
        videoPlayer.prepareCompleted -= HandleVideoPrepared;
        videoPlayer.loopPointReached -= HandleVideoFinished;
    }
}