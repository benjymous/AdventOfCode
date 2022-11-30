using System.Collections.Generic;
using System.Linq;

namespace AoC.Utils.Collections
{
    public class TreeNode<TKeyType, TDataType>
    {
        public TKeyType Key { get; set; }
        public TDataType Value { get; set; }

        public int Id { get; set; }

        public TreeNode<TKeyType, TDataType> Parent = null;

        public List<TreeNode<TKeyType, TDataType>> Children { get; set; } = new List<TreeNode<TKeyType, TDataType>>();

        public int GetDescendantCount() => Children.Count + Children.Select(c => c.GetDescendantCount()).Sum();

        public override string ToString() => Key.ToString();
    }

    public class Tree<TKeyType> : Tree<TKeyType, object> { }

    public class Tree<TKeyType, TDataType>
    {
        readonly Dictionary<TKeyType, TreeNode<TKeyType, TDataType>> index = new();

        public IEnumerable<TKeyType> GetIndex() => index.Keys;
        public IEnumerable<TreeNode<TKeyType, TDataType>> GetNodes() => index.Values;

        TreeNode<TKeyType, TDataType> root = null;
        public TreeNode<TKeyType, TDataType> GetRoot()
        {
            root ??= TraverseToRoot(index.Keys.First()).Last();
            return root;
        }

        public TreeNode<TKeyType, TDataType> GetNode(TKeyType key)
        {
            if (!index.ContainsKey(key))
            {
                index[key] = new TreeNode<TKeyType, TDataType> { Key = key, Id = index.Count };
            }
            return index[key];
        }

        public TreeNode<TKeyType, TDataType> AddNode(TKeyType key, TDataType value)
        {
            var node = GetNode(key);
            node.Value = value; return node;
        }

        public void AddPair(TKeyType parent, TKeyType child)
        {
            var p = GetNode(parent);
            var c = GetNode(child);
            p.Children.Add(c);
            c.Parent = p;
        }

        public void AddBidirectional(TKeyType node1, TKeyType node2)
        {
            var n1 = GetNode(node1);
            var n2 = GetNode(node2);
            n1.Children.Add(n2);
            n2.Children.Add(n1);
        }

        public IEnumerable<TreeNode<TKeyType, TDataType>> TraverseToRoot(TKeyType key)
        {
            var node = GetNode(key);
            while (node.Parent != null)
            {
                yield return node.Parent;
                node = node.Parent;
            }
        }
    }

}