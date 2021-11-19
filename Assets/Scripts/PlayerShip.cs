using UnityEngine;

public sealed class PlayerShip : ShipController
{
    public bool AutoBrake = true;

    PlayerMark Mark
    {
        get
        {
            if (!__mark__)
                __mark__ = GetComponent<PlayerMark>();
            return __mark__;
        }
    }

    PlayerMark __mark__;

    public void Update()
    {
        bool autoBrake = AutoBrake;

        if (Input.GetKey(KeyCode.A))
            transform.Rotate(Ship.RotationSpeed * Time.deltaTime * Vector3.forward, Space.Self);
        if (Input.GetKey(KeyCode.D))
            transform.Rotate(Ship.RotationSpeed * Time.deltaTime * Vector3.back, Space.Self);

        Ship.EnginePower = Mathf.Clamp(Ship.EnginePower + (Input.mouseScrollDelta.y / 4f), 0, 5);

        if (Input.GetMouseButton(0))
        {
            var rotation = Mark.Cameroid.rotation;
            transform.Rotate(Ship.RotationSpeed * Time.deltaTime * PlayerMark.MouseRotation, Space.Self);
            Mark.Cameroid.rotation = rotation;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Ship.ExtraForward();
            autoBrake = false;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            Ship.Forward();
            autoBrake = false;
        }
        if (Input.GetKey(KeyCode.S))
        {
            Ship.Brake();
            autoBrake = false;
        }

        if (autoBrake)
            Ship.AutoBrake();

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
        Ship.Fire = Input.GetKey(KeyCode.Space);
    }

    public override void Warn(Vector3 moveTo, Ship.Warning how)
    {
        Mark.Cameroid.localPosition = Random.insideUnitSphere * how.shakePower;
        if (how.showText)
            if (Warner.Self)
                Warner.Self.Show();
    }

    public override void Death()
    {
        Mark.IfDie();
    }
}