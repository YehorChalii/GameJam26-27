using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private float startY;
    [SerializeField] private float endY;
    [SerializeField] private float raiseDuration;

    private float _timer;
    private bool _raise;

    public void Raise() => _raise = true;

    private void Awake()
    {
        SetTargetY(startY);
    }

    private void Update()
    {
        if (_raise)
        {
            HandleRaise();
        }
    }

    private void HandleRaise()
    {
        _timer += Time.deltaTime;
        float t = Mathf.Clamp01(_timer / raiseDuration);

        float targetY = Mathf.Lerp(startY, endY, t);
        SetTargetY(targetY);

        if (t >= 1f)
        {
            this.enabled = false;
        }
    }

    private void SetTargetY(float targetY)
    {
        transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
    }
}
