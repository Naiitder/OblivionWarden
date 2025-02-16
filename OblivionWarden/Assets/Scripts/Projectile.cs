using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;  
    [SerializeField] protected float lifetime = 2f;

    protected int damage = 0;
    public int Damage { get { return damage; } set { damage = value; } }

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
