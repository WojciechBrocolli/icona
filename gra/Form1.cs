using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gra
{
    public partial class Form1 : Form
    {
        DBDataContext DB = new DBDataContext();
        const int fieldSize = 25;
        Graphics g;


        public Form1()
        {
            InitializeComponent();
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            //listBox1.DisplayMember = "Name";

            loadPictures();
        }
        private void loadPictures()
        {
            listBox1.Items.Clear();
            listBox1.Items.AddRange(DB.Picture.ToArray());
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Picture p = new Picture()
            {
                Name = textBox1.Text,
                Width = (int)numericUpDown1.Value,
                Height = (int)numericUpDown2.Value
            };
            DB.Picture.InsertOnSubmit(p);
            DB.SubmitChanges();

            textBox1.Text = "";
            numericUpDown1.Value = 0;
            numericUpDown2.Value = 0;
            loadPictures();
        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null && listBox1.SelectedItem is Picture)
            {
                Picture p = listBox1.SelectedItem as Picture;
                pictureBox1.Image = new Bitmap(p.Width * fieldSize + 2, p.Height * fieldSize + 2);

                g = Graphics.FromImage(pictureBox1.Image);

                g.Clear(Color.LightGray);

                for (int x = 0; x < p.Width; x++)
                {
                    for (int y = 0; y < p.Height; y++)
                    {
                        Pixel pix = p.Pixel.SingleOrDefault(tmp => tmp.X == x && tmp.Y == y);
                        if(pix!=null)
                        {
                            g.FillRectangle(new SolidBrush(Color.FromArgb(pix.Color)), x * fieldSize, y * fieldSize, fieldSize, fieldSize);
                        }
                        g.DrawRectangle(new Pen(Color.Black), x * fieldSize, y * fieldSize, fieldSize, fieldSize);
                    }
                }
                pictureBox1.Refresh();
            }


        }

        private void PictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && listBox1.SelectedItem != null && listBox1.SelectedItem is Picture)
            {
                Picture pic = listBox1.SelectedItem as Picture;
                Pixel pix = pic.Pixel.SingleOrDefault(tmp => tmp.X == e.X / fieldSize && tmp.Y == e.Y / fieldSize);
                if (pix == null)
                {
                     pix = new Pixel()
                    {
                        X = e.X / fieldSize,
                        Y = e.Y / fieldSize
                    };
                    pix.Color = button2.BackColor.ToArgb();
                    pic.Pixel.Add(pix);
                }
                pix.Color = button2.BackColor.ToArgb();
                DB.SubmitChanges();
                ListBox1_SelectedIndexChanged(null, null);

            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            cd.Color = button2.BackColor;
            if(cd.ShowDialog()==DialogResult.OK)
            {

                button2.BackColor = cd.Color;
            }
         }

        private void Button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Ikona|*.ico";
            if(sfd.ShowDialog()==DialogResult.OK)
            {
                pictureBox1.Image.Save(sfd.FileName, ImageFormat.Icon);

            }
        }
    }
}
