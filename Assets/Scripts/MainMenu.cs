using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject campaignPanel;
    public GameObject levelsPanel;
    public GameObject settingsPanel;

    public Slider Sounds;
    public Slider Music;


    private void Start()
    {
        GoToMainMenu();
    }
    
    
    
    public void GoToMainMenu()
    {
        mainPanel.SetActive(true);
        settingsPanel.SetActive(false);
        levelsPanel.SetActive(false);
        campaignPanel.SetActive(false);
    }
    
    public void OnCampaignButtonClick()
    {
        mainPanel.SetActive(false);
        campaignPanel.SetActive(true);
    }
    public void OnLevelsButtonClick()
    {
        mainPanel.SetActive(false);
        levelsPanel.SetActive(true);
    }
    public void OnSettingsButtonClick()
    {
        mainPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }
    public void OnQuitButtonClick()
    {
        Application.Quit();
    }
    
    void Update()
    {
        
    }
}