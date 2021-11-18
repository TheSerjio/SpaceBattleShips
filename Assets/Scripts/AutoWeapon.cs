using UnityEngine;

public class AutoWeapon : MonoBehaviour, IFireControl
{
    public float rotationSpeed;
    public Transform body;
    public Rigidbody target;
    private Ship parent;
    public ShipWeapon weapon;
    public bool automatic;
    [Tooltip("less value -> more rotation")] [Range(-1, 1)] public float maxAngle;

    bool IFireControl.Fire => automatic ? target : parent.Fire;
    Ship IFireControl.Parent => parent;

    public void Update()
    {
        if (parent)
            if (target)
            {
                weapon.Parent = this;
                body.RotateTowards(Utils.ShootTo(weapon.transform.position, parent.RB.velocity, target, weapon ? weapon.AntiSpeed : 0), rotationSpeed * Time.deltaTime, false);
                if (weapon.IsOutOfRange(Vector3.Distance(weapon.transform.position, target.transform.position)))
                    target = null;
                else
                    while (Vector3.Dot(transform.forward, body.forward) < maxAngle)
                    {
                        body.RotateTowards(body.position + transform.forward, Time.deltaTime * rotationSpeed, false);
                        target = null;
                    }
            }
            else
            {
                body.RotateTowards(body.position + transform.forward, Time.deltaTime * rotationSpeed, false);
                body.localEulerAngles = (Vector2)body.localEulerAngles;
                var tar = GameCore.Self.FindTargetShip(parent.team, transform);
                if (tar)
                    target = tar.RB;
            }
        else
            parent = GetComponentInParent<Ship>();
    }
}