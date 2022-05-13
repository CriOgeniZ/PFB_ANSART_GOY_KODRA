///-----------------------------------------------------------------
///    Author   : Ethan CHATELARD
///    Date     : 04/05/2022
///-----------------------------------------------------------------

using UnityEngine;
using UnityEngine.Events;

namespace Com.EthanCHATELARD.Interactives
{
    public class Trigger : MonoBehaviour
    {
        [Header("Trigger")]
        [SerializeField] protected UnityEvent<Transform> _onEnter       = new UnityEvent<Transform>();
        [SerializeField] protected UnityEvent<Transform> _onExit        = new UnityEvent<Transform>();

        public event UnityAction<Transform> OnEnter
        {
            add     => _onEnter.AddListener(value);
            remove  => _onEnter.RemoveListener(value);
        }

        public event UnityAction<Transform> OnExit
        {
            add     => _onExit.AddListener(value);
            remove  => _onExit.RemoveListener(value);
        }

        #region UnityMessageFunctions Triggers
        protected void OnTriggerEnter2D(Collider2D collision)
        {
            EnterTrigger(collision);
        }

        protected void OnTriggerExit2D(Collider2D collision)
        {
            ExitTrigger(collision);
        }

        protected void OnTriggerEnter(Collider collision)
        {
            EnterTrigger(collision);
        }

        protected void OnTriggerExit(Collider collision)
        {
            ExitTrigger(collision);
        }
        #endregion

        virtual protected void EnterTrigger(Component collision)
        {
            _onEnter.Invoke(collision.transform);
        }

        virtual protected void ExitTrigger(Component collision)
        {
            _onExit.Invoke(collision.transform);
        }

        virtual protected void OnDestroy()
        {
            _onEnter.RemoveAllListeners();
            _onExit.RemoveAllListeners();
        }
    }
}