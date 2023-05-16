using System;
using HurricaneVR.Framework.Weapons;
using UnityEngine;

namespace HurricaneVR.Framework.Components
{
    public class HVRDamageHandlerVariant : HVRDamageHandlerBase
    {
        public float Life = 100f;

        public bool Damageable = true;

        private Animator knightAnimator;

        public Rigidbody Rigidbody { get; private set; }

        public HVRDestructible Desctructible;

        void Start()
        {
            Rigidbody = GetComponent<Rigidbody>();
            if (!Desctructible)
                Desctructible = GetComponent<HVRDestructible>();

            knightAnimator = GetComponent<Animator>();
        }

        public override void TakeDamage(float damage)
        {
            if (Damageable)
            {
                Life -= damage;
            }

            if (Life <= 0)
            {
                if (Desctructible)
                {
                    Desctructible.Destroy();
                }

                if (gameObject.CompareTag("Knight"))
                {
                    knightAnimator.SetBool("isDead", true);
                }
            }
        }

        public override void HandleDamageProvider(HVRDamageProvider damageProvider, Vector3 hitPoint, Vector3 direction)
        {
            base.HandleDamageProvider(damageProvider, hitPoint, direction);

            if (Rigidbody)
            {
                Rigidbody.AddForceAtPosition(direction.normalized * damageProvider.Force, hitPoint, ForceMode.Impulse);
            }
        }

        private void Update()
        {
            print(Life);
        }
    }
}