using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ImageForm MyImage = new ImageForm(); // 建立秀圖物件 
            MyImage.Show();// 顯示秀圖照片
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ImageForm MyImage = new ImageForm();
            MyImage.doGray(MyImage.getRGBData());
            MyImage.Show();// 顯示秀圖照片
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ImageForm MyImage = new ImageForm();
            MyImage.findedge(MyImage.getRGBData());
            MyImage.Show();// 顯示秀圖照片
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ImageForm MyImage = new ImageForm();
            MyImage.findline(MyImage.getRGBData());
            MyImage.Show();// 顯示秀圖照片
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }


    }


    class ImageForm : Form { 
        Image image; // 建構子 
        public ImageForm() { // Step 1: 載入影像 
            image = Image.FromFile(@"../../../arrow.jpg");
            this.Text = @"Picture";
        }


        protected override void OnPaint(PaintEventArgs e) {
                                                                        // Step 2: 調整視窗的大小
        this.Height=image.Height; 
        this.Width=image.Width;
                                                                      // Step 3: 顯示出影像
        e.Graphics.DrawImage(image, new Point[] { new Point(0, 0), new Point(image.Width / 2, 0), new Point(0, image.Height / 2) });
        }

        
        /// <summary>
        /// //////////////////////////////////////////////////////////////////////////
        /// </summary>
        /// <param name="rgbData"></param>
        public void findedge(int[, ,] rgbData)
        {
            Bitmap bimage = new Bitmap(image);
            int Height = bimage.Height;
            int Width = bimage.Width;

            int[, ,] black = new int[Width, Height, 1];
            int[, ,] temp_black = new int[Width, Height, 1];
            int[, ,] erotion_black = new int[Width, Height, 1];
            int[, ,] pre_erotion_black = new int[Width, Height, 1];
            int[, ,] edge = new int[Width, Height, 1];

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (rgbData[x, y, 0] < 60 && rgbData[x, y, 1] < 60 && rgbData[x, y, 2] < 60)                             //find black pixcel               
                    {
                        bimage.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                        pre_erotion_black[x, y, 0] = 1;
                    }
                    //else if(rgbData[x, y, 0]>160 && rgbData[x, y, 1]>160 && rgbData[x, y, 2]>160)
                    //bimage.SetPixel(x, y, Color.FromArgb(255, 255, 0));

                    else
                    {
                        bimage.SetPixel(x, y, Color.FromArgb(255, 255, 255));
                        pre_erotion_black[x, y, 0] = 0;
                    }
                }
            }

            /*
            ///////////////////////////////////////////////////////////////////////////////////////erotion
            for (int y = 1; y < Height - 1; y++)
            {
                for (int x = 1; x < Width - 1; x++)
                {
                    erotion_black[x, y, 0] = pre_erotion_black[x - 1, y - 1, 0] & pre_erotion_black[x, y - 1, 0] & pre_erotion_black[x + 1, y - 1, 0] & pre_erotion_black[x - 1, y, 0] & pre_erotion_black[x, y, 0] & pre_erotion_black[x + 1, y, 0] & pre_erotion_black[x - 1, y + 1, 0] & pre_erotion_black[x, y + 1, 0] & pre_erotion_black[x + 1, y + 1, 0];
                }
            }


            /////////////////////////////////////////////////////////////////////////////////////////////dilation
            for (int y = 1; y < Height - 1; y++)
            {
                for (int x = 1; x < Width - 1; x++)
                {
                    black[x, y, 0] = erotion_black[x - 1, y - 1, 0] | erotion_black[x, y - 1, 0] | erotion_black[x + 1, y - 1, 0] | erotion_black[x - 1, y, 0] | erotion_black[x, y, 0] | erotion_black[x + 1, y, 0] | erotion_black[x - 1, y + 1, 0] | erotion_black[x, y + 1, 0] | erotion_black[x + 1, y + 1, 0];
                }
            }
            */
            for (int y = 2; y < Height - 2; y++)
            {
                for (int x = 2; x < Width - 2; x++)
                {
                    //temp_black[x, y, 0] = black[x, y, 0] * 16 - black[x, y - 1, 0] * 2 - black[x, y + 1, 0] * 2 - black[x - 1, y, 0] * 2 - black[x + 1, y, 0] * 2 - black[x - 1, y - 1, 0] - black[x - 1, y + 1, 0] - black[x + 1, y - 1, 0] - black[x + 1, y + 1, 0] - black[x, y - 2, 0] - black[x, y + 2, 0] - black[x - 2, y, 0] - black[x + 2, y, 0];  //高斯運算子
                    //temp_black[x, y, 0] = black[x, y, 0] * -1 + black[x, y + 1, 0] * -2 + black[x, y + 2, 0] * -1 + black[x + 2, y, 0] * 1 + black[x + 2, y + 1, 0] * 2 + black[x + 2, y + 2, 0] * 1;                                                                                                                                                       //prewitt 運算子                                      
                    temp_black[x, y, 0] = pre_erotion_black[x, y, 0] * -1 + pre_erotion_black[x, y + 1, 0] * -2 + pre_erotion_black[x, y + 2, 0] * -1 + pre_erotion_black[x + 2, y, 0] * 1 + pre_erotion_black[x + 2, y + 1, 0] * 2 + pre_erotion_black[x + 2, y + 2, 0] * 1 + pre_erotion_black[x + 1, y + 2, 0] * -2 + pre_erotion_black[x + 1, y, 0] * 2;                                                                                                 //sobel 運算子
                }
            }
            
            int pixcelcounter = 0, th, tmpsum = 0;

            for (int y = 2; y < Height - 2; y++)
            {
                for (int x = 2; x < Width - 2; x++)
                {
                    pixcelcounter++;
                    tmpsum = tmpsum + temp_black[x, y, 0];
                }
            }

            th = 2 * (tmpsum / pixcelcounter) + 1;                                   //特殊方法決定 threshod 所有的黑點(sobel過後的)/圖片的大小

            for (int y = 0; y < Height - 2; y++)
            {
                for (int x = 0; x < Width - 2; x++)
                {
                    bimage.SetPixel(x + 1, y + 1, Color.FromArgb(255, 255, 255));
                    edge[x + 1, y + 1, 0] = 0;

                    if ((temp_black[x, y + 1, 0] >= 0) && (temp_black[x + 2, y + 1, 0] <= 0) && ((temp_black[x, y + 1, 0] - temp_black[x + 2, y + 1, 0]) >= th))    //為什麼不用絕對值@@
                    {
                        bimage.SetPixel(x + 1, y + 1, Color.FromArgb(0, 0, 0));
                        edge[x + 1, y + 1, 0] = 1;
                        continue;
                    }

                    if ((temp_black[x, y + 1, 0] >= 0) && (temp_black[x + 2, y + 1, 0] <= 0) && ((temp_black[x + 2, y + 1, 0] - temp_black[x, y + 1, 0]) >= th))      //這兩個不是一樣
                    {
                        bimage.SetPixel(x + 1, y + 1, Color.FromArgb(0, 0, 0));
                        edge[x + 1, y + 1, 0] = 1;
                        continue;
                    }

                    if ((temp_black[x + 1, y, 0] >= 0) && (temp_black[x + 1, y + 2, 0] <= 0) && ((temp_black[x + 1, y, 0] - temp_black[x + 1, y + 2, 0]) >= th))
                    {
                        bimage.SetPixel(x + 1, y + 1, Color.FromArgb(0, 0, 0));
                        edge[x + 1, y + 1, 0] = 1;
                        continue;
                    }

                    if ((temp_black[x + 1, y, 0] >= 0) && (temp_black[x + 1, y + 2, 0] <= 0) && ((temp_black[x + 1, y + 2, 0] - temp_black[x + 1, y, 0]) >= th))
                    {
                        bimage.SetPixel(x + 1, y + 1, Color.FromArgb(0, 0, 0));
                        edge[x + 1, y + 1, 0] = 1;
                        continue;
                    }



                }
            }
            image = bimage;
            this.Refresh();

            image.Save(@"../../../After_processing_of_findedge.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
        }


        public void dohsv(int[, ,] rgbData)
        {
            Bitmap bimage = new Bitmap(image);
            int Height = bimage.Height;
            int Width = bimage.Width;

            int  ir,ig,ib,imin,imax;
            int th,ts,tv,diffvmin;
            int[, ,] arosion = new int[Width, Height, 1];
            int[, ,] temp_arosion = new int[Width, Height, 1];
            int[, ,] dilation = new int[Width, Height, 1];

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    ir = rgbData[x, y, 0];
                    ig = rgbData[x, y, 1];
                    ib = rgbData[x, y, 2];


                   


                    /////////////////////////////////////////////////////////////////////////////////////hsv
                    if (ir > ig)
                    {
                        imax = ir;
                        imin = ig;
                    }
                    else
                    {
                        imax = ig;
                        imin = ir;
                    }
                    
                    if (imax > ib)
                    {
                        if (imin > ib)
                            imin = ib;
                    }
                    else
                    imax = ib;

                    tv = imax;
                    diffvmin = imax - imin;

                    if ((tv != 0) && (diffvmin != 0))
                    {
                        ts = (255 * diffvmin) / imax;
                        if (tv == ir) th = (ig - ib) * 60 / diffvmin;
                        else if (tv == ig) th = 120 + (ib - ir) * 60 / diffvmin;
                        else th = 240 + (ir - ig) * 60 / diffvmin;
                        if (th < 0) th += 360;
                        th &= 0x0000FFFF;
                    }
                    else
                    {
                        tv = 0;
                        ts = 0;
                        th = 0xFFFF;
                    }

                    ts = ts * 100 / 255;
                    tv = tv * 100 / 255;
                //    hsv_bimage.SetPixel(x, y, Color.FromArgb(th, ts, tv));
                    
                    //////////////////////////////////////////////////////////////////////////////////////filter
                    int h_max, h_min;
                    int h,s,v;

                    int m_hue = 15;
                    int m_hue_tolerance = 15*2;
                    int m_min_saturation = 0;
                    int m_min_value = 0;
                    int th_temp;


                    h_max = m_hue + m_hue_tolerance;
                    h_min = m_hue - m_hue_tolerance;

                    if (h_max > 360)
                        h_max -= 360;
                    if (h_min < 0)
                        h_min += 360;
                    th_temp = th >> 8;
                    th_temp = th_temp << 8;
                    h = (th) | (th & 0XFF);
                    s = ts & 0xFF;
                    v = tv & 0xFF;

                    if (h > 360)
                        h = h % 360;

                    if (((int)s >= m_min_saturation) && ((int)v >= m_min_value))
                    {
                        if (h_min <= h_max)
                        {
                            if ((h_min < (int)h) && ((int)h < h_max))
                            {
                                bimage.SetPixel(x, y, Color.FromArgb(10, 10, 10));
                                arosion[x,y,0] = 1;
                            }
                            else
                            {
                                bimage.SetPixel(x, y, Color.FromArgb(255, 255, 255));
                                arosion[x, y, 0] = 0;
                            }
                        }

                        else
                        {
                            if ((h_min < (int)h) || ((int)h < h_max))
                            {
                                bimage.SetPixel(x, y, Color.FromArgb(10, 10, 10));
                                arosion[x, y, 0] = 1;
                            }
                            else
                            {
                                bimage.SetPixel(x, y, Color.FromArgb(255, 255, 255));
                                arosion[x, y, 0] = 0;
                            }
                        }
                    }
                    else
                    {
                        bimage.SetPixel(x, y, Color.FromArgb(255, 255, 255));
                        arosion[x, y, 0] = 0;
                    }


                   
                    
                }
            }
            ///////////////////////////////////////////////////////////////////////////////////////erotion
            for (int y = 1; y < Height-1; y++)
            {
                for (int x = 1; x < Width-1; x++)
                {
                    temp_arosion[x, y, 0] = arosion[x - 1, y - 1, 0] & arosion[x, y - 1, 0] & arosion[x + 1, y - 1, 0] & arosion[x - 1, y, 0] & arosion[x, y, 0] & arosion[x + 1, y, 0] & arosion[x - 1, y + 1, 0] & arosion[x, y + 1, 0] & arosion[x + 1, y + 1, 0];
                }
            }


            /////////////////////////////////////////////////////////////////////////////////////////////dilation
            for (int y = 1; y < Height - 1; y++)
            {
                for (int x = 1; x < Width - 1; x++)
                {
                    dilation[x, y, 0] = temp_arosion[x - 1, y - 1, 0] | temp_arosion[x, y - 1, 0] | temp_arosion[x + 1, y - 1, 0] | temp_arosion[x - 1, y, 0] | temp_arosion[x, y, 0] | temp_arosion[x + 1, y, 0] | temp_arosion[x - 1, y + 1, 0] | temp_arosion[x, y + 1, 0] | temp_arosion[x + 1, y + 1, 0];
                }
            }
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (dilation[x, y, 0] == 1)
                    bimage.SetPixel(x, y, Color.FromArgb(10, 10, 10));
                    else
                    bimage.SetPixel(x, y, Color.FromArgb(255, 255, 255));
                }
            }





            image = bimage;
            this.Refresh();

            image.Save(@"c:\Users\darren\Desktop\god.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        public void doGray(int[, ,] rgbData)
        {
            // Step 1: 建立 Bitmap 元件
            Bitmap bimage = new Bitmap(image);
            int Height = bimage.Height;
            int Width = bimage.Width;

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    int gray = (rgbData[x, y, 0] + rgbData[x, y, 1] + rgbData[x, y, 2]) / 3; 
                    bimage.SetPixel(x, y, Color.FromArgb(gray, gray, gray));
                }
            }
            image = bimage;
            this.Refresh();
                        
            image.Save(@"../../../After_processing_of_dogray.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
           
        }

        public void findline(int[, ,] rgbData)
        {
            Bitmap bimage = new Bitmap(image);
            int Height = bimage.Height;
            int Width = bimage.Width;

            int[, ,] black = new int[Width, Height, 1];
            int[, ,] temp_black = new int[Width, Height, 1];
            int[, ,] erotion_black = new int[Width, Height, 1];
            int[, ,] pre_erotion_black = new int[Width, Height, 1];
            int[, ,] edge = new int[Width, Height, 1];
            
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (rgbData[x, y, 0] < 60 && rgbData[x, y, 1] < 60 && rgbData[x, y, 2] < 60)                             //find black pixcel               
                    {
                        bimage.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                        pre_erotion_black[x, y, 0] = 1;
                    }
                    //else if(rgbData[x, y, 0]>160 && rgbData[x, y, 1]>160 && rgbData[x, y, 2]>160)
                    //bimage.SetPixel(x, y, Color.FromArgb(255, 255, 0));

                    else
                    {
                        bimage.SetPixel(x, y, Color.FromArgb(255, 255, 255));
                        pre_erotion_black[x, y, 0] = 0;
                    }
                }
            }

            
            ///////////////////////////////////////////////////////////////////////////////////////erotion
            for (int y = 1; y < Height - 1; y++)
            {
                for (int x = 1; x < Width - 1; x++)
                {
                    erotion_black[x, y, 0] = pre_erotion_black[x - 1, y - 1, 0] & pre_erotion_black[x, y - 1, 0] & pre_erotion_black[x + 1, y - 1, 0] & pre_erotion_black[x - 1, y, 0] & pre_erotion_black[x, y, 0] & pre_erotion_black[x + 1, y, 0] & pre_erotion_black[x - 1, y + 1, 0] & pre_erotion_black[x, y + 1, 0] & pre_erotion_black[x + 1, y + 1, 0];
                }
            }


            /////////////////////////////////////////////////////////////////////////////////////////////dilation
            for (int y = 1; y < Height - 1; y++)
            {
                for (int x = 1; x < Width - 1; x++)
                {
                    black[x, y, 0] = erotion_black[x - 1, y - 1, 0] | erotion_black[x, y - 1, 0] | erotion_black[x + 1, y - 1, 0] | erotion_black[x - 1, y, 0] | erotion_black[x, y, 0] | erotion_black[x + 1, y, 0] | erotion_black[x - 1, y + 1, 0] | erotion_black[x, y + 1, 0] | erotion_black[x + 1, y + 1, 0];
                }
            }



            
            /*
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                     black[x,y,0] = (rgbData[x, y, 0] + rgbData[x, y, 1] + rgbData[x, y, 2]) / 3;
                    //bimage.SetPixel(x, y, Color.FromArgb(gray, gray, gray));
                }
            }*/


            for (int y = 2; y < Height-2; y++)
            {
                for (int x = 2; x < Width - 2; x++)
                {
                    //temp_black[x, y, 0] = black[x, y, 0] * 16 - black[x, y - 1, 0] * 2 - black[x, y + 1, 0] * 2 - black[x - 1, y, 0] * 2 - black[x + 1, y, 0] * 2 - black[x - 1, y - 1, 0] - black[x - 1, y + 1, 0] - black[x + 1, y - 1, 0] - black[x + 1, y + 1, 0] - black[x, y - 2, 0] - black[x, y + 2, 0] - black[x - 2, y, 0] - black[x + 2, y, 0];  //高斯運算子
                    //temp_black[x, y, 0] = black[x, y, 0] * -1 + black[x, y + 1, 0] * -2 + black[x, y + 2, 0] * -1 + black[x + 2, y, 0] * 1 + black[x + 2, y + 1, 0] * 2 + black[x + 2, y + 2, 0] * 1;                                                                                                                                                       //prewitt 運算子                                      
                    temp_black[x, y, 0] = black[x, y, 0] * -1 + black[x, y + 1, 0] * -2 + black[x, y + 2, 0] * -1 + black[x + 2, y, 0] * 1 + black[x + 2, y + 1, 0] * 2 + black[x + 2, y + 2, 0] * 1 + black[x + 1, y + 2, 0] * -2 + black[x + 1, y , 0] * 2;                                                                                                 //sobel 運算子
                }
            }

            int pixcelcounter=0,th,tmpsum=0;

             for (int y = 2; y < Height-2; y++)
            {
                for (int x = 2; x < Width-2; x++)
                {
                    pixcelcounter++;
                    tmpsum = tmpsum + temp_black[x, y, 0];
                }
            }

             th = 2 * (tmpsum / pixcelcounter)+1;                                   //特殊方法決定 threshod 所有的黑點(sobel過後的)/圖片的大小
       
             for (int y = 0; y < Height - 2; y++)
             {
                 for (int x = 0; x < Width - 2; x++)
                 {
                     bimage.SetPixel(x+1, y+1, Color.FromArgb(255, 255, 255));
                     edge[x+1, y+1, 0] = 0;

                     if ((temp_black[x, y + 1, 0] >= 0) && (temp_black[x + 2, y + 1, 0] <= 0) && ((temp_black[x, y + 1, 0] - temp_black[x + 2, y + 1, 0]) >= th))    //為什麼不用絕對值@@
                     {
                         bimage.SetPixel(x + 1, y + 1, Color.FromArgb(0, 0, 0));
                         edge[x + 1, y + 1, 0] = 1;
                         continue;
                     }

                     if ((temp_black[x, y + 1, 0] >= 0) && (temp_black[x + 2, y + 1, 0] <= 0) && ((temp_black[x+2, y + 1, 0] - temp_black[x, y + 1, 0]) >= th))      //這兩個不是一樣
                     {
                         bimage.SetPixel(x + 1, y + 1, Color.FromArgb(0, 0, 0));
                         edge[x + 1, y + 1, 0] = 1;
                         continue;
                     }

                     if ((temp_black[x+1, y, 0] >= 0) && (temp_black[x +1, y + 2, 0] <= 0) && ((temp_black[x + 1, y , 0] - temp_black[x+1, y + 2, 0]) >= th))
                     {
                         bimage.SetPixel(x + 1, y + 1, Color.FromArgb(0, 0, 0));
                         edge[x + 1, y + 1, 0] = 1;
                         continue;
                     }

                     if ((temp_black[x+1, y , 0] >= 0) && (temp_black[x + 1, y + 2, 0] <= 0) && ((temp_black[x + 1, y + 2, 0] - temp_black[x+1, y , 0]) >= th))
                     {
                         bimage.SetPixel(x + 1, y + 1, Color.FromArgb(0, 0, 0));
                         edge[x + 1, y + 1, 0] = 1;
                         continue;
                     }



                 }
             }
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////find m -0.5~0.5
             double slope;
             double intercept;

             double slope_range;
             double intercept_range;
             double min_intercept;

            // min_intercept = -0.5*( Width);
             min_intercept = 260;

             int intercept_size = 500;                                                                          //decide how many (b)parts do we need    y=mx+b
             int slope_size = 100;                                                                              //decide how many (m)parts do we need

             intercept_range = (Height + Width) / intercept_size;                                               //decide b range
             slope_range = (0.5 - (-0.5)) / slope_size;                                                         //decide m range    

             int[,] vote_line = new int[slope_size, intercept_size];                                            //creat vote box

             int slope_i, intercept_j;
             
             
             int max_vote=0;
             int max_slope_i=0;
             int max_intercept_j=0;

            

            int[, ,] max_line = new int[Width, Height, 1];
             


             for (slope_i = 0; slope_i < slope_size; slope_i++)
                 for (intercept_j = 0; intercept_j < intercept_size; intercept_j++)
                     vote_line[slope_i, intercept_j] = 0;                                                       //initial all vote box


             for (int y = 0; y < Height - 2; y++)
             {
                 for (int x = 0; x < Width - 2; x++)
                 {
                        if (edge[x,y,0]==1)
                            for (slope_i = 0; slope_i < slope_size; slope_i++)
                        {
                            for (intercept_j = 0; intercept_j < intercept_size; intercept_j++)
                            {
                                intercept = -1* x * (-0.5+slope_i * slope_range) + y;                             //key function:y=mx+b--->b=-mx+y

                                if ((intercept > intercept_range * intercept_j + min_intercept) && (intercept < intercept_range * (intercept_j + 1) + min_intercept))
                                     vote_line[slope_i, intercept_j] ++;
                            }
                        }


                 }
             }
            

