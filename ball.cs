using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace snake
{
    class Vector2
    {
        public double X, Y;
        public Vector2(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
        public Vector2(Vector2 F)
        {
            this.X = F.X;
            this.Y = F.Y;
        }
        public Vector2(Point F)
        {
            this.X = F.X;
            this.Y = F.Y;
        }
        public Vector2(double x,double y)
        {
            this.X = x;
            this.Y = y;
        }
    }
    class planet
    {
        int radius;
        public int mass;
        public Vector2 location;
        Vector2 velocity;
        public SolidBrush Texture;
        Rectangle rect;
        planet(int radius, int mass, Point location, SolidBrush Texture)
        {
            this.radius = radius;
            this.mass = mass;
            this.location = new Vector2(location);
            this.Texture = Texture;
            this.velocity = new Vector2(0, 0);
        }
        public planet(Point location,Vector2 vel)
        {
            this.radius = 3;
            this.mass = 1;
            this.location = new Vector2(location);
            this.Texture = new SolidBrush(Color.Black);
            this.velocity = vel;
        }
        public Rectangle GetRect()
        {
            rect = new Rectangle((int)location.X - radius,(int)location.Y - radius, 2 * radius, 2 * radius);
            return rect;
        }
        public SolidBrush GetTexture()
        {
            return Texture;
        }
        public void GetNext(Vector2 F, double time, planet b)
        {
            velocity.X += F.X * time / mass;
            velocity.Y += F.Y * time / mass;
            location.X += velocity.X * time;
            location.Y += velocity.Y * time;
        }
        public Vector2 GetF(planet b)
        {
            Vector2 S = new Vector2(b.location.X - this.location.X, b.location.Y - this.location.Y);
            double distance = Math.Sqrt(S.X * S.X + S.Y * S.Y);
            Vector2 F = new Vector2(0, 0);
            int G = 300*300*100;
            F.X = G * this.mass * b.mass / (Math.Pow(distance, 3)) * S.X;
            F.Y = G * this.mass * b.mass / (Math.Pow(distance, 3)) * S.Y;

            return F;
        }
        public string GetV()
        {
            return string.Format("({0:F},{1:F})({2:F},{3:F})", this.velocity.X, this.velocity.Y,this.location.X,this.location.Y);
        }
    }

    public class Form1 : Form
    {
        Timer timer1;
        Graphics graphics;
        int time_tick;

        planet a, b;

        Label l1, l2,time;
        int i;
        public Form1()
        {
            i = 0;
            time_tick = 1;

            this.Text = "弹球模拟";
            this.Size = new System.Drawing.Size(800, 600);

            graphics = this.CreateGraphics();

            timer1 = new Timer();
            timer1.Interval = time_tick;
            timer1.Tick += new EventHandler(TimePass);
            timer1.Start();

            a = new planet(new Point(200, 300),new Vector2(-30,100));
            a.Texture = new SolidBrush(Pens.Blue.Color);
            b = new planet(new Point(500, 300),new Vector2(30,-50));
            b.Texture = new SolidBrush(Pens.Black.Color);
            b.mass = 10;

            l1 = new Label();
            l1.AutoSize = true;
            this.Controls.Add(l1);
            l2 = new Label();
            l2.AutoSize = true;
            this.Controls.Add(l2);
            time = new Label();
            time.Location = new Point(0,0);
            time.AutoSize = true;
            this.Controls.Add(time);
        }
        public void TimePass(object o, EventArgs e)
        {
            i+=time_tick;
            if (i % 50 == 0)
            {
                if(i%1000==0)
                {
                    //this.Refresh();
                }
                
                time.Text = string.Format("{0:F}s",(double)i*time_tick/1000);
                //l1.Text = a.GetV();
                //l1.Location = new Point(a.GetRect().Left,a.GetRect().Bottom);
                //l2.Text = b.GetV();
                //l2.Location = new Point(b.GetRect().Left,b.GetRect().Bottom);
                graphics.FillEllipse(a.GetTexture(), a.GetRect());
                graphics.FillEllipse(b.GetTexture(), b.GetRect());
            }

            Vector2 Fa = a.GetF(b);
            Vector2 Fb = b.GetF(a);
            a.GetNext(Fa, (double)time_tick/1000, b);
            b.GetNext(Fb, (double)time_tick/1000, a);
        }
    }
}
