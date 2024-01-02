using System;
using Game.Shared;
using UniRx;
using UnityEngine;

namespace Game.Player
{
    public class Health : MonoBehaviour, IHealth
    {
        private readonly ReactiveProperty<uint> _currentHealth = new();
        private readonly CompositeDisposable _disposable = new();

        [SerializeField] private CombatCollider _combatCollider;
        [SerializeField] private uint _maxHealth = 100;
        [SerializeField] private bool _godMode;

        public event Action OnDamage;

        public BoolReactiveProperty IsDead { get; } = new();
        public ReadOnlyReactiveProperty<uint> CurrentHealth { get; private set; }

        public void Init()
        {
            CurrentHealth = new ReadOnlyReactiveProperty<uint>(_currentHealth);
            
            _currentHealth.Value = _maxHealth;
            _currentHealth.Subscribe(health => { if (health == 0) Die(); }).AddTo(_disposable);

            _combatCollider.OnDamage += TakeDamage;
        }

        public void SetHealth(uint health)
        {
            _currentHealth.Value = health;
        }

        public void ApplyAdViewHealth()
        {
            _currentHealth.Value += Constants.HealthForAdView;
        }

        private void TakeDamage(uint damage)
        {
            #if UNITY_EDITOR
            if (_godMode == true) return;
            #endif

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

        private void OnDestroy()
        {
            _disposable.Clear();

            _combatCollider.OnDamage -= TakeDamage;
        }

        private void Die()
        {
            IsDead.Value = true;
            
            Destroy(transform.parent.gameObject);
        }
    }
}
