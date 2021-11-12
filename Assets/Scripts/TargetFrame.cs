using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(RectTransform))]
public class TargetFrame : MonoBehaviour
{
    public Ship target;
    public ulong number;
    RectTransform rect;
    [SerializeField] GameObject image;
    public TMPro.TextMeshProUGUI text;
    float timeLeft;
    [SerializeField] GameObject onHit;

    public UnityEngine.UI.Graphic[] all;

    public float MaxVisionDistance;

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
                foreach (var q in all)
                {
                    var c = q.color;
                    c.a = Mathf.Lerp(1, 0, dist / MaxVisionDistance);//TODO optimization
                    q.color = c;
                }
                if (!image.activeSelf)
                {
                    image.SetActive(true);
                    text.enabled = true;
                }
                Vector2 pos = cam.WorldToScreenPoint(target.transform.position);
                rect.position = pos;
                text.text = $"{target.name}{number}:{Mathf.RoundToInt(target.RelativeEnergy * 100)}:{Mathf.Round(Utils.ToSadUnits(dist))}";
                if (timeLeft < 0)
                {
                    if(onHit.activeSelf)
                    onHit.SetActive(false);
                }
                else
                    timeLeft -= Time.deltaTime;
            }
            else if (image.activeSelf)
            {
                image.SetActive(false);
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