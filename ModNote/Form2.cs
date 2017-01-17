using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ModNote
{
    public partial class Form2 : Form
    {
       
        public Form2()
        {
          
            InitializeComponent();

        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            Close(); 
        }

        public void button1_Click(object sender, EventArgs e)
        {
            string title = textBox2.Text;
            string courseCode = textBox1.Text;
            string dueDate = dateTimePicker1.Text;
            string notes = richTextBox1.Text;


            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("You must add a module code!", "ModNote",
                MessageBoxButtons.OK, MessageBoxIcon.Stop);

            }

           

            else {
                // Compose a string that consists of three lines.

                // Write the string to a file.
                System.IO.StreamWriter file = new System.IO.StreamWriter(courseCode + ".txt");



                file.WriteLine("CODE:\r" + courseCode + "\r\rTITLE\r" + title + "\r\rSYNOPSIS\r" + notes + "\r\rDUE DATE\r" + dueDate);

                file.Close();

               
           

                MessageBox.Show("Note saved!", "ModNote",
                   MessageBoxButtons.OK, MessageBoxIcon.Information);
             
                this.Close();
                }
          }

        public string code
        {
            get { return textBox1.Text; }
        }
      }
}
