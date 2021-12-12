using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngineInternal;

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
    public GameObject Menu;
    public Text FPS;
    private float fps;

    public void Start()
    {
        ShowHide(false);
    }


    public void ShowHide(bool show)
    {
        Stats.SetActive(show);
    }

    protected override void OnSingletonAwake()
    {
        canva = GetComponent<Canvas>();
    }

    public void Update()
    {
        if (!canva.worldCamera)
            canva.worldCamera = GameCore.MainCamera;
        SliderValue.text = Engines.value.ToString();
        WorlsCanvas.worldCamera = GameCore.MainCamera;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Menu.SetActive(!Menu.activeSelf);
            Time.timeScale = Menu.activeSelf ? 0 : 1;
        }

        var realFPS = 1f / Time.deltaTime;

        var q = Time.unscaledDeltaTime;

        fps = (fps + realFPS * q) / (1 + q);

        FPS.text = Mathf.RoundToInt(fps).ToString();
    }
    
    public void ButtonGoToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}