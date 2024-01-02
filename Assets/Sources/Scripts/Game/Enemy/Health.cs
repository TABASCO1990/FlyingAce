using System;
using Game.Shared;
using UniRx;
using UnityEngine;

namespace Game.Enemy
{
    public class Health : MonoBehaviour, IHealth
    {
        private readonly ReactiveProperty<uint> _currentHealth = new();
        private readonly CompositeDisposable _disposable = new();

        [SerializeField] private CombatCollider _combatCollider;
        [SerializeField] private uint _maxHealth = 1;

        public static event Action BossDied;
        public event Action OnDamage;

        public ReadOnlyReactiveProperty<uint> CurrentHealth { get; private set; }
        public BoolReactiveProperty IsDead { get; } = new();

        private void OnDestroy()
        {
            _disposable.Clear();

            _combatCollider.OnDamage -= TakeDamage;
        }
        
        public void Init(bool isBoss)
        {
            CurrentHealth = new ReadOnlyReactiveProperty<uint>(_currentHealth);

            _currentHealth.Value = _maxHealth;
            _currentHealth.Subscribe(health => { if (health == 0) Die(); }).AddTo(_disposable);

            if (isBoss == true)
            {
                IsDead.Subscribe(isDead =>
                {
                    if (isDead == true)
                    {
                        BossDied?.Invoke();
                    }
                });
            }

            _combatCollider.OnDamage += TakeDamage;
        }

        private void TakeDamage(uint damage)
        {
            if (damage >= _currentHealth.Value)
            {
                _currentHealth.Value = 0;
            }
            else
            {
                _currentHealth.Value -= damage;
                OnDamage?.Invoke();
            }
        }

        private void Die()
        {
            IsDead.Value = true;

            Destroy(transform.parent.gameObject);
        }
    }
}
