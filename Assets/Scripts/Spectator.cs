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
                var mode = new Locating
                {
                    Distance = Locating.Sorting.Any,
                    InFrontOfMe = true,
                    Size = Locating.Sorting.Any,
                    SomeRandom = false
                };
                var cam = GetComponent<Camera>();
                transform.LookAt(cam.ScreenToWorldPoint(Input.mousePosition + Vector3.forward));
                var tar = GameCore.Self.FindTargetShip(team, true, mode, transform);
                if (tar)
                    if (!PlayerMark.Self)
                        tar.gameObject.AddComponent<PlayerMark>();
            }
    }

    public void ComeHere(Vector3 pos, Quaternion rot) => transform.SetPositionAndRotation(pos, rot);
}