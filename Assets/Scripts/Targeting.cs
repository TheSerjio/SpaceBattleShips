using UnityEngine;

public class Targeting
{
    public bool Fire;

    public virtual void OperateAutoWeapon(Ship parent,AutoWeapon what)
    {
        what.body.RotateTowards(what.body.position + what.body.forward, what.rotationSpeed, false);
    }

    public sealed class TargetDirection : Targeting
    {
        public Vector3 Dir;

        public override void OperateAutoWeapon(Ship parent,AutoWeapon what)
        {
            what.body.RotateTowards(what.body.position + Dir, what.rotationSpeed, false);
        }
    }

    public sealed class TargetRigidBody : Targeting
    {
        public Rigidbody RB;

        public override void OperateAutoWeapon(Ship parent,AutoWeapon what)
        {
            what.body.RotateTowards(Utils.ShootTo(parent.RB, RB, what.weapon.AntiSpeed), what.rotationSpeed, false);
        }

        public TargetRigidBody(Rigidbody rb)
        {
            RB = rb;
        }
    }
}