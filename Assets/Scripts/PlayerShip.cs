using UnityEngine;

public sealed class PlayerShip : ShipController
{
    public bool AutoBrake;
    public float MouseSense;

    public void Update()
    {
        Vector3 rotation = Vector3.back * Input.GetAxis("Horizontal");
        if (Input.GetMouseButton(1))
        {
            Vector2 pos = Input.mousePosition;
            var size = new Vector2(Screen.width, Screen.height);
            pos -= size / 2f;
            pos = pos * MouseSense / size;
            rotation += new Vector3(-pos.y / (Mathf.Abs(pos.y) + 1), pos.x / (Mathf.Abs(pos.x) + 1));
        }
        ship.EnginePower = GameUI.It.Engines.value;
        ship.BrakePower = GameUI.It.Brakes.value;
        if (rotation != Vector3.zero)
            transform.Rotate(ship.rotationSpeed * Time.deltaTime * rotation, Space.Self);
        bool q = true;
        if (Input.GetKey(KeyCode.W))
        {
            ship.EnginePower += Input.mouseScrollDelta.y;
            ship.Forward();
        }
        else if (AutoBrake)
        {
            ship.Brake();
            q = false;
        }
        if (Input.GetKey(KeyCode.S))
            if (q)
                ship.Brake();

        if (Input.GetKeyDown(KeyCode.Tab))
            AutoBrake = !AutoBrake;

        if (Input.GetKey(KeyCode.Z))
            ship.Brake();
        if (Input.GetKey(KeyCode.Alpha1))
            ship.LookAt(RB.position + RB.velocity);
        if (Input.GetKey(KeyCode.Alpha2))
            ship.LookAt(RB.position - RB.velocity);
        if (Input.GetKey(KeyCode.Alpha3))
            ship.LookAt(Vector3.zero);
        if (Input.GetKey(KeyCode.Space))
            ship.Fire();
    }
}