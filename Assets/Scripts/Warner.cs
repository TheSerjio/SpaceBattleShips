using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// component of warning window
/// </summary>
[RequireComponent(typeof(Graphic))]
[DisallowMultipleComponent]
public class Warner : SINGLETON<Warner>
{
    public Graphic[] images;

    public Gradient color;

    public Color current;

    public void Update()
    {
        current = Vector4.MoveTowards(current, new Color(0, 0, 0, 0), Time.deltaTime);

        foreach (var q in images)
            q.color = current;
    }

    public void Show()
    {
        current = color.Evaluate(Time.time % 1);
    }
}