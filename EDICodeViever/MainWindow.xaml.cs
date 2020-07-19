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
        EdiControler ediConroler;

        public MainWindow()
        {
            InitializeComponent();
            chkbxSepare.IsChecked = true;
            chkbxCutEmpty.IsChecked = true;
            chkbxCutCharacters.IsChecked = true;
            chkbxCutCenter.IsChecked = true;
            pbarWorkingWithFiles.Visibility = Visibility.Hidden;

          
            chkbxs.Add("chkbxSepare", chkbxSepare);
            chkbxs.Add("chkbxCutEmpty", chkbxCutEmpty);
            chkbxs.Add("CutCharacters", chkbxCutCharacters);
            chkbxs.Add("chkbxCutCenter", chkbxCutCenter);

            tbxs.Add("tbxIn", tbxIn);
            tbxs.Add("tbxOut", tbxOut);
            tbxs.Add("tbxStart", tbxStart);
            tbxs.Add("tbxEnd", tbxEnd);
            tbxs.Add("tbxAction", tbxAction);
            tbxs.Add("tbxSeparator", tbxSeparator);

            buttons.Add("btnRatowac", btnRatowac);
            buttons.Add("btnChangeFiles", btnChangeFiles);
            buttons.Add("btnClose", btnClose);
            buttons.Add("btnMaximalise", btnMaximalise);
            buttons.Add("btnMinimalise", btnMinimalise);

            pbars.Add("pbarWorkingWithFiles", pbarWorkingWithFiles);

            ediConroler = new EdiControler(this, chkbxs, tbxs, buttons, pbars);
        }

        static string pathDesktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        static string pathDestination = pathDesktop + "\\EDICodeViever Out\\";
        bool WindowMaximalized = false;
        bool LockerTowrite = false;


        Dictionary<string, CheckBox> chkbxs = new Dictionary<string, CheckBox>();
        Dictionary<string, TextBox> tbxs = new Dictionary<string, TextBox>();
        Dictionary<string, Button> buttons = new Dictionary<string, Button>();
        Dictionary<string, ProgressBar> pbars = new Dictionary<string, ProgressBar>();

        //do usuniecia
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

        //do usuniecia
        string ReverseStr(string input)
        {
            string output = "";
            for (int clk = input.Length - 1; clk >= 0; clk--)
            {
                output += input[clk];
            }
            return output;
        }

        //do usuniecia
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

        //do usuniecia
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

        //do usuniecia
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

        //do usuniecia
        private void chkbxSepare_Checked(object sender, RoutedEventArgs e)
        {
            chkbxCutEmpty.IsEnabled = false;
            chkbxCutEmpty.IsChecked = false;
            chkbxCutCharacters.IsEnabled = true;
            chkbxCutCenter.IsEnabled = true;
            tbxSeparator.IsEnabled = true;
            tbxAction.IsEnabled = true;
        }

        //do usuniecia
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

        //do usuniecia
        private void chkbxCutCharacters_Checked(object sender, RoutedEventArgs e)
        {
            chkbxCutEmpty.IsEnabled = true;
        }

        //do usuniecia
        private void chkbxCutCharacters_Unchecked(object sender, RoutedEventArgs e)
        {
            chkbxCutEmpty.IsChecked = false;
            chkbxCutEmpty.IsEnabled = false;
        }

        //do usuniecia
        private void chkbxCutCenter_Checked(object sender, RoutedEventArgs e)
        {
            tbxStart.IsEnabled = true;
            tbxEnd.IsEnabled = true;
        }

        //do usuniecia
        private void chkbxCutCenter_Unchecked(object sender, RoutedEventArgs e)
        {
            tbxStart.IsEnabled = false;
            tbxEnd.IsEnabled = false;
        }

        //do usuniecia
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

        //do usuniecia
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

        //zostawic
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


        public void actualiseControls(Dictionary<string, Object> controls)
        {
            Dictionary<string, TextBox> txtBoxes = (Dictionary<string, TextBox>)controls["textBoxes"];
            Dictionary<string, CheckBox> checkBoxes = (Dictionary<string, CheckBox>)controls["checkBoxes"];
            Dictionary<string, Button> buttons = (Dictionary<string, Button>)controls["buttons"];
            Dictionary<string, ProgressBar> pgBars = (Dictionary<string, ProgressBar>)controls["progressBars"];

            //TODO: dodac aktualizacje wszystkich kontrolek
            pbars.Add("pbarWorkingWithFiles", pbarWorkingWithFiles);

            chkbxSepare.IsChecked = checkBoxes["chkbxSepare"].IsChecked;
            chkbxSepare.IsEnabled = checkBoxes["chkbxSepare"].IsEnabled;
            chkbxCutEmpty.IsChecked = checkBoxes["chkbxCutEmpty"].IsChecked;
            chkbxCutEmpty.IsEnabled = checkBoxes["chkbxCutEmpty"].IsEnabled;
            chkbxCutCharacters.IsChecked = checkBoxes["chkbxCutCharacters"].IsChecked;
            chkbxCutCharacters.IsEnabled = checkBoxes["chkbxCutCharacters"].IsEnabled;
            chkbxCutCenter.IsChecked = checkBoxes["chkbxCutCenter"].IsChecked;
            chkbxCutCenter.IsEnabled = checkBoxes["chkbxCutCenter"].IsEnabled;

            tbxIn.Text = txtBoxes["tbxIn"].Text;
            tbxIn.IsEnabled = txtBoxes["tbxIn"].IsEnabled;
            tbxOut.Text = txtBoxes["tbxOut"].Text;
            tbxOut.IsEnabled = txtBoxes["tbxOut"].IsEnabled;
            tbxStart.Text = txtBoxes["tbxStart"].Text;
            tbxStart.IsEnabled = txtBoxes["tbxStart"].IsEnabled;
            tbxEnd.Text = txtBoxes["tbxEnd"].Text;
            tbxEnd.IsEnabled = txtBoxes["tbxEnd"].IsEnabled;
            tbxAction.Text = txtBoxes["tbxAction"].Text;
            tbxAction.IsEnabled = txtBoxes["tbxAction"].IsEnabled;
            tbxSeparator.Text = txtBoxes["tbxSeparator"].Text;
            tbxSeparator.IsEnabled = txtBoxes["tbxSeparator"].IsEnabled;

            btnRatowac.Visibility = buttons["btnRatowac"].Visibility;
            btnRatowac.Opacity = buttons["btnRatowac"].Opacity;
            btnRatowac.Content = buttons["btnRatowac"].Content;
            btnChangeFiles.Visibility = buttons["btnChangeFiles"].Visibility;
            btnChangeFiles.Opacity = buttons["btnChangeFiles"].Opacity;
            btnChangeFiles.Content = buttons["btnChangeFiles"].Content;
            btnClose.Visibility = buttons["btnClose"].Visibility;
            btnClose.Opacity = buttons["btnClose"].Opacity;
            btnClose.Content = buttons["btnClose"].Content;
            btnMaximalise.Visibility = buttons["btnMaximalise"].Visibility;
            btnMaximalise.Opacity = buttons["btnMaximalise"].Opacity;
            btnMaximalise.Content = buttons["btnMaximalise"].Content;
            btnMinimalise.Visibility = buttons["btnMinimalise"].Visibility;
            btnMinimalise.Opacity = buttons["btnMinimalise"].Opacity;
            btnMinimalise.Content = buttons["btnMinimalise"].Content;

            pbarWorkingWithFiles.Visibility = pgBars["pbarWorkingWithFiles"].Visibility;
            pbarWorkingWithFiles.Opacity = pgBars["pbarWorkingWithFiles"].Opacity;
            pbarWorkingWithFiles.Value = pgBars["pbarWorkingWithFiles"].Value;
            pbarWorkingWithFiles.Minimum = pgBars["pbarWorkingWithFiles"].Minimum;
            pbarWorkingWithFiles.Maximum = pgBars["pbarWorkingWithFiles"].Maximum;
        }
    }
}

