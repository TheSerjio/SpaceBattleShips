using UnityEngine;

public abstract class ShipTrail : MonoBehaviour
{
    /// <param name="speed">positive, 0~5?</param>
    public abstract void SetTrailLent(float speed);
}