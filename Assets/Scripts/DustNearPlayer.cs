using UnityEngine;
using UnityEngine.VFX;

public class DustNearPlayer : MonoBehaviour
{
    Rigidbody rb;
    PlayerMark pm;
    VisualEffect ve;

    public void Init(PlayerMark PM, Rigidbody RB)
    {
        pm = PM;
        rb = RB;
        ve = GetComponent<VisualEffect>();
    }

    public void FixedUpdate()
    {
        transform.position = rb.position + (Mathf.Sqrt(Mathf.Sqrt(rb.velocity.magnitude)) * Time.deltaTime * rb.velocity);//idk [why|how] this works
        ve.SetVector3("grav", transform.InverseTransformDirection(-rb.velocity));
        var q = rb.velocity.magnitude / (rb.velocity.magnitude + 1);
        ve.SetFloat("X scale", q / 2);
        ve.SetFloat("Y scale", q * 10);
        ve.SetFloat("capacity", Mathf.Clamp(rb.velocity.magnitude, 0, 300));
        if (!pm)
            Destroy(gameObject);
    }
}