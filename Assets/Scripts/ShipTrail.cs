using UnityEngine;

public abstract class ShipTrail : MonoBehaviour
{
    [SerializeField]protected float size;
    /// <param name="speed">positive, 0~5?</param>
    public abstract void SetTrailLent(float speed);
}