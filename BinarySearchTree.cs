using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

/*
Implementation of binary search tree
Copyright Zameer (http://xameeramir.github.io/)
*/

namespace BinarySearchTree
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ObjectPersistence persister = new ObjectPersistence();
                const string TreeStoreFile = @"D:\SavedTree.bin";

                #region Construct the tree

                Console.WriteLine("Constructing tree...\t");

                BST bst = new BST();
                bst.Insert(20);
                bst.Insert(25);
                bst.Insert(45);
                bst.Insert(15);
                bst.Insert(67);
                bst.Insert(43);
                bst.Insert(80);
                bst.Insert(33);
                bst.Insert(67);
                bst.Insert(99);
                bst.Insert(91);

                #endregion

                #region Traverse the tree

                Console.WriteLine(@"----------------------------------------------------
Before serialization and de-serialization");

                Console.WriteLine("\nInorder Traversal (Left - Root - Right):");
                bst.Inorder(bst.GetRoot());
                Console.WriteLine("\nPreorder Traversal (Root - Left - Right): ");
                bst.Preorder(bst.GetRoot());
                Console.WriteLine("\nPostorder Traversal (Left - Right - Root): ");
                bst.Postorder(bst.GetRoot());

                #endregion

                #region Serialize and de-serialize the tree to and from a file

                Console.WriteLine(@"
----------------------------------------------------
Storing (serializing) tree to file...");

                persister.StoreBSTToFile(bst, TreeStoreFile);

                Console.WriteLine(string.Format("Tree saved to {0} in binary format\nDe-serializing in process...\t", TreeStoreFile));

                bst = persister.ReadBSTFromFile(TreeStoreFile);

                Console.WriteLine("\nAfter serialization and de-serialization");

                Console.WriteLine("\nInorder Traversal (Left - Root - Right):");
                bst.Inorder(bst.GetRoot());
                Console.WriteLine("\nPreorder Traversal (Root - Left - Right): ");
                bst.Preorder(bst.GetRoot());
                Console.WriteLine("\nPostorder Traversal (Left - Right - Root): ");
                bst.Postorder(bst.GetRoot());

                #endregion

                #region Finding maximum, minimum and size of BST

                Console.WriteLine(@"
----------------------------------------------------
Other details of the tree");

                Console.WriteLine("Minimum value in the tree: " + bst.GetMinimum(bst.GetRoot()));

                Console.WriteLine("Maximum value in the tree: " + bst.GetMaximum(bst.GetRoot()));

                Console.WriteLine("Size of the tree: " + bst.GetSize(bst.GetRoot()));

                #endregion

            }
            catch (Exception ex)
            {
                Console.WriteLine("Oops!\n" + ex.Message);
            }

            //Keep the console running

            Console.WriteLine("\nPRESS ENTER TO TERMINATE...");
            Console.ReadLine();

        }
    }

    /// <summary>
    /// Takes care of serialization and de-serialization of a BST
    /// </summary>
    class ObjectPersistence
    {
        /// <summary>
        /// Stores a BST object to a binary file
        /// </summary>
        /// <param name="bst"></param>
        /// <param name="TreeStoreFile"></param>
        public void StoreBSTToFile(BST bst, string TreeStoreFile)
        {
            try
            {
                Stream SerializationFileStream = File.Create(TreeStoreFile);
                BinaryFormatter serializer = new BinaryFormatter();
                serializer.Serialize(SerializationFileStream, bst);
                SerializationFileStream.Close();
            }
            catch
            {
                throw new Exception("Some error occured while storing tree data to file");
            }            
        }

        /// <summary>
        /// Reads binary data from a file to construct a BST object
        /// </summary>
        /// <param name="TreeStoreFile"></param>
        /// <returns></returns>
        public BST ReadBSTFromFile(string TreeStoreFile)
        {
            try
            {
                //Make sure that a file exists
                if (!File.Exists(TreeStoreFile))
                {
                    File.Create(TreeStoreFile);
                }

                BST bst = new BST();
                Stream DeserializationFileStream = File.OpenRead(TreeStoreFile);
                BinaryFormatter deserializer = new BinaryFormatter();
                bst = (BST)deserializer.Deserialize(DeserializationFileStream);
                DeserializationFileStream.Close();

                return bst;
            }
            catch
            {
                throw new Exception("Some error occured while reading tree data from file");
            }
        }

    }

    /// <summary>
    /// A binary search tree Node. Has a key, possibly left and right child
    /// </summary>
    [Serializable]
    class Node : System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// The value inside a node
        /// </summary>
        public int key { get; set; }

        /// <summary>
        /// Left child tree attached to a node
        /// </summary>
        public Node leftc;

        /// <summary>
        /// Right child tree attached to a node
        /// </summary>
        public Node rightc;

        public Node()
        {
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    }

    /// <summary>
    /// Represents a binary search tree
    /// </summary>
    [Serializable]
    class BST : System.ComponentModel.INotifyPropertyChanged
    {

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Value not inside tree. This represents pseudo nodes next to end nodes
        /// </summary>
        const int MARKER = -1;

        public Node root;

        public BST()
        {
            root = null;
        }

        /// <summary>
        /// Returns the root node
        /// </summary>
        /// <returns></returns>
        public Node GetRoot()
        {
            return root;
        }

        /// <summary>
        /// Inserts a key into a node
        /// </summary>
        /// <param name="key">The value to be inserted to a node</param>
        public void Insert(int key)
        {
            Node newNode = new Node();
            newNode.key = key;

            /*If tree don't have a root, set a new node as root*/
            if (root == null)
            {
                root = newNode;
            }
            else
            {
                /*The root is set*/
                Node current = root;
                Node parent;
                while (true)
                {
                    /*Initially, assuming current node as parent*/
                    parent = current;

                    /*Less value on the left side*/
                    if (key < current.key)
                    {
                        current = current.leftc;
                        if (current == null)
                        {
                            parent.leftc = newNode;
                            return;
                        }
                    }
                    else
                    {
                        /*Large value on the left side*/
                        current = current.rightc;
                        if (current == null)
                        {
                            parent.rightc = newNode;
                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Root - Left - Right
        /// </summary>
        /// <param name="Root"></param>
        public void Preorder(Node Root)
        {
            if (Root != null)
            {
                Console.Write(Root.key + " ");
                Preorder(Root.leftc);
                Preorder(Root.rightc);
            }
        }

        /// <summary>
        /// Left - Root - Right
        /// </summary>
        /// <param name="Root"></param>
        public void Inorder(Node Root)
        {
            if (Root != null)
            {
                Inorder(Root.leftc);
                Console.Write(Root.key + " ");
                Inorder(Root.rightc);
            }
        }

        /// <summary>
        /// Left - Right - Root
        /// </summary>
        /// <param name="Root"></param>
        public void Postorder(Node Root)
        {
            if (Root != null)
            {
                Postorder(Root.leftc);
                Postorder(Root.rightc);
                Console.Write(Root.key + " ");
            }
        }

        /// <summary>
        ///  Returns the minimum value in the Binary Search Tree
        /// </summary>
        /// <param name="Root"></param>
        /// <returns></returns>
        public int GetMinimum(Node Root)
        {
            if (Root == null)
            {
                return 0;
            }

            Node currentNode = root;

            while (currentNode.leftc != null)
            {
                currentNode = currentNode.leftc;
            }

            return currentNode.key;
        }

        /// <summary>
        ///  Returns the maximum value in the Binary Search Tree
        /// </summary>
        /// <param name="Root"></param>
        /// <returns></returns>
        public int GetMaximum(Node Root)
        {
            if (Root == null)
            {
                return 0;
            }

            Node currentNode = root;

            while (currentNode.rightc != null)
            {
                currentNode = currentNode.rightc;
            }

            return currentNode.key;
        }

        /// <summary>
        /// Returns the size of the Binary Search Tree
        /// </summary>
        /// <param name="Root"></param>
        public int GetSize(Node Root)
        {
            if (Root == null)
            {
                return 0;
            }
            else
            {
                return GetSize(Root.leftc) + 1 + GetSize(Root.rightc);
            }
        }


    }
}
