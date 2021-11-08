using UnityEngine;

public class SkyBox : MonoBehaviour
{
    public void Update()
    {
        if (GameCore.MainCamera)
            transform.position = GameCore.MainCamera.transform.position;
    }
}