using UnityEngine;
using UnityEngine.VFX;

public class Explosion : PoolableComponent
{
    protected float lifeLeft;

    public VisualEffect effect;

    public void Update()
    {
        lifeLeft -= Time.deltaTime;
        if (lifeLeft < 0)
            gameObject.SetActive(false);
    }

    public override void ReInit()
    {
        lifeLeft = 10;
        transform.rotation = Random.rotationUniform;
        effect.Reinit();
    }
}