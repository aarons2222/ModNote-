using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace ModNote
{
    
     public partial class Form1 : Form
    {
        public  void regex()
        {
           string fileName = listBox1.GetItemText(listBox1.SelectedItem);
            string dueDate = File.ReadAllText(fileName);

            Regex regex = new Regex(@"\d{2}/\d{2}/\d{4}");
            Match mat = regex.Match(dueDate);
            string duedate = mat.ToString();
            label3.Text = duedate;

            string currentDate = DateTime.Today.ToString("dd/MM/yyyy");
            label5.Text = "Today's date: " + currentDate;

            DateTime due_date = Convert.ToDateTime(duedate);
            DateTime current_date = Convert.ToDateTime(currentDate);
            TimeSpan difference = due_date - current_date;

            var days = difference.TotalDays;

            if (days >= 0)
            {
                label5.ForeColor = System.Drawing.Color.Black;
                label5.Text = "You have " + days.ToString() + "  days until your deadline!";
            }
            else {
                label5.ForeColor = System.Drawing.Color.Red;

                label5.Text = "The deadline for this assignment has passed!!";
            }
        }
        public void globals()//global method for code reuse
        { //disable buttons if list is empty
            if (listBox1.Items.Count == 0)
            {
                button3.Enabled = false;
                richTextBox1.Text = "Add module or create new";
            }
            else button3.Enabled = true;
            if (listBox1.Items.Count == 0)
                button4.Enabled = false;
            else button4.Enabled = true;
            if (listBox1.Items.Count == 0)
                button5.Enabled = false;
            else button5.Enabled = true;
            //amount of modules counter
            string value = listBox1.Items.Count.ToString();
            label2.Text = "Number of modules: " + value;

            if (listBox1.Items.Count == 0)
            {
                label3.Text = "";
            }

            if (listBox1.Items.Count > 0)
            {
                listBox1.SelectedIndex = 0;
            }
        }
        public Form1()
        {
            InitializeComponent();
            DirectoryInfo dir = new DirectoryInfo(@"../Debug/");
            FileInfo[] files = dir.GetFiles("*.txt");
            Dictionary<FileInfo, DateTime> filesWithDueDate = new Dictionary<FileInfo, DateTime>();

            foreach (FileInfo file in files)
            {
                string dueDate = File.ReadAllText(file.FullName);

                Regex regex = new Regex(@"\d{2}/\d{2}/\d{4}");
                Match mat = regex.Match(dueDate);

                DateTime duedate = Convert.ToDateTime(mat.ToString());

                filesWithDueDate.Add(file, duedate);
            }

            var sortedFiles = filesWithDueDate.OrderBy(a => a.Value).Select(b => b.Key.Name).ToArray();
            listBox1.Items.AddRange(sortedFiles);
            globals();
            string currentDate = DateTime.Today.ToString("dd/MM/yyyy");
            label1.Text = "Today's date: " + currentDate;

            globals();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog op = new OpenFileDialog())
            {
                op.Filter = "Text Files|*.txt"; // filter
        
                    if (op.ShowDialog() == DialogResult.OK)
                    {
                        richTextBox1.LoadFile(op.FileName, RichTextBoxStreamType.PlainText);

                        this.Text = op.FileName;
                        string filename = Path.GetFileName(op.FileName);
                        StreamWriter writer = new StreamWriter(filename);
                        string newFile = filename;
                    
                        if (!listBox1.Items.Contains(newFile))
                            listBox1.Items.Add(newFile);
                        using (writer)
                        {
                            writer.Write(richTextBox1.Text);
                        }
                    globals();
                  
                     MessageBox.Show("Note added!", "ModNote",
                     MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
             } // Clean up the OpenFileDialog instance     
            }

        //exit application
        
        public void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string fileName = listBox1.GetItemText(listBox1.SelectedItem);
            if (File.Exists(fileName))
            {
             regex();
            }
            if (File.Exists(fileName))
            {
                richTextBox1.Text = File.ReadAllText(fileName);
            }
        }
             //delete
         private void button3_Click(object sender, EventArgs e)
         {
            DialogResult dr = MessageBox.Show("Are you sure you want to delete that module?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                //Gets selected listbox item as string
                string fileName = listBox1.GetItemText(listBox1.SelectedItem);

                if (File.Exists(fileName))
                {   // not deleting, yet!
                    File.Delete(fileName);
                }
                //removes selected item from listbox
                foreach (int Index in listBox1.SelectedIndices.Cast<int>().Select(x => x).Reverse())
                listBox1.Items.RemoveAt(Index);

                richTextBox1.Clear();
                richTextBox1.Focus();
         
                globals();//Remove this if not needed
               
                MessageBox.Show("Note deleted!!!", "ModNote",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
                //delete row from database or datagridview...
            }
            else if (DialogResult == DialogResult.No)
            {
                //Nothing to do
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 m = new Form2();
            m.ShowDialog();
            this.Text = m.code;
            string code = m.code + ".txt";
            listBox1.Items.Add(code);
            globals();
        }
       //Save notes
       private void button4_Click(object sender, EventArgs e)
        {
           string notes = richTextBox1.Text;
           string fileName = listBox1.GetItemText(listBox1.SelectedItem);

            if (string.IsNullOrWhiteSpace(richTextBox1.Text))
            {
                MessageBox.Show("You must choose a module!!", "ModNote",
                MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }else{
                // Write the string to a file.
                System.IO.StreamWriter file = new System.IO.StreamWriter(fileName);
                file.WriteLine(notes);

                file.Close();
                MessageBox.Show("Note saved!", "ModNote",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            richTextBox1.AppendText("\r\r>>>>>>>>>>>>>>>> TYPE NOTES BELOW <<<<<<<<<<<<<<<<<");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            listBox1.SelectedItems.Clear();

            for (int i = listBox1.Items.Count - 1; i >= 0; i--)
            {
                if (listBox1.Items[i].ToString().ToLower().Contains(searchBox.Text.ToLower()))
                {
                    listBox1.SetSelected(i, true);
                }
            }
        }

        private void searchBox_MouseClick(object sender, MouseEventArgs e)
        {
              searchBox.Text = string.Empty;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to close ModNote?", "Close Application", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                e.Cancel = true;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure you want close ModNote?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                Application.Exit();
            }
            else { // do nothing
            }
        }
    }
}





