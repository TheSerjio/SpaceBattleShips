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
    [SerializeField]GameObject onHit;

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
            var cam = Camera.main;
            if (!cam)
            {
                target = null;
                return;
            }
            if (Vector3.Dot(cam.transform.forward, cam.transform.position - target.transform.position) < 0)
            {
                image.SetActive(true);
                text.enabled = true;
                Vector2 pos = cam.WorldToScreenPoint(target.transform.position);
                rect.position = pos;
                text.text = $"{target.name}{number}:{Mathf.Round(Utils.ToSadUnits(Vector3.Distance(player.transform.position, target.transform.position)))}:{Mathf.RoundToInt(Utils.ToSadUnits(target.RB))}";
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
            Destroy(gameObject);
        }
    }

    public void OnHit()
    {
        timeLeft = 0.25f;
        onHit.SetActive(true);
    }
}