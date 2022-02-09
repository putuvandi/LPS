using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LPS
{
    public partial class Form4 : Form
    {
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;
        Form2 f1;
        int code;
        public Form4(Form2 f, int kode)
        {
            InitializeComponent();
            f1 = f;
            code = kode;
            if (code == 3)
            {
                button1.Text = "OK";
                txtcari.Hide();
                textBox2.Focus();
                label1.Text = "Masukkan Jam yang diinginkan (HH:mm)";
                this.ActiveControl = textBox2;
            }
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
        }

        private const int CS_DropShadow = 0x00020000;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CS_DropShadow;
                return cp;
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                proses(txtcari.Text);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            proses(txtcari.Text);
        }

        private void proses(String lps)
        {
            if (code == 1)
            {
                f1.getdata(lps, 1);
            }
            else if (code == 2)
            {
                f1.getdata(lps, 2);
            }
            else if (code == 3)
            {
                f1.changetime(textBox2.Text + label2.Text + textBox3.Text);
            }
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtcari_Click(object sender, EventArgs e)
        {

        }

        private void Form4_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }

        private void Form4_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        private void Form4_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }
    }
}
