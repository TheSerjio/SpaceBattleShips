using UnityEngine;

public class PlayerMark : SINGLETON<PlayerMark>
{
    public Ship Ship { get; private set; }

    public Transform Cameroid { get; private set; }

    private Transform defaultCam;
    private Transform sniperCam;

    private Camera Cam;

    private float DefaultCameraAngle;
    
    private const float SlowCamera = 180;
    private const float FastCamera = SlowCamera * 3;

    public float sniperness { get; private set; }

    private bool isSniper;

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
            foreach (var pfc in GetComponentsInChildren<PlaceForCamera>())
                switch (pfc.type)
                {
                    case PlaceForCamera.Type.Default:
                        defaultCam = pfc.transform;
                        break;
                    case PlaceForCamera.Type.Parent:
                        Cameroid = pfc.transform;
                        break;
                    case PlaceForCamera.Type.Sniper:
                        sniperCam = pfc.transform;
                        break;
                    default:
                        Debug.LogError(pfc.type);
                        break;
                }

            if (!sniperCam)
                sniperCam = defaultCam;
            
            GameCore.MainCamera.gameObject.SetActive(false);
            var obj = Instantiate(DataBase.Get().CameraPrefab, Cameroid);
            Cam = obj.GetComponent<Camera>();
            DefaultCameraAngle = Cam.fieldOfView;
            GameCore.MainCamera = Cam;
            var t = obj.transform;
            t.localPosition = defaultCam.localPosition;
            t.localEulerAngles = Vector3.zero;
            t.localScale = Vector3.one;
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
        Ship.frame.gameObject.SetActive(false);
        AudioManager.Self.SetPlayer(Ship);
        TargetFrame.PlayerDistance = Ship.mainWeapon.FrameDistance;
    }

    public void SwitchPlayer()
    {
        Ship = GetComponent<Ship>();
        if (GetComponent<PlayerShip>())
        {
            if (LevelManager.CanExitFromShip)
            {
                GameUI.Self.ShowHide(true);
                DestroyImmediate(GetComponent<PlayerShip>());
                GetComponent<ShipController>().enabled = true;
            }
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
            Cameroid.Rotate((1 - sniperness) * FastCamera * Time.deltaTime * MouseRotation / power);
        }

        var r = Cameroid.localRotation;
        r.SetLookRotation(Vector3.forward);
        Cameroid.localRotation = Quaternion.RotateTowards(Cameroid.localRotation, r, SlowCamera * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Return))
            SwitchPlayer();
        if (Input.GetKeyDown(KeyCode.Tilde) && LevelManager.CanExitFromShip)
        {
            if (GetComponent<PlayerShip>())
                SwitchPlayer();
            IfDie(false);
            Destroy(this);
            return;
        }

        if (Input.GetKeyDown(KeyCode.E))
            isSniper = !isSniper;

        sniperness = Mathf.MoveTowards(sniperness, isSniper ? 1 : 0, Time.deltaTime);

        {
            Cam.transform.localPosition = Vector3.Slerp(defaultCam.localPosition, sniperCam.localPosition, sniperness);
        }
        Cam.fieldOfView = Mathf.Lerp(DefaultCameraAngle, Ship.SniperCameraAngle, sniperness);

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

    public void IfDie(bool exploded)
    {
        GameUI.Self.ShowHide(false);
        Spectator.Self.ComeHere(Cam.transform.position, Cam.transform.rotation, Ship.ExplosionSize);
        Destroy(Cam.gameObject);
        Ship.PlayerMarked = false;
        Ship.frame.gameObject.SetActive(true);
        if (exploded)
            AudioManager.PlaySound(DataBase.Get().PlayerDeath, false);
        if (AudioManager.Self.Player == Ship)
            AudioManager.Self.SetPlayer(null);
    }
}