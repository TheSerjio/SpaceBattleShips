using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(RectTransform))]
public class TargetFrame : MonoBehaviour
{
    public Ship target;
    public ulong number;
    RectTransform rect;
    [SerializeField] RectTransform image;
    public TMPro.TextMeshProUGUI text;
    float timeLeft;
    [SerializeField] GameObject onHit;

    const float sizeMultiply = 500;
    const float minFrameSize = 18;
    const float maxFrameSize = 256;
    const float maxDist = 300;

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
                if (dist > maxDist)
                {
                    if (image.gameObject.activeSelf)
                    {
                        image.gameObject.SetActive(false);
                        text.enabled = false;
                    }
                    return;
                }
                else if (!image.gameObject.activeSelf)
                {
                    image.gameObject.SetActive(true);
                    text.enabled = true;
                }
                {
                    //this somehow sets color of text
                    var c = text.color;
                    c.a = Mathf.Lerp(1, 0, dist / maxDist);
                    text.color = c;
                }
                image.sizeDelta = Vector2.one * Mathf.Clamp(sizeMultiply / Mathf.Sqrt(dist), minFrameSize, maxFrameSize);
                rect.position = (Vector2)cam.WorldToScreenPoint(target.transform.position);
                text.text = $"{target.name}{number}:{Mathf.RoundToInt(target.RelativeEnergy * 100)}:{Mathf.Round(Utils.ToSadUnits(dist))}";
                if (timeLeft < 0)
                {
                    if(onHit.activeSelf)
                    onHit.SetActive(false);
                }
                else
                    timeLeft -= Time.deltaTime;
            }
            else if (image.gameObject.activeSelf)
            {
                image.gameObject.SetActive(false);
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