using UnityEngine;

[RequireComponent(typeof(Script))]
public class GiveMeFrame : Script
{
    protected override void OnAwake()
    {
    }

    public void Start()
    {
        Spawner.CreateFrame(GetComponent<Ship>());
    }
}