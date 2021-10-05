using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public sealed class GameUI : SINGLETON<GameUI>
{
    private Ship target;
    public Slider Engines;
    public Slider Brakes;
    public RectTransform Shields;
    public RectTransform Power;
    public RectTransform Health;
    public RectTransform Velocity;
    public RectTransform CVelocity;
    public Text VelocityText;

    public void Update()
    {
        if (target)
        {
            if (target.Shield)
                Shields.localScale = new Vector3(target.Shield.Relative, 1, 1);
            Power.localScale = new Vector3(target.RelativeEnergy, 1, 1);
            Health.localScale = new Vector3(target.RelativeHealth, 1, 1);
            VelocityText.text = Mathf.RoundToInt(Utils.ToSadUnits(target.RB)).ToString();
            var cam = Camera.main;
            if (cam)
            {
                Velocity.position = (Vector2)cam.WorldToScreenPoint(target.RB.position + target.RB.velocity);
                CVelocity.gameObject.SetActive(target.RB.velocity.sqrMagnitude > 0.1f);
                CVelocity.position = (Vector2)cam.WorldToScreenPoint(cam.transform.position + target.RB.velocity);
            }
        }
        else
        {
            var tar = FindObjectOfType<PlayerShip>();
            if (tar)
                target = tar.Ship;
        }
    }
}