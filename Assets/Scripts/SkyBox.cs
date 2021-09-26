using UnityEngine;

public class SkyBox : MonoBehaviour
{
    public Transform target;

    void Update()
    {
        transform.position = target.position;
    }
}