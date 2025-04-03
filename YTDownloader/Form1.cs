using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace YTDownloader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            await DownloadVideoAsync();
        }

        private async Task DownloadVideoAsync()
        {
            string url = textBox1.Text;
            string format = radioButton1.Checked ? "mp3" : "mp4";

            string outputFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), @"Downloads\YTDownloader");
            Directory.CreateDirectory(outputFolder);

            string fileFormat, arguments;

            if (format == "mp3")
            {
                fileFormat = "bestaudio";
                arguments = $"-f {fileFormat} -x --audio-format mp3 -o \"{outputFolder}/%(title)s.%(ext)s\" \"{url}\"";
            }
            else
            {
                fileFormat = "bestvideo+bestaudio";
                arguments = $"-f bestvideo+bestaudio[ext=m4a] --merge-output-format mp4 -o \"{outputFolder}/%(title)s.%(ext)s\" \"{url}\"";
            }

            Process process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "yt-dlp.exe",
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };


            process.Start();

            // Çıktıları gerçek zamanlı olarak UI'ye yaz
            await Task.Run(async () =>
            {
                while (!process.StandardOutput.EndOfStream)
                {
                    string line = await process.StandardOutput.ReadLineAsync();
                    AppendTextSafe(line + "\n");
                }
            });

            // Hata mesajlarını kontrol et
            string errorOutput = await process.StandardError.ReadToEndAsync();
            if (!string.IsNullOrWhiteSpace(errorOutput))
            {
                AppendTextSafe("HATA: " + errorOutput + "\n");
            }

            process.WaitForExit();
            AppendTextSafe("İndirme tamamlandı! Dosyalar 'Downloads' klasörüne kaydedildi.\n");
        }

        // UI Thread güvenliği için RichTextBox'a yazma işlemi
        private void AppendTextSafe(string text)
        {
            if (richTextBox1.InvokeRequired)
            {
                richTextBox1.Invoke(new Action(() => richTextBox1.AppendText(text)));
            }
            else
            {
                richTextBox1.AppendText(text);
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox1.ForeColor = System.Drawing.Color.Black;
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                textBox1.Text = "Video or Playlist URL";
                textBox1.ForeColor = System.Drawing.Color.Gray;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = "Video or Playlist URL";
            textBox1.ForeColor = System.Drawing.Color.Gray;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
    }
}
