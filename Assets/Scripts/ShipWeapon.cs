using UnityEngine;

public abstract class ShipWeapon : MonoBehaviour
{
    [SerializeField] private float ReloadTime;
    private float CoolDown;
    public Ship Parent
    {
        get
        {
            if (!ship)
                ship = GetComponentInParent<Ship>();
            return ship;
        }
    }
    private Ship ship;


    public void Fire()
    {
        if (CoolDown <= 0)
        {
            CoolDown = ReloadTime;
            DoFire();
        }
    }

    /// <summary>
    /// Dont call it directly
    /// </summary>
    protected abstract void DoFire();

    void Update()
    {
        if (CoolDown >= 0)
            CoolDown -= Time.deltaTime;
    }
}