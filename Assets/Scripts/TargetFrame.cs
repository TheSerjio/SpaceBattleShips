using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(RectTransform))]
public class TargetFrame : MonoBehaviour
{
    public Ship target;
    public ulong number;
    RectTransform rect;
    [SerializeField] GameObject image;
    static Ship player;
    [SerializeField] TMPro.TextMeshProUGUI text;
    float timeLeft;
    [SerializeField] GameObject onHit;

    public UnityEngine.UI.Graphic[] all;

    public float MaxVisionDistance;

    public void Start()
    {
        if (!player)
        {
            var obj = FindObjectOfType<PlayerShip>();
            if (obj)
                player = obj.Ship;
        }
        rect = GetComponent<RectTransform>();
    }

    public void Update()
    {
        if (target)
        {
            var cam = GameCore.MainCamera;
            if (!cam)
            {
                target = null;
                return;
            }
            if (Vector3.Dot(cam.transform.forward, cam.transform.position - target.transform.position) < 0)
            {
                var dist = Vector3.Distance(target.transform.position, cam.transform.position);
                foreach (var q in all)
                {
                    var c= q.color;
                    c.a = Mathf.Lerp(1, 0, dist / MaxVisionDistance);//TODO optimization
                    q.color = c;
                }
                image.SetActive(true);
                text.enabled = true;
                Vector2 pos = cam.WorldToScreenPoint(target.transform.position);
                rect.position = pos;
                if (player)
                    text.text = $"{target.name}{number}:{Mathf.Round(Utils.ToSadUnits(Vector3.Distance(player.transform.position, target.transform.position)))}";
                if (timeLeft < 0)
                    onHit.SetActive(false);
                else
                    timeLeft -= Time.deltaTime;
            }
            else
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
            if (s == player)
                onHit.SetActive(true);
    }
}