using UnityEngine;
using System.IO;

public class IconCreator : MonoBehaviour
{
    public Camera renderCamera;
    public RenderTexture renderTexture;
    public string savePath = "Assets/IconMaker/";
    private string defaultPath;
    public string iconName = "icon";
    int counter = 0;

    public void CaptureIcon()
    {
        defaultPath = savePath + iconName+counter.ToString()+ ".png";
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = renderTexture;

        renderCamera.targetTexture = renderTexture;
        renderCamera.Render();

        Texture2D tex = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBAHalf, false);
        tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        tex.Apply();

        byte[] bytes = tex.EncodeToPNG();
        File.WriteAllBytes(defaultPath, bytes);

        // Cleanup
        renderCamera.targetTexture = null;
        RenderTexture.active = currentRT;
        Destroy(tex);

        Debug.Log("Icon saved to: " + defaultPath);
        counter++;
    }
}
