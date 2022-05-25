using UnityEngine;
using UnityEngine.UI;

public sealed class ShipWithCountButton : MonoBehaviour
{
    public MotherShip.Data data;
    public Image image;
    public Text countText;

    public void Update()
    {
        if (data == null || data.count == 0)
        {
            image.color = default;
            countText.text = "";
        }
        else
        {
            image.color = Color.white;
            image.sprite = data.ship.Preview;
            countText.text = data.count.ToString();
        }
    }
}