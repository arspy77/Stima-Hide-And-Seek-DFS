using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.Threading.Tasks;
using WindowsFormsApp1;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.GraphViewerGdi;
using Label = System.Windows.Forms.Label;

namespace WindowsFormsApp1
{
    public partial class MainForm : Form
    {
        // Attribute
        private RootedTree st;
        private Queue<Tuple<int, int, int>> queryList;
        private Microsoft.Msagl.Drawing.Graph graph;
        private Microsoft.Msagl.GraphViewerGdi.GViewer viewer;


        //Method
        public MainForm()
        {
            InitializeComponent();
        }


        private static string GetFilename()
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

        private void animateGraph()
        {
            initializeGraph();
            foreach (int el in st.checkedNode)
            {
                graph.FindNode(el.ToString()).Attr.FillColor = Microsoft.Msagl.Drawing.Color.LightPink;
                viewer.Graph = graph;
                panel1.Controls.Clear();
                panel1.Controls.Add(viewer);
                var taskwait = Task.Delay(1000);
                taskwait.Wait();
            }
            int[] path = st.pathNode.ToArray();
            for (int i = path.Length-1; i >= 0; i--)
            {
                graph.FindNode(path[i].ToString()).Attr.FillColor = Microsoft.Msagl.Drawing.Color.RoyalBlue;
                viewer.Graph = graph;
                panel1.Controls.Clear();
                panel1.Controls.Add(viewer);
                var taskwait = Task.Delay(1000);
                taskwait.Wait();
            }
        }

        private void initializeGraph()
        {
            //create a viewer object 
            viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
            //create a graph object 
            graph = new Microsoft.Msagl.Drawing.Graph("graph");
            //create the graph content 
            for (int i = 1; i <= st.n; ++i)
            {
                foreach (int j in st.childNode[i])
                    graph.AddEdge(i.ToString(), j.ToString());
            }
            viewer.Graph = graph;
            panel1.SuspendLayout();
            viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            panel1.Controls.Clear();
            panel1.Controls.Add(viewer);
            panel1.ResumeLayout();
        }


        private void button2_Click_1(object sender, EventArgs e)
        {

            string filename = GetFilename();
            if (filename != "")
            {
                try
                {
                    st = new RootedTree(filename);
                    button2.Visible = false;
                    button3.Visible = true;
                    button4.Visible = true;
                    button5.Visible = true;
                    queryList = new Queue<Tuple<int, int, int>>();
                    initializeGraph();
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

        private void button3_Click_1(object sender, EventArgs e)
        {
            try
            {
                string query = GetQueryDialogue();
                string[] splitQuery = query.Split(' ');
                Tuple<int,int,int> t = new Tuple<int, int, int>
                (
                    Int32.Parse(splitQuery[0]),
                    Int32.Parse(splitQuery[1]),
                    Int32.Parse(splitQuery[2])
                );
                queryList.Enqueue(t);
                button1.Visible = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Query Format Error");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string filename = GetFilename();
            if (filename != "")
            {
                using (StreamReader sr = new StreamReader(filename))
                {
                    try
                    {
                        string line = sr.ReadLine();
                        int nQuery = Int32.Parse(line);
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
                            queryList.Enqueue(t);
                            button1.Visible = true;
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Error while reading file.");
                    }
                }
            }
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            button2.Visible = true;
            button1.Visible = false;
            button3.Visible = false;
            button4.Visible = false;
            button5.Visible = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Tuple<int, int, int> t = queryList.Dequeue();
            st.Query(t.Item1, t.Item2, t.Item3);
            MessageBox.Show("Query : " + t.Item1.ToString() + " " + t.Item2.ToString() + " " + t.Item3.ToString());
            animateGraph();
            if (st.pathNode.Count == 0)
            {
                MessageBox.Show("Tidak ada jawaban untuk query tersebut.");
            }
            else
            {
                MessageBox.Show("Jawaban untuk query tersebut telah ditemukan.");
            }

            if (queryList.Count == 0)
            {
                button1.Visible = false;
            }
        }
    }
}
