using UnityEngine;

public sealed class PlayerShip : Ship
{
    public override bool IsPlayerControlled => true;

    public float MouseSensitivity;

    public override void OnStart()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    public override void OnUpdate()
    {
        float z = 0;
        if (Input.GetKey(KeyCode.Q))
            z++;
        if (Input.GetKey(KeyCode.E))
            z--;

        transform.Rotate(-Input.GetAxis("Vertical") * Time.deltaTime * rotationSpeed, Input.GetAxis("Horizontal") * Time.deltaTime * rotationSpeed, z * Time.deltaTime * rotationSpeed, Space.Self);
        if (Input.GetKey(KeyCode.LeftShift))
        {
            RB.velocity += speed * Time.deltaTime * transform.forward;
            ConfigureTrails(true);
        }
        else
            ConfigureTrails(false);

        if (Input.GetKey(KeyCode.Z))
            RB.velocity = Vector3.MoveTowards(RB.velocity, Vector3.zero, speed * Time.deltaTime);
        if (Input.GetKey(KeyCode.Space))
            Fire();
    }

    public override void OnDestroy() { }
}