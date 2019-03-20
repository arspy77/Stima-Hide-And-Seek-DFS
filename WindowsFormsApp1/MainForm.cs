using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Xml.XPath;
using WindowsFormsApp1;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.GraphViewerGdi;
using Label = System.Windows.Forms.Label;

namespace WindowsFormsApp1
{
    public partial class MainForm : Form
    {
        private RootedTree rootedTree;
        private Queue<Tuple<int, int, int>> queryList;
        private Microsoft.Msagl.Drawing.Graph graph;
        // alat visualisasi graf dari MSAGL
        private GViewer viewer;


        public MainForm()
        {
            InitializeComponent();
        }


        // Mencari file input dari dialog dan mengembalikan path file tersebut
        private static string _getFilename()
        {
            OpenFileDialog dialogue = new OpenFileDialog();
            if (dialogue.ShowDialog() == DialogResult.OK)
            {
                return dialogue.FileName;
            }
            else
            {
                return "";
            }
        }


        // Membaca query dari dialog
        private static string _getQuery()
        {
            Form prompt = new Form()
            {
                Width = 500,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "GetQueryDialogue",
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 50, Top = 20, Text = "Query : (0/1 X Y)" };
            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;
            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }

        // Membaca nama file dari dialog
        private static string _getOutFilename()
        {
            Form prompt = new Form()
            {
                Width = 500,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "GetFilenameDialogue",
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 50, Top = 20, Text = "Enter OutFilename: " };
            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;
            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }


        // Menganimasikan query pada graf dengan mengubah warna startNode, goalNode, checkedNode, dan pathNode
        private void _animateGraph(int startNode, int goalNode)
        {
            _initializeGraph();
            // zoom in view untuk agar view terfokus ke satu node dan beberapa di sekitarnya
            // rasio zoom diset
            viewer.ZoomF = 0.05*Math.Max(viewer.GraphHeight, viewer.GraphWidth) / viewer.Graph.FindNode("1").Attr.Height;
            viewer.ZoomInPressed();

            // mengubah warna startNode menjadi kuning
            viewer.Graph.FindNode(startNode.ToString()).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Yellow;
            // set node yang baru diubah warnanya menjadi center dari view
            viewer.CenterToPoint(viewer.Graph.FindNode(startNode.ToString()).Attr.Pos);
            graphPanel.Refresh();
            var tw = Task.Delay(1000);
            tw.Wait();

            viewer.Graph.FindNode(goalNode.ToString()).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Red;
            viewer.CenterToPoint(viewer.Graph.FindNode(goalNode.ToString()).Attr.Pos);
            graphPanel.Refresh();
            tw = Task.Delay(1000);
            tw.Wait();

            int CheckedCount = rootedTree.CheckedNode.Count;
            // set waktu transisi antar node dalam animasi
            double waitTime = Math.Min((double)1000, 20000.0/CheckedCount);        
            foreach (int el in rootedTree.CheckedNode)
            {
                viewer.Graph.FindNode(el.ToString()).Attr.FillColor = Microsoft.Msagl.Drawing.Color.LightPink;
                viewer.CenterToPoint(viewer.Graph.FindNode(el.ToString()).Attr.Pos);
                graphPanel.Refresh();
                tw = Task.Delay((int)waitTime);
                tw.Wait();
            }
            int[] path = rootedTree.PathNode.ToArray();
            for (int i = path.Length - 1; i >= 0; --i)
            {
                viewer.Graph.FindNode(path[i].ToString()).Attr.FillColor = Microsoft.Msagl.Drawing.Color.RoyalBlue;
            }
            viewer.ZoomOutPressed();
            viewer.ZoomOutPressed();
            if (Math.Abs(viewer.Graph.FindNode(goalNode.ToString()).Attr.Pos.X - viewer.Graph.FindNode(startNode.ToString()).Attr.Pos.X) > Math.Abs(viewer.Graph.FindNode(goalNode.ToString()).Attr.Pos.Y - viewer.Graph.FindNode(startNode.ToString()).Attr.Pos.Y))
            {
                viewer.ZoomF = viewer.GraphWidth / Math.Abs(viewer.Graph.FindNode(goalNode.ToString()).Attr.Pos.X - viewer.Graph.FindNode(startNode.ToString()).Attr.Pos.X);
            }
            else
            {
                viewer.ZoomF = viewer.GraphHeight / Math.Abs(viewer.Graph.FindNode(goalNode.ToString()).Attr.Pos.Y - viewer.Graph.FindNode(startNode.ToString()).Attr.Pos.Y);
            }
            viewer.ZoomF *= 0.5;
            var x = new object[2];
            x[0] = viewer.Graph.FindNode(goalNode.ToString());
            x[1] = viewer.Graph.FindNode(startNode.ToString());
            viewer.ZoomInPressed();
            viewer.CenterToGroup(x);
            graphPanel.Refresh();
        }


        // Menginisialisasi gambar graph
        private void _initializeGraph()
        {
            graph = new Graph();
            for (int i = 1; i <= rootedTree.NNode; ++i)
            {
                foreach (int j in rootedTree.ChildNode[i])
                    graph.AddEdge(i.ToString(), j.ToString());
            }
            viewer.Graph = graph;
            graphPanel.SuspendLayout();
            viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            graphPanel.ResumeLayout();
        }


