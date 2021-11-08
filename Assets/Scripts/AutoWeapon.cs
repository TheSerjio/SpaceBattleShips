using UnityEngine;

public class AutoWeapon : MonoBehaviour
{
    public float rotationSpeed;
    public Transform body;
    private Rigidbody target;
    private Ship parent;
    public ShipWeapon weapon;
    [Range(-1, 1)] public float maxAngle;

    public void Update()
    {
        if (parent)
            if (target)
            {
                body.RotateTowards(Utils.ShootTo(parent.RB, target, weapon ? weapon.AntiSpeed : 0), rotationSpeed * Time.deltaTime);
                if (Vector3.Dot(transform.forward, body.forward) < maxAngle)
                    body.RotateTowards(body.position + transform.forward, Time.deltaTime * rotationSpeed * 2);
            }
            else
            {
                var tar = GameCore.Self.FindTargetShip(parent.team);
                if (tar)
                    target = tar.RB;
            }
        else
            parent = GetComponentInParent<Ship>();

    }
}