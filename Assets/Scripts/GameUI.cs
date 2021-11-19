using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
[RequireComponent(typeof(Canvas))]
public sealed class GameUI : SINGLETON<GameUI>
{
    public Slider Engines;
    public RectTransform Shields;
    public RectTransform Power;
    public Image PowerImage;
    public RectTransform Health;
    public RectTransform Velocity;
    public Text VelocityText;
    private Canvas canva;
    public Text ShipCount;
    public Text SliderValue;
    public RectTransform ForwardAim;

    protected override void OnAwake()
    {
        canva = GetComponent<Canvas>();
    }

    public void Update()
    {
        if (!canva.worldCamera)
            canva.worldCamera = GameCore.MainCamera;
        SliderValue.text = Engines.value.ToString();
    }
}