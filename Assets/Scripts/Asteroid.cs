using UnityEngine;

public class Asteroid : BaseEntity
{
    public float HP, Lifetime;

    public override void OnDamaged(float dmg, BaseEntity from)
    {
        HP -= dmg;
        if (HP <= 0)
        {
            Destroy(Instantiate(DataBase.Get().AsteroidExplosion, transform.position, Quaternion.identity), 10);
            Destroy(gameObject);
        }
    }

    protected override void OnEntityAwake()
    {

    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, Random.value, Random.value);
        Gizmos.DrawWireSphere(Ttransform.position, size / 2f);
    }

    public void Update()
    {
        var t = GameCore.MainCamera.transform;
        var d = (t.position - transform.position).sqrMagnitude;
        if (d < 10000)
            Lifetime = 30;
        else
        {
            Lifetime -= Time.deltaTime;
            if (Lifetime < 0)
                if (Vector3.Dot(t.forward, (transform.position - t.position).normalized) < 0)
                    Destroy(gameObject);
        }
    }
}