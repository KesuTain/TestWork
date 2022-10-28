using TMPro;
using TW.PlayerSystem.Controller;
using TW.SpellSystem.Controller;
using TW.SpellSystem.Model;
using UnityEngine;
using UnityEngine.UI;

namespace TW.SpellSystem.View
{
    public class SpellView : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private GameObject _spellBlock;
        [SerializeField] private TextMeshProUGUI _costLabel;
        [SerializeField] private TextMeshProUGUI _stateLabel;
        [SerializeField] private TextMeshProUGUI _descriptionLabel;
        [SerializeField] private Button _buttonBuy;
        [SerializeField] private Button _restoreAll;

        private PlayerController _playerController;
        private SpellController _spellController;
        private void Awake()
        {
            _playerController ??= FindObjectOfType<PlayerController>();
            _spellController ??= FindObjectOfType<SpellController>();
        }

        private void Start()
        {
            _restoreAll.onClick.AddListener(() =>
            {
                _spellController.RestoreAllSpells();
            });
        }

        public void ViewSpell(SpellModel spellModel)
        {
            _spellBlock.SetActive(true);
            _descriptionLabel.SetText(spellModel.Desctription);

            switch (spellModel.SpellState)
            {
                case SpellStateEnum.Close:
                    _costLabel.SetText(spellModel.Cost.ToString());
                    _stateLabel.SetText("Close");
                    _buttonBuy.interactable = false;
                    break;

                case SpellStateEnum.Open:
                    _costLabel.SetText(string.Empty);
                    _stateLabel.SetText("Restore");

                    if (_spellController.CanBeRestored(spellModel))
                        _buttonBuy.interactable = true;
                    else
                        _buttonBuy.interactable = false;

                    _buttonBuy.onClick.RemoveAllListeners();
                    _buttonBuy.onClick.AddListener(() => {
                        _playerController.AddMoney(spellModel.RestoreSpell());
                        _buttonBuy.onClick.RemoveAllListeners();
                        _spellBlock.SetActive(false);
                    });
                    break;

                case SpellStateEnum.Access:
                    _costLabel.SetText(spellModel.Cost.ToString());
                    _stateLabel.SetText("Buy");
                    _buttonBuy.interactable = true;
                    _buttonBuy.onClick.RemoveAllListeners();
                    _buttonBuy.onClick.AddListener(() =>
                    {
                        if (_playerController.TryToSpendMoney(spellModel.Cost))
                        {
                            spellModel.BuySpell();
                            _buttonBuy.onClick.RemoveAllListeners();
                            _spellBlock.SetActive(false);
                        }
                    });
                    break;

                case SpellStateEnum.Root:
                    _costLabel.SetText(string.Empty);
                    _stateLabel.SetText("Can't restore");

                    _buttonBuy.interactable = false;
                    break;
            }
        }
    }
}
