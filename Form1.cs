using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace budzik
{
    public partial class Form1 : Form
    {
        int leftbox = 210;
        int left = 3;
        int leftbox2 = 230;
        int lefttext = 280;
        Point point;
        bool muzyka = true;

        List<DateTimePicker> datetimepicker = new List<DateTimePicker>();
        List<CheckBox> checkbox = new List<CheckBox>();
        List<DateTime> datetime = new List<DateTime>();
        List<Panel> panel = new List<Panel>();
        List<CheckBox> checkbox2 = new List<CheckBox>();
        List<TextBox> textbox = new List<TextBox>();

        public Form1()
        {
            InitializeComponent();

            wczytajczas();

            timer3.Interval = 200;
            timer3.Start();
            timer3.Tick += new EventHandler(timer_Tick3);

            this.FormClosing += new FormClosingEventHandler(Form1_Closing);

            this.notifyIcon1.DoubleClick += new EventHandler(notifyclick);

            timer1.Interval = 1000;
            timer1.Start();
            timer1.Tick += new EventHandler(zegar);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            dodajalarm();
        }

        private void zakończToolStripMenuItem_Click(object sender, EventArgs e)
        {
            zapiszczas();
            Application.Exit();
        }

        private void Form1_Closing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.ApplicationExitCall)
            {
                notifyIcon1.Visible = true;
                this.Hide();
                e.Cancel = true;
            }
        }

        void timer_Tick3(object sender, EventArgs e)
        {
            for (int i = 0; i < checkbox.Count; i++)
            {
                if (datetimepicker[i].Value <= DateTime.Now && checkbox[i].Checked == true && datetimepicker[i].Value.AddSeconds(5) >= DateTime.Now)
                {
                    if (String.IsNullOrEmpty(textbox[i].Text))
                    {
                        notifyIcon1.BalloonTipText = "Alarm ";
                    }
                    else
                    {
                        notifyIcon1.BalloonTipText = textbox[i].Text;
                    }
                    notifyIcon1.ShowBalloonTip(0);
                    System.Media.SoundPlayer player = new System.Media.SoundPlayer();
                    player.SoundLocation = @"../../save/2.wav";
                    player.Load();
                    if (muzyka == true)
                    {
                        player.Play();
                    }
                    timer3.Stop();
                    notifyIcon1.BalloonTipClicked += new EventHandler(powiadomienie);

                    checkbox[i].Checked = false;
                }

                if (checkbox2[i].Checked == true)
                {
                    for (int x = i; x < datetimepicker.Count; x++)
                    {
                        if (x + 1 < datetimepicker.Count)
                        {
                            datetimepicker[x].Value = datetimepicker[x + 1].Value;
                            textbox[x].Text = textbox[x + 1].Text;
                        }
                    }

                    checkbox2[i].Checked = false;
                    datetimepicker.RemoveAt(datetimepicker.Count - 1);

                    checkbox.RemoveAt(checkbox.Count - 1);
                    textbox.RemoveAt(textbox.Count - 1);

                    checkbox2.RemoveAt(checkbox2.Count - 1);
                    panel[panel.Count - 1].Controls.Clear();
                    panel.RemoveAt(panel.Count - 1);

                    panel1.Controls.RemoveAt(panel.Count);
                }
            }
        }

        private void powiadomienie(object sender, EventArgs e)
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer();
            player.SoundLocation = @"../../save/2.wav";
            player.Stop();
            timer3.Start();
        }

        private void notifyclick(object sender, EventArgs e)
        {
            this.Show();
        }

        void zapiszczas()
        {
            string[] temp;
            string[] tekst;
            temp = new string[datetimepicker.Count];
            tekst = new string[textbox.Count];
            for (int i = 0; i < datetimepicker.Count; i++)
            {
                temp[i] = datetimepicker[i].ToString();
                temp[i] = temp[i].Substring(44);
                tekst[i] = textbox[i].Text;
            }
            string path = System.IO.Directory.GetCurrentDirectory();
            System.IO.File.WriteAllLines(@"../../save/save.txt", temp);
            System.IO.File.WriteAllLines(@"../../save/text.txt", tekst);
        }

        void wczytajczas()
        {
            DateTime odczyt;
            string[] temp = System.IO.File.ReadAllLines(@"../../save/save.txt");
            string[] tekst = System.IO.File.ReadAllLines(@"../../save/text.txt");

            for (int i = 0; i < temp.Length; i++)
            {
                dodajalarm();
                odczyt = DateTime.Parse(temp[i]);
                datetimepicker[i].Value = odczyt;
                textbox[i].Text = tekst[i];
            }
        }

        void dodajalarm()
        {
            if (panel.Count != 0)
            {
                point = panel[panel.Count - 1].Location;
                point.Y += 40;
            }
            else
            {
                point.X = 0;
                point.Y = 0;
            }

            //panel2
            Panel newpanel = new Panel();
            newpanel.Left = 0;
            newpanel.Width = 450;
            newpanel.Height = 32;
            panel.Add(newpanel);
            newpanel.Location = point;
            panel1.Controls.Add(newpanel);

            //datetimepicker
            DateTimePicker newdatetimepicker = new DateTimePicker();
            newdatetimepicker.Left = left;
            newdatetimepicker.Top = 0;
            datetimepicker.Add(newdatetimepicker);
            newpanel.Controls.Add(newdatetimepicker);
            newdatetimepicker.Format = DateTimePickerFormat.Custom;
            newdatetimepicker.CustomFormat = "MM/dd/yyyy HH:mm:ss";

            //checkbox
            CheckBox newcheckbox = new CheckBox();
            newcheckbox.Left = leftbox;
            newcheckbox.Top = 5;
            checkbox.Add(newcheckbox);
            newpanel.Controls.Add(newcheckbox);
            newcheckbox.Text = null;
            newcheckbox.AutoSize = true;

            //checkbox usuwanie
            CheckBox newcheckbox2 = new CheckBox();
            newcheckbox2.Left = leftbox2;
            newcheckbox2.Top = 0;
            checkbox2.Add(newcheckbox2);
            newpanel.Controls.Add(newcheckbox2);
            newcheckbox2.Text = "X";
            newcheckbox2.AutoSize = true;
            newcheckbox2.Appearance = Appearance.Button;

            //textbox
            TextBox newtextbox = new TextBox();
            newtextbox.Left = lefttext;
            newtextbox.Top = 0;
            newtextbox.Width = 100;
            textbox.Add(newtextbox);
            newpanel.Controls.Add(newtextbox);
            newtextbox.Text = "Alarm";
        }

        void zegar(object sender, EventArgs e)
        {
            dateTimePicker2.Value = DateTime.Now;
        }

        private void trybCichyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (muzyka == true)
            {
                muzyka = false;
                contextMenuStrip1.Items[1].Text = "Dzwięk";
            }
            else
            {
                muzyka = true;
                contextMenuStrip1.Items[1].Text = "Tryb cichy";
            }
        }

        private void notifyIcon1_BalloonTipClosed(object sender, EventArgs e)
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer();
            player.SoundLocation = @"../../save/2.wav";
            player.Stop();
            timer3.Start();
        }
    }
}
