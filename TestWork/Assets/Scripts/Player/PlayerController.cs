using System;
using UnityEngine;

namespace TW.PlayerSystem.Controller
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private int _money;
        public int Money { get { return _money; } }
        public event Action<int> OnChangeMoneyCount;

        private void Start()
        {
            OnChangeMoneyCount?.Invoke(_money);
        }

        public void AddMoney(int value)
        {
            _money += value;
            OnChangeMoneyCount?.Invoke(_money);
        }

        public bool TryToSpendMoney(int value)
        {
            if(_money - value >= 0)
            {
                _money -= value;
                OnChangeMoneyCount?.Invoke(_money);
                return true;
            }

            return false;
        }
    }
}
