using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using MSCPB = MotherShipControlPanelButton;

public class MotherShipControlPanel : UnityEngine.UI.ToggleGroup
{
    public MotherShip player;

    public GameObject prefab;

    private List<MSCPB> buttons;

    public void Update()
    {
        UnityEngine.Events.UnityAction<bool> F(MSCPB m) => (bool _) => Choice(m);
        if (buttons == null)
            buttons = new List<MSCPB>();
        else while (buttons.Count < player.all.Length)
            {
                var b = Instantiate(prefab, transform);
                b.GetComponent<RectTransform>().position += Vector3.right * 160 * buttons.Count;
                var m = b.GetComponent<MSCPB>();
                m.data = player.all[buttons.Count];
                buttons.Add(m);
                var t = b.GetComponent<Toggle>();
                t.group = this;
                t.onValueChanged.AddListener(F(m));
            }
    }

    public void Choice(MSCPB b)
    {
        player.SpawningType = b.data.ship;
    }
}