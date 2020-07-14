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
            chkbxCutEmpty.IsEnabled = true;
            chkbxCutCharacters.IsEnabled = true;
            chkbxCutCenter.IsEnabled = true;
        }

        private void chkbxSepare_Unchecked(object sender, RoutedEventArgs e)
        {
            chkbxCutEmpty.IsChecked = false;
            chkbxCutCharacters.IsChecked = false;
            chkbxCutCenter.IsChecked = false;
            chkbxCutEmpty.IsEnabled = false;
            chkbxCutCharacters.IsEnabled = false;
            chkbxCutCenter.IsEnabled = false;
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


    }
}
