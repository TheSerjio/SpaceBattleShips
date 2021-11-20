using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(RectTransform))]
public class TargetFrame : MonoBehaviour
{
    public static float PlayerAntiBulletSpeed;
    public static Rigidbody Player;

    public Ship target;
    public string Name;

    [SerializeField] RectTransform image;
    public TMPro.TextMeshProUGUI text;
    float timeLeft;
    [SerializeField] GameObject onHit;
    [SerializeField] RectTransform probablyPosition;
    [SerializeField] UnityEngine.UI.Image probabPos;

    const float minFrameSize = 18;
    const float maxFrameSize = 36;
    const float maxDist = 300;
    const float maxTextDist = 200;

    private void SetScale(RectTransform what, float size)
    {
        what.sizeDelta = Vector2.one * size * new Plane(GameCore.MainCamera.transform.forward, GameCore.MainCamera.transform.position).GetDistanceToPoint(what.position);
    }

    public void Update()
    {
        if (target)
        {
            var cam = GameCore.MainCamera;
            if (!cam)
                return;
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

                    c = probabPos.color;
                    c.a = Mathf.Lerp(1, 0, dist / maxTextDist);
                    probabPos.color = c;
                }

                image.sizeDelta = Vector2.one * Mathf.Lerp(maxFrameSize, minFrameSize, dist / maxDist);
                if (Player)
                {
                    if (!probablyPosition.gameObject.activeSelf)
                        probablyPosition.gameObject.SetActive(true);
                    probablyPosition.position = cam.WorldToScreenPoint(Utils.ShootTo(Player, target.RB, PlayerAntiBulletSpeed));
                    SetScale(probablyPosition, 0.1f);
                }
                else if (probablyPosition.gameObject.activeSelf)
                    probablyPosition.gameObject.SetActive(false);

                transform.position = target.transform.position;
                SetScale(image, 0.1f);
                transform.LookAt(transform.position + cam.transform.forward);
                var e = transform.eulerAngles;
                e.z = cam.transform.eulerAngles.z;
                transform.eulerAngles = e;

                //text.text = $"{Name}:{Mathf.RoundToInt(target.RelativeEnergy * 100)}:{Mathf.Round(Utils.ToSadUnits(dist))}";
                text.text = $"{Name}:{Mathf.RoundToInt(target.RelativeEnergy * 100)}";
                text.fontSize = Vector3.Distance(transform.position, cam.transform.position) / 20f;


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

    public void OnHit()
    {
        timeLeft = 0.25f;
        onHit.SetActive(true);
    }
}