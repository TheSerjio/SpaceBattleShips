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
    public RectTransform levelPanel, campaignPanel;
    public GameObject[] panels;
    public Text PlayerCampaignMoneyText;
    private Level currentLevel;
    private int PlayerMoney_C;
    private System.Collections.Generic.List<MotherShip.Data> PlayerShips_C;

    public GameObject[] shipSelectionButtons;

    public GameObject PlayerCampaignIconsParent, EnemyCampaignIconsParent;
    
    public ShipWithCountButton[] PlayerCampaignIcons, EnemyCampaignIcons;

    public void OnValidate()
    {
        PlayerCampaignIcons = PlayerCampaignIconsParent.GetComponentsInChildren<ShipWithCountButton>();
        EnemyCampaignIcons = EnemyCampaignIconsParent.GetComponentsInChildren<ShipWithCountButton>();
    }

    private void CreateLevels(RectTransform panel, bool campaign, int count)
    {
        var levels = DataBase.Get().Levels;
        int y = 0;
        int i = 0;
        foreach (var level in levels)
        {
            if (level.IsCampaignLevel != campaign)
                continue;
            if (i++ > count)
                continue;
            var obj = Instantiate(levelButtonPrefab, panel);
            obj.GetComponent<RectTransform>().anchoredPosition = Vector2.down * ((y++) * 64 + 64);
            obj.GetComponentInChildren<Text>().text = level.Name;
            obj.GetComponent<Button>().onClick.AddListener(() => OnLevelClick(level));
        }
    }

    public void Start()
    {
        SoundSettingHandler.CheckInit();
        OpenPanel(mainPanel);
        CreateLevels(levelPanel, false, int.MaxValue);
        CreateLevels(campaignPanel, true, FileSystem.GetCampaign().Count);
        foreach (var q in shipSelectionButtons)
            q.SetActive(false);

        Cursor.SetCursor(DataBase.Get().MenuCursor, Vector2.zero, CursorMode.Auto);
    }

    private void OnLevelClick(Level level)
    {
        currentLevel = level;

        if (level.IsCampaignLevel)
        {
            var file = FileSystem.GetCampaign();
            PlayerShips_C = new System.Collections.Generic.List<MotherShip.Data>();
            if (level.previous == null)
            {
                PlayerMoney_C = 1000;
            }
            else
            {
                var q = file[level.previous.BuildingIndex];
                PlayerShips_C = q.Item1;
                PlayerMoney_C = q.Item2;
            }
            for (int i = 0; i < PlayerCampaignIcons.Length; i++)
            {
                var icon = PlayerCampaignIcons[i];
                if (i < PlayerShips_C.Count)
                    icon.data = PlayerShips_C[i];
                else
                    icon.data = null;
            }
            var enemyes = level.enemyes;
            for(int i = 0; i < EnemyCampaignIcons.Length; i++)
            {
                var icon = EnemyCampaignIcons[i];
                if (i < enemyes.Length)
                    icon.data = enemyes[i];
                else
                    icon.data = null;
            }
        }
        else
        {
            var stars = FileSystem.GetLevels().TryGetValue(level.BuildingIndex, out var some) ? some : System.Array.Empty<StarType>();
            for (var i = 0; i < shipSelectionButtons.Length; i++)
            {
                if (level.ships.Length > i)
                {
                    var button = shipSelectionButtons[i];
                    button.SetActive(true);
                    var ship = level.ships[i];
                    var it = button.GetComponent<ShipSelectButton>();
                    it.ship.sprite = ship.Preview;
                    it.star.sprite = DataBase.Get().StarColor(stars.Length > i ? stars[i] : StarType.No);
                    button.GetComponent<Button>().onClick.AddListener(() => OnShipSelectClick(ship));
                }
                else
                    shipSelectionButtons[i].SetActive(false);
            }
        }
    }

    private void OnShipSelectClick(ShipData what)
    {
        LevelManager.startedWith = new[] { what };
        LoadLevel();
    }

    public void LoadLevel()
    {
        if (currentLevel)
        {
            LevelManager.currentLevel = currentLevel;
            SceneManager.LoadScene(currentLevel.BuildingIndex);
        }
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
        PlayerCampaignMoneyText.text = PlayerMoney_C.ToString();
    }
}