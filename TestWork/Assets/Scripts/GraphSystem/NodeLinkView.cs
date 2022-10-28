using System.Collections.Generic;
using TW.GraphSystem.Controller;
using TW.GraphSystem.NodeModel;
using UnityEngine;

namespace TW.GraphSystem.View
{
    public class NodeLinkView : MonoBehaviour
    {
        private NodeController _nodeController;

        [SerializeField] private LineRenderer _lineRendererPrefab;
        [SerializeField] private GameObject _lineParent;

        private void OnEnable()
        {
            _nodeController ??= FindObjectOfType<NodeController>();
            _nodeController.OnGetNodes += ViewLinkLines;
        }

        private void OnDisable()
        {
            _nodeController ??= FindObjectOfType<NodeController>();
            _nodeController.OnGetNodes -= ViewLinkLines;
        }

        public void ViewLinkLines(List<Node> nodes)
        {
            foreach (Node node in nodes)
            {
                foreach(Node link in node.GetListLinks())
                {
                    LineRenderer line = Instantiate(_lineRendererPrefab, _lineParent.transform);

                    line.SetPosition(0, node.transform.localPosition);
                    line.SetPosition(1, link.transform.localPosition);
                }
            }
        }
    }
}
