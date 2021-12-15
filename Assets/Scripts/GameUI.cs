using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngineInternal;

[DisallowMultipleComponent]
[RequireComponent(typeof(Canvas))]
public sealed class GameUI : SINGLETON<GameUI>
{
    [SerializeField] private GameObject[] Stats;
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
    
    public bool LockPause { get; set; }

    public GameObject WinText;
    public GameObject FailText;
    public GameObject LargeGoToMainMenuButton;

    public void Start()
    {
        ShowHide(false);
    }


    public void ShowHide(bool show)
    {
        foreach (var q in Stats)
            q.SetActive(show);
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

        if(!LockPause)
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Menu.SetActive(!Menu.activeSelf);
                Time.timeScale = Menu.activeSelf ? 0 : 1;
            }

        var q = Time.unscaledDeltaTime;

        var realFPS = 1f / q;
        
        fps = (fps + realFPS * q) / (1 + q);

        FPS.text = Mathf.RoundToInt(fps).ToString();
    }
    
    public void ButtonGoToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}