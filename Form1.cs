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
    public partial class Form1 : Form
    {
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;
        public Form1()
        {
            InitializeComponent();
            loadform(new Form2(0));
        }

        public void loadform(object Form)
        {
            if (this.panel7.Controls.Count > 0)
                this.panel7.Controls.RemoveAt(0);

            Form f = Form as Form;
            f.TopLevel = false;
            f.Dock = DockStyle.Fill;
            this.panel7.Controls.Add(f);
            this.panel7.Tag = f;
            f.Show();
        }

        private void position(Button b)
        {
            p1.Location = new Point(b.Location.X - p1.Width, b.Location.Y);
            foreach (Control ctr in p1.Controls)
            {
                if (ctr.GetType() == typeof(Button))
                {
                    if (ctr.Name == b.Name)
                    {
                        b.BackColor = Color.FromArgb(25, 118, 210);
                    }
                    else
                    {
                        ctr.BackColor = Color.FromArgb(77, 86, 86);
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            position(button5);
            loadform(new Form2(2));
        }

        private void button6_Click(object sender, EventArgs e)
        {
            position(button6);
            loadform(new Form5());
        }

        private void button5_Click(object sender, EventArgs e)
        {
            position(button5);
            loadform(new Form2(3));
        }

        private void button7_Click(object sender, EventArgs e)
        {
            position(button5);
            loadform(new Form2(1));
        }

        private void button9_Click(object sender, EventArgs e)
        {
            position(button9);
            loadform(new Form6());
        }

        private void button11_Click(object sender, EventArgs e)
        {
            position(button11);
            loadform(new Form7());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form8 f8 = new Form8();
            f8.Show();
        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }

        private void panel2_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        private void panel2_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            panel7.Location = new Point(panel7.Location.X - 10);
        }
    }
}
