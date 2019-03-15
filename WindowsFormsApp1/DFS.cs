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
        private const int _rootNode = 1;
        public int n { get; private set; } 
        public List<List<int>> childNode { get; private set; } 
        public List<int> parentNode { get; private set; } 
        private List<bool> _visited;
        public List<int> checkedNode { get; private set; } 
        public Stack<int> pathNode { get; private set; } 
        public bool isFound { get; private set; } 


        // Method 
        // ctor - Generate tree sekaligus cek siklis atau tak
        public RootedTree(string filename)
        {
            using (var sr = new StreamReader(filename))
            {
                var line = sr.ReadLine();
                n = int.Parse(line);
                childNode = new List<List<int>>();
                for (int i = 0; i <= n; i++) childNode.Add(new List<int>());
                parentNode = new List<int>();
                for (int i = 0; i <= n; i++) parentNode.Add(0);
                _visited = new List<bool>();
                for (int i = 0; i <= n; i++) _visited.Add(false);

                for (int i = 0; i < n-1; i++)
                {
                    line = sr.ReadLine();
                    var splitLine = line.Split(' ');
                    int a = int.Parse(splitLine[0]);
                    int b = int.Parse(splitLine[1]);
                    childNode[a].Add(b);
                    childNode[b].Add(a);
                }
            }
            _makeRootedTree(_rootNode);
            for (int i = 1; i <= n; ++i)
                if (!_visited[i]) throw new CustomException("Input generated graph with a cycle");
        }

        private void _makeRootedTree(int idx)
        {
            if (_visited[idx]) throw new CustomException("Input generated graph with a cycle");
            _visited[idx] = true;
            foreach (int next in childNode[idx])
            {
                parentNode[next] = idx;
                childNode[next].Remove(idx);
                _makeRootedTree(next);
            }
        }

        public void Query(int mode, int X, int Y)
        {
            checkedNode = new List<int>();
            pathNode = new Stack<int>();
            isFound = false;
            if (mode == 0)
                _DFSIn(Y, X);
            else
                _DFSOut(Y, X);
        }

        private void _DFSIn(int curr, int target)
        {
            checkedNode.Add(curr);
            pathNode.Push(curr);
            if (curr == target) 
            {
                isFound = true;
                return;
            }

            if (curr != _rootNode)
                _DFSIn(parentNode[curr], target);
            if (!isFound)
            {
                pathNode.Pop();
            }
        }

        private void _DFSOut(int curr, int target)
        {
            checkedNode.Add(curr);
            pathNode.Push(curr);
            if (curr == target) 
            {
                isFound = true;
                return;
            }
            foreach (int next in childNode[curr])
            {
                _DFSOut(next, target);
                if (isFound) return;
            }
            if (!isFound)
            {
                pathNode.Pop();
            }
        }
    }
}
