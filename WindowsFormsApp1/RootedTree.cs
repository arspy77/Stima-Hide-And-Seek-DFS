using System;
using System.Collections.Generic;
using System.IO;

namespace WindowsFormsApp1
{
    class RootedTree
    {
        // Attribute
        private List<int> parentNode;
        private List<List<int>> childNode;
        private int n;
        private List<bool> passed;

        // Method
        public RootedTree(string filename)
        {
            using (StreamReader sr = new StreamReader(filename))
            {
                string line = sr.ReadLine();
                n = Int32.Parse(line);
                childNode = new List<List<int>>(n+1);
                parentNode = new List<int>(n+1);
                for (int i = 1; i < n; i++)
                {
                    line = sr.ReadLine();
                    string[] splitLine = line.Split(' ');
                    int a = Int32.Parse(splitLine[0]);
                    int b = Int32.Parse(splitLine[1]);
                    childNode[a].Add(b);
                    childNode[b].Add(a);
                }
            }
        }

        public bool ExistCircuit()
        {
            passed = new List<bool>(n+1);
            return ExistCircuitRec(1, 0);
        }

        public bool ExistCircuitRec(int index, int prevIndex)
        {
            bool exist = false;
            foreach (int next in childNode[index])
            {
                if (passed[next] == true && next != prevIndex)
                {
                    exist = true;
                }

                else
                {
                    passed[index] = true;
                    exist = exist || ExistCircuitRec(next, index);
                    passed[index] = false;
                }
            }
            return exist;
        }

        public void ConvertToTree()
        {
            passed = new List<bool>(n + 1);
            ConvertToTreeRec(1);
        }

        public void ConvertToTreeRec(int index)
        {
            foreach (int next in childNode[index])
            {
                if (passed[next] == true)
                {
                    parentNode[index] = next;
                    childNode[index].Remove(next);
                }
                else
                {
                    ConvertToTreeRec(next);
                }
            }
        }
    }
}
