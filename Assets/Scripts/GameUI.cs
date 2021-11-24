using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
[RequireComponent(typeof(Canvas))]
public sealed class GameUI : SINGLETON<GameUI>
{
    [SerializeField] private GameObject Stats;
    public Slider Engines;
    public RectTransform Shields;
    public Image I_Shields;
    public RectTransform Power;
    public Image I_Power;
    public RectTransform Health;
    public RectTransform Velocity;
    public Text VelocityText;
    private Canvas canva;
    public Text ShipCount;
    public Text SliderValue;
    public RectTransform ForwardAim;
    public Canvas WorlsCanvas;
    public Color C_Shield;
    public Color C_Energy;
    public Color C_Red;

    public void ShowHide(bool showStats, bool showEngineSlider)
    {
        Stats.SetActive(showStats);
        Engines.gameObject.SetActive(showEngineSlider);
    }

    protected override void OnAwake()
    {
        canva = GetComponent<Canvas>();
    }

    public void Update()
    {
        if (!canva.worldCamera)
            canva.worldCamera = GameCore.MainCamera;
        SliderValue.text = Engines.value.ToString();
        WorlsCanvas.worldCamera = GameCore.MainCamera;
    }
}