using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;


namespace LR6_server
{
    public partial class Form1 : Form
    {
        //Путь к файлу, в котором будет храниться информация 
        String fileName = "data.txt";
        int fileCount = 0;
        //Создание объекта класса TcpListener 
        TcpListener listener = null; //Создание объекта класса Socket 
        Socket socket = null;
        //Создание объекта класса NetworkStream 
        NetworkStream ns = null;
        //Создание объекта класса кодировки ASCIIEncoding 
        ASCIIEncoding ae = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            listener = new TcpListener(IPAddress.Any, 5555);
            // Активация listen’ера 
            listener.Start();
            
                socket = listener.AcceptSocket();

                if (socket.Connected)
                {
                    ns = new NetworkStream(socket);
                    ae = new ASCIIEncoding();
                    //Создаем новый экземпляр класса ThreadClass 
                    ThreadClass threadClass = new ThreadClass();
                    //Создаем новый поток 
                    Thread thread = threadClass.Start(ns, fileName, fileCount, this);
                }
            


        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
