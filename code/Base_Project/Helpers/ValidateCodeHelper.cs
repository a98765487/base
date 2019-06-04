﻿using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace Base_Project.Helpers
{
    public class ValidateCodeHelper : System.Web.UI.Page
    {
        public byte[] CreateValidateCode()
        {
            string randomcode = this.CreateRandomCode(4);
            Session["ValidateCode"] = randomcode;
            return this.CreateImage(randomcode);
        }
        ///  <summary>
        ///  生成隨機碼
        ///  </summary>
        ///  <param  name="length">隨機碼個數</param>
        ///  <returns></returns>
        private string CreateRandomCode(int length)
        {
        int rand;
        char code;
        string randomcode = String.Empty;

        //生成一定長度的驗證碼
        System.Random random = new Random();
        for (int i = 0; i < length; i++)
        {
            rand = random.Next();

            if (rand % 3 == 0)
            {
                code = (char)('A' + (char)(rand % 26));
            }
            else
            {
                code = (char)('0' + (char)(rand % 10));
            }

            randomcode += code.ToString();
        }
            return randomcode;
        }

        ///  <summary>
        ///  建立隨機碼圖片
        ///  </summary>
        ///  <param  name="randomcode">隨機碼</param>
        private byte[] CreateImage(string randomcode)
        {
            byte[] data = null;

            int randAngle = 45; //隨機轉動角度
            int mapwidth = (int)(randomcode.Length * 23);
            Bitmap map = new Bitmap(mapwidth, 28);//建立圖片背景
            Graphics graph = Graphics.FromImage(map);
            graph.Clear(Color.AliceBlue);//清除畫面，填充背景
            graph.DrawRectangle(new Pen(Color.Black, 0), 0, 0, map.Width - 1, map.Height - 1);//畫一個邊框
                                                                                                //graph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;//模式


            Random rand = new Random();

            //背景噪點生成
            Pen blackPen = new Pen(Color.LightGray, 0);
            for (int i = 0; i < 50; i++)
            {
                int x = rand.Next(0, map.Width);
                int y = rand.Next(0, map.Height);
                graph.DrawRectangle(blackPen, x, y, 1, 1);
            }


            //驗證碼旋轉，防止機器識別
            char[] chars = randomcode.ToCharArray();//拆散字串成單字元陣列

            //文字距中
            StringFormat format = new StringFormat(StringFormatFlags.NoClip);
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            //定義顏色
            Color[] c = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };
            //定義字型 
            string[] font = { "Verdana", "Microsoft Sans Serif", "Comic Sans MS", "Arial", "宋體" };

            for (int i = 0; i < chars.Length; i++)
            {
                int cindex = rand.Next(7);
                int findex = rand.Next(5);

                Font f = new System.Drawing.Font(font[findex], 13, System.Drawing.FontStyle.Bold);//字型樣式(引數2為字型大小)
                Brush b = new System.Drawing.SolidBrush(c[cindex]);

                Point dot = new Point(16, 16);
                //graph.DrawString(dot.X.ToString(),fontstyle,new SolidBrush(Color.Black),10,150);//測試X座標顯示間距的
                float angle = rand.Next(-randAngle, randAngle);//轉動的度數

                graph.TranslateTransform(dot.X, dot.Y);//移動游標到指定位置
                graph.RotateTransform(angle);
                graph.DrawString(chars[i].ToString(), f, b, 1, 1, format);
                //graph.DrawString(chars[i].ToString(),fontstyle,new SolidBrush(Color.Blue),1,1,format);
                graph.RotateTransform(-angle);//轉回去
                graph.TranslateTransform(2, -dot.Y);//移動游標到指定位置
            }
            //生成圖片
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            map.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            data = ms.GetBuffer();
            graph.Dispose();
            map.Dispose();

            return data;

        }
    }
}
