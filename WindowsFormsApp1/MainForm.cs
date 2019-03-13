using System;
using System.Drawing;
using System.Windows.Forms;

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
            Button b1 = new Button {Text = "Read Graph From File", Location = new Point(10, 10), Size = new Size(150, 20)};
            b1.Click += new EventHandler(Button1_Click);
            Controls.Add(b1);
            Button b2 = new Button {Text = "Read Query Manual", Location = new Point(100, 100), Size = new Size(150,20)};
            b2.Click += new EventHandler(Button2_Click);
            Controls.Add(b2);
            Button b3 = new Button {Text = "Read Query From File", Location = new Point(200, 200), Size = new Size(150, 20)};
            b3.Click += new EventHandler(Button3_Click);
            Controls.Add(b3);
        }

        private static string GetFilename()
        {
            OpenFileDialog dialogue = new OpenFileDialog();
            if (dialogue.ShowDialog() == DialogResult.OK)
            {
                string filename = dialogue.FileName;
                return filename;
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
                treeRead = true;
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (treeRead)
            {

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

        public static string ShowDialog(string text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 500,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 50, Top = 20, Text = text };
            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }
    }
}
