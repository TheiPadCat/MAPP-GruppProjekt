using Unity;
using UnityEngine;

public interface IEnemy {
    public delegate void death(System.Type enemyType, GameObject prefab = null);
    public static death Death;
    public abstract void Attack();
    public abstract void Die();
    public abstract void TakeDamage(float damage);
}
