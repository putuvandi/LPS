using ExcelDataReader;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LPS
{
    public partial class Form8 : Form
    {
        MySqlConnection con;
        ConnectionDB db = new ConnectionDB();
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;
        string key = null;
        public Form8()
        {
            InitializeComponent();
            con = new MySqlConnection(db.getConnection());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "Excel 97-2003 Workbook|*.xls|Excel Workbook|*.xlsx" })
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    textBox1.Text = openFileDialog.FileName;

                    using (var stream = File.Open(openFileDialog.FileName, FileMode.Open, FileAccess.Read))
                    {
                        using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration()
                            {
                                ConfigureDataTable = (_) => new ExcelDataTableConfiguration() { UseHeaderRow = true }
                            });
                            tableCollection = result.Tables;
                           // comboBox1.Items.Clear();
                            foreach (DataTable table in tableCollection)
                               // comboBox1.Items.Add(table.TableName);
                                key = table.TableName;
                           // MessageBox.Show(key);
                             DataTable dt = tableCollection[key];
                              dataGridView1.DataSource = dt;
                            comboBox1.SelectedIndex = comboBox1.FindStringExact("LPS");
                        }
                    }
                }
            }
        }

        DataTableCollection tableCollection;
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //DataTable dt = tableCollection[comboBox1.SelectedItem.ToString()];
           // dataGridView1.DataSource = dt;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text.Equals("LPS"))
            {
                importLPS();
            }
            else if (comboBox1.Text.Equals("CUST"))
            {
                importCUST();
            }
            else
            {
                MessageBox.Show("Periksa kembali file yang diupload sudah benar!!", "ERROR!!");
            }
        }

        private void importCUST()
        {
            Cursor.Current = Cursors.WaitCursor;
            con.Open();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = con;

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                     //   string date = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
                     if(dataGridView1.Rows[i].Cells[0].Value == null)
                    {

                    }else
                    {
                        string kode = dataGridView1.Rows[i].Cells[0].Value.ToString();
                        string namatemp = dataGridView1.Rows[i].Cells[1].Value.ToString();
                        string kontaktemp = dataGridView1.Rows[i].Cells[2].Value.ToString();
                        string nama = namatemp.Replace("'", "''");
                        string kontak = kontaktemp.Replace("'", "''");
                        string area = dataGridView1.Rows[i].Cells[4].Value.ToString();

                        cmd.CommandText = @"select * from tbcust where id=@id";
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;

                        cmd.Parameters.Add("@id", MySqlDbType.VarChar).Value = kode;
                        // MySqlCommand cmm2 = new MySqlCommand("select * from tbnota where nota='" + customer + "'", con);
                        Object obj2 = cmd.ExecuteScalar();
                        if (obj2 == null)
                        {
                            cmd.CommandText = @"insert into tbcust (id,namacust,kontak,area)values(@id2,@namacust2,@kontak2,@area2)";
                            cmd.CommandType = CommandType.Text;
                            cmd.Connection = con;

                            cmd.Parameters.Add("@id2", MySqlDbType.VarChar).Value = kode;
                            cmd.Parameters.Add("@namacust2", MySqlDbType.VarChar).Value = nama;
                            cmd.Parameters.Add("@kontak2", MySqlDbType.VarChar).Value = kontak;
                            cmd.Parameters.Add("@area2", MySqlDbType.VarChar).Value = area;
                            // string StrQuery = @"insert into tbnota (nota,date,customer,br,tempo)values('" + nota + "','" + date + "','" + customer + "','" +br+ "','" +tempo+ "')";
                            //cmd.CommandText = StrQuery;
                            cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            cmd.CommandText = @"UPDATE tbcust SET namacust=@namacust3,kontak=@kontak3,area=@area3 WHERE id=@id3 ";
                            cmd.CommandType = CommandType.Text;
                            cmd.Connection = con;

                            cmd.Parameters.Add("@id3", MySqlDbType.VarChar).Value = kode;
                            cmd.Parameters.Add("@namacust3", MySqlDbType.VarChar).Value = nama;
                            cmd.Parameters.Add("@kontak3", MySqlDbType.VarChar).Value = kontak;
                            cmd.Parameters.Add("@area3", MySqlDbType.VarChar).Value = area;
                            //string StrQuery = @"UPDATE tbnota SET customer='"+customer+"',br='"+br+"',tempo='"+tempo+"',date='"+date+"' WHERE nota='"+nota+"' ";
                            //   cmd.CommandText = StrQuery;
                            cmd.ExecuteNonQuery();
                        }


                        cmd.Parameters.Clear();
                    }
                        
                }
                con.Close();
                MessageBox.Show("Berhasil Mengimport data customer!!");
            }
            // this.Close();
            this.dataGridView1.DataSource = null;
            this.dataGridView1.Rows.Clear();
            comboBox1.Text = "";
            textBox1.Text = "";
            //  comboBox1.Items.Clear();
            tableCollection.Clear();
            Cursor.Current = Cursors.Default;
        }

        private void importLPS()
        {
            Cursor.Current = Cursors.WaitCursor;
            con.Open();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = con;

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {


                    if (Convert.ToInt32(dataGridView1.Rows[i].Cells[0].Value) == 0)
                    {

                    }
                    else
                    {
                        string date = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
                        string nota = dataGridView1.Rows[i].Cells[1].Value.ToString();
                        int tempotmp = Int16.Parse(dataGridView1.Rows[i].Cells[2].Value.ToString());
                        string kode = dataGridView1.Rows[i].Cells[4].Value.ToString();
                        string customertmp = dataGridView1.Rows[i].Cells[5].Value.ToString();
                        string customer = customertmp.Replace("'", "''");
                        int brtemp = Int32.Parse(dataGridView1.Rows[i].Cells[11].Value.ToString());
                        string br;
                        string tempo;
                        if (tempotmp == 0)
                        {
                            tempo = "CASH";
                        }
                        else
                        {
                            tempo = "TOP";
                        }
                        if (brtemp < 1000000)
                        {
                            br = "LESS";
                        }
                        else
                        {
                            br = "";
                        }
                        cmd.CommandText = @"select * from tbnota where nota=@nota";
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;

                        cmd.Parameters.Add("@nota", MySqlDbType.VarChar).Value = nota;
                        // MySqlCommand cmm2 = new MySqlCommand("select * from tbnota where nota='" + customer + "'", con);
                        Object obj2 = cmd.ExecuteScalar();
                        if (obj2 == null)
                        {
                            cmd.CommandText = @"insert into tbnota (nota,date,customer,br,kodecust,tempo)values(@nota2,@date2,@customer2,@br2,@kodecust2,@tempo2)";
                            cmd.CommandType = CommandType.Text;
                            cmd.Connection = con;

                            cmd.Parameters.Add("@nota2", MySqlDbType.VarChar).Value = nota;
                            cmd.Parameters.Add("@date2", MySqlDbType.VarChar).Value = date;
                            cmd.Parameters.Add("@customer2", MySqlDbType.VarChar).Value = customer;
                            cmd.Parameters.Add("@br2", MySqlDbType.VarChar).Value = br;
                            cmd.Parameters.Add("@kodecust2", MySqlDbType.VarChar).Value = kode;
                            cmd.Parameters.Add("@tempo2", MySqlDbType.VarChar).Value = tempo;
                            // string StrQuery = @"insert into tbnota (nota,date,customer,br,tempo)values('" + nota + "','" + date + "','" + customer + "','" +br+ "','" +tempo+ "')";
                            //cmd.CommandText = StrQuery;
                            cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            cmd.CommandText = @"UPDATE tbnota SET customer=@customer3,br=@br3,kodecust=@kodecust3,tempo=@tempo3,date=@date3 WHERE nota=@nota3 ";
                            cmd.CommandType = CommandType.Text;
                            cmd.Connection = con;

                            cmd.Parameters.Add("@nota3", MySqlDbType.VarChar).Value = nota;
                            cmd.Parameters.Add("@date3", MySqlDbType.VarChar).Value = date;
                            cmd.Parameters.Add("@customer3", MySqlDbType.VarChar).Value = customer;
                            cmd.Parameters.Add("@br3", MySqlDbType.VarChar).Value = br;
                            cmd.Parameters.Add("@kodecust3", MySqlDbType.VarChar).Value = kode;
                            cmd.Parameters.Add("@tempo3", MySqlDbType.VarChar).Value = tempo;
                            //string StrQuery = @"UPDATE tbnota SET customer='"+customer+"',br='"+br+"',tempo='"+tempo+"',date='"+date+"' WHERE nota='"+nota+"' ";
                            //   cmd.CommandText = StrQuery;
                            cmd.ExecuteNonQuery();
                        }

                    }
                    cmd.Parameters.Clear();
                }
                con.Close();
                MessageBox.Show("Berhasil Mengimport data!!");
            }
            // this.Close();
            this.dataGridView1.DataSource = null;
            this.dataGridView1.Rows.Clear();
            comboBox1.Text = "";
            textBox1.Text = "";
            //  comboBox1.Items.Clear();
            tableCollection.Clear();
            Cursor.Current = Cursors.Default;
        }

        private void Form8_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        private void Form8_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void Form8_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Equals(""))
            {

            }
            else if (key != null)
            {
                DataTable dt = tableCollection[key];
                dataGridView1.DataSource = dt;
            }
        }
    }
}
