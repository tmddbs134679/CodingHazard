
public class CameraManager : Singleton<CameraManager>
{
    public FPSVirtualCamera FPSVirtualCamera { get; private set; }

    private void Awake()
    {
        FPSVirtualCamera = FindAnyObjectByType<FPSVirtualCamera>();
    }
}