        // Membaca graf dari file eksternal dan menginisialisasi rootedTree
        private void _readGraphButtonClick(object sender, EventArgs e)
        {

            string filename = _getFilename();
            if (filename != "")
            {
                try
                {
                    rootedTree = new RootedTree(filename);
                    readGraphButton.Visible = false;
                    addQueryFromManualInputButton.Visible = true;
                    addQueryFromFileButton.Visible = true;
                    discardGraphButton.Visible = true;
                    viewer = new GViewer();
                    queryList = new Queue<Tuple<int, int, int>>();
                    graphPanel.Controls.Add(viewer);
                    _initializeGraph();
                }
                catch (CustomException c)
                {
                    MessageBox.Show(c.Message);
                }
                catch (Exception)
                {
                    MessageBox.Show("Error while reading file.");
                }
            }
        }


        // Membaca query secara manual dari inputtan pengguna
        private void _addQueryFromManualInputButtonClick(object sender, EventArgs e)
        {
            string query = _getQuery();
            if (query != "")
            {
                string[] splitQuery = query.Split(' ');
                try
                {
                    Tuple<int, int, int> t = new Tuple<int, int, int>
                    (
                        Int32.Parse(splitQuery[0]),
                        Int32.Parse(splitQuery[1]),
                        Int32.Parse(splitQuery[2])
                    );
                    if (rootedTree.CheckQuery(t.Item1, t.Item2, t.Item3))
                    {
                        queryList.Enqueue(t);
                        executeQueryButton.Visible = true;
                        executeAllQueryAndSaveToFileButton.Visible = true;
                    }
                    else
                        MessageBox.Show("Query format error");
                }
                catch (Exception)
                {
                    MessageBox.Show("Query Format Error");
                }
            }
        }


        // Membaca query dari file eksternal
        private void _addQueryFromFileButtonClick(object sender, EventArgs e)
        {
            string filename = _getFilename();
            if (filename != "")
            {
                using (StreamReader sr = new StreamReader(filename))
                {
                    try
                    {
                        string line = sr.ReadLine();
                        int nQuery = Int32.Parse(line);
                        int readQuery = 0;
                        for (int i = 0; i < nQuery; i++)
                        {
                            line = sr.ReadLine();
                            string[] splitLine = line.Split(' ');
                            Tuple<int, int, int> t = new Tuple<int, int, int>
                            (
                                Int32.Parse(splitLine[0]),
                                Int32.Parse(splitLine[1]),
                                Int32.Parse(splitLine[2])
                            );
                            if (rootedTree.CheckQuery(t.Item1, t.Item2, t.Item3))
                            {
                                queryList.Enqueue(t);
                                executeQueryButton.Visible = true;
                                executeAllQueryAndSaveToFileButton.Visible = true;
                                ++readQuery;
                            }
                        }
                        MessageBox.Show("Successfully added " + readQuery + " query(s).");
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Error while reading file.");
                    }
                }
            }
            
        }


        // Membuang graf yang ada sekarang
        private void _discardGraphButtonClick(object sender, EventArgs e)
        {
            readGraphButton.Visible = true;
            executeQueryButton.Visible = false;
            addQueryFromManualInputButton.Visible = false;
            addQueryFromFileButton.Visible = false;
            discardGraphButton.Visible = false;
            executeAllQueryAndSaveToFileButton.Visible = false;
            graphPanel.Controls.Clear();
        }


        // Menutup window aplikasi
        private void _quitButtonClick(object sender, EventArgs e)
        {
            this.Close();
        }


        // Memproses query terdepan dalam antrian
        private void _executeQueryButtonClick(object sender, EventArgs e)
        {
            try
            {
                Tuple<int, int, int> t = queryList.Dequeue();
                rootedTree.Query(t.Item1, t.Item2, t.Item3);
                MessageBox.Show("Query : " + t.Item1.ToString() + " " + t.Item2.ToString() + " " + t.Item3.ToString());
                _animateGraph(t.Item3, t.Item2);
                if (rootedTree.PathNode.Count == 0)
                {
                    MessageBox.Show("TIDAK\nTidak ada jawaban untuk query tersebut.");
                }
                else
                {
                    string path = "";
                    foreach (int el in rootedTree.PathNode)
                    {
                        path = "->" + el.ToString() + path;
                    }

                    MessageBox.Show("YA\nJawaban untuk query tersebut telah ditemukan.\nPath :  " + path.Substring(2));
                }

            }
            catch(CustomException c)
            {
                MessageBox.Show(c.Message);
            }

            if (queryList.Count == 0)
            {
                executeQueryButton.Visible = false;
                executeAllQueryAndSaveToFileButton.Visible = false;
            }
        }

        // Memproses semua query yang berada pada queryList dan memasukkan hasilnya ke file eksternal
        private void _executeAllQueryAndSaveToFIleButtonClick(object sender, EventArgs e)
        {
            string filename = _getOutFilename();
            if (filename != "")
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@AppDomain.CurrentDomain.BaseDirectory + filename))
                {
                    while (queryList.Count > 0)
                    {
                        Tuple<int, int, int> t = queryList.Dequeue();
                        rootedTree.Query(t.Item1, t.Item2, t.Item3);
                        if (rootedTree.PathNode.Count > 0)
                        {
                            file.WriteLine("Ya");
                        }
                        else
                        {
                            file.WriteLine("Tidak");
                        }
                    }
                    executeQueryButton.Visible = false;
                    executeAllQueryAndSaveToFileButton.Visible = false;
                    MessageBox.Show("Output berhasil disimpan di " + filename);
                }
            }
        }
    }
}
