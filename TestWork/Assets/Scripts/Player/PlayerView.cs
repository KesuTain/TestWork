using UnityEngine;
using TW.PlayerSystem.Controller;
using TMPro;

namespace TW.PlayerSystem.View
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _countMoney;
        private PlayerController _playerController;

        private void OnEnable()
        {
            _playerController ??= FindObjectOfType<PlayerController>();
            _playerController.OnChangeMoneyCount += SetMoney;
        }

        private void OnDisable()
        {
            _playerController ??= FindObjectOfType<PlayerController>();
            _playerController.OnChangeMoneyCount -= SetMoney;
        }

        private void SetMoney(int value)
        {
            _countMoney.SetText(value.ToString());
        }
    }
}
