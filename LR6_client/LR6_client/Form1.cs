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
using System.Text;

namespace LR6_client
{
    public partial class Form1 : Form
    {
        TcpClient tcp_client = new TcpClient("localhost", 5555);
        ASCIIEncoding ae = new ASCIIEncoding();
        public Form1()
        {
            InitializeComponent();
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            listBox1.Visible = false;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            

        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            NetworkStream ns = tcp_client.GetStream();
            if (radioButton1.Checked == true)
            {
                //Создаем объект класса NetworkStream и ассоциируем его объектом класса TcpClient 
                
                String command = "view";
                String res = command + "|";
                listBox2.Items.Clear();
                //Создаем переменные типа byte[] для отправки запроса и получения результата 
                var  sent = ae.GetBytes(res);
                var recieved = new byte[256];
                //Отправляем запрос на сервер 
                ns.Write(sent, 0, sent.Length);
                //Получаем результат выполнения запроса с сервера 
                ns.Read(recieved, 0, recieved.Length);
                //Отображаем полученный результат в клиентском RichTextBox 
                var a = ae.GetString(recieved);
                List<string> list = a.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries).ToList(); ;
                list.Remove(list.Last());
                //richTextBox1.Text = ae.GetString(recieved);
                List<Record> records = new List<Record>();
                for(int i=0; i<list.Count-1; i++)
                {
                    var rec = new Record();
                    rec.mark = list[i];
                    i++;
                    rec.price = int.Parse(list[i]);
                    i++;
                    rec.name = list[i];
                    records.Add(rec);
                    string item = rec.mark + ' ' + rec.price.ToString() + ' ' + rec.name;
                    listBox2.Items.Add(item);
                }
                String status = "=>Command sent:view data";
                //Отображеем служебную информацию в клиентском ListBox 
                listBox1.Items.Add(status);
                
            }
            if (radioButton2.Checked == true)
            {
                var rec = new Record(textBox1.Text, int.Parse(textBox2.Text), textBox3.Text);
                string item = rec.mark + ' ' + rec.price.ToString() + ' ' + rec.name;
                listBox2.Items.Add(item);
                
                String command = "add";
                String res = command + "|" + item + ' ';

                //Создаем переменные типа byte[] для отправки запроса и получения результата 
                var sent = ae.GetBytes(res);
                var recieved = new byte[256];
                //Отправляем запрос на сервер 
                ns.Write(sent, 0, sent.Length);
                String status = "=>Command sent:add data";
                //Отображеем служебную информацию в клиентском ListBox 
                listBox1.Items.Add(status);
            }
            if (radioButton3.Checked == true)
            {
                var selectedItem = listBox2.SelectedItem.ToString();
                listBox2.Items.Remove(listBox2.SelectedItem);
                string command = "delete";
                string res = command + "|" + selectedItem + ' ';
                
                var sent = ae.GetBytes(res);
                ns.Write(sent, 0, sent.Length);
                String status = "=>Command sent:delete data";
                //Отображеем служебную информацию в клиентском ListBox 
                listBox1.Items.Add(status);

            }
            if (radioButton4.Checked == true)
            {
                var item = textBox1.Text + " " + textBox2.Text + " " + textBox3.Text;
                listBox2.Items[listBox2.SelectedIndex] =item;
                string command = "edit";
                string res = command + "|" + item + " " + listBox2.SelectedIndex.ToString() + " ";
                var sent = ae.GetBytes(res);
                ns.Write(sent, 0, sent.Length);
                String status = "=>Command sent:edit data";
                //Отображеем служебную информацию в клиентском ListBox 
                listBox1.Items.Add(status);
            }
            if (radioButton5.Checked == true)
            {
                String command = "search";
                String res = command + "|" + textBox1.Text+ " ";
                listBox2.Items.Clear();
                //Создаем переменные типа byte[] для отправки запроса и получения результата 
                var sent = ae.GetBytes(res);
                var recieved = new byte[256];
                //Отправляем запрос на сервер 
                ns.Write(sent, 0, sent.Length);
                //Получаем результат выполнения запроса с сервера 
                ns.Read(recieved, 0, recieved.Length);
                //Отображаем полученный результат в клиентском RichTextBox 
                var a = ae.GetString(recieved);
                List<string> list = a.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries).ToList(); ;
                list.Remove(list.Last());
                //richTextBox1.Text = ae.GetString(recieved);
                List<Record> records = new List<Record>();
                if (list.Count > 1)
                {
                    for (int i = 0; i < list.Count - 1; i++)
                    {
                        var rec = new Record();
                        rec.mark = list[i];
                        i++;
                        rec.price = int.Parse(list[i]);
                        i++;
                        rec.name = list[i];
                        records.Add(rec);
                        string item = rec.mark + ' ' + rec.price.ToString() + ' ' + rec.name;
                        listBox2.Items.Add(item);
                    }
                }
                else listBox2.Items.Add(list[0]);
                String status = "=>Command sent:search data";
                //Отображеем служебную информацию в клиентском ListBox 
                listBox1.Items.Add(status);
            }
            }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked) listBox1.Visible = true;
            else listBox1.Visible = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            
                textBox1.Enabled = !textBox1.Enabled;
                textBox2.Enabled = !textBox2.Enabled;
                textBox3.Enabled = !textBox3.Enabled;
            
        }
        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = !textBox1.Enabled;
            textBox2.Enabled = !textBox2.Enabled;
            textBox3.Enabled = !textBox3.Enabled;
        }
        
        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var item = listBox2.SelectedItem.ToString().Split();
                textBox1.Text = item[0];
                textBox2.Text = item[1];
                textBox3.Text = item[2];
            }
            catch (Exception ex) { }
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = !textBox1.Enabled;
        }
    }
}