System.IO.StreamWriter find_line = new System.IO.StreamWriter("data.txt");


                        for (slope_i = 0; slope_i < slope_size; slope_i++)
                        {
                            for (intercept_j = 0; intercept_j < intercept_size; intercept_j++)
                            {

                                if (vote_line[slope_i, intercept_j] > max_vote)
                                {
                                    max_vote = vote_line[slope_i, intercept_j];                                                     //find max vote box
                                    max_slope_i = slope_i;
                                    max_intercept_j = intercept_j;
                                }                 


                                find_line.Write(slope_i * slope_range-0.5);
                                find_line.Write(" ");
                                find_line.Write(intercept_range * intercept_j + min_intercept);
                                find_line.Write(" ");
                                find_line.WriteLine(vote_line[slope_i, intercept_j]);
                            }
                        }
                        find_line.WriteLine(max_vote);                      

                       

                        find_line.Close();

            /////////////////////////////////////////////////////////////////////////////////////////////
                        


System.IO.StreamWriter inter = new System.IO.StreamWriter("intercept.txt");
            for (int y = 0; y < Height ; y++)
             {
                 for (int x = 0; x < Width ; x++)
                 {

                     if (edge[x, y, 0] == 1)
                     {
                         intercept = -1 * (max_slope_i * slope_range - 0.5) * x + y;



                         if ((intercept > intercept_range * max_intercept_j + min_intercept) && (intercept < intercept_range * (max_intercept_j + 1) + min_intercept))
                         {
                             inter.WriteLine(intercept);
                             bimage.SetPixel(x, y, Color.FromArgb(255, 0, 0));                                                                      //acording to the max vote box to change color
                             max_line[x, y-30, 0] = 1;
                         }

                         else
                         {
                             max_line[x, y-30, 0] = 0;
                         }

                        
                     }
                 }
             }

            inter.Close();
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////
            System.IO.StreamWriter overlap = new System.IO.StreamWriter("overlap.txt");
            for (int x = 0; x < Width-2; x++)
                for (int y = 0; y < Height-2; y++)
                {
                    if ((max_line[x, y, 0] == 1) && (pre_erotion_black[x, y, 0] == 0))
                    {
                        //bimage.SetPixel(x, y, Color.FromArgb(0, 255, 0));
                        overlap.WriteLine(x);
                    }
                }

            overlap.Close();
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
            System.IO.StreamWriter max_line_XY = new System.IO.StreamWriter("max_line_XY.txt");

            for (int x = 0; x < Width ; x++)
                for (int y = 0; y < Height ; y++)
                {
                    if (max_line[x, y, 0] == 1)
                    {
                        max_line_XY.Write(x);
                        max_line_XY.Write(" ");
                        max_line_XY.WriteLine(y);
                        break;
                    }
                }

            max_line_XY.Close();
          
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
             

             //System.IO.StreamWriter radius = new System.IO.StreamWriter("data.txt");
            
            //Console.WriteLine("Hello World!  {0}",System.Math.Sin(System.Math.PI / 2)); 

             //sw.WriteLine(System.Math.Sin(50 / 2));
            // sw.Close();
            
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////find_line
             /*
            int theda_segment;
             int r_segment;
             double r;
             double min_r = -1 * Width;
             double max_r = System.Math.Sqrt(Width * Width + Height * Height); ;
             int r_range=10;
             int r_num= (int)(max_r-min_r)/r_range;

             int theda_range = 1;
             int theda_num = 360 / theda_range;
            
             double pi=System.Math.PI;
             int[,] vote = new int[theda_num, r_num];
            
          
            //int vote[theda_num][r_num];

           
           
             for (theda_segment = 0; theda_segment < theda_num; theda_segment++)
                 for (r_segment = 0; r_segment < r_num; r_segment++)
                      vote[theda_segment, r_segment] = 0;



             for (int y = 0; y < Height; y++)
             {
                 for (int x = 0; x < Width; x++)
                 {
                         if (edge[x, y , 0]==1)
                     for (theda_segment = 0; theda_segment < theda_num; theda_segment++)
                     {
                         r = x * System.Math.Cos(theda_segment * theda_range * pi / 180) + y * System.Math.Sin(theda_segment * theda_range * pi / 180);

                         //radius.WriteLine(r);

                         for (r_segment = 0; r_segment < r_num; r_segment++)
                         {
                             if (r > min_r + r_segment * r_range && r < (r_segment + 1) * r_range + min_r)
                             {
                                 vote[theda_segment, r_segment]++;
                                

                                
                             }
                         }
                     }
                 }
             }
      
            
             for (int y = 0; y < Height; y++)
             {
                 for (int x = 0; x < Width; x++)
                 {
                     if (edge[x, y, 0] == 1)
                         for (theda_segment = 0; theda_segment < theda_num; theda_segment++)
                         {
                             for (r_segment = 0; r_segment < r_num; r_segment++)
                             {
                                 if (vote[theda_segment, r_segment] > 30  )//&& theda_segment < 130 && theda_segment>120
                                 {
                                     //radius.WriteLine(theda_segment);
                                     r = x * System.Math.Cos(theda_segment * theda_range * pi / 180) + y * System.Math.Sin(theda_segment * theda_range * pi / 180);
                                     if (r > min_r + r_segment * 10 && r < (r_segment + 1) * 10 + min_r)
                                     {
                                         bimage.SetPixel(x, y, Color.FromArgb(255, 0, 0));
                                     }
                                 }
                             }
                         }
                          
                 }
             }

            */
                 
                 
                 
                 image = bimage;
            this.Refresh();
            image.Save(@"../../../After_processing_of_findline.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            //radius.Close();
        }


        public int[,,] getRGBData() {
        // Step 1: 利用 Bitmap 將 image 包起來
        Bitmap abimage = new Bitmap(image);
        int Height = abimage.Height;
        int Width = abimage.Width;
        int[,,]rgbData=new int[Width,Height,3];

        // Step 2: 取得像點顏色資訊
        for(int y=0;y<Height;y++){
        for(int x=0;x<Width;x++){
        Color color=abimage.GetPixel(x,y);
        rgbData[x, y, 0] = color.R;
        rgbData[x, y, 1] = color.G;
        rgbData[x, y, 2] = color.B;
        }
        }

        return rgbData;
        }


    }







}
