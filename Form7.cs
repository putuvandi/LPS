using MySql.Data.MySqlClient;
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
    public partial class Form7 : Form
    {
        MySqlConnection con;
        ConnectionDB db = new ConnectionDB();
        string message3;
        string gettable;
        string getname;
        string getid;
        int code;
        public Form7()
        {
            InitializeComponent();
            con = new MySqlConnection(db.getConnection());
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text.Equals("DRIVER"))
            {
                code = 1;
                getid = "iddriver";
                gettable = "tbdriver";
                getname = "driver";
                getdata(gettable, code);

            }
            else if (comboBox1.Text.Equals("HELPER"))
            {
                code = 2;
                getid = "id";
                gettable = "tbhelper";
                getname = "namaHelper";
                getdata(gettable, code);

            }
        }

        private void save(string table, string nama)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (con.State != ConnectionState.Open)
                {
                    con.Close();
                    con.Open();
                }
                else
                {
                    con.Open();
                }
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.CommandText = @"insert into " + gettable + " (" + getname + ")values(@getnama)";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;

                    cmd.Parameters.Add("@table", MySqlDbType.VarChar).Value = table;
                    cmd.Parameters.Add("@nama", MySqlDbType.VarChar).Value = nama;
                    cmd.Parameters.Add("@getnama", MySqlDbType.VarChar).Value = textBox1.Text;

                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Berhasil Menyimpan data!!");
                    textBox1.Text = "";
                    getdata(gettable, code);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error6");
            }
        }

        private void delete()
        {
            try
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Close();
                    con.Open();
                }
                else
                {
                    con.Open();
                }
                using (MySqlCommand cmd = new MySqlCommand())
                {

                    cmd.CommandText = @"delete from " + gettable + " where " + getname + "=@nama";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;

                    cmd.Parameters.Add("@gettable", MySqlDbType.VarChar).Value = gettable;
                    cmd.Parameters.Add("@getname", MySqlDbType.VarChar).Value = getname;
                    cmd.Parameters.Add("@nama", MySqlDbType.VarChar).Value = textBox1.Text;

                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Berhasil Menghapus data!!");
                    getdata(gettable, code);
                    textBox1.Text = "";
                    // getid();

                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error9");
            }
        }

        private void update()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (con.State != ConnectionState.Open)
                {
                    con.Close();
                    con.Open();
                }
                else
                {
                    con.Open();
                }
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.CommandText = @"update " + gettable + " set " + getname + "=@nama where " + getid + "=@message";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;

                    cmd.Parameters.Add("@gettable", MySqlDbType.VarChar).Value = gettable;
                    cmd.Parameters.Add("@getname", MySqlDbType.VarChar).Value = getname;
                    cmd.Parameters.Add("@nama", MySqlDbType.VarChar).Value = textBox1.Text;
                    cmd.Parameters.Add("@getid", MySqlDbType.VarChar).Value = getid;
                    cmd.Parameters.Add("@message", MySqlDbType.VarChar).Value = message3;

                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Berhasil Mengupdate data!!");
                    textBox1.Text = "";
                    getdata(gettable, code);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error6");
            }
        }

        private void getdata(string query, int code)
        {
            this.dataGridView1.DataSource = null;
            this.dataGridView1.Rows.Clear();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"select * from " + query;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;

                cmd.Parameters.Add("@query", MySqlDbType.VarChar).Value = query;

                if (con.State != ConnectionState.Open)
                {
                    con.Close();
                    con.Open();
                }
                else
                {
                    con.Open();
                }
                cmd.ExecuteNonQuery();
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    if (code == 1)
                    {
                        dataGridView1.Rows.Add($"{reader.GetString("iddriver")}", $"{reader.GetString("driver")}");
                    }
                    else if (code == 2)
                    {
                        dataGridView1.Rows.Add($"{reader.GetString("id")}", $"{reader.GetString("namaHelper")}");
                    }



                }
            }

            con.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text.Equals(""))
            {
                MessageBox.Show("Pilih terlebih dahulu pada combobox!!");
            }
            else
            {
                save(gettable, getname);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            message3 = (dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            textBox1.Text = (dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            update();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            delete();
        }
    }
}
