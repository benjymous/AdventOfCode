using System.Collections.Generic;
using System.Linq;

namespace AoC.Utils.Collections
{
    public class TreeNode<TKeyType, TDataType>
    {
        public TKeyType Key { get; set; }
        public TDataType Value { get; set; }

        public TreeNode<TKeyType, TDataType> Parent = null;

        public List<TreeNode<TKeyType, TDataType>> Children { get; set; } = new List<TreeNode<TKeyType, TDataType>>();

        public int GetDescendantCount() => Children.Count + Children.Select(c => c.GetDescendantCount()).Sum();

        public override string ToString()
        {
            return Key.ToString();
        }
    }

    public class Tree<TKeyType> : Tree<TKeyType, object> { }

    public class Tree<TKeyType, TDataType>
    {
        Dictionary<TKeyType, TreeNode<TKeyType, TDataType>> index = new Dictionary<TKeyType, TreeNode<TKeyType, TDataType>>();

        public IEnumerable<TKeyType> GetIndex() => index.Keys;
        public IEnumerable<TreeNode<TKeyType, TDataType>> GetNodes() => index.Values;

        TreeNode<TKeyType, TDataType> root = null;
        public TreeNode<TKeyType, TDataType> GetRoot()
        {
            if (root == null)
            {
                root = TraverseToRoot(index.Keys.First()).Last();
            }
            return root;
        }

        public TreeNode<TKeyType, TDataType> GetNode(TKeyType key)
        {
            if (!index.ContainsKey(key))
            {
                index[key] = new TreeNode<TKeyType, TDataType> { Key = key };
            }
            return index[key];
        }

        public void AddPair(TKeyType parent, TKeyType child)
        {
            var p = GetNode(parent);
            var c = GetNode(child);
            p.Children.Add(c);
            c.Parent = p;
        }

        public List<TreeNode<TKeyType, TDataType>> TraverseToRoot(TKeyType key)
        {
            var output = new List<TreeNode<TKeyType, TDataType>>();
            var node = GetNode(key);

            while (node.Parent != null)
            {
                output.Add(node.Parent);
                node = node.Parent;
            }

            return output;
        }
    }

}