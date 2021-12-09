using UnityEngine;

public class SunContainer : MonoBehaviour
{
    public Renderer Big;
    public Renderer Medium;
    public Renderer Small;

    public void Do(SunData data)
    {
        Big.material.SetColor(Utils.ShaderID(ShaderName.Colorio), data.big);
        Medium.material.SetColor(Utils.ShaderID(ShaderName.Colorio), data.medium);
        Small.material.SetColor(Utils.ShaderID(ShaderName.Colorio), data.small);
    }
}