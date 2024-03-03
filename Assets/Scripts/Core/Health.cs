using System;
using UnityEngine;


namespace Game.Core
{
    public class Health : MonoBehaviour
    {
        [SerializeField]private float maxHealth = 20f;

        private float _currentAmount;

        public float MaxHealth { get => maxHealth;}
        public float CurrentAmount { get => _currentAmount; }

        public Action<float> OnHealthDamaged;

        public Action<float> OnHealthHealed;
        public Action<float,float> OnCurrentHealthSet;

        public Action OnDeath;

        // Start is called before the first frame update
        void Start()
        {
            _currentAmount = maxHealth;
            OnCurrentHealthSet += SetHealth;
            OnHealthDamaged += TakeDamage;
            OnHealthHealed += RepairHealth;
        }

        private void SetHealth(float amount,float maxAmount)
        {
            _currentAmount = Mathf.Clamp(amount,0f,maxAmount);
            if(_currentAmount == 0f)
            {
                OnDeath?.Invoke();
            }
        }

        private void TakeDamage(float amount)
        {
            OnCurrentHealthSet?.Invoke(_currentAmount - amount,maxHealth);
        }

        private void RepairHealth(float amount)
        {
            OnCurrentHealthSet?.Invoke(_currentAmount + amount,maxHealth);
        }

        private void OnDestroy() 
        {
            OnCurrentHealthSet -= SetHealth;
            OnHealthDamaged -= TakeDamage;
        }
    }
}
