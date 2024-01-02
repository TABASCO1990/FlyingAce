using Game.Shared;
using UnityEngine;

namespace Game.Bullet
{
    [RequireComponent(typeof(Combat))]
    [RequireComponent(typeof(Movement))]
    public class Main : MonoBehaviour
    {
        private Combat _combat;
        private Health _health;
        private CombatCollider _combatCollider;

        public Movement Movement { get; private set; }

        public void Init(ColliderOwner colliderToDamage)
        {
            _combat = GetComponent<Combat>();
            _health = GetComponent<Health>();
            _combatCollider = GetComponent<CombatCollider>();
            Movement = GetComponent<Movement>();

            _combat.Init(_combatCollider, colliderToDamage, _health);
            _health.Init(_combatCollider);
            Movement.Init(_health);
        }       
    }
}
