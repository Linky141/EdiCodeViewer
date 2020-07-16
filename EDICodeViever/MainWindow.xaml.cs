using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace EDICodeViever
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            chkbxSepare.IsChecked = true;
            chkbxCutEmpty.IsChecked = true;
            chkbxCutCharacters.IsChecked = true;
            chkbxCutCenter.IsChecked = true;
            pbarWorkingWithFiles.Visibility = Visibility.Hidden;
        }

        static string pathDesktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        static string pathDestination = pathDesktop + "\\EDICodeViever Out\\";
        bool WindowMaximalized = false;
        bool LockerTowrite = false;

        private string Separe(string input)
        {
            string noFormatted = input;
            string formatted = "";

            for (int clk = 0; clk < noFormatted.Length; clk++)
            {
                if (noFormatted[clk] != tbxSeparator.Text[0]) formatted += noFormatted[clk];
                else
                {
                    if (tbxAction.Text == "\\n") { formatted += noFormatted[clk]; formatted += "\n"; }
                    else if (tbxAction.Text == "\\t") { formatted += noFormatted[clk]; formatted += "\t"; }
                    else { formatted += noFormatted[clk]; formatted += tbxAction.Text; }
                }
            }

            return formatted;
        }


        string ReverseStr(string input)
        {
            string output = "";
            for (int clk = input.Length - 1; clk >= 0; clk--)
            {
                output += input[clk];
            }
            return output;
        }

        private string CutCharacters(string input)
        {
            try
            {
                List<string> goodCharacters = new List<string>();

                using (StreamReader sr = new StreamReader("GoodCharacters.txt"))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        goodCharacters.Add(line);
                    }
                }
                goodCharacters.Add("\n");
                goodCharacters.Add("\r");
                goodCharacters.Add("\r\n");


                string noEdited = input;
                string edited = "";

                for (int clk = 0; clk < noEdited.Length; clk++)
                {
                    if (goodCharacters.Contains(noEdited[clk].ToString())) edited += noEdited[clk];
                }

                edited = ReverseStr(edited);
                bool blocking = false;
                char last = ' ';
                string edited2 = "";

                if (chkbxCutEmpty.IsChecked == true)
                {
                    for (int clk = 0; clk < edited.Length; clk++)
                    {
                        if (last == '+' && edited[clk] == ':') blocking = true;
                        if (edited[clk] != ':') blocking = false;
                        if (!blocking) edited2 += edited[clk];
                        last = edited[clk];
                    }
                }
                else
                {
                    edited2 = edited;
                }

                //edited2.Reverse();
                edited2 = ReverseStr(edited2);

                return edited2;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return null;
        }

        private string CutCenter(string input)
        {
            string surowy = input;
            string[] lines = surowy.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            string output = "";
            bool adding = false;
            for (int clk = 0; clk < lines.Length; clk++)
            {
                if (lines[clk].Length >= 3) if (lines[clk].Substring(0, 3) == tbxStart.Text) adding = true;
                if (adding) output += lines[clk] + "\r\n";
                if (lines[clk].Length >= 3) if (lines[clk].Substring(0, 3) == tbxEnd.Text) adding = false;
            }

            return output;
            


        }

        private void btnRatowac_Click(object sender, RoutedEventArgs e)
        {
            LockerTowrite = true;

            if (chkbxSepare.IsChecked == true)
            {
                string text = tbxIn.Text;
                text = Separe(text);
                if (chkbxCutCharacters.IsChecked == true)
                {
                    text=CutCharacters(text);
                }
                if (chkbxCutCenter.IsChecked == true)
                {
                    text=CutCenter(text);
                }
                tbxOut.Text = text;
            }
            LockerTowrite = false;
        }

        private void chkbxSepare_Checked(object sender, RoutedEventArgs e)
        {
            chkbxCutEmpty.IsEnabled = false;
            chkbxCutEmpty.IsChecked = false;
            chkbxCutCharacters.IsEnabled = true;
            chkbxCutCenter.IsEnabled = true;
            tbxSeparator.IsEnabled = true;
            tbxAction.IsEnabled = true;
        }

        private void chkbxSepare_Unchecked(object sender, RoutedEventArgs e)
        {
            chkbxCutEmpty.IsChecked = false;
            chkbxCutCharacters.IsChecked = false;
            chkbxCutCenter.IsChecked = false;
            chkbxCutEmpty.IsEnabled = false;
            chkbxCutCharacters.IsEnabled = false;
            chkbxCutCenter.IsEnabled = false;
            tbxSeparator.IsEnabled = false;
            tbxAction.IsEnabled = false;
            tbxStart.IsEnabled = false;
            tbxEnd.IsEnabled = false;
        }

        private void chkbxCutCharacters_Checked(object sender, RoutedEventArgs e)
        {
            chkbxCutEmpty.IsEnabled = true;
        }

        private void chkbxCutCharacters_Unchecked(object sender, RoutedEventArgs e)
        {
            chkbxCutEmpty.IsChecked = false;
            chkbxCutEmpty.IsEnabled = false;
        }

        private void chkbxCutCenter_Checked(object sender, RoutedEventArgs e)
        {
            tbxStart.IsEnabled = true;
            tbxEnd.IsEnabled = true;
        }

        private void chkbxCutCenter_Unchecked(object sender, RoutedEventArgs e)
        {
            tbxStart.IsEnabled = false;
            tbxEnd.IsEnabled = false;
        }


        string CutFileName(string input)
        {
            string tmp = "";
            for (int clk = input.Length - 1; clk > 0; clk--)
            {
                if (input[clk] == '\\')
                {
                    tmp = ReverseStr(tmp);
                    return tmp;
                }
                tmp += input[clk];

            }
            tmp = ReverseStr(tmp);
            return tmp;
        }

        private async void btnChangeFiles_Click(object sender, RoutedEventArgs e)
        {
            btnChangeFiles.Visibility = Visibility.Hidden;
            btnChangeFiles.Opacity = 0;
            pbarWorkingWithFiles.Visibility = Visibility.Visible;
            pbarWorkingWithFiles.Opacity = 1;
            pbarWorkingWithFiles.Value = 0;
            pbarWorkingWithFiles.Minimum = 0;
            await Task.Run(() => {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "All files (*.*)|*.*|Text files (*.txt)|*.txt";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                    pbarWorkingWithFiles.Dispatcher.Invoke(() => pbarWorkingWithFiles.Maximum = openFileDialog.FileNames.Length);
                if (!Directory.Exists(pathDestination)) Directory.CreateDirectory(pathDestination);
                else
                {
                    Directory.Delete(pathDestination, true);
                    Directory.CreateDirectory(pathDestination);
                }
                int index = 0;
                foreach (string filename in openFileDialog.FileNames)
                {
                        try
                        {
                            using (StreamReader sr = new StreamReader(filename))
                            {
                                string line = sr.ReadToEnd();
                                tbxIn.Dispatcher.Invoke(() => tbxIn.Text = line);
                                object tmpo = null;
                                RoutedEventArgs tmpr = null;
                                btnRatowac.Dispatcher.Invoke(() => btnRatowac_Click(tmpo, tmpr));
                                
                            }
                            while(LockerTowrite) Thread.Sleep(100);
                            string path = pathDestination + "EDICodeVieverOUT_" + CutFileName(filename);
                            if (!File.Exists(path))
                            {
                                using (StreamWriter sw = File.CreateText(path))
                                {
                                    string tmp = "";                                   
                                    tbxOut.Dispatcher.Invoke(() => tmp = tbxOut.Text);
                                 //   MessageBox.Show(tmp);
                                    sw.Write(tmp);
                                }
                            }
                        }
                        catch (IOException ex)
                        {
                            MessageBox.Show("The file could not be read:\n" + ex.Message);
                        }
                        index++;
                        pbarWorkingWithFiles.Dispatcher.Invoke(() => pbarWorkingWithFiles.Value++);
                    }
                
            }
                tbxIn.Dispatcher.Invoke(() => tbxIn.Text = "GOTOWE!");
                tbxOut.Dispatcher.Invoke(() => tbxOut.Text = "");

                for (int clk = 0; clk < 10; clk++) { pbarWorkingWithFiles.Dispatcher.Invoke(() => pbarWorkingWithFiles.Opacity -= 0.1); Thread.Sleep(50); }
                btnChangeFiles.Dispatcher.Invoke(() => btnChangeFiles.Visibility = Visibility.Visible);
                for (int clk = 0; clk < 10; clk++) { btnChangeFiles.Dispatcher.Invoke(() => btnChangeFiles.Opacity += 0.1); Thread.Sleep(50); }
                Thread.Sleep(200);
                pbarWorkingWithFiles.Dispatcher.Invoke(() => pbarWorkingWithFiles.Visibility = Visibility.Hidden);

            });
        }

        private void btnMinimalise_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnMaximalise_Click(object sender, RoutedEventArgs e)
        {
            if (!WindowMaximalized)
            {
                WindowState = WindowState.Maximized;
                WindowMaximalized = true;
                btnMaximalise.Content = "v";
            }
            else
            {
                WindowState = WindowState.Normal;
                WindowMaximalized = false;
                btnMaximalise.Content = "^";
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) this.DragMove();
        }
    }
}

