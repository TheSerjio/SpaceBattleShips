using UnityEngine;

public class SpaceTextureGenerator : ScriptableObject
{
    public Material material;

    private const int size = 1024 * 8;

    #if UNITY_EDITOR

    [ContextMenu("Do it")]
    public void Do()
    {
        var text = new Texture2D(size, size);

        var buffer = new RenderTexture(size, size, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);

        Graphics.Blit(null, buffer, material);

        text.ReadPixels(new Rect(0, 0, size, size), 0, 0, false);

        var path = $"{UnityEditor.AssetDatabase.GetAssetPath(this).Replace(name + ".", "")}{System.Environment.TickCount}.png";

        System.IO.File.WriteAllBytes(path, text.EncodeToPNG());
        UnityEditor.AssetDatabase.Refresh();
        
        DestroyImmediate(buffer);
        
        DestroyImmediate(text);
    }
#endif
}