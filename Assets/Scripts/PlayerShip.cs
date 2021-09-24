using UnityEngine;

public sealed class PlayerShip : Ship
{
    public override bool IsPlayerControlled => true;

    public float MouseSensitivity;

    private bool autoBrake;

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
        {
            if (autoBrake)
                Brake();
            ConfigureTrails(false);
        }

        if (Input.GetKeyDown(KeyCode.Tab))
            autoBrake = !autoBrake;

        if (Input.GetKey(KeyCode.Z))
            Brake();
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetKey(KeyCode.Alpha1))
                LookAt(RB.position + RB.velocity);
            if (Input.GetKey(KeyCode.Alpha2))
                LookAt(RB.position - RB.velocity);
            if (Input.GetKey(KeyCode.Alpha1))
                LookAt(RB.position + RB.velocity);
        }
        if (Input.GetKey(KeyCode.Space))
            Fire();
    }

    public override void OnDestroy() { }
}