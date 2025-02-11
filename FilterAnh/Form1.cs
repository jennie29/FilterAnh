﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FilterAnh
{
    public partial class Form1 : Form
    {
        Bitmap bitmap;
        public Form1()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            if (open.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //Bước 1:
                bitmap = new Bitmap(open.FileName);
                pictureBox1.Image = bitmap;
            }
        }
        unsafe
           private void button1_Click(object sender, EventArgs e)
        {
            //Buoc 2: Dùng phương pháp Lockbits để truy cập trực tiếp vào nội dung chứa điểm ảnh đó
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
           
            //Buoc 3: offset duoc them vao de chia het cho 8 (64 bit)
            
            int offset = data.Stride - bitmap.Width * 3;

            //Buoc 4:Truy cap con tro dau tien (unsafe mode)

            byte* p = (byte*)data.Scan0;

            //Buoc 5: Duyet theo con tro dau tien
            for(int i=0; i < bitmap.Height; i++)
            {
                for(int j=0; j < bitmap.Width; j++)
                {
                    //Tro t den con tro dau tien(BGR)
                    int t = (p[0] + p[1] +p[2])/3;
                    //p[0]=Blue;p[1]=Green,p[2]=Red

                    p[0] = (byte)t;
                    p[1] = (byte)t;
                    p[2] = (byte)t;

                    //Chuyen con tro 3 buoc nhay de den duoc con tro tiep theo
                    p += 3;
                }
                p += offset;
            }

            //Buoc 6: Unlock bitmap
            bitmap.UnlockBits(data);
            pictureBox2.Image = bitmap;
        }
        unsafe
        private void button2_Click(object sender, EventArgs e)
        {
            //Buoc 2: Dùng phương pháp Lockbits để truy cập trực tiếp vào nội dung chứa điểm ảnh đó
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);

            //Buoc 3: offset duoc them vao de chia het cho 8 (64 bit)

            int offset = data.Stride - bitmap.Width * 3;

            //Buoc 4:Truy cap con tro dau tien (unsafe mode)

            byte* p = (byte*)data.Scan0;

            //Buoc 5: Duyet theo con tro dau tien
            for (int i = 0; i < bitmap.Height; i++)
            {
                for (int j = 0; j < bitmap.Width; j++)
                {
                    //Tro t den con tro dau tien(BGR)
                    int a = (int)(0.272 * p[2] + 0.534 * p[1]  + 0.131 * p[0] );
                    int b = (int)(0.349 * p[2] + 0.686 * p[1]  + 0.168 * p[0] );
                    int c = (int)(0.393 * p[2] + 0.769 * p[1]  + 0.189 * p[0] );

                    //p[0]=Blue;p[1]=Green,p[2]=Red

                    p[0] = (byte)a;
                    p[1] = (byte)b;
                    p[2] = (byte)c;

                    if (a > 255) { p[0] = 255; }
                    if (b > 255) { p[1] = 255; }
                    if (c > 255) { p[2] = 255; }

                    //Chuyen con tro 3 buoc nhay de den duoc con tro tiep theo
                    p += 3;
                }
                p += offset;
            }
            //Buoc 6: Unlock bitmap
            bitmap.UnlockBits(data);
            pictureBox2.Image = bitmap;
        }


        }
    }