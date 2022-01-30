using UnityEngine;

public class CameraScreenResolution : MonoBehaviour
{
    private static float defaultAspect = 16f / 9f;
    private Camera _cam;

    void Start()
    {
        _cam = GetComponent<Camera>();
        if (_cam.aspect < defaultAspect)
        {
            _cam.orthographicSize = _cam.orthographicSize * defaultAspect / _cam.aspect;
        }
    }
}