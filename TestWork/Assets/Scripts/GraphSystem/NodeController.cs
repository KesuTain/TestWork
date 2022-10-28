using System.Collections.Generic;
using UnityEngine;
using TW.GraphSystem.NodeModel;
using TW.GraphSystem.PathFinding;
using System;

namespace TW.GraphSystem.Controller
{
    public class NodeController : MonoBehaviour
    {
        private List<Node> _nodes;
        private Node _rootNode;

        public event Action<List<Node>> OnGetNodes;

        private void Awake()
        {
            InitLinks();
        }

        private void InitLinks()
        {
            _nodes = FindAllNodes();
            _rootNode = FindRootNode(_nodes);

            OnGetNodes?.Invoke(_nodes);
        }

        private Node FindRootNode(List<Node> nodes)
        {
            Node root = null;

            foreach (Node node in nodes)
            {
                if (node.IsRoot)
                {
                    root = node;
                    break;
                }
            }

            if (root == null)
            {
                Debug.LogError("Haven't root node!");
                return null;
            }

            return root;
        }

        private List<Node> FindAllNodes()
        {
            return new List<Node>(FindObjectsOfType<Node>());
        }

        public bool TryToFindPathBetweenPoint(Node start, Node end)
        {
            AStarPathFinding pathFinding = new AStarPathFinding(_nodes);

            return pathFinding.HasPathBetweenPoint(start, end);
        }

        public Node GetRootNode()
        {
            return _rootNode;
        }
        

        public List<Node> GetExistNodes()
        {
            return _nodes;
        }
    }
}
