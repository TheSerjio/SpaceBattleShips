using System;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public GameObject Menu;

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
    }

    public void ButtonGoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}