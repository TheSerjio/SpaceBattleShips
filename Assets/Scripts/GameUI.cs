using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
[RequireComponent(typeof(Canvas))]
public sealed class GameUI : SINGLETON<GameUI>
{
    public Slider Engines;
    public Slider Brakes;
    public RectTransform Shields;
    public RectTransform Power;
    public RectTransform Health;
    public RectTransform Velocity;
    public Text VelocityText;
    private Canvas canva;

    protected override void OnAwake()
    {
        canva = GetComponent<Canvas>();
    }

    public void Update()
    {
        if (!canva.worldCamera)
            canva.worldCamera = GameCore.MainCamera;
    }
}