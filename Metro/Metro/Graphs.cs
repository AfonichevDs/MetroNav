using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Metro
{
    class Node
    {
        public string Name { get; set; }
        public int Value { get; set; }
        public Node Ancestor { get; set; }
        public bool IsChecked { get; set; }
        public Point SPoint { get; set; }

        public Node(string name, int value, Point point, bool ischecked = false)
        {
            Name = name;
            Value = value;
            IsChecked = ischecked;
            Ancestor = new Node();
            SPoint = point;
        }

        public Node(int value, Point point, bool ischecked = false)
        {
            Name = null;
            Value = value;
            IsChecked = ischecked;
            Ancestor = new Node();
            SPoint = point;
        }
        public Node() { }
    }

    class Edge
    {
        public const int Weight = 1;

        public Node FirstNode { get; private set; }
        public Node LastNode { get; private set; }

        public Edge(Node firstNode, Node lastNode)
        {
            LastNode = lastNode;
            FirstNode = firstNode;
        }
    }

    class Dijkstra
    {
        public Node[] Nodes { get; private set; }
        public Edge[] Edges { get; private set; }
        public Node StartPoint { get; private set; }

        public int STEPS { get; set; } = 0;

        public Dijkstra(Node[] ns, Edge[] eds)
        {
            Nodes = ns;
            Edges = eds;
        }

        public void RunAlgorithm(Node start)
        {
            if (Nodes.Length == 0 || Edges.Length == 0)
            {
                throw new Exception();
            }

            StartPoint = start;
            OneStep(StartPoint);

            foreach (Node node in Nodes)
            {
                Node unchecknode = UncheckedNode();

                if (unchecknode != null)
                {
                    OneStep(unchecknode);
                }
                else break;
            }
        }

        public void OneStep(Node node)
        {
            IEnumerable<Node> neighbours = GetNeighbours(node);

            Node minNode = new Node();
            foreach (var n in neighbours)
            {
                if (n.IsChecked == false)
                {
                    int newvalue = node.Value + Edge.Weight;
                    if (n.Value > newvalue)
                    {
                        n.Value = newvalue;
                        n.Ancestor = node;
                    }
                }
            }
            node.IsChecked = true;
        }

        public IEnumerable<Node> GetNeighbours(Node node)
        {
            IEnumerable<Node> frpoints = from fp in Edges where fp.FirstNode == node select fp.LastNode;
            IEnumerable<Node> scpoints = from sc in Edges where sc.LastNode == node select sc.FirstNode;

            IEnumerable<Node> totalnodes = frpoints.Concat(scpoints);

            return totalnodes;
        }

        private Node UncheckedNode()
        {
            IEnumerable<Node> unpoints = from ns in Nodes where ns.IsChecked == false select ns;

            if (unpoints.Count() != 0)
            {
                int minvalue = unpoints.First().Value;
                Node minNode = unpoints.First();
                foreach (var n in unpoints)
                {
                    if (n.Value < minvalue)
                    {
                        minvalue = n.Value;
                        minNode = n;
                    }
                }
                return minNode;
            }
            return null;
        }

        public List<Node> MinAlgorithm(Node end)
        {
            if (end == null) throw new Exception("end==null");
            List<Node> path = new List<Node>();
            Node temp = new Node();
            temp = end;

            while (temp != this.StartPoint)
            {
                STEPS++;
                path.Add(temp);
                temp = temp.Ancestor;
            }
            path.Add(StartPoint);
            return path;
        }
    }
}
