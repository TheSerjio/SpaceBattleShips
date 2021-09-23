using UnityEngine;

[DisallowMultipleComponent]
public sealed class Rotation : MonoBehaviour
{
    public Vector3 rotation;
    public Space how;

    void Update()
    {
        transform.Rotate(rotation * Time.deltaTime, how);
    }
}