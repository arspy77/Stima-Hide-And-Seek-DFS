using System;
using System.Collections.Generic;
using System.IO;

namespace WindowsFormsApp1
{
    class RootedTree
    {
        // Attribute
        public List<int> parentNode;
        public List<List<int>> childNode;
        public int n;
        public List<bool> passed;

        // Method
        public RootedTree(string filename)
        {
            using (StreamReader sr = new StreamReader(filename))
            {
                string line = sr.ReadLine();
                n = Int32.Parse(line);
                childNode = new List<List<int>>(n+1);
                parentNode = new List<int>(n + 1);
                passed = new List<bool>(n + 1);
                for (int i = 0; i <= n; ++i)
                {
                    childNode.Add(new List<int> { });
                    parentNode.Add(0);
                    passed.Add(false);
                }
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
       
        
        public void ConvertToTree()
        {
            ConvertToTreeRec(1);
        }

        public void ConvertToTreeRec(int index)
        {
            passed[index] = true;
            foreach (int next in childNode[index])
            {
                if (passed[next] == true)
                {
                    throw new Exception();
                }
                else
                {
                    parentNode[next] = index;
                    childNode[next].Remove(index);
                    ConvertToTreeRec(next);
                }
            }
        }
    }
}
