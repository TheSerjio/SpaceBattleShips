using UnityEngine;

public sealed class PlayerShip : Ship
{
    public override bool IsPlayerControlled => true;

    public float MouseSensitivity;

    public override void OnStart()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    private float GetAxis(string name)
    {
        var q = Input.GetAxis(name) * MouseSensitivity;
        return Time.deltaTime * q / (Mathf.Abs(q) + 1);
    }

    public override void OnUpdate()
    {
        transform.Rotate(-GetAxis("Mouse Y") * rotationSpeed, GetAxis("Mouse X") * rotationSpeed, -Input.GetAxis("Horizontal") * Time.deltaTime * rotationSpeed, Space.Self);

        var vert = Input.GetAxis("Vertical");

        RB.velocity += vert * speed * Time.deltaTime * transform.forward;

        ConfigureTrails(vert != 0);

        if (Input.GetKey(KeyCode.Z))
            RB.velocity = Vector3.MoveTowards(RB.velocity, Vector3.zero, speed * Time.deltaTime);
        if (Input.GetKey(KeyCode.Q))
            LookAt(RB.position + RB.velocity);
        if (Input.GetKey(KeyCode.E))
            LookAt(RB.position - RB.velocity);
    }

    public override void OnDestroy() { }
}