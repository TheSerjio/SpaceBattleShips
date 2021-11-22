using UnityEngine;

[ExecuteAlways]
[DisallowMultipleComponent]
public sealed class EditorLooker : MonoBehaviour
{
    public Camera cam;

    public RectTransform t;

    public float size;

    public void LateUpdate()
    {
        t.sizeDelta = new Plane(cam.transform.forward, cam.transform.position).GetDistanceToPoint(transform.position) * size * Vector2.one;
        transform.LookAt(transform.position  -cam.transform.forward);
        var e = transform.eulerAngles;
        e.z = -cam.transform.eulerAngles.z;
        transform.eulerAngles = e;
    }
}