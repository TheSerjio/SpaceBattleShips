using UnityEngine;

public class Planet : COLLECTOR<Ship>
{
    public void FixedUpdate()
    {
        foreach (var q in All)
            if (q)
            {
            }
            else
            {
                RemoveNull();
                break;
            }
    }
}