using UnityEngine;

public abstract class StatusEffectAbstract : MonoBehaviour
{
    public Color color = Color.white;
    public abstract void Apply(GameObject target);
}
