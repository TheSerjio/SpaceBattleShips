using UnityEngine;

[DisallowMultipleComponent]
public sealed class Spectator : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;
    public float zRotationSpeed;
    public float mouseSpeed;

    public void Update()
    {
        transform.Translate(Input.GetAxis("Horizontal") * speed, 0, Input.GetAxis("Vertical") * speed, Space.Self);
        if (Input.GetMouseButton(0))
            transform.Rotate(Input.GetAxis("Mouse Y") * rotationSpeed, -Input.GetAxis("Mouse X") * rotationSpeed, 0, Space.Self);
        if (Input.GetMouseButton(1))
            transform.Translate(-Input.GetAxis("Mouse X") * mouseSpeed, -Input.GetAxis("Mouse Y") * mouseSpeed, 0, Space.Self);
        if (Input.GetKey(KeyCode.Q))
            transform.Rotate(0, 0, Time.deltaTime * zRotationSpeed, Space.Self);
        if (Input.GetKey(KeyCode.E))
            transform.Rotate(0, 0, Time.deltaTime * -zRotationSpeed, Space.Self);
    }
}