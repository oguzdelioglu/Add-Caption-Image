using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AddCaptionImage
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public void beklet(int kacsn)
        {
            wait.Interval = kacsn * 1000;
            wait.Enabled = true;
            while (wait.Enabled == true)
            {
                System.Windows.Forms.Application.DoEvents();
            }
        }
        private static string CleanFileName(string fileName)
        {
            return Path.GetInvalidFileNameChars().Aggregate(fileName, (current, c) => current.Replace(c.ToString(), string.Empty));
        }
        private void button1_Click(object sender, EventArgs e)
        {
            seturl();
        }
        public void wbbekle()
        {
            while (wb.ReadyState != WebBrowserReadyState.Complete)
            {
                Application.DoEvents();
            }
        }
        public void seturl()
        {
            link.Text = link.Text.Replace("https", "http");
          editor:
            wb.Navigate("http://wigflip.com/automotivator/?src=editor");
            try
            {
                beklet(6);
                wb.Document.GetElementById("transloadUrl").InnerText = link.Text;
                wb.Document.Forms[2].InvokeMember("submit");
                beklet(5);
            }
            catch
            {
                goto editor;
            }

            try
            {
                HtmlElement textArea = wb.Document.All["text"];
                if (textArea != null)
                {
                    textArea.InnerText = yazı.Text;
                }
                HtmlElementCollection col = wb.Document.GetElementsByTagName("input");
                foreach (HtmlElement helemnt in col)
                {
                    if (helemnt.GetAttribute("value") == "Preview")
                    {
                        helemnt.InvokeMember("Click");
                    }

                    if (helemnt.GetAttribute("id") == "saveButton")
                    {
                        beklet(3);
                        helemnt.InvokeMember("Click");
                    }
                }
                beklet(5);
                col = wb.Document.GetElementsByTagName("a");
                foreach (HtmlElement helemnt in col)
                {
                    if (helemnt.GetAttribute("id") == "download")
                    {
                        try
                        {
                            using (WebClient client2 = new WebClient())
                            {
                                string filename = yazı.Text;
                                try
                                {
                                    filename = filename.Substring(0, 206);
                                }
                                catch
                                {

                                }
                                try
                                {
                                    filename = CleanFileName(filename);
                                }
                                catch
                                {

                                }
                                client2.DownloadFile(helemnt.GetAttribute("href"), "images\\" + filename + ".jpg");
                            }
                        }
                        catch
                        {
                        }
                    }
                }
            }
            catch
            {
                goto editor;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            try { Directory.CreateDirectory(Application.StartupPath + "\\images\\"); } catch { }
        }

        private void wait_Tick(object sender, EventArgs e)
        {
            wait.Enabled = false;
        }
    }
}
