using System.Collections;
using System.Collections.Generic;
using Utilities.Unity;
using UnityEngine;
using System;

namespace Procedural.Builders
{
    [Serializable]
    public class VectorTree : Tree<Vector3>
    {
        [Header("Node Data")]
        public int maxDepth;
        public VectorTreeAngle branchGrowthDirector;

        System.Random random;

        public VectorTree()
        {
            random = new System.Random((int)DateTime.Now.Ticks);
        }

        public VectorTree(int seed)
        {
            random = new System.Random(seed);
        }

        public void Build()
        {
            this.root = new Node<Vector3>();
            this.root.data = Vector3.zero;
            nodes.Add(this.root);

            GenerateNode(this.root, 0); 
        }

        public void GenerateNode(Node<Vector3> node, int depth)
        {
            if (depth == maxDepth)
            {
                return;
            }

            node.children = new Node<Vector3>[1];

            for (int i = 0; i < node.children.Length; i++)
            {
                Node<Vector3> newNode = new Node<Vector3>();
                newNode.parent = node;
                newNode.data = branchGrowthDirector.GeneratePoint(node.data, node.data + NodeDirection(node), random);

                nodes.Add(newNode);
                node.children[i] = newNode;
                GenerateNode(newNode, depth + 1);
            }
        }
        
        public Vector3 NodeDirection(Node<Vector3> node)
        {
            if (node.parent == null)
            {
                return Vector3.up;
            }

            return node.data - node.parent.data;
        }

        public void DrawNodes(Transform at)
        {
            Gizmos.color = Color.green;
            foreach (var node in nodes)
            {
                Gizmos.DrawSphere(node.data + at.position, 0.05f);
                if (node.children == null || node.children.Length == 0)
                {
                    continue;
                }

                for (int i = 0; i < node.children.Length; i++)
                {
                    Gizmos.DrawLine(node.data + at.position, node.children[i].data + at.position);
                    GizmoExtension.DrawConeDegrees(node.data + at.position, at.position + node.data + NodeDirection(node), branchGrowthDirector.BranchAngle);
                }
            }
        }
    }
}
