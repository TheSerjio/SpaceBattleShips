using UnityEngine;

public class PlayerMark : SINGLETON<PlayerMark>
{
    private Ship Ship;
    public Transform Cameroid { get; set; }

    const float SlowCamera = 180;
    const float FastCamera = SlowCamera * 3;


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
        Ship = GetComponent<Ship>();

        Transform q = null;
        foreach (var pfc in GetComponentsInChildren<PlaceForCamera>())
            if (pfc.parent)
                Cameroid = pfc.transform;
            else
                q = pfc.transform;

        GameCore.MainCamera.gameObject.SetActive(false);
        var cam = Instantiate(DataBase.Get().CameraPrefab);
        GameCore.MainCamera = cam.GetComponent<Camera>();
        cam.transform.SetParent(q);
        cam.transform.localPosition = Vector3.zero;
        cam.transform.localEulerAngles = Vector3.zero;
        cam.transform.localScale = Vector3.one;
        Instantiate(DataBase.Get().DustPrefab, transform);
    }

    public void Update()
    {
        TargetFrame.Player = Ship.RB;
        TargetFrame.PlayerAntiBulletSpeed = Ship.mainWeapon ? Ship.mainWeapon.AntiSpeed : 1;

        var cam = GameCore.MainCamera;

        Cameroid.localPosition = Vector3.MoveTowards(Cameroid.localPosition, Vector3.zero, Time.deltaTime);

        if (Self != this)
            Destroy(this);

        if (Input.GetMouseButton(1))
        {
            var power = Vector3.Distance(transform.forward, Cameroid.forward) + 1;
            Cameroid.Rotate(FastCamera * Time.deltaTime * MouseRotation / power);
        }
        var r = Cameroid.localRotation;
        r.SetLookRotation(Vector3.forward);
        Cameroid.localRotation = Quaternion.RotateTowards(Cameroid.localRotation, r, SlowCamera * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (GetComponent<PlayerShip>())
            {
                DestroyImmediate(GetComponent<PlayerShip>());
                GetComponent<ShipController>().enabled = true;
            }
            else
            {
                GetComponent<ShipController>().enabled = false;
                gameObject.AddComponent<PlayerShip>();
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            IfDie();
            Destroy(Cameroid.GetChild(0).GetChild(0).gameObject);
            Destroy(this);
        }

        //update ui
        {
            var RB = Ship.RB;
            var ui = GameUI.Self;
            if (Ship.Shield)
                ui.Shields.localScale = new Vector3(Ship.Shield.Relative, 1, 1);
            var rel = Ship.RelativeEnergy;
            ui.Power.localScale = new Vector3(rel, 1, 1);
            var f = Mathf.Lerp(Mathf.Sin(Time.time * Mathf.PI * 4) / 2 + 0.5f, 1, Mathf.Sqrt(rel));
            ui.PowerImage.color = new Color(f, f, f, 1);
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
        var c = Cameroid.GetChild(0).GetChild(0);
        Spectator.Self.ComeHere(c.position, c.rotation);
    }
}