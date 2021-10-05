using UnityEngine;

public class SkyBox : MonoBehaviour
{
    public Transform target;

    public void Update()
    {
        if (target)
            transform.position = target.position;
    }
}