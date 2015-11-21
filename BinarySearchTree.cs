using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinarySearchTree;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;

/*
* C# Program to Implement Binary Search Tree using Linked List
*/
namespace BinarySearchTree
{
    /// <summary>
    /// A binary tree Node has a key, possibly left and right child
    /// </summary>
    //The Serializable attribute tells the compiler that everything in the class can be persisted to a file. Because the PropertyChanged event is handled by a Windows Form object, it cannot be serialized. The NonSerialized attribute can be used to mark class members that should not be persisted.
    [Serializable()]
    public class Node : System.ComponentModel.INotifyPropertyChanged
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

    [Serializable()]
    public class Tree : System.ComponentModel.INotifyPropertyChanged
    {

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Value not inside tree. This represents pseudo nodes next to end nodes
        /// </summary>
        const int MARKER = -1;

        public Node root;

        public Tree()
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
        /// <param name="_key"></param>
        public void Add(int _key)
        {
            Node newNode = new Node();
            newNode.key = _key;

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
                    if (_key < current.key)
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

    class Program
    {
        static void Main(string[] args)
        {
            const string FileName = @"D:\SavedTree.bin";

            #region Construct the tree

            Tree theTree = new Tree();
            theTree.Add(20);
            theTree.Add(25);
            theTree.Add(45);
            theTree.Add(15);
            theTree.Add(67);
            theTree.Add(43);
            theTree.Add(80);
            theTree.Add(33);
            theTree.Add(67);
            theTree.Add(99);
            theTree.Add(91);

            #endregion

            #region Traverse the tree

            Console.WriteLine("Before serialization and de-serialization\n");
            Console.WriteLine(" ");

            Console.WriteLine("Inorder Traversal (Left - Root - Right):");
            theTree.Inorder(theTree.GetRoot());
            Console.WriteLine("\n\n");
            Console.WriteLine("Preorder Traversal (Root - Left - Right): ");
            theTree.Preorder(theTree.GetRoot());
            Console.WriteLine("\n\n");
            Console.WriteLine("Postorder Traversal (Left - Right - Root): ");
            theTree.Postorder(theTree.GetRoot());
            Console.WriteLine("\n\n");

            #endregion

            #region Find maximum, minimum and size of BST

            Console.WriteLine("Maximum ");
            
            Console.WriteLine("The minimum value in the BST: " + theTree.GetMinimum(theTree.GetRoot()));
            Console.WriteLine("\n\n");

            Console.WriteLine("The maximum value in the BST: " + theTree.GetMaximum(theTree.GetRoot()));
            Console.WriteLine("\n\n");

            Console.WriteLine("The size of the BST: " + theTree.GetSize(theTree.GetRoot()));
            Console.WriteLine("\n\n");

            #endregion

            #region Serialize and de-serialize the tree to and from a file

            Console.WriteLine("Serializing...");
            Console.WriteLine("\n\n");

            Stream SerializationFileStream = File.Create(FileName);
            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(SerializationFileStream, theTree);
            SerializationFileStream.Close();

            Console.WriteLine("De-serializing...");
            Console.WriteLine("\n\n");
            if (File.Exists(FileName))
            {
                Stream DeserializationFileStream = File.OpenRead(FileName);
                BinaryFormatter deserializer = new BinaryFormatter();
                theTree = (Tree)deserializer.Deserialize(DeserializationFileStream);
                DeserializationFileStream.Close();
            }


            Console.WriteLine("After serialization and de-serialization");
            Console.WriteLine("\n\n");

            Console.WriteLine("Inorder Traversal (Left - Root - Right):");
            theTree.Inorder(theTree.GetRoot());
            Console.WriteLine("\n\n");
            Console.WriteLine("Preorder Traversal (Root - Left - Right): ");
            theTree.Preorder(theTree.GetRoot());
            Console.WriteLine("\n\n");
            Console.WriteLine("Postorder Traversal (Left - Right - Root): ");
            theTree.Postorder(theTree.GetRoot());
            Console.WriteLine("\n\n");

            #endregion

            //Keep the console running

            Console.WriteLine("Press enter to terminate");
            Console.ReadLine();

        }
    }
}
