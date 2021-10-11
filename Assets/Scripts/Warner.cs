using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// component of warning window
/// </summary>
[RequireComponent(typeof(Graphic))]
public class Warner : MonoBehaviour
{
    public Graphic text;

    private Graphic my;

    public Gradient color;

    public void Awake()
    {
        my = GetComponent<Graphic>();
    }

    public void Update()
    {
        text.color = my.color * color.Evaluate(Time.time % 1);
    }
}