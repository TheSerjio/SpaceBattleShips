using UnityEngine;
using UnityEngine.UI;

public class BuyShipPanel : Script
{
    public Image icon;
    public ShipData ship;
    public Text text;
    public Slider slider;

    protected override void OnAwake() { }

    public void ShipSelect(ShipData data)
    {
        ship = data;
        icon.sprite = data ? data.Preview : null;
    }

    public void OnEnable()
    {
        ShipSelect(null);
    }

    public void Buy()
    {
        gameObject.SetActive(false);
    }

    public void Update()
    {
        if (ship)
            text.text = $"{ship.Name} x{slider.value}\n{slider.value * ship.cost}$";
        else
            text.text = "Select ship";
        if (Input.GetKeyDown(KeyCode.Escape))
            gameObject.SetActive(false);
    }
}