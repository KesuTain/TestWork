using System.Collections.Generic;
using TW.SpellSystem.Model;
using UnityEngine;

namespace TW.GraphSystem.NodeModel
{
    public class Node : MonoBehaviour
    {
        [SerializeField] private List<Node> _links;

        [Header("Parameters")]
        [SerializeField] private bool _isRoot;
        public bool IsRoot { get { return _isRoot; } }

        public SpellModel SpellState { get; internal set; }

        public void AddToNodes(Node node)
        {
            _links.Add(node);
        }

        public void RemoveFromNodes(Node node)
        {
            _links.Remove(node);
        }

        public List<Node> GetListLinks()
        {
            return _links;
        }

        public void ClearListLinks()
        {
            _links.Clear();
        }
    }
}