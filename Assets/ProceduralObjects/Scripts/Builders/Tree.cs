using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Procedural.Builders
{
    public class Tree<T>
    {
        public class Node<J>
        {
            public Node<J>[] children;
            public Node<J> parent;
            public J data;
        }

        public Node<T> root;
        public List<Node<T>> nodes = new List<Node<T>>();

        public Tree()
        {
            
        }

        public Node<T> GetRoot()
        {
            return root;
        }

        public List<Node<T>> GetNodes()
        {
            return nodes;
        }
    }
}
