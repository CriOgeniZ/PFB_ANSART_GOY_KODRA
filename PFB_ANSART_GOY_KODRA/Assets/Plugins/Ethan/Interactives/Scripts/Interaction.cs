///-----------------------------------------------------------------
///    Author   : Ethan CHATELARD
///    Date     : 04/05/2022
///-----------------------------------------------------------------

using UnityEngine;
using UnityEngine.Events;

namespace Com.EthanCHATELARD.Interactives
{
    public delegate void InteractionEventHandler(Interaction sender);

    public enum KeyType
    {
        [Tooltip("Do Activate and shutdown.")]
        UNIQUE,
        [Tooltip("Change between Activate and Desactivate")]
        SWITCH,
        [Tooltip("Repeate Activate as many time key is pressed.")]
        REPEAT,
    }

    public class Interaction : Trigger
    {
        [Header("Interaction")]
        [SerializeField] protected KeyCode keyToPress                           = default;
        [SerializeField] protected KeyType keyType                              = default;

        [Space(10)]
        [SerializeField] protected UnityEvent<Transform> _onActivate            = new UnityEvent<Transform>();
        [SerializeField] protected UnityEvent<Transform> _onDesactivate         = new UnityEvent<Transform>();

        public event UnityAction<Transform> OnActivate
        {
            add     => _onActivate.AddListener(value);
            remove  => _onActivate.RemoveListener(value);
        }

        public event UnityAction<Transform> OnDesactivate
        {
            add     => _onDesactivate.AddListener(value);
            remove  => _onDesactivate.RemoveListener(value);
        }

        public event InteractionEventHandler OnInput;

        protected bool isActive;
        protected bool isAlreadyUsed;

        protected Component targetCollider;

        protected void Interaction_OnInput(Interaction sender)
        {
            if (keyType == KeyType.UNIQUE)
            {
                if (isAlreadyUsed)      return;

                _onActivate.Invoke(targetCollider.transform);

                isAlreadyUsed = true;
            }
            else if (keyType == KeyType.SWITCH)
            {
                if (!isActive)          _onActivate.Invoke(targetCollider.transform);
                else                    _onDesactivate.Invoke(targetCollider.transform);

                isActive = !isActive;
            }
            else if (keyType == KeyType.REPEAT)
            {
                _onActivate.Invoke(targetCollider.transform);
            }
        }

        override protected void EnterTrigger(Component collision)
        {
            base.EnterTrigger(collision);

            targetCollider = collision;
            OnInput += Interaction_OnInput;
        }

        override protected void ExitTrigger(Component collision)
        {
            base.EnterTrigger(collision);

            targetCollider = null;
            OnInput -= Interaction_OnInput;
        }

        virtual protected void Update()
        {
            if (Input.GetKeyDown(keyToPress))
                OnInput?.Invoke(this);
        }

        override protected void OnDestroy()
        {
            OnInput -= Interaction_OnInput;

            _onActivate.RemoveAllListeners();
            _onDesactivate.RemoveAllListeners();

            base.OnDestroy();
        }
    }
}