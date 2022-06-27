using UnityEngine;
using UnityEngine.UI;

public class BuyShipPanel : Script
{
    public Image icon;
    public ShipData ship;
    public Text text;
    public Slider slider;
    private int Number => (int)slider.value;
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
        if (!ship)
            return;
        var c = Number * ship.cost;

        if (MainMenu.PlayerMoney_C >= c)
        {
            MainMenu.PlayerMoney_C -= c;
            foreach (var data in MainMenu.PlayerShips_C)
                if (data.ship == ship)
                {
                    data.count += Number;
                    goto End;
                }
            MainMenu.PlayerShips_C.Add(new MotherShip.Data() { ship = ship, count = Number });
            End:;
            MainMenu.Self.UpdatePlayerShipIcons();
        }
    }

    public void Update()
    {
        if (ship)
            text.text = $"{ship.Name} x{Number}\n{Number * ship.cost}$";
        else
            text.text = "Select ship";
        if (Input.GetKeyDown(KeyCode.Escape))
            gameObject.SetActive(false);
    }
}