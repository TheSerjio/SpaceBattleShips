using UnityEngine;

public class SkyBox : MonoBehaviour
{
    public void Start()
    {
        if (LevelManager.currentLevel)
            GetComponent<Renderer>().material.mainTexture = LevelManager.currentLevel.SpaceTexture;
    }
    
    
    
    public void Update()
    {
        if (GameCore.MainCamera)
            transform.position = GameCore.MainCamera.transform.position;
    }
}