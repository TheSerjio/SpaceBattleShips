using UnityEngine;

public sealed class PlayerShip : ShipController
{
    public bool AutoBrake;
    public float MouseSense;
    public GameUI ui;

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
        Ship.EnginePower = GameUI.It.Engines.value;
        Ship.BrakePower = GameUI.It.Brakes.value;
        if (rotation != Vector3.zero)
            transform.Rotate(Ship.rotationSpeed * Time.deltaTime * rotation, Space.Self);
        bool q = true;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            ui.Engines.value = ui.Engines.maxValue;
            Ship.Forward();
        }
        else if (Input.GetKey(KeyCode.W))
        {
            ui.Engines.value += Input.mouseScrollDelta.y;
            Ship.Forward();
        }
        else if (AutoBrake)
        {
            Ship.Brake();
            q = false;
        }
        if (Input.GetKey(KeyCode.S))
        {
            ui.Brakes.value += Input.mouseScrollDelta.y;
            if (q)
                Ship.Brake();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
            AutoBrake = !AutoBrake;

        if (Input.GetKey(KeyCode.Z))
            Ship.Brake();
        if (Input.GetKey(KeyCode.Alpha1))
            Ship.LookAt(RB.position + RB.velocity);
        if (Input.GetKey(KeyCode.Alpha2))
            Ship.LookAt(RB.position - RB.velocity);
        if (Input.GetKey(KeyCode.Alpha3))
            Ship.LookAt(Vector3.zero);
        if (Input.GetKey(KeyCode.Space))
            Ship.Fire();
    }
}