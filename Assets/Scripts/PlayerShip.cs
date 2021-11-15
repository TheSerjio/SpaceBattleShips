using UnityEngine;

public sealed class PlayerShip : ShipController
{
    public bool AutoBrake = true;
    GameUI ui;
    Transform cameroid;

    const float CameraRotation = 180;

    public void Start()
    {
        Transform q = null;
        foreach (var pfc in GetComponentsInChildren<PlaceForCamera>())

            if (pfc.parent)
                cameroid = pfc.transform;
            else
                q = pfc.transform;

        var cam = Instantiate(DataBase.Get().CameraPrefab);
        cam.transform.SetParent(q);
        cam.transform.localPosition = Vector3.zero;
        cam.transform.localScale = Vector3.one;
        Instantiate(DataBase.Get().DustPrefab, transform);
    }

    public void Update()
    {
        if (!ui)
        {
            ui = FindObjectOfType<GameUI>();
            return;
        }
        if (Input.GetKey(KeyCode.A))
            if (Ship.TakeEnergy(Time.deltaTime * Ship.EngineCons))
                RB.velocity -= Ship.EnginePower * Time.deltaTime * transform.right / 2;
        if (Input.GetKey(KeyCode.D))
            if (Ship.TakeEnergy(Time.deltaTime * Ship.EngineCons))
                RB.velocity += Ship.EnginePower * Time.deltaTime * transform.right / 2;

        if (Input.GetMouseButton(1))
            cameroid.Rotate(-Input.GetAxis("Mouse Y") * CameraRotation * Time.deltaTime, Input.GetAxis("Mouse X") * CameraRotation * Time.deltaTime, 0, Space.Self);
        else
        {
            var r = cameroid.localRotation;
            r.SetLookRotation(Vector3.forward);
            cameroid.localRotation = Quaternion.RotateTowards(cameroid.localRotation, r, CameraRotation * Time.deltaTime);
        }

        Vector3 rotation = default;
        if (Input.GetMouseButton(0))
        {
            Vector2 pos = 2 * Input.mousePosition / new Vector2(Screen.width, Screen.height);
            pos -= Vector2.one;
            rotation += new Vector3(-pos.y, pos.x);
        }
        if (Input.GetKey(KeyCode.Q))
            rotation += Vector3.forward;
        if (Input.GetKey(KeyCode.E))
            rotation += Vector3.back;

        Ship.EnginePower = GameUI.Self.Engines.value;
        Ship.BrakePower = GameUI.Self.Brakes.value;
        if (rotation != Vector3.zero)
            transform.Rotate(Ship.RotationSpeed * Time.deltaTime * rotation, Space.Self);
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Ship.ExtraForward();
        }
        else if (Input.GetKey(KeyCode.W))
        {
            ui.Engines.value += Input.mouseScrollDelta.y;
            Ship.Forward();
        }
        else if (AutoBrake)
        {
            Ship.AutoBrake();
        }
        if (Input.GetKey(KeyCode.S))
        {
            ui.Brakes.value += Input.mouseScrollDelta.y;
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
        Ship.Fire = Input.GetKey(KeyCode.Space);

        cameroid.localPosition = Vector3.MoveTowards(cameroid.localPosition, Vector3.zero, Time.deltaTime);
    }

    public override void Warn(Vector3 moveTo, Ship.Warning how)
    {
        cameroid.localPosition = Random.insideUnitSphere * how.shakePower;
        if (how.showText)
            if (Warner.Self)
                Warner.Self.Show();
    }

    public override void Death()
    {
        cameroid.SetParent(null, true);
    }
}