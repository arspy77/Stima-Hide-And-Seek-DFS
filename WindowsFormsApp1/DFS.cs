using System;
using System.Collections.Generic;
using System.IO;

namespace WindowsFormsApp1
{
    internal class CustomException : Exception
    {
        public CustomException(string _msg) : base(string.Format(_msg))
        {
        }
    }

    internal class RootedTree
    {
        public int NNode { get; private set; }

        // root dari RootedTree, posisi castle
        public const int rootNode = 1;

        // ParentNode merupakan node parent (node yang mendekati rootNode) dari suatu node
        public List<int> ParentNode { get; private set; } 

        // ChildNode semua adjacent node selain ParentNode
        public List<List<int>> ChildNode { get; private set; } 

        // CheckedNode list node yang telah dicek untuk suatu query
        public List<int> CheckedNode { get; private set; } 

        // PathNode list node path dari start ke goal suatu query
        public Stack<int> PathNode { get; private set; }

        // IsFound diset true jika ditemukan path untuk suatu query 
        public bool IsFound { get; private set; } 


        // Untuk mengecek apakah ada node yang dikunjungi dua kali (kasus cycle)
        private List<bool> _visited;


        // Membaca format tree dari adjacency list menjadi rooted tree
        public RootedTree(string filename)
        {
            using (var sr = new StreamReader(filename))
            {
                var line = sr.ReadLine();
                NNode = int.Parse(line);
                ChildNode = new List<List<int>>();
                for (int i = 0; i <= NNode; ++i)
                    ChildNode.Add(new List<int>());
                ParentNode = new List<int>();
                for (int i = 0; i <= NNode; ++i) 
                    ParentNode.Add(0);
                _visited = new List<bool>();
                for (int i = 0; i <= NNode; ++i)
                    _visited.Add(false);

                // mengisi ChildNode dari adjacency list
                for (int i = 0; i < NNode - 1; ++i)
                {
                    line = sr.ReadLine();
                    var splitLine = line.Split(' ');
                    int a = int.Parse(splitLine[0]);
                    int b = int.Parse(splitLine[1]);
                    ChildNode[a].Add(b);
                    ChildNode[b].Add(a);
                }
            }

            // memanggil DFS dari rootNode
            _makeRootedTree(rootNode);
            for (int i = 1; i <= NNode; ++i)
                // ada node yang belum dikunjungi => tidak membentuk tree dan ada cycle di antara node tersebut
                if (!_visited[i]) 
                    throw new CustomException("Input generated graph with a cycle.");
        }


        // Fungsi DFS untuk mencari ParentNode dari setiap node
        private void _makeRootedTree(int idx)
        {
            if (_visited[idx])
                throw new CustomException("Input generated graph with a cycle.");
            _visited[idx] = true;
            foreach (int next in ChildNode[idx])
            {
                ParentNode[next] = idx;
                ChildNode[next].Remove(idx);
                _makeRootedTree(next);
            }
        }


        // Mengecek kevalidan query
        public bool CheckQuery(int mode, int X, int Y)
        {
            return mode >= 0 && mode <= 1 && X >= 1 && X <= NNode && Y >= 1 && Y <= NNode;
        }


        // Mengerjakan query pencarian path
        public void Query(int mode, int X, int Y)
        {
            CheckedNode = new List<int>();
            PathNode = new Stack<int>();
            IsFound = false;
            if (!CheckQuery(mode, X, Y))
                throw new CustomException("Out of range query input.");
            if (mode == 0)
                _DFSIn(Y, X);
            else
                _DFSOut(Y, X);
        }


        // Melakukan pencarian dengan node menuju rootNode
        private void _DFSIn(int curr, int target)
        {
            CheckedNode.Add(curr);
            PathNode.Push(curr);
            if (curr == target) 
            {
                IsFound = true;
                return;
            }
            if (curr != rootNode)
                _DFSIn(ParentNode[curr], target);
            if (!IsFound)
                PathNode.Pop();
        }


        // Melakukan pencarian dengan DFS, node menjauhi rootNode
        private void _DFSOut(int curr, int target)
        {
            CheckedNode.Add(curr);
            PathNode.Push(curr);
            if (curr == target) 
            {
                IsFound = true;
                return;
            }
            foreach (int next in ChildNode[curr])
            {
                _DFSOut(next, target);
                if (IsFound) return;
            }
            if (!IsFound)
            {
                PathNode.Pop();
            }
        }
    }
}
