using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KinoTTHK_K
{
    public partial class TimeFilm : Form
    {
        SqlConnection connect = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename =|DataDirectory|KinoDB.mdf; Integrated Security = True");
        SqlCommand cmd;
        SqlDataAdapter adapter;

        ComboBox listTime;
        ListBox listLanguage;
        PictureBox listPicture;


        public TimeFilm()
        {
            connect.Open();
            this.Text = "Choose Film";
            adapter = new SqlDataAdapter("SELECT * FROM Time", connect);
            DataTable film_tabel = new DataTable();
            adapter.Fill(film_tabel);
            listTime = new ComboBox();
            listTime.Font = new Font("Calibri", 12);
            listTime.Location = new Point(10, 20);
            listTime.Text = "Vali aeg";
            foreach (DataRow row in film_tabel.Rows)
            {
                listTime.Items.Add(row["TimeFilm"]);
            }
            //listTime.MouseClick += listTime_SelectedIndexChanged;
            this.Controls.Add(listTime);
            connect.Close();


            connect.Open();
            adapter = new SqlDataAdapter("SELECT * FROM Film", connect);
            DataTable language_tabel = new DataTable();
            adapter.Fill(language_tabel);
            listLanguage = new ListBox();
            listLanguage.Font = new Font("Calibri", 12);
            listLanguage.Location = new Point(10, 80);
            foreach (DataRow row in language_tabel.Rows)
            {
                listLanguage.Items.Add(row["Language"]);
            }
            this.Controls.Add(listLanguage);
            connect.Close();

            connect.Open();
            adapter = new SqlDataAdapter("SELECT * FROM Film", connect);
            DataTable picture_tabel = new DataTable();
            adapter.Fill(picture_tabel);
            listPicture = new PictureBox();
            listPicture.Size = new Size(250, 250);
            listPicture.Location = new Point(210, 20);
            connect.Close();
        }
    }


    /*private void listTime_SelectedIndexChanged(object sender, EventArgs e)
    {
        
    }*/
}
