using UnityEngine;

public class PlayerShip : Ship
{
    public override bool IsPlayerControlled => true;

    public void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    private float getAxis(string name)
    {
        var q = Input.GetAxis(name);
        return Time.deltaTime * q / (Mathf.Abs(q) + 1);
    }

    public void Update()
    {
        //Input.GetAxis is already multiplied by delta
        transform.Rotate(-getAxis("Mouse Y") * rotationSpeed, getAxis("Mouse X") * rotationSpeed, -getAxis("Horizontal") * rotationSpeed, Space.Self);
        RB.velocity += Input.GetAxis("Vertical") * speed * Time.deltaTime * transform.forward;

        if (Input.GetKey(KeyCode.Z))
            RB.velocity = Vector3.MoveTowards(RB.velocity, Vector3.zero, speed * Time.deltaTime);
    }
}