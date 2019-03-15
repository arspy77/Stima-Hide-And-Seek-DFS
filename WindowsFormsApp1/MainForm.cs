using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace WindowsFormsApp1
{
    public partial class MainForm : Form
    {
        // Attribute
        private RootedTree st;
        private bool treeRead = false;

        //Method
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Button b1 = new Button { Text = "Read Graph From File", Location = new Point(10, 10), Size = new Size(150, 20) };
            b1.Click += new EventHandler(Button1_Click);
            Controls.Add(b1);
            Button b2 = new Button { Text = "Read Query Manual", Location = new Point(100, 100), Size = new Size(150, 20) };
            b2.Click += new EventHandler(Button2_Click);
            Controls.Add(b2);
            Button b3 = new Button { Text = "Read Query From File", Location = new Point(200, 200), Size = new Size(150, 20) };
            b3.Click += new EventHandler(Button3_Click);
            Controls.Add(b3);
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

        private void Button1_Click(object sender, EventArgs e)
        {
            string filename = GetFilename();
            if (filename != "")
            {
                st = new RootedTree(filename);
                st.ConvertToTree();
                treeRead = true;
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (treeRead)
            {
                try
                {
                    string query = GetQueryDialogue();
                    string[] splitQuery = query.Split(' ');
                    int type = Int32.Parse(splitQuery[0]);
                    int x = Int32.Parse(splitQuery[1]);
                    int y = Int32.Parse(splitQuery[2]);
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

        private void Button3_Click(object sender, EventArgs e)
        {
            if (treeRead)
            {
                string filename = GetFilename();

            }
            else
            {
                MessageBox.Show("Graph file must be read first.");
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
    }
}
