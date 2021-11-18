using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(RectTransform))]
public class TargetFrame : MonoBehaviour
{
    public static float PlayerAntiBulletSpeed;
    public static Rigidbody Player;

    public Ship target;
    public string Name;
    RectTransform rect;
    [SerializeField] RectTransform image;
    public TMPro.TextMeshProUGUI text;
    float timeLeft;
    [SerializeField] GameObject onHit;
    [SerializeField] RectTransform probablyPosition;

    const float minFrameSize = 18;
    const float maxFrameSize = 36;
    const float maxDist = 300;
    const float maxTextDist = 200;

    public void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    public void Update()
    {
        if (target)
        {
            var cam = GameCore.MainCamera;
            if (Vector3.Dot(cam.transform.forward, cam.transform.position - target.transform.position) < 0)
            {
                var dist = Vector3.Distance(target.transform.position, cam.transform.position);
                if (!image.gameObject.activeSelf)
                {
                    image.gameObject.SetActive(true);
                    probablyPosition.gameObject.SetActive(true);
                    text.enabled = true;
                }
                {
                    //this somehow sets color of text
                    var c = text.color;
                    c.a = Mathf.Lerp(1, 0, dist / maxTextDist);
                    text.color = c;
                }

                image.sizeDelta = Vector2.one * Mathf.Lerp(maxFrameSize, minFrameSize, dist / maxDist);
                if (Player)
                {
                    if (!probablyPosition.gameObject.activeSelf)
                        probablyPosition.gameObject.SetActive(true);
                    probablyPosition.position = (Vector2)cam.WorldToScreenPoint(Utils.ShootTo(Player, target.RB, PlayerAntiBulletSpeed));
                }
                else if (probablyPosition.gameObject.activeSelf)
                    probablyPosition.gameObject.SetActive(false);

                rect.position = (Vector2)cam.WorldToScreenPoint(target.transform.position);
                text.text = $"{Name}:{Mathf.RoundToInt(target.RelativeEnergy * 100)}:{Mathf.Round(Utils.ToSadUnits(dist))}";
                if (timeLeft < 0)
                {
                    if (onHit.activeSelf)
                        onHit.SetActive(false);
                }
                else
                    timeLeft -= Time.deltaTime;
            }
            else if (image.gameObject.activeSelf)
            {
                image.gameObject.SetActive(false);
                probablyPosition.gameObject.SetActive(false);
                text.enabled = false;
            }
        }
        else
        {
            onHit.transform.SetParent(transform.parent);
            onHit.SetActive(true);
            Destroy(onHit, 1);
            Destroy(gameObject);
        }
    }

    public void OnHit(BaseEntity from)
    {
        timeLeft = 0.25f;
        if (from is Ship s)
            if (s.UseCheats)
                onHit.SetActive(true);
    }
}