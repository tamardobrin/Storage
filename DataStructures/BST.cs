using System;

namespace DataStructures
{

    public class BST<T> where T : IComparable<T>
    {
        int count = 0;
        Node root;
        public bool isEmpty()
        {
            if (root == null) return true;
            else return false;
        }

        public void Add(T item) // O(logN)
        {
            if (root == null)
            {
                root = new Node(item);
                return;
            }

            Node tmp = root;
            while (true)
            {
                if (item.CompareTo(tmp.value) < 0) // item < tmp.value - go left
                {
                    if (tmp.left == null)
                    {
                        tmp.left = new Node(item);
                        break;
                    }
                    else tmp = tmp.left;
                }
                else // go right
                {
                    if (tmp.right == null)
                    {
                        tmp.right = new Node(item);
                        break;
                    }
                    else tmp = tmp.right;
                }
            }

        }

        public bool Search(T item, out T foundItem) // O(log(N))
        {
            Node tmp = root;
            while (tmp != null)
            {
                if (item.CompareTo(tmp.value) == 0)
                {
                    foundItem = tmp.value;
                    return true;
                }
                if (item.CompareTo(tmp.value) < 0) tmp = tmp.left;
                else tmp = tmp.right;
            }
            foundItem = default;
            return false;
        }
        public bool SearchClosest(T item, out T foundItem)
        {
            Node newNode = root;
            Node tmp = root;
            while (tmp != null)
            {
                if (item.CompareTo(tmp.value) == 0)
                {
                    foundItem = tmp.value;
                    return true;
                }
                if (item.CompareTo(tmp.value) >= 0) tmp = tmp.right;
                else
                {
                    newNode = tmp;
                    tmp = tmp.left;
                    count++;
                }
            }
            if (count > 0)
            {
                foundItem = newNode.value;
                return true;
            }
            foundItem = default;
            return false;
        }

        public bool Remove(T item)
        {
            Node tmp = root, parent = null;
            bool wentLeft = false;
            while (tmp != null && tmp.value.CompareTo(item) != 0)
            {
                if (tmp.value.CompareTo(item) < 0)
                {
                    parent = tmp;
                    tmp = tmp.right;
                    wentLeft = false;
                }
                else
                {
                    parent = tmp;
                    tmp = tmp.left;
                    wentLeft = true;
                }
            }
            if (tmp == null) return false;
            if (tmp.left == null && tmp.right == null)
            {
                if (tmp == root)
                {
                    root = null;
                }
                else
                {
                    if (wentLeft) parent.left = null;
                    else parent.right = null;
                }
                return true;

            }
            else if (tmp.left == null)
            {
                if (tmp == root)
                {
                    root = tmp.right;
                }
                else
                {
                    if (wentLeft) parent.left = tmp.right;
                    else parent.right = tmp.right;
                }
                return true;

            }
            else if (tmp.right == null)
            {
                if (tmp == root)
                {
                    root = tmp.left;
                }
                else
                {
                    if (wentLeft) parent.left = tmp.left;
                    else parent.right = tmp.left;
                }
                return true;

            }
            else
            {
                Node t = tmp;
                Node savedNode = tmp.left;
                while (savedNode.right != null)
                {
                    t = savedNode;
                    savedNode = savedNode.right;
                }
                tmp.value = savedNode.value;
                if (t == tmp) t.left = savedNode.left;
                else t.right = savedNode.left;
                return true;

            }
        }
        public int GetLevelsCnt()
        {
            return GetLevelsCnt(root);
        }


        int GetLevelsCnt(Node subTreeRoot)
        {
            if (subTreeRoot == null) return 0;

            int leftTreeDepth = GetLevelsCnt(subTreeRoot.left);
            int rightTreeDepth = GetLevelsCnt(subTreeRoot.right);

            return Math.Max(leftTreeDepth, rightTreeDepth) + 1;
        }

        public void ScanInOrder(Action<T> singleItemAction)  // Action<T> => void Func(T item)
        {
            ScanInOrder(root, singleItemAction);
        }

        void ScanInOrder(Node subTreeRoot, Action<T> singleItemAction)
        {
            if (subTreeRoot == null) return;

            ScanInOrder(subTreeRoot.left, singleItemAction);
            singleItemAction(subTreeRoot.value); //invoke
            ScanInOrder(subTreeRoot.right, singleItemAction);
        }

        public void ScanInOrderWithoutItem(T item, Action<T> singleItemAction)  // Action<T> => void Func(T item)
        {

            ScanInOrderWithoutItem(root, item, singleItemAction);
        }
        public void ScanInOrderWithoutItem(Node tmp, T item, Action<T> singleItemAction)
        {
            if (tmp == null) return;
            ScanInOrderWithoutItem(tmp.left, item, singleItemAction);
            if (tmp.value.CompareTo(item) > 0)
                singleItemAction(tmp.value); //invoke

            ScanInOrderWithoutItem(tmp.right, item, singleItemAction);
        }

        public class Node
        {
            public T value;
            public Node left;
            public Node right;

            public Node(T value)
            {
                this.value = value;
                left = right = null;
            }
        }

    }

}
