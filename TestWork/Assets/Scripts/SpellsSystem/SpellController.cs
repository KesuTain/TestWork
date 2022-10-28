using System.Collections.Generic;
using TW.GraphSystem.Controller;
using TW.GraphSystem.NodeModel;
using TW.GraphSystem.PathFinding;
using TW.PlayerSystem.Controller;
using TW.SpellSystem.Model;
using UnityEngine;

namespace TW.SpellSystem.Controller
{
    public class SpellController : MonoBehaviour
    {
        private NodeController _nodeController;
        private PlayerController _playerController;
        private Node _baseSpell;
        private List<Node> _nodes;

        private void Start()
        {
            _nodeController ??= FindObjectOfType<NodeController>();
            _playerController ??= FindObjectOfType<PlayerController>();
            Init();
        }

        private void Init()
        {
            _baseSpell = _nodeController.GetRootNode();

            foreach (Node node in _baseSpell.GetListLinks())
            {
                SpellModel spell = node as SpellModel;
                spell.SetState(SpellStateEnum.Access);
            }

            _nodes = _nodeController.GetExistNodes();

            foreach (SpellModel spell in _nodes)
            {
                spell.OnBuy += UpdateAfterBuying;
                spell.OnRestore += UpdateAfterRestore;
            }
        }

        public void RestoreAllSpells()
        {
            foreach(Node node in _nodes)
            {
                SpellModel spell = (SpellModel)node;
                _playerController.AddMoney(spell.RestoreSpell());
            }
        }

        private void UpdateAfterBuying(SpellModel spell)
        {
            List<Node> links = spell.GetListLinks();

            foreach(Node node in links)
            {
                SpellModel neighbour = (SpellModel)node;
                if (neighbour.SpellState == SpellStateEnum.Close && neighbour.SpellState != SpellStateEnum.Root)
                    neighbour.SetState(SpellStateEnum.Access);
            }
        } 

        private void UpdateAfterRestore(SpellModel spell)
        {
            List<Node> links = spell.GetListLinks();

            foreach (Node node in links)
            {
                SpellModel neighbour = (SpellModel)node;
                if (neighbour.SpellState == SpellStateEnum.Access && neighbour.SpellState != SpellStateEnum.Root)
                {
                    List<Node> neighbourLinks = neighbour.GetListLinks();

                    bool hasOpenAbout = false;

                    foreach(Node neighbourNode in neighbourLinks)
                    {
                        SpellModel neighbourSpell = (SpellModel)neighbourNode;
                        if (neighbourSpell.SpellState == SpellStateEnum.Open || neighbourSpell.SpellState == SpellStateEnum.Root)
                        {
                            hasOpenAbout = true;
                            break;
                        }
                    }

                    if(!hasOpenAbout)
                        neighbour.SetState(SpellStateEnum.Close);
                }
            }

            foreach(Node node in links)
            {
                SpellModel neighbour = (SpellModel)node;
                if (neighbour.SpellState == SpellStateEnum.Root || neighbour.SpellState == SpellStateEnum.Open)
                    spell.SetState(SpellStateEnum.Access);
            }
        }

        public bool CanBeRestored(SpellModel spell)
        {
            AStarPathFinding pathFinding = new AStarPathFinding(_nodes);

            void UpdateClosePoint()
            {
                pathFinding.AddClosePoint(spell);

                foreach(Node node in _nodes)
                {
                    SpellModel closeSpell = (SpellModel)node;
                    if (closeSpell.SpellState == SpellStateEnum.Close || closeSpell.SpellState == SpellStateEnum.Access)
                        pathFinding.AddClosePoint(closeSpell);
                }
            }


            foreach(Node node in spell.GetListLinks())
            {
                pathFinding.ClearExploredAndReachableNodes();
                UpdateClosePoint();

                SpellModel currentSpell = (SpellModel)node;
                if(currentSpell.SpellState == SpellStateEnum.Open && currentSpell.SpellState != SpellStateEnum.Root)
                {
                    if (!pathFinding.HasPathBetweenPoint(node, _baseSpell))
                        return false;
                }
            }

            return true;
        }
    }
}
