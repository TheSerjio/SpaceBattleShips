using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public sealed class GameUI : MonoBehaviour
{
    public static GameUI It;
    public Ship target;
    public Slider Engines;
    public Slider Brakes;
    public RectTransform Shields;
    public RectTransform Power;
    public RectTransform Health;
    public RectTransform Velocity;
    public RectTransform CVelocity;


    public void Awake()
    {
        if (It)
            Debug.Log($"Duplication! [{name}] and [{It.name}]");
        else
            It = this;
    }

    public void Update()
    {
        if (target)
        {
            Shields.localScale = new Vector3(target.Shield / target.MaxShield, 1, 1);
            Power.localScale = new Vector3(target.Energy / target.MaxEnergy, 1, 1);
            Health.localScale = new Vector3(target.Health / target.MaxHealth, 1, 1);
            var cam = Camera.main;
            Velocity.position = (Vector2)cam.WorldToScreenPoint(target.RB.position + target.RB.velocity);
            CVelocity.position = (Vector2)cam.WorldToScreenPoint(cam.transform.position + target.RB.velocity);
        }
    }
}