using System;
using System.Collections;
using DefaultNamespace;
using UnityEngine;

namespace SalvadorNovo.CharacterRelated
{
    public class BulletBehaviour : MonoBehaviour, IKillable
    {
        [SerializeField] int damage = 20;
        [SerializeField] GameObject debris, bloodDebris;
        public float destroyTime = 5.0f;

        Collision _col;
        
        GameObject _instDebris;

        public bool isBaloon;

        int _bounceCounter;
        [SerializeField] private int _maxBounces;

        void Start()
        {
            StartCoroutine(ExpirationDate());
        }

        IEnumerator ExpirationDate()
        {
            while (PlayerMove.Player.isInflating) yield return null;
            print(destroyTime);
            yield return new WaitForSeconds(destroyTime);
            Die();
        }
        
        void OnCollisionEnter(Collision col)
        {
            _col = col;
            if(isBaloon) Baloon();
            else         Bullet();
        }

        void Bullet()
        {
            DealDamage();
            Debris();
            Die();
        }

        void Baloon()
        {
            if(PlayerMove.Player.isInflating) return;

            _bounceCounter++;
            DealDamage();

            if (_bounceCounter < _maxBounces) return;

            Debris();
            Die();
        }

        void DealDamage()
        {
            var damageable = _col.gameObject.GetComponent<IDamageable>();
            damageable?.TakeDamage(damage);
        }
        
        void Debris()
        {
            _instDebris = _col.transform.name == "Player" ? bloodDebris : debris;
            Instantiate(_instDebris, _col.GetContact(0).point, Quaternion.LookRotation(-transform.forward));
        }
        
        public void Die()
        {
            if(_col is null) return;
            if(_col.gameObject.CompareTag("Bullet")) return;
            Destroy(gameObject);
        }
    }
}
