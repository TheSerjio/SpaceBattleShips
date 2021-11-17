using UnityEngine;

public class AutoWeapon : MonoBehaviour, IFireControl
{
    public float rotationSpeed;
    public Transform body;
    private Rigidbody target;
    private Ship parent;
    public ShipWeapon weapon;
    public bool automatic;
    public bool turret;
    [Tooltip("less value -> more rotation")] [Range(-1, 1)] public float maxAngle;

    bool IFireControl.Fire => automatic ? target : parent.Fire;
    Ship IFireControl.Parent => parent;

    public void Update()
    {
        if (parent)
            if (target)
            {
                weapon.Parent = this;
                body.RotateTowards(Utils.ShootTo(parent.RB, target, weapon ? weapon.AntiSpeed : 0), rotationSpeed * Time.deltaTime);
                if (Vector3.Dot(transform.forward, body.forward) < maxAngle)
                {
                    body.RotateTowards(body.position + transform.forward, Time.deltaTime * rotationSpeed * 2);
                    target = null;
                }
//                if (turret) { }
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