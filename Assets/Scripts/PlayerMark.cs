using UnityEngine;

public class PlayerMark : SINGLETON<PlayerMark>
{
    private Ship Ship;
    public Transform Cameroid { get; private set; }

    private const float SlowCamera = 180;
    private const float FastCamera = SlowCamera * 3;


    public static Vector2 MouseRotation
    {
        get
        {
            Vector2 tar = ((Input.mousePosition / new Vector2(Screen.width, Screen.height)) - new Vector2(0.5f, 0.5f)) * 2;
            return new Vector2(Mathf.Clamp(-tar.y, -1, 1), Mathf.Clamp(tar.x, -1, 1));
        }
    }

    public void Start()
    {
        GameUI.Self.ShowHide(true);
        Ship = GetComponent<Ship>();
        Ship.PlayerMarked = true;
        {
            Transform q = null;
            foreach (var pfc in GetComponentsInChildren<PlaceForCamera>())
                if (pfc.parent)
                    Cameroid = pfc.transform;
                else
                    q = pfc.transform;

            GameCore.MainCamera.gameObject.SetActive(false);
            var cam = Instantiate(DataBase.Get().CameraPrefab, q);
            GameCore.MainCamera = cam.GetComponent<Camera>();
            cam.transform.localPosition = Vector3.zero;
            cam.transform.localEulerAngles = Vector3.zero;
            cam.transform.localScale = Vector3.one;
        }

        Instantiate(DataBase.Get().DustPrefab, transform).GetComponent<DustNearPlayer>().Init(this, Ship);

        foreach(var q in Ship.GetComponentsInChildren<ShipLineTrail>())
        {
            q.gameObject.SetActive(false);
            var trail = Instantiate(DataBase.Get().BetterTrailPrefab,Ship.transform);
            if (q.useCustomGradient)
                trail.GetComponentInChildren<UnityEngine.VFX.VisualEffect>().SetGradient("Color", q.effectGradient);
            trail.transform.localPosition = q.transform.localPosition;
            trail.transform.localScale = 2 * q.Line.startWidth * Vector3.one;
            trail.AddComponent<ShipParticleTrail>();
        }
        Ship.FindTrails();
    }

    public void SwitchPlayer()
    {
        Ship = GetComponent<Ship>();
        if (GetComponent<PlayerShip>())
        {
            GameUI.Self.ShowHide(true);
            DestroyImmediate(GetComponent<PlayerShip>());
            GetComponent<ShipController>().enabled = true;
        }
        else
        {
            GameUI.Self.ShowHide(true);
            GetComponent<ShipController>().enabled = false;
            Ship.Brain = gameObject.AddComponent<PlayerShip>();
        }
    }

    public void Update()
    {
        TargetFrame.Player = Ship.RB;
        TargetFrame.PlayerAntiBulletSpeed = Ship.mainWeapon ? Ship.mainWeapon.AntiSpeed : 1;

        var cam = GameCore.MainCamera;

        Cameroid.localPosition = Vector3.MoveTowards(Cameroid.localPosition, Vector3.zero, Time.deltaTime);

        if (Self != this)
            Destroy(this);

        if (Input.GetKey(KeyCode.Mouse2))
        {
            var power = Vector3.Distance(transform.forward, Cameroid.forward) + 1;
            Cameroid.Rotate(FastCamera * Time.deltaTime * MouseRotation / power);
        }

        var r = Cameroid.localRotation;
        r.SetLookRotation(Vector3.forward);
        Cameroid.localRotation = Quaternion.RotateTowards(Cameroid.localRotation, r, SlowCamera * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Return))
            SwitchPlayer();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GetComponent<PlayerShip>())
                SwitchPlayer();
            IfDie();
            Destroy(Cameroid.GetChild(0).GetChild(0).gameObject);
            Ship.PlayerMarked = false;
            Destroy(this);
            return;
        }

        //update ui
        //yes, its here
        {
            var RB = Ship.RB;
            var ui = GameUI.Self;
            var q = Mathf.Sin(Time.time * 20) / 2 + 0.5f;
            if (Ship.Shield)
            {
                ui.Shields.localScale = new Vector3(Ship.Shield.Relative, 1, 1);
                ui.I_Shields.color = Ship.Shield.HasShield ? ui.C_Shield : Color.Lerp(ui.C_Shield, ui.C_Red, q);
            }
            else
                ui.Shields.localScale = Vector3.zero;
            var rel = Ship.RelativeEnergy;
            ui.Power.localScale = new Vector3(rel, 1, 1);
            ui.I_Power.color = Ship.TakeEnergy(0) ? ui.C_Energy : Color.Lerp(ui.C_Energy, ui.C_Red, q);

            ui.Health.localScale = new Vector3(Ship.RelativeHealth, 1, 1);
            ui.VelocityText.text = Mathf.RoundToInt(Utils.ToSadUnits(RB)).ToString();
            ui.Velocity.gameObject.SetActive(RB.velocity.sqrMagnitude > 0.1f);
            ui.Velocity.position = (Vector2)cam.WorldToScreenPoint(cam.transform.position + RB.velocity);
            ui.ForwardAim.position = (Vector2)cam.WorldToScreenPoint(cam.transform.position + transform.forward);
            ui.Engines.value = Ship.EnginePower;
        }
    }

    public void IfDie()
    {
        GameUI.Self.ShowHide(false);
        var c = Cameroid.GetChild(0).GetChild(0);
        Spectator.Self.ComeHere(c.position, c.rotation);
    }
}