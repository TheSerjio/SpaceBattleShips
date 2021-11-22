using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
[RequireComponent(typeof(RectTransform))]
public class TargetFrame : MonoBehaviour
{
    public static float PlayerAntiBulletSpeed;
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

    const float maxTextDist = 200;

    private void SetScale(RectTransform what, float size)
    {
        what.sizeDelta = new Plane(GameCore.MainCamera.transform.forward, GameCore.MainCamera.transform.position).GetDistanceToPoint(what.position) * size * Vector2.one;
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
                c.a = Mathf.Lerp(1, 0, dist / maxTextDist);
                text.color = c;

                c = probabPos.color;
                c.a = Mathf.Lerp(1, 0, dist / maxTextDist);
                probabPos.color = c;
            }

            transform.position = target.transform.position;
            SetScale(GetComponent<RectTransform>(), 0.1f);
            transform.LookAt(transform.position + cam.forward);
            var e = transform.eulerAngles;
            e.z = cam.eulerAngles.z;
            transform.eulerAngles = e;

            //text.text = $"{Name}:{Mathf.RoundToInt(target.RelativeEnergy * 100)}:{Mathf.Round(Utils.ToSadUnits(dist))}";
            text.text = $"{Name}:{Mathf.RoundToInt(target.RelativeEnergy * 100)}";
            text.fontSize = Vector3.Distance(transform.position, cam.position) / 30f;

            SetScale(image, Mathf.Lerp(0.25f, 0.05f, dist / 350));

            frameImage.pixelsPerUnitMultiplier = 500f / Vector3.Distance(transform.position, cam.position);

            if (Player)
            {
                if (!probablyPosition.gameObject.activeSelf)
                    probablyPosition.gameObject.SetActive(true);
                probablyPosition.position = Utils.ShootTo(Player, target.RB, PlayerAntiBulletSpeed);
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
}