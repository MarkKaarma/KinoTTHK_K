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
    public partial class Roomid : Form
    {
        int i, j;
        int room;
        Label[,] _arr;
        Button buy;
        //Timer tm;
        Label label1;
        private List<Label> _labels;
        List<string> pol = new List<string>();



        SqlCommand cmd;
        SqlDataAdapter adapter;
        SqlConnection connect = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename =|DataDirectory|KinoDB.mdf; Integrated Security = True");


        public Roomid(int i_, int j_, int rom)
        {
            _arr = new Label[i_, j_];
            room = rom;
            this.AutoSize = true;
            this.Text = "Ap_polo_kino"; // Измененение название приложения *только космитическое
            _labels = new List<Label>();
            connect.Open();
            cmd = new SqlCommand("SELECT Place From Placee WHERE Hall=@id", connect);
            cmd.Parameters.AddWithValue("@id", room);
            cmd.ExecuteNonQuery();
            SqlDataReader reader;
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                pol.Add(reader.GetValue(0).ToString());
            }
            connect.Close();
            InitializeComponent();
            for (var i = 0; i <= 39; i++)
            {
                Label a = new Label()
                {
                    Name = "place" + i,
                    Height = 50,
                    Width = 50,
                    MinimumSize = new Size(50, 50),
                    BorderStyle = BorderStyle.Fixed3D,
                    BackColor = Color.Green
                };
                a.MouseClick += A_MouseClick;
                _labels.Add(a);

                if (pol.Contains(a.Name))
                {
                    a.BackColor = Color.Red;
                }

                // 581, 517
                var x = 0;
                var y = 0;

                foreach (var lbl in _labels)
                {
                    if (x >= 400)
                    {
                        x = 0;
                        y = y + lbl.Height + 2;
                    }

                    lbl.Location = new Point(x, y);
                    this.Controls.Add(lbl);
                    x += lbl.Width;
                }
            }


            // Пишем сюда чтобы позже вызывать к примеру Label, Button и т.д.

            buy = new Button(); // Добавление кнопки. Дает возможность "купить" место и резервирует место
            buy.Text = "Osta";
            buy.Font = new Font("Calibri", 12);
            buy.AutoSize = true;
            buy.Location = new Point(30, i_ * 50 + 20);
            buy.MouseClick += Buy_MouseClick;
            this.Controls.Add(buy);
        // Создается таймер
            tm = new Timer();
            tm.Tick += new EventHandler(tm_tick);
            tm.Interval = 1;
            tm.Start();

            label1 = new Label();
            this.Controls.Add(label1);
            label1.Text = "00:00";
            label1.AutoSize = true;
            label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            label1.Font = new Font("Calibri", 24);
            label1.BackColor = Color.SlateGray;
            label1.ForeColor = Color.Orange;
            label1.Location = new Point(25, i_ * 50 + 85);
            
        }

        private void Buy_MouseClick(object sender, MouseEventArgs e)
        {
            foreach (var el in _labels)
            {
                if (el.BackColor == Color.Red)
                {
                    connect.Open();
                    cmd = new SqlCommand("INSERT INTO Placee(Place, Hall) VALUES(@pla, @hal)", connect);
                    cmd.Parameters.AddWithValue("@pla", el.Name);
                    cmd.Parameters.AddWithValue("@hal", room);
                    cmd.ExecuteNonQuery();
                    connect.Close();
                }
            }

            MessageBox.Show("Ваш выбор был сохранен");
        }

        private void A_MouseClick(object sender, MouseEventArgs e)
        {
            var a = (Label)sender;
            if (a.BackColor == Color.Green)
            {
                a.BackColor = Color.Red;
            }
        }



        Timer tm = null;
        int startValue =  60 * 60 * 5; // Тут меняем цирфу и тогда таймер поменяется. К примеру: 1 - одна минута и т.д.

        private string Int2StringTime(int time)
        {
            int hours = (time - (time % (60 * 60))) / (60 * 60);
            int minutes = (time - time % 60) / 60 - hours * 60;
            return String.Format("{0:00}:{1:00}", hours, minutes);
        }
        private void tm_tick(object sender, EventArgs e)
        {
            if (startValue != 0)
            {
                label1.Text = Int2StringTime(startValue);
                startValue--;
            }
            else
            {
                (sender as Timer).Stop();
                (sender as Timer).Dispose();
                MessageBox.Show("Вы не успели купить билет. Попробуйте ещё раз");
                this.Close();
            }
        }

        

        


    }
}

