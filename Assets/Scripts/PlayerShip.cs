using UnityEngine;

public sealed class PlayerShip : ShipController
{
    public bool AutoBrake = true;
    GameUI ui;
    Transform cameroid;

    const float SlowCamera = 180;
    const float FastCamera = SlowCamera * 3;

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


    static Vector2 MouseRotation
    {
        get
        {
            Vector2 tar = ((Input.mousePosition / new Vector2(Screen.width, Screen.height)) - new Vector2(0.5f, 0.5f)) * 2;
            return new Vector2(Mathf.Clamp(-tar.y, -1, 1), Mathf.Clamp(tar.x, -1, 1));
        }
    }

    public void Update()
    {

        var cam = GameCore.MainCamera;

        bool autoBrake = AutoBrake;
        if (!ui)
        {
            ui = FindObjectOfType<GameUI>();
            return;
        }
        ui.Engines.value += Input.mouseScrollDelta.y / 4f;

        transform.Rotate(Input.GetAxis("Horizontal") * Ship.RotationSpeed * Time.deltaTime * Vector3.up);
        cameroid.Rotate(Input.GetAxis("Horizontal") * SlowCamera * Time.deltaTime * Vector3.down / (0.5f + Vector3.Distance(cameroid.forward, transform.forward)));

        if (!Input.GetMouseButton(1))
        {
            var power = Vector3.Distance(transform.forward, cameroid.forward) + 1;
            cameroid.Rotate(FastCamera * Time.deltaTime * MouseRotation / power);
        }
        var r = cameroid.localRotation;
        r.SetLookRotation(Vector3.forward);
        cameroid.localRotation = Quaternion.RotateTowards(cameroid.localRotation, r, SlowCamera * Time.deltaTime);
        
        if (Input.GetMouseButton(0))
        {
            var rotation = cameroid.rotation;
            transform.Rotate(Ship.RotationSpeed * Time.deltaTime * MouseRotation, Space.Self);
            cameroid.rotation = rotation;
        }
        if (Input.GetKey(KeyCode.Q))
            transform.Rotate(Ship.RotationSpeed * Time.deltaTime * Vector3.forward, Space.Self);
        if (Input.GetKey(KeyCode.E))
            transform.Rotate(Ship.RotationSpeed * Time.deltaTime * Vector3.back, Space.Self);

        Ship.EnginePower = GameUI.Self.Engines.value;

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

        cameroid.localPosition = Vector3.MoveTowards(cameroid.localPosition, Vector3.zero, Time.deltaTime);

        //update ui
        {
            var ui = GameUI.Self;
            if (Ship.Shield)
                ui.Shields.localScale = new Vector3(Ship.Shield.Relative, 1, 1);
            ui.Power.localScale = new Vector3(Ship.RelativeEnergy, 1, 1);
            ui.Health.localScale = new Vector3(Ship.RelativeHealth, 1, 1);
            ui.VelocityText.text = Mathf.RoundToInt(Utils.ToSadUnits(RB)).ToString();
            ui.Velocity.gameObject.SetActive(RB.velocity.sqrMagnitude > 0.1f);
            ui.Velocity.position = (Vector2)cam.WorldToScreenPoint(cam.transform.position + RB.velocity);
        }
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