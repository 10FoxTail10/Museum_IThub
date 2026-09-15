using UnityEngine;

public class PixelArtResolutionManager : MonoBehaviour
{
    public int targetWidth = 1024;
    public int targetHeight = 1344;

    void Start()
    {
        Screen.SetResolution(targetWidth, targetHeight, false);
        
        if (Camera.main != null)
        {
            Camera.main.aspect = (float)targetWidth / targetHeight;
        }
    }
}