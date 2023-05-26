using UnityEngine;

public class BombShatterScript : MonoBehaviour
{
    [SerializeField] private ParticleSystem explodeParticles;
    [SerializeField] private int damage = 0;
    [SerializeField] float explosionRadius;
    [SerializeField] LayerMask enemyLayer;

    private bool canHurtEnemy = false;

    private void Start()
    {
        GetComponent<Rigidbody2D>().AddTorque(500);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Fiender tar inte skada ifall de aktiverar triggern innan bomben sprängs. Lade inte annan metod för att ge skada /Joel
        /*
        if (canHurtEnemy && other.CompareTag("Enemy"))
        {
            other.GetComponent<IEnemy>()?.TakeDamage(damage); // vem vet om detta någonsin kommer hända, men man kan aldrig vara säker ; ) så la till en null check ändå
            canHurtEnemy = false;
        } 
        */
    }
    public void Explode()
    {
        

        DealDamage();
        GameObject.Find("AudioMan").GetComponent<AudioScript>().Split(transform.position);
        canHurtEnemy = true;
        explodeParticles.Emit(15);
        explodeParticles.transform.parent = null;


    }


    public void DealDamage()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyLayer);
        foreach (Collider2D collider in collider2Ds)
        {

            Debug.Log("TAKE DAMGAGE");
            collider.GetComponent<IEnemy>().TakeDamage(damage);

        }
       // Destroy(transform.root.gameObject);
    }

}
