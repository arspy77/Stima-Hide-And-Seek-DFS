using System;
using System.Collections.Generic;
using System.IO;

namespace HideAndSeek {
    class CustomException : Exception {
        public CustomException(String _msg) : base(String.Format(_msg)) {

        }
    }

    class RootedTree {
        // Attribute
        private List<int> parentNode;
        private List<List<int>> childNode;
        private int n;
        private List<int> spanningTree;
        // Dipake buat DFS
        List<List<int>> pathClosure; //Set of currPath

        // Method 
        // ctor - Generate tree sekaligus cek siklis atau tak
        public RootedTree(string filename) {
            using (StreamReader sr = new StreamReader(filename)) {
                string line = sr.ReadLine();
                n = Int32.Parse(line);
                childNode = new List<List<int>>();
                for (int i = 0; i <= n; i++) {
                    childNode.Add(new List<int>());
                }
                parentNode = new List<int>();
                for (int i = 0; i <= n; i++) {
                    parentNode.Add(0);
                }
                spanningTree = new List<int>();
                for (int i = 0; i <= n; i++) {
                    spanningTree.Add(i);
                }

                for (int i = 1; i <= n - 1; i++) {
                    line = sr.ReadLine();
                    string[] splitLine = line.Split(' ');
                    int a = Int32.Parse(splitLine[0]);
                    int b = Int32.Parse(splitLine[1]);
                    if (spanningTree[a] == spanningTree[b]) { //Cek Siklik, cek ae
                        throw new CustomException("Error : input generated a graph (siklik) at pass-" + i);
                    } else {
                        childNode[a].Add(b);
                        parentNode[b] = a;
                        int secSpanTree = spanningTree[b];
                        for (int j = 1; j <= n; j++) {
                            if (spanningTree[j] == secSpanTree) {
                                spanningTree[j] = spanningTree[a];
                            }
                        }
                    }
                }
            }
        }

        public List<int> query(String[] inp) {
            //query mereturn path yang dapat dilalui dengan melewati rumah-X, 
            //kalo ngeluarin path kosong berarti ga ketemu rumah-X
            int mode = Int32.Parse(inp[0]);
            int X = Int32.Parse(inp[1]);
            int Y = Int32.Parse(inp[2]);
            pathClosure = new List<List<int>>(); //garbage collector bakal dealoc pathClosure lama sebelum new lagi, cmiw
            if (mode == 0) {
                DFSIn(Y, new List<int>());
            } else { //mode == 1
                DFSOut(Y, new List<int>());
            }
            //printPathClosure(); //uncomment for debug, bisa pake pathClosure kalo mau dibikin animasi ;)
            foreach (List<int> path in pathClosure) {
                if (path.Contains(X)) {
                    return path;
                }
            }
            return new List<int>();
        }
        
        //NOTE : Walaupun banyak newnya tapi kalo udah keluar fungsi, garbage collectornya C# bakal langsung dealok objeknya
        private void DFSIn(int currVer, List<int> currPath) {
            currPath.Add(currVer);
            //Basis
            if (currVer == 1) {
                pathClosure.Add(new List<int>(currPath));
            //Rekuren
            } else {
                DFSIn(parentNode[currVer], new List<int>(currPath));
            }
        }

        //NOTE : Walaupun banyak newnya tapi kalo udah keluar fungsi, garbage collectornya C# bakal langsung dealok objeknya
        private void DFSOut(int currVer, List<int> currPath) {
            currPath.Add(currVer);
            //Basis
            if (childNode[currVer].Count == 0) {
                pathClosure.Add(new List<int>(currPath));
            //Rekurens
            } else {
                foreach (int i in childNode[currVer]) {
                    DFSOut(i, new List<int>(currPath));
                }
            }
        }

        public void printNode() {
            Console.WriteLine("List Parent :");
            bool skip = true; //ngeskip idx ke-0 
            foreach (int parent in parentNode) {
                if (!skip) {
                    Console.Write(parent + " | ");
                } else {
                    skip = !skip;
                }
            }
            Console.WriteLine();
            Console.WriteLine("List Children :");
            foreach (List<int> children in childNode) {
                foreach (int child in children) {
                    Console.Write(child + " ");
                }
                Console.Write("| ");
            }
            Console.WriteLine();
        }

        public void printPathClosure() {
            foreach (List<int> path in  pathClosure) {
                Console.Write("[");
                foreach (int vertex in path) {
                    Console.Write(vertex + ", ");
                }
                Console.WriteLine("]");
            }
        }
    }

    class ProgramUtama {
        static void Main(String[] args) {
            try {
                List<int> path;
                RootedTree Tree = new RootedTree("test.txt");
                Tree.printNode();
                String[] inp;
                inp = Console.ReadLine().Split(' ');
                while (inp.Length != 1) {
                    path = Tree.query(inp);
                    if (path.Count != 0) {
                        Console.WriteLine("YA");
                        Console.WriteLine("Jalur yang ditempuh :");
                        foreach (int i in path) {
                            Console.Write(i + " ");
                        }
                        Console.WriteLine();
                    } else {
                        Console.WriteLine("TIDAK");
                    }
                    inp = Console.ReadLine().Split(' ');
                }
            } catch (CustomException ex) {
                Console.WriteLine(ex.Message);
            }
            Console.ReadLine();
        }
    }
}
