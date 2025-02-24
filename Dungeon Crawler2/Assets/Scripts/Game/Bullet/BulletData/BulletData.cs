using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "New Bullet", menuName = "ScriptableObjects/Bullet")]
public class BulletData : ScriptableObject
{
    [SerializeField] public float speed = 10f;
    [SerializeField] public int damage = 10;
    [SerializeField] public float despawnDistance = 20f;
    [SerializeField] public Sprite sprite;
    [SerializeField] public int rotationAngle = 0;
    [SerializeField] public AudioClip audioClip;
    [SerializeField] public float damageModifier = 1f;
}