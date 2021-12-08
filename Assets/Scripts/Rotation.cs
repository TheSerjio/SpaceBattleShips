using UnityEngine;

[DisallowMultipleComponent]
public sealed class Rotation : MonoBehaviour
{
    public Vector3 rotation;
    public Space how;

    public void Start()
    {
        transform.Rotate(rotation * 1000 * Random.value, how);
    }

    public void Update()
    {
        transform.Rotate(rotation * Time.deltaTime, how);
    }
}