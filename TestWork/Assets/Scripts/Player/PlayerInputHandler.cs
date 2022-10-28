using System.Collections.Generic;
using TW.GraphSystem.Controller;
using TW.GraphSystem.NodeModel;
using TW.SpellSystem.Model;
using TW.SpellSystem.View;
using UnityEngine;

namespace TW.PlayerSystem.Handler
{
    public class PlayerInputHandler : MonoBehaviour
    {
        private NodeController _nodeController;
        private SpellView _spellView;

        private void Awake()
        {
            _nodeController ??= FindObjectOfType<NodeController>();

            _nodeController.OnGetNodes += SubscribeNodes;
        }

        private void SubscribeNodes(List<Node> nodes)
        {
            foreach (SpellModel spell in nodes)
            {
                spell.OnAction += HandleSpellClick;
            }
        }

        private void HandleSpellClick(SpellModel spell)
        {
            _spellView ??= FindObjectOfType<SpellView>();

            _spellView.ViewSpell(spell);
        }
    }
}
