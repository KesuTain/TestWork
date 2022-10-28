using System.Collections.Generic;
using TW.GraphSystem.NodeModel;

namespace TW.GraphSystem.PathFinding
{
    public class AStarPathFinding
    {
        private List<Node> _nodes;
        private List<Node> _explored = new List<Node>();
        private List<Node> _reachable = new List<Node>();

        public AStarPathFinding(List<Node> nodes)
        {
            _nodes = nodes;
        }

        public bool HasPathBetweenPoint(Node start, Node end)
        {
            _reachable.Add(start);

            while(_reachable.Count != 0)
            {
                for (int i = 0; i < _reachable.Count; i++)
                {
                    FindReachableNeighbours(_reachable[i]);

                    if (_reachable.Contains(end))
                        return true;
                }
            }

            return false;
        }

        public void AddClosePoint(Node node)
        {
            _explored.Add(node);
        }

        public void AddOpenPoint(Node node)
        {
            _reachable.Add(node);
        }
        public void ClearExploredAndReachableNodes()
        {
            _explored.Clear();
            _reachable.Clear();
        }

        private void FindReachableNeighbours(Node node)
        {
            foreach(Node neighbour in node.GetListLinks())
            {
                if (!_explored.Contains(neighbour) && !_reachable.Contains(neighbour))
                {
                    _reachable.Add(neighbour);
                }
            }

            _explored.Add(node);
            _reachable.Remove(node);
        }
    }
}
