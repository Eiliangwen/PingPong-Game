using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Deployment.Internal.CodeSigning;
using System.Data.Common;
using System.Security.Cryptography;

namespace Pingpong
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Bitmap btm;  //bitmapa pixela
        Graphics g;  //grafika i crtezi
        Graphics ekran;  //ekran na kojem ce biti prikazano

        Thread th;

        //postavljamo crteze s kojima baratamo
        Rectangle lopta = Rectangle.Empty;      
        Rectangle lijevo = Rectangle.Empty;
        Rectangle desno = Rectangle.Empty;
        //

        int lopta_brzina = 15;          //brzina lopte
        int lopta_brzinaY = 20;         //brzina lopte po Y osi
        int kretanje_brzina = 15;           //brzina pravokutnika

        Point kretanjeTo = Point.Empty;        //kuda se kreće pravokutnik
        Point loptakretanje = Point.Empty;    //kuda se lopta kreće

        bool drawing = true;  // crtez je istinit



        private void Form1_Load(object sender, EventArgs e)
        {

            //postavlja se program crtez
            btm = new Bitmap(this.Width, this.Height);  // visina širina
            g = Graphics.FromImage(btm);
            ekran = this.CreateGraphics();  //grafika ekrana
            //
            lopta = new Rectangle(this.Width / 2, this.Height / 2, 50, 50);   // širina, duzina i pozicija lopte  50 visina širina

            desno = new Rectangle(this.Width - 70, this.Height / 4, 15, 150); // širina, duzina i pozicija desnog pravokutnika
            lijevo = new Rectangle(5, this.Height / 2, 20, 150);   // širina, duzina i pozicija lijevog pravokutnika

            th = new Thread(draw);
            th.IsBackground = true;  //crta se pozadina

            th.Start();


        }

        public void draw()
        {
            try
            {
                while (drawing)
                {
                    kretanjeTo.Y = Cursor.Position.Y;  // desni pravokutnik se kreće prema lokaciji našeg miša na ekranu

                    g.Clear(Color.White);  //globalna boja se očisti

                    if (kretanjeTo.Y > desno.Y + 100) desno.Y += kretanje_brzina;   
                    if (kretanjeTo.Y < desno.Y + 100) desno.Y -= kretanje_brzina;

                    if (lopta.Y > lijevo.Y + 100) lijevo.Y += kretanje_brzina;  //brzine ako se udari u kut ekrana se promjeni brzina
                    if (lopta.Y < lijevo.Y + 100) lijevo.Y -= kretanje_brzina; //brzine ako se udari u kut ekrana se promjeni brzina

                    lopta.X += lopta_brzina;

                    if (loptakretanje.Y > lopta.Y) lopta.Y += lopta_brzinaY;
                    if (loptakretanje.Y < lopta.Y) lopta.Y -= lopta_brzinaY;

                    if (desno.IntersectsWith(lopta))    //ako se lopta udari u nešto brzina se promjeni u obrnutu
                    {
                        lopta_brzina *= -1;

                    }

                    if (lijevo.IntersectsWith(lopta))  //ako se lopta udari u nešto brzina se promjeni u obrnutu
                    {
                        lopta_brzina *= -1;
                    }

                    if (lopta.Y < 20) loptakretanje.Y = this.Height; // ako izade van ekrana lopta se vrati
                    if (lopta.Y > this.Height - 80) loptakretanje.Y = 0; // ako izade van ekrana lopta se vrati

                    if (lopta.X < -40) lopta.X = this.Width / 2; // ako izade van ekrana lopta se vrati
                    if (lopta.X > this.Width) lopta.X = this.Width / 2; // ako izade van ekrana lopta se vrati

                    g.FillRectangle(Brushes.Black, desno);  //boja za desni pravokutnik
                    g.FillRectangle(Brushes.Black, lijevo);   //boja za lijevi pravokutnik
                    g.FillEllipse(Brushes.Red, lopta);  //boja lopte

                   ekran.DrawImage(btm, 0, 0, this.Width, this.Height);    //nacrtava ekran pixele

                }

            }

            catch { }

        }



    }
}
