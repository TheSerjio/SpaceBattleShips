using UnityEngine;

public sealed class PlayerShip : Ship
{
    public bool AutoBrake;
    public float MouseSense;
    public override void OnFixedUpdate() { }
    public override void OnStart() { }

    public override void OnUpdate()
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
        transform.Rotate(rotationSpeed * Time.deltaTime * rotation, Space.Self);
        if (Input.GetKey(KeyCode.LeftShift))
        {
            ConfigTrails(1);
            Forward();
        }
        else
        {
            ConfigTrails(0);
            if (AutoBrake)
                Brake();
        }
        if (Input.GetKeyDown(KeyCode.Tab))
            AutoBrake = !AutoBrake;

        if (Input.GetKey(KeyCode.Z))
            Brake();
        if (Input.GetKey(KeyCode.Alpha1))
            LookAt(RB.position + RB.velocity);
        if (Input.GetKey(KeyCode.Alpha2))
            LookAt(RB.position - RB.velocity);
        if (Input.GetKey(KeyCode.Alpha3))
            LookAt(Vector3.zero);
        if (Input.GetKey(KeyCode.Space))
            Fire();
    }

    public override void WhenDestroy() { }
}