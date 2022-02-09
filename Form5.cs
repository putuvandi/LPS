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
    public partial class Form5 : Form
    {
        MySqlConnection con;
        ConnectionDB db = new ConnectionDB();
        public Form5()
        {
            InitializeComponent();
            con = new MySqlConnection(db.getConnection());
        }

        private void cari(string code)
        {
            this.dataGridView1.DataSource = null;
            this.dataGridView1.Rows.Clear();
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = @"select lps.time,notaproses.nota, notaproses.customer, notaproses.loading, notaproses.terkirim, notaproses.kembali, notaproses.keterangan, lps.no_kendaraan, lps.driver, lps.helper, lps.periode, lps.tgl, lps.id_lps from notaproses JOIN lps ON lps.id_lps = notaproses.id_lps where (notaproses.nota LIKE '%" + code + "%') or (notaproses.customer LIKE '%" + code + "%') ORDER BY lps.id_lps desc";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;

                cmd.Parameters.Add("@code", MySqlDbType.VarChar).Value = code;
                //cmd.Parameters.Add("@akhir", MySqlDbType.VarChar).Value = akhir;
                con.Open();
                cmd.ExecuteNonQuery();


                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {

                    string date = $"{reader.GetString("tgl")}" + " " + $"{reader.GetString("periode")}";
                    this.dataGridView1.Rows.Add($"{reader.GetString("nota")}", $"{reader.GetString("customer")}", $"{reader.GetString("time")}", $"{reader.GetString("loading")}", $"{reader.GetString("terkirim")}", $"{reader.GetString("kembali")}", $"{reader.GetString("keterangan")}", $"{reader.GetString("no_kendaraan")}", $"{reader.GetString("driver")}", $"{reader.GetString("helper")}", date, $"{reader.GetString("id_lps")}");

                }
                cmd.Parameters.Clear();
            }


            con.Close();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cari(textBox1.Text);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cari(textBox1.Text);
        }
    }
}
