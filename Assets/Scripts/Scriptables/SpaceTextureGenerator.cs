using UnityEngine;

public class SpaceTextureGenerator : ScriptableObject
{
    public float Q;

    public Material material;
    
    const int size = 512;
    
    #if UNITY_EDITOR

    public void Do()
    {
        var text = new Texture2D(size, size);

        var buffer = new RenderTexture(size, size, 0);

        Graphics.Blit(null, buffer, material);
        
        System.IO.File.WriteAllBytes($"{UnityEditor.AssetDatabase.GetAssetPath(this)}.hehehe.png", text.EncodeToPNG());
    }

#endif
}