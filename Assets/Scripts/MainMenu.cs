using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Slider Sounds;
    public Slider Music;
    public AudioSource sound;
    public float soundLevel;

    public GameObject levelButtonPrefab;
    public GameObject mainPanel;
    public RectTransform levelPanel;
    public GameObject[] panels;
    private Level currentLevel;

    public GameObject[] shipSelectionButtons;

    private void Start()
    {
        SoundSettingHandler.CheckInit();
        OpenPanel(mainPanel);
        var levels = DataBase.Get().Levels;
        for (var i = 0; i < levels.Length; i++)
        {
            var level = levels[i];
            var obj = Instantiate(levelButtonPrefab, levelPanel);
            obj.GetComponent<RectTransform>().anchoredPosition = Vector2.down * (i * 64 + 64);
            obj.GetComponentInChildren<Text>().text = level.Name;
            obj.GetComponent<Button>().onClick.AddListener(() => OnLevelClick(level));
        }

        foreach (var q in shipSelectionButtons)
            q.SetActive(false);

        Cursor.SetCursor(DataBase.Get().MenuCursor, Vector2.zero, CursorMode.Auto);
    }

    private void OnLevelClick(Level level)
    {
        currentLevel = level;
        for (var i = 0; i < shipSelectionButtons.Length; i++)
        {
            if (level.ships.Length > i)
            {
                var button = shipSelectionButtons[i];
                button.SetActive(true);
                var ship = level.ships[i];
                var it = button.GetComponent<ShipSelectButton>();
                it.ship.sprite = ship.Preview;
                it.star.enabled = false;
                button.GetComponent<Button>().onClick.AddListener(() => OnShipSelectClick(ship));
            }
            else
                shipSelectionButtons[i].SetActive(false);
        }
    }

    private void OnShipSelectClick(ShipData what)
    {
        LevelManager.type = LevelManager.Type.Level;
        LevelManager.currentLevel = currentLevel;
        LevelManager.startedWith = new[] {what};
        SceneManager.LoadScene(currentLevel.BuildingIndex);
    }

    public void OpenPanel(GameObject what)
    {
        foreach (var q in panels)
            q.SetActive(q == what);
    }
    
    public void OnQuitButtonClick()
    {
        Application.Quit();
    }

    public void Update()
    {
        sound.volume = soundLevel * AudioManager.MusicLevel;
    }
}