using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace LR6_server
{
    class ThreadClass
    {
        //Путь к файлу, в котором будет храниться информация 
      public String fileName { get; set; }
        int fileCount { get; set; }
        //Создание объекта класса TcpListener 
        TcpListener listener { get; set; } //Создание объекта класса Socket 
        Socket socket { get; set; }
        //Создание объекта класса NetworkStream 
        NetworkStream ns { get; set; }
        //Создание объекта класса кодировки ASCIIEncoding 
        ASCIIEncoding ae { get; set; }
        Form1 form { get; set; }
        public Thread Start(NetworkStream ns, String fileName, int fileCount, Form1 form)
        {
            this.ns = ns;
            this.ae = new ASCIIEncoding();
            this.fileName = fileName;

            this.fileCount = fileCount;
            this.form = form;
            //Создание нового экземпляра класса Thread 
            Thread thread = new Thread(new ThreadStart((ThreadOperations)));
            //Запуск потока 
            thread.Start();
            return thread;
          
        }

        void ThreadOperations()
        {

            //Создаем новую переменную типа byte[] 
            var received = new byte[256]; //С помощью сетевого потока считываем в переменную received данные от клиента 
            while (true)
            {
                ns.Read(received, 0, received.Length);
                if (received != null)
                {
                    var s1 = ae.GetString(received);
                    var i = s1.IndexOf("|", 0);
                    var cmd = s1.Substring(0, i);
                    if (cmd.CompareTo("view") == 0)
                    {
                        // Создаем переменную типа byte[] для отправки ответа клиенту 
                        byte[] sent = new byte[256];
                        //Создаем объект класса FileStream для последующего чтения информации из файла 
                        FileStream fstr = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                        List<Record> recordsList = new List<Record>();

                        //   fstr.Seek(0, SeekOrigin.Begin);
                        StreamReader sreader = new StreamReader(fstr);
                        /*  while (!sreader.EndOfStream)
                          {
                              var splitedrecord = sreader.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                              var record = new Record(splitedrecord[0], int.Parse(splitedrecord[1]), splitedrecord[2]);
                              recordsList.Add(record);
                      }*/
                        //Запись в переменную sent содержания прочитанного файла 
                        sent = ae.GetBytes(sreader.ReadToEnd());
                        sreader.Close();
                        fstr.Close(); //Отправка информации клиенту 
                        ns.Write(sent, 0, sent.Length);
                        
                    }
                    if (cmd.CompareTo("add") == 0)
                    {
                        var item = s1.Substring(i + 1, s1.Length - (i + 1)).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        item.Remove(item.Last());
                        FileStream fstr = new FileStream("data.txt", FileMode.OpenOrCreate, FileAccess.Write);
                        fstr.Seek(0, SeekOrigin.End);
                        StreamWriter sw = new StreamWriter(fstr);
                        //запись в файл 
                        sw.WriteLine(item[0] + " " + item[1] + " " + item[2] + " ");
                        sw.Close();
                        fstr.Close();

                    }

                    if (cmd.CompareTo("delete") == 0)
                    {
                        var item = s1.Substring(i + 1, s1.Length - (i + 1)).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        item.Remove(item.Last());
                        List<Record> recordsList = new List<Record>();
                        FileStream fstr = new FileStream("data.txt", FileMode.OpenOrCreate, FileAccess.Read);
                        fstr.Seek(0, SeekOrigin.Begin);
                        StreamReader sr = new StreamReader(fstr);
                        while (!sr.EndOfStream)
                        {
                            var splitedrecord = sr.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            var record = new Record(splitedrecord[0], int.Parse(splitedrecord[1]), splitedrecord[2]);
                            recordsList.Add(record);
                        }
                        sr.Close();
                        fstr.Close();
                        var remItem = new Record(item[0], int.Parse(item[1]), item[2]);
                        var it = recordsList.Where(iq => iq.mark == remItem.mark && iq.name == remItem.name && iq.price == remItem.price).FirstOrDefault();
                        var index = recordsList.IndexOf(it);
                        recordsList.Remove(recordsList[recordsList.IndexOf(it)]);
                        FileStream fstr1 = new FileStream("data.txt", FileMode.Truncate, FileAccess.Write);
                        fstr1.Seek(0, SeekOrigin.Begin);
                        StreamWriter sw = new StreamWriter(fstr1);
                        foreach (Record record in recordsList)
                        {
                            sw.WriteLine(record.mark + " " + record.price.ToString() + " " + record.name + " ");
                        }
                        sw.Close();
                        fstr1.Close();

                    }
                    if (cmd.CompareTo("edit") == 0)
                    {
                        var item = s1.Substring(i + 1, s1.Length - (i + 1)).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        item.Remove(item.Last());
                        List<Record> recordsList = new List<Record>();
                        FileStream fstr = new FileStream("data.txt", FileMode.OpenOrCreate, FileAccess.Read);
                        fstr.Seek(0, SeekOrigin.Begin);
                        StreamReader sr = new StreamReader(fstr);
                        while (!sr.EndOfStream)
                        {
                            var splitedrecord = sr.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            var record = new Record(splitedrecord[0], int.Parse(splitedrecord[1]), splitedrecord[2]);
                            recordsList.Add(record);
                        }
                        sr.Close();
                        fstr.Close();
                        var remItem = new Record(item[0], int.Parse(item[1]), item[2]);
                        recordsList[int.Parse(item[3])] = remItem;
                        FileStream fstr1 = new FileStream("data.txt", FileMode.Truncate, FileAccess.Write);
                        fstr1.Seek(0, SeekOrigin.Begin);
                        StreamWriter sw = new StreamWriter(fstr1);
                        foreach (Record record in recordsList)
                        {
                            sw.WriteLine(record.mark + " " + record.price.ToString() + " " + record.name + " ");
                        }
                        sw.Close();
                        fstr1.Close();
                    }
                    if (cmd.CompareTo("search") == 0)
                    {
                        var mark = s1.Substring(i + 1, s1.Length - (i + 1)).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        mark.Remove(mark.Last());
                        // Создаем переменную типа byte[] для отправки ответа клиенту 
                        byte[] sent = new byte[256];
                        //Создаем объект класса FileStream для последующего чтения информации из файла 
                        FileStream fstr = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                        List<Record> recordsList = new List<Record>();

                        //   fstr.Seek(0, SeekOrigin.Begin);
                        StreamReader sreader = new StreamReader(fstr);
                          while (!sreader.EndOfStream)
                          {
                              var splitedrecord = sreader.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                              var record = new Record(splitedrecord[0], int.Parse(splitedrecord[1]), splitedrecord[2]);
                              recordsList.Add(record);
                      }
                        var sentList = recordsList.Where(item => item.mark == mark[0]).ToList();
                        string sentstring ="";
                        if (sentList.Count > 0)
                        {
                            foreach (var item in sentList)
                            {
                                sentstring += item.mark + " " + item.price.ToString() + " " + item.name + " ";
                            }
                        }
                        else sentstring = "NotFound ";
                        //Запись в переменную sent содержания прочитанного файла 
                        sent = ae.GetBytes(sentstring);
                        sreader.Close();
                        fstr.Close(); //Отправка информации клиенту 
                        ns.Write(sent, 0, sent.Length);
                        

                    }
                }
            }
        }
    }
}
