using UnityEngine;

public class AutoWeapon : MonoBehaviour, IFireControl
{
    public float rotationSpeed;
    public Transform body;
    private Rigidbody target;
    private Ship parent;
    public ShipWeapon weapon;
    [Range(-1, 1)] public float maxAngle;

    bool IFireControl.Fire => target;

    Ship IFireControl.Parent => parent;

    public void Update()
    {
        if (parent)
            if (target)
            {
                if (!ReferenceEquals(weapon.Parent, this))
                    weapon.Parent = this;
                body.RotateTowards(Utils.ShootTo(parent.RB, target, weapon ? weapon.AntiSpeed : 0), rotationSpeed * Time.deltaTime);
                if (Vector3.Dot(transform.forward, body.forward) < maxAngle)
                {
                    body.RotateTowards(body.position + transform.forward, Time.deltaTime * rotationSpeed * 2);
                    target = null;
                }
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