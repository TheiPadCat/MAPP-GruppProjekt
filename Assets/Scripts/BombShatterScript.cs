using UnityEngine;

public class BombShatterScript : MonoBehaviour
{
    [SerializeField] private ParticleSystem explodeParticles;
    [SerializeField] private int damage = 0;

    private bool canHurtEnemy = false;

    private void Start()
    {
        GetComponent<Rigidbody2D>().AddTorque(500);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (canHurtEnemy && other.CompareTag("Enemy"))
        {
            other.GetComponent<IEnemy>()?.TakeDamage(damage); // vem vet om detta någonsin kommer hända, men man kan aldrig vara säker ; ) så la till en null check ändå
            canHurtEnemy = false;
        } 
    }
    public void Explode()
    {
        canHurtEnemy = true;
        explodeParticles.Emit(15);
        explodeParticles.transform.parent = null;
    }

}
