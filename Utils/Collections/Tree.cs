using System.Collections;
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

        int cachedChildCount = -1;
        public int GetDescendantCount()
        {
            cachedChildCount = cachedChildCount == -1 ? Children.Sum(c => c.GetDescendantCount()) : cachedChildCount;
            return Children.Count + cachedChildCount;
        }

        public override string ToString() => Key.ToString();
    }

    public class Tree<TKeyType> : Tree<TKeyType, object> { }

    public class Tree<TKeyType, TDataType> : IEnumerable<TreeNode<TKeyType, TDataType>>
    {
        readonly Dictionary<TKeyType, TreeNode<TKeyType, TDataType>> index = new();

        public IEnumerable<TKeyType> GetIndex() => index.Keys;

        public TKeyType GetRootKey() => TraverseToRoot(index.Keys.First()).Last();

        public TreeNode<TKeyType, TDataType> GetNode(TKeyType key) => index.GetOrCalculate(key, _ => new TreeNode<TKeyType, TDataType> { Key = key, Id = index.Count });

        public TreeNode<TKeyType, TDataType> this[TKeyType key] => GetNode(key);

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

        public IEnumerable<TKeyType> TraverseToRoot(TKeyType key)
        {
            var node = GetNode(key);
            while (node.Parent != null)
            {
                yield return node.Parent.Key;
                node = node.Parent;
            }
        }

        public IEnumerator<TreeNode<TKeyType, TDataType>> GetEnumerator() => index.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

}