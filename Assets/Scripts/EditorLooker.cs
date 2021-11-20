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
        t.sizeDelta = Vector2.one * size * new Plane(cam.transform.forward, cam.transform.position).GetDistanceToPoint(transform.position);
        transform.LookAt(transform.position  -cam.transform.forward);
        var e = transform.eulerAngles;
        e.z = -cam.transform.eulerAngles.z;
        transform.eulerAngles = e;
    }
}