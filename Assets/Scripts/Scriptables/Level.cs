using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Object/Level")]
public class Level : ValidableScriptableObject
{
#if UNITY_EDITOR
    public UnityEditor.SceneAsset scene;
#endif
    public ShipData[] ships;
    public int BuildingIndex;

    public override IEnumerable<Warning> Validate()
    {
#if UNITY_EDITOR
        BuildingIndex = SceneUtility.GetBuildIndexByScenePath(UnityEditor.AssetDatabase.GetAssetPath(scene));
        UnityEditor.EditorUtility.SetDirty(this);
#endif
        if (!scene)
            yield return Warn(Level.Error, "no scene");
        if (BuildingIndex < 1)
            yield return Warn(Level.Error, "add scene to build");
    }

    public SunData sun;

    public string Name;

    public Texture SpaceTexture;
}