using UnityEngine;
using TW.GraphSystem.NodeModel;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TW.SpellSystem.Model
{
    public enum SpellStateEnum { Close, Open, Access, Root }

    public class SpellModel : Node, IPointerClickHandler
    {
        [SerializeField] private int _cost;
        public int Cost { get { return _cost; } }
        [SerializeField] private string _description;
        public string Desctription { get { return _description; } } 
        [SerializeField] private SpellStateEnum _spellState = SpellStateEnum.Close;
        public SpellStateEnum SpellState { get { return _spellState; } }

        public event Action<SpellModel> OnAction;
        public event Action<SpellModel> OnBuy;
        public event Action<SpellModel> OnRestore;

        public void SetState(SpellStateEnum state)
        {
            _spellState = state;

            switch (state)
            {
                case SpellStateEnum.Open:
                    GetComponent<Image>().color = Color.green;
                    break;
                case SpellStateEnum.Access:
                    GetComponent<Image>().color = Color.yellow;
                    break;
                case SpellStateEnum.Close:
                    GetComponent<Image>().color = Color.red;
                    break;
            }
        }

        public void BuySpell()
        {
            if (_spellState == SpellStateEnum.Access)
            {
                SetState(SpellStateEnum.Open);
                OnBuy?.Invoke(this);
                OnAction?.Invoke(this);
            }
        }

        public int RestoreSpell()
        {
            if(_spellState == SpellStateEnum.Open)
            {
                SetState(SpellStateEnum.Close);
                OnRestore?.Invoke(this);
                OnAction?.Invoke(this);
                return _cost;
            }

            return 0;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnAction?.Invoke(this);
        }
    }
}
