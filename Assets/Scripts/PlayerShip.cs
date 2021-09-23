using UnityEngine;

public sealed class PlayerShip : Ship
{
    public override bool IsPlayerControlled => true;

    public override void OnStart()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    private float GetAxis(string name)
    {
        var q = Input.GetAxis(name);
        return Time.deltaTime * q / (Mathf.Abs(q) + 1);
    }

    public override void OnUpdate()
    {
        transform.Rotate(-GetAxis("Mouse Y") * rotationSpeed, GetAxis("Mouse X") * rotationSpeed, -Input.GetAxis("Horizontal") * Time.deltaTime * rotationSpeed, Space.Self);
        RB.velocity += Input.GetAxis("Vertical") * speed * Time.deltaTime * transform.forward;

        if (Input.GetKey(KeyCode.Z))
            RB.velocity = Vector3.MoveTowards(RB.velocity, Vector3.zero, speed * Time.deltaTime);
    }
}