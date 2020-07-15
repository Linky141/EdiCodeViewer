using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;

namespace EDICodeViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
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
        }

        static string pathDesktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        static string pathDestination = pathDesktop + "\\EDICodeViever Out\\";

        private void Separe()
        {
            string noFormatted = tbxIn.Text;
            string formatted = "";

            for (int clk = 0; clk < noFormatted.Length; clk++)
            {
                if (noFormatted[clk] != tbxSeparator.Text[0]) formatted += noFormatted[clk];
                else {
                if(tbxAction.Text == "\\n") { formatted += noFormatted[clk]; formatted += "\n";}
                else if(tbxAction.Text == "\\t"){ formatted += noFormatted[clk]; formatted += "\t";}
                else { formatted += noFormatted[clk]; formatted += tbxAction.Text;}
                }
            }

            tbxOut.Text = formatted;
        }


        string ReverseStr(string input)
    {
        string output = "";
        for (int clk = input.Length-1; clk >= 0; clk--)
        {
            output += input[clk];
        }
        return output;
    }

        private void CutCharacters()
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

                string noEdited = tbxOut.Text;
                string edited = "";
                tbxOut.Text = "";

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

                tbxIn.Text = edited2;
                object obj = null;
                RoutedEventArgs rag = null;
                Separe();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CutCenter()
        {
            string surowy = tbxOut.Text;
            string[] lines = surowy.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            string output = "";
            bool adding = false;
            for (int clk = 0; clk < lines.Length; clk++)
            {
                if (lines[clk].Length >= 3) if (lines[clk].Substring(0, 3) == tbxStart.Text) adding = true;
                if (adding) output += lines[clk];
                if (lines[clk].Length >= 3) if (lines[clk].Substring(0, 3) == tbxEnd.Text) adding = false;
            }

            tbxIn.Text = output;
            object obj = null;
            RoutedEventArgs rag = null;
            Separe();


        }

        private void btnRatowac_Click(object sender, RoutedEventArgs e)
        {
            if (chkbxSepare.IsChecked == true)
            {
                Separe();

                if (chkbxCutCharacters.IsChecked == true)
                {
                    CutCharacters();
                }
                if (chkbxCutCenter.IsChecked == true)
                {
                    CutCenter();
                }
            }

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
            for (int clk = input.Length-1; clk > 0; clk--)
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

        private void btnChangeFiles_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Multiselect = true;
			openFileDialog.Filter = "All files (*.*)|*.*|Text files (*.txt)|*.txt";
			openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			if(openFileDialog.ShowDialog() == true)
			{

                if (!Directory.Exists(pathDestination)) Directory.CreateDirectory(pathDestination);
                else
                {
                    Directory.Delete(pathDestination, true);
                    Directory.CreateDirectory(pathDestination);
                }
                int index = 0;
				foreach(string filename in openFileDialog.FileNames){
                    try
                    {   // Open the text file using a stream reader.
                        using (StreamReader sr = new StreamReader(filename))
                        {
                            String line = sr.ReadToEnd();
                            tbxIn.Text = line;
                            object tmpo = null;
                            RoutedEventArgs tmpr = null;
                            btnRatowac_Click(tmpo, tmpr);
                        }
                        string path = pathDestination + "EDICodeVieverOUT_" + CutFileName(filename);
                        if (!File.Exists(path))
                        {
                            using (StreamWriter sw = File.CreateText(path))
                            {
                                sw.Write(tbxOut.Text);
                            }
                        }
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show("The file could not be read:\n" + ex.Message);
                    }
                    index++;
                }
			}
            tbxIn.Text = "GOTOWE!";
            tbxOut.Text = "";
        }


    }
}
