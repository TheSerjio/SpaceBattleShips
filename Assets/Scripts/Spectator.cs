using UnityEngine;

[DisallowMultipleComponent]
public sealed class Spectator : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;
    public float scrollSpeed;
    public float zRotationSpeed;

    public void Update()
    {
        transform.Translate(Input.GetAxis("Horizontal") * speed, Input.GetAxis("Vertical") * speed, Input.GetAxis("Mouse ScrollWheel") * scrollSpeed, Space.Self);
        if (Input.GetMouseButton(2))
            transform.Rotate(Input.GetAxis("Mouse Y") * rotationSpeed, -Input.GetAxis("Mouse X") * rotationSpeed, 0, Space.Self);
        if (Input.GetKey(KeyCode.Q))
            transform.Rotate(0, 0, Time.deltaTime * zRotationSpeed, Space.Self);
        if (Input.GetKey(KeyCode.E))
            transform.Rotate(0, 0, Time.deltaTime * -zRotationSpeed, Space.Self);
    }
}