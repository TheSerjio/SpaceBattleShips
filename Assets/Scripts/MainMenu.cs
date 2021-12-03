using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject campaignPanel;
    public GameObject levelsPanel;
    public GameObject settingsPanel;
    
    public void OnCampaignButtonClick()
    {
        campaignPanel.SetActive(true);
    }
    public void OnLevelsButtonClick()
    {
        levelsPanel.SetActive(true);
    }
    public void OnSettingsButtonClick()
    {
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