using UnityEngine;

[RequireComponent(typeof(Camera))]
[DisallowMultipleComponent]
public sealed class Spectator : SINGLETON<Spectator>
{
    public float speed;
    public float scrollSpeed;
    public float rotationSpeed;
    public float zRotationSpeed;
    public float mouseSpeed;
    public Team team;

    public void Update()
    {
        var d = Time.deltaTime;
        transform.Translate(Input.GetAxis("Horizontal") * speed * d, Input.GetAxis("Vertical") * speed * d, Input.GetAxis("Mouse ScrollWheel") * scrollSpeed, Space.Self);
        if (Input.GetMouseButton(0))
            transform.Rotate(Input.GetAxis("Mouse Y") * rotationSpeed, -Input.GetAxis("Mouse X") * rotationSpeed, 0, Space.Self);
        if (Input.GetMouseButton(1))
            transform.Translate(-Input.GetAxis("Mouse X") * mouseSpeed, -Input.GetAxis("Mouse Y") * mouseSpeed, 0, Space.Self);
        if (Input.GetKey(KeyCode.Q))
            transform.Rotate(0, 0, d * zRotationSpeed, Space.Self);
        if (Input.GetKey(KeyCode.E))
            transform.Rotate(0, 0, d * -zRotationSpeed, Space.Self);
        if (Input.GetMouseButtonDown(0))
            if (Input.GetKey(KeyCode.Space))
            {
                var cam = GetComponent<Camera>();
                var tar = GameCore.Self.FindTeamMateShipFromCamera(team, transform.position, cam.ScreenToWorldPoint(Input.mousePosition + Vector3.forward) - transform.position);
                if (tar)
                    if (!PlayerMark.Self)
                        tar.gameObject.AddComponent<PlayerMark>();
            }
    }

    public void ComeHere(Vector3 pos, Quaternion rot) => transform.SetPositionAndRotation(pos, rot);
}