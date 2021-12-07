using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
[RequireComponent(typeof(RectTransform))]
public class TargetFrame : Script
{
    public static float PlayerAntiBulletSpeed;
    public static float PlayerDistance = 1;
    public static Rigidbody Player;

    public Ship target;
    public string Name;

    public Image frameImage;
    [SerializeField] RectTransform image;
    public TMPro.TextMeshProUGUI text;
    float timeLeft;
    [SerializeField] RectTransform onHit;
    [SerializeField] RectTransform probablyPosition;
    [SerializeField] Image probabPos;

    private static void SetScale(RectTransform what, float size)
    {
        var t = GameCore.MainCamera.transform;
        what.sizeDelta = new Plane(t.forward, t.position).GetDistanceToPoint(what.position) *
            GameCore.MainCamera.fieldOfView / 72f * size * Vector2.one;
    }

    public void Update()
    {
        //TODO review and make magic numbers const
        if (target)
        {
            var cam = GameCore.MainCamera.transform;
            if (!cam)
                return;
            var dist = Vector3.Distance(target.transform.position, cam.position);

            {
                //this sets alpha of text and circle
                var c = text.color;
                c.a = Mathf.Lerp(1, 0, dist / PlayerDistance);
                text.color = c;

                if (dist > PlayerDistance)
                    probabPos.color = new Color(1, 1, 1, PlayerDistance / dist);
                else
                    probabPos.color = Color.Lerp(Color.yellow, Color.magenta, dist / PlayerDistance);
            }

            transform.position = target.transform.position;
            SetScale(GetComponent<RectTransform>(), 0.1f);
            transform.LookAt(transform.position + cam.forward);
            var e = transform.eulerAngles;
            e.z = cam.eulerAngles.z;
            transform.eulerAngles = e;

            //text.text = $"{Name}:{Mathf.RoundToInt(target.RelativeEnergy * 100)}:{Mathf.Round(Utils.ToSadUnits(dist))}";
            text.text = $"{Name}:{Mathf.RoundToInt(target.RelativeEnergy * 100)}";
            text.fontSize = Vector3.Distance(transform.position, cam.position) * GameCore.MainCamera.fieldOfView / 2000f;

            SetScale(image, Mathf.Lerp(0.25f, 0.05f, dist / 350));

            frameImage.pixelsPerUnitMultiplier = 40000f / GameCore.MainCamera.fieldOfView / Vector3.Distance(transform.position, cam.position);

            if (Player)
            {
                if (!probablyPosition.gameObject.activeSelf)
                    probablyPosition.gameObject.SetActive(true);
                probablyPosition.position = Utils.ShootTo(Player, target.RB, PlayerAntiBulletSpeed, 3);
                SetScale(probablyPosition, 0.025f);
            }

            else if (probablyPosition.gameObject.activeSelf)
                probablyPosition.gameObject.SetActive(false);


            if (timeLeft < 0)
            {
                if (onHit.gameObject.activeSelf)
                    onHit.gameObject.SetActive(false);
            }
            else
                timeLeft -= Time.deltaTime;
        }
        else
        {
            onHit.SetParent(transform.parent);
            onHit.gameObject.SetActive(true);
            Destroy(onHit.gameObject, 1);
            Destroy(gameObject);
        }
    }

    public void OnHit()
    {
        timeLeft = 0.25f;
        onHit.gameObject.SetActive(true);
    }

    protected override void OnAwake()
    {
    }
}