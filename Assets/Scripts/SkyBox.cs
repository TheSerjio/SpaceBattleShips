using UnityEngine;

public class SkyBox : MonoBehaviour
{
    public Transform target;

    void Update()
    {
        if (target)
            transform.position = target.position;
    }
}