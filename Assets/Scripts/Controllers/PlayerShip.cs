using UnityEngine;

public sealed class PlayerShip : ShipController
{
    public bool AutoBrake = true;

    private PlayerMark Mark
    {
        get
        {
            if (!__mark__)
                __mark__ = GetComponent<PlayerMark>();
            return __mark__;
        }
    }

    private PlayerMark __mark__;

    private Targeting.TargetDirection target;
    
    public void Start()
    {
        target = new Targeting.TargetDirection();
    }

    public void Update()
    {
        target.Fire = Input.GetKey(KeyCode.Mouse1);
        Ship.Target = target;
        
        var autoBrake = AutoBrake;

        if (Input.GetKey(KeyCode.A))
            transform.Rotate(Ship.RotationSpeed * Time.deltaTime * Vector3.forward, Space.Self);
        if (Input.GetKey(KeyCode.D))
            transform.Rotate(Ship.RotationSpeed * Time.deltaTime * Vector3.back, Space.Self);

        Ship.EnginePower += Input.mouseScrollDelta.y / 4f;

        if (Input.GetKey(KeyCode.Mouse0))
            transform.Rotate(Ship.RotationSpeed * Time.deltaTime * PlayerMark.MouseRotation, Space.Self);

        target.Dir = GameCore.MainCamera.ScreenToWorldPoint(Input.mousePosition + Vector3.forward) -
                     GameCore.MainCamera.transform.position;
        

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
            Ship.Brake(true);
            autoBrake = false;
        }

        if (autoBrake)
            Ship.AutoBrake();

        if (Input.GetKeyDown(KeyCode.Tab))
            AutoBrake = !AutoBrake;
        
        if (Input.GetKey(KeyCode.Alpha1))
            Ship.LookAt(RB.position + RB.velocity);
        if (Input.GetKey(KeyCode.Alpha2))
            Ship.LookAt(RB.position - RB.velocity);
        if (Input.GetKey(KeyCode.Alpha3))
            Ship.LookAt(Vector3.zero);

        if (Input.GetKeyDown(KeyCode.T))
            Detonate();

        if (Input.GetKeyDown(KeyCode.Q))
            Ship.RegenShieldFromEnergy();
    }

    public override void Warn(Vector3 moveTo, Ship.Warning how)
    {
        Mark.Cameroid.localPosition = Random.insideUnitSphere * how.shakePower * (1 - Mark.sniperness);
        if (how.showText)
            if (Warner.Self)
                Warner.Self.Show();
    }
}