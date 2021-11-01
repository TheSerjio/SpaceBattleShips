using UnityEngine;

public abstract class ShipTrail : MonoBehaviour
{

    /// <param name="dir">direction, -1~1</param>
    /// <param name="speed">positive</param>
    public abstract void SetTrailLent(float dir, float speed);
}