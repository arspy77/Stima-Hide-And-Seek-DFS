using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;

namespace WindowsFormsApp1
{
    public partial class MainForm : Form
    {
        // Attribute
        private RootedTree st;
        private bool treeRead = false;
        private List<Tuple<int, int, int>> queryList;
        private int nQuery;

        //Method
        public MainForm()
        {
            InitializeComponent();
        }


        private static string GetFilename()
        {
            //return "C:/Users/Ariel Razumardi/source/repos/WindowsFormsApp1/Stima-Hide-And-Seek-DFS/WindowsFormsApp1/TextFile1.txt";
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


        public static string GetQueryDialogue()
        {
            Form prompt = new Form()
            {
                Width = 500,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "GetQueryDialogue",
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 50, Top = 20, Text = "Query : " };
            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;
            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }

        private void Button1_Click_1(object sender, EventArgs e)
        {
            
            //create a form 
            System.Windows.Forms.Form form = new System.Windows.Forms.Form();
            //create a viewer object 
            Microsoft.Msagl.GraphViewerGdi.GViewer viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
            //create a graph object 
            Microsoft.Msagl.Drawing.Graph graph = new Microsoft.Msagl.Drawing.Graph("graph");
            //create the graph content 
            for (int i = 1; i <= st.n; ++i)
            {
                foreach (int j in st.childNode[i])
                    graph.AddEdge(i.ToString(), j.ToString());
            }
            viewer.Graph = graph;
            //associate the viewer with the form 
            form.SuspendLayout();
            viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            form.Controls.Add(viewer);
            form.ResumeLayout();
            //show the form 
            form.ShowDialog();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            string filename = GetFilename();
            if (filename != "")
            {
                st = new RootedTree(filename);
                treeRead = true;
                button1.Visible = true;
                button2.Visible = false;
                button3.Visible = true;
                button4.Visible = true;
                button5.Visible = true;
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (treeRead)
            {
                try
                {
                    string query = GetQueryDialogue();
                    string[] splitQuery = query.Split(' ');
                    int mode = Int32.Parse(splitQuery[0]);
                    int x = Int32.Parse(splitQuery[1]);
                    int y = Int32.Parse(splitQuery[2]);
                    st.Query(mode, x, y);
                    string s1 = "", s2 = "";
                    foreach (int el in st.checkedNode)
                    {
                        s1 += el.ToString() + " ";
                    }

                    foreach (int el in st.pathNode)
                    {
                        s2 = " " + el.ToString() + s2;
                    }

                    MessageBox.Show("checked : " + s1);
                    MessageBox.Show("path : " + s2);
                }
                catch (Exception)
                {
                    MessageBox.Show("Query Format Error");
                }
            }
            else
            {
                MessageBox.Show("Graph file must be read first.");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (treeRead)
            {
                string filename = GetFilename();
                using (StreamReader sr = new StreamReader(filename))
                {
                    string line = sr.ReadLine();
                    nQuery = Int32.Parse(line);
                    queryList = new List<Tuple<int, int, int>>();
                    for (int i = 0; i < nQuery; i++)
                    {
                        line = sr.ReadLine();
                        string[] splitLine = line.Split(' ');
                        Tuple<int, int, int> t = new Tuple<int, int, int>(Int32.Parse(splitLine[0]), Int32.Parse(splitLine[0]), Int32.Parse((splitLine[2])));
                        queryList.Add(t);
                    }
                }
            }
            else
            {
                MessageBox.Show("Graph file must be read first.");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string filename = GetFilename();
            if (filename != "")
            {
                st = new RootedTree(filename);
                treeRead = true;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
