using DefaultNamespace;
using UnityEngine;

public class HealthSystem : MonoBehaviour, IDamageable
{
    [SerializeField] private string _name;
    [SerializeField] private int _health;
    
    private IKillable _killable;

    private void Awake() => _killable = GetComponent<IKillable>();

    public void TakeDamage(int dmg)
    {
        Health -= dmg;
        if(Health <= 0) _killable.Die();
        //Debug.Log(_name + "'s Health: " + Health);
    }

    public int Health { get => _health; private set => _health = value; }
}
