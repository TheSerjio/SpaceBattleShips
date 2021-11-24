using System;
using UnityEngine;

public class AutoWeapon : MonoBehaviour
{
    public float rotationSpeed;
    public Transform body;
    private Ship parent;
    public ShipWeapon weapon;
    public bool automatic;

    [Tooltip("less value -> more rotation")] [Range(-1, 1)]
    public float maxAngle;

    public void Awake()
    {
        parent = GetComponentInParent<Ship>();
    }

    public void Update()
    {
        parent.Target.OperateAutoWeapon(parent, this);
    }
}