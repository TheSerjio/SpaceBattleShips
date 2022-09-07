using UnityEngine.UI;
using UnityEngine;

public class MotherShipControlPanelButton : MonoBehaviour
{
    public MotherShip.Data data;

    public Image icon;

    public Text text;

    public void Update()
    {
        icon.sprite = data.ship.Preview; 
        text.text = data.count.ToString();
    }
}