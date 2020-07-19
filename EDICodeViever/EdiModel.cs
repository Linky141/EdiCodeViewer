using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Controls;
using System.Windows;
using Microsoft.Win32;
using System.Threading;

namespace EDICodeViever
{
    class EdiModel
    {
        private EdiControler ediControler;

        private static string pathDesktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private static string pathDestination = pathDesktop + "\\EDICodeViever Out\\";

        public EdiModel(EdiControler controler)
        {
            this.ediControler = controler;
        }

        public string Separe(string input, string separator, string action)
        {
            string noFormatted = input;
            string formatted = "";

            for (int clk = 0; clk < noFormatted.Length; clk++)
            {
                if (noFormatted[clk] != separator[0]) formatted += noFormatted[clk];
                else
                {
                    if (action == "\\n") { formatted += noFormatted[clk]; formatted += "\n"; }
                    else if (action == "\\t") { formatted += noFormatted[clk]; formatted += "\t"; }
                    else { formatted += noFormatted[clk]; formatted += action; }
                }
            }

            return formatted;
        }

        public string ReverseStr(string input)
        {
            string output = "";
            for (int clk = input.Length - 1; clk >= 0; clk--)
            {
                output += input[clk];
            }
            return output;
        }

        public string CutCharacters(string input, bool chkbxCutEmpty)
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

                if (chkbxCutEmpty == true)
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
                //TODO: rzuc wyjatkiem i wyswietl go
                //MessageBox.Show(ex.Message);
            }
            return null;
        }

        public string CutCenter(string input, string tbxStart, string tbxEnd)
        {
            string surowy = input;
            string[] lines = surowy.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            string output = "";
            bool adding = false;
            for (int clk = 0; clk < lines.Length; clk++)
            {
                if (lines[clk].Length >= 3) 
                    if (lines[clk].Substring(0, 3) == tbxStart) 
                        adding = true;

                if (adding) 
                    output += lines[clk] + "\r\n";
                
                if (lines[clk].Length >= 3) 
                    if (lines[clk].Substring(0, 3) == tbxEnd) 
                        adding = false;
            }

            return output;
        }

        public string CutFileName(string input)
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

        public async void generateFiles(Dictionary<string, Object> controls)
        {
            Dictionary<string, TextBox> txtBoxes = (Dictionary<string, TextBox>) controls["textBoxes"];
            Dictionary<string, CheckBox> checkBoxes = (Dictionary<string, CheckBox>) controls["checkBoxes"];
            Dictionary<string, Button> buttons = (Dictionary<string, Button>) controls["buttons"];
            Dictionary<string, ProgressBar> pbars = (Dictionary<string, ProgressBar>)controls["progressBars"]; 

            buttons["btnChangeFiles"].Visibility = Visibility.Hidden;
            buttons["btnChangeFiles"].Opacity = 0;
            pbars["pbarWorkingWithFiles"].Visibility = Visibility.Visible;
            pbars["pbarWorkingWithFiles"].Opacity = 1;
            pbars["pbarWorkingWithFiles"].Value = 0;
            pbars["pbarWorkingWithFiles"].Minimum = 0;
            await Task.Run(() => {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Multiselect = true;
                openFileDialog.Filter = "All files (*.*)|*.*|Text files (*.txt)|*.txt";
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                if (openFileDialog.ShowDialog() == true)
                {
                    pbars["pbarWorkingWithFiles"].Dispatcher.Invoke(() => pbars["pbarWorkingWithFiles"].Maximum = openFileDialog.FileNames.Length);
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
                                txtBoxes["tbxIn"].Dispatcher.Invoke(() => txtBoxes["tbxIn"].Text = line);
                                object tmpo = null;
                                RoutedEventArgs tmpr = null;
                                buttons["btnRatowac"].Dispatcher.Invoke(() => ediControler.btnRatowac_Click(tmpo, tmpr));

                            }
                            while (ediControler.LockerToWrite) Thread.Sleep(100);
                            string path = pathDestination + "EDICodeVieverOUT_" + CutFileName(filename);
                            if (!File.Exists(path))
                            {
                                using (StreamWriter sw = File.CreateText(path))
                                {
                                    string tmp = "";
                                    txtBoxes["tbxOut"].Dispatcher.Invoke(() => tmp = txtBoxes["tbxOut"].Text);
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
                        pbars["pbarWorkingWithFiles"].Dispatcher.Invoke(() => pbars["pbarWorkingWithFiles"].Value++);


                        controls["buttons"] = buttons;
                        controls["textBoxes"] = txtBoxes;
                        controls["checkBoxes"] = checkBoxes;
                        controls["progressBars"] = pbars;
                        ediControler.actualiseControls();
                    }

                }
                txtBoxes["tbxIn"].Dispatcher.Invoke(() => txtBoxes["tbxIn"].Text = "GOTOWE!");
                txtBoxes["tbxOut"].Dispatcher.Invoke(() => txtBoxes["tbxOut"].Text = "");

                controls["textBoxes"] = txtBoxes;
                ediControler.actualiseControls();

                for (int clk = 0; clk < 10; clk++) 
                {
                    pbars["pbarWorkingWithFiles"].Dispatcher.Invoke(() => pbars["pbarWorkingWithFiles"].Opacity -= 0.1);
                    controls["progressBars"] = pbars;
                    ediControler.actualiseControls();
                    Thread.Sleep(50); 
                }
                
                buttons["btnChangeFiles"].Dispatcher.Invoke(() => buttons["btnChangeFiles"].Visibility = Visibility.Visible);

                for (int clk = 0; clk < 10; clk++) 
                {
                    buttons["btnChangeFiles"].Dispatcher.Invoke(() => buttons["btnChangeFiles"].Opacity += 0.1);
                    controls["buttons"] = buttons;
                    ediControler.actualiseControls();
                    Thread.Sleep(50); 
                }

                Thread.Sleep(200);
                pbars["pbarWorkingWithFiles"].Dispatcher.Invoke(() => pbars["pbarWorkingWithFiles"].Visibility = Visibility.Hidden);

                controls["progressBars"] = pbars;
                ediControler.actualiseControls();
            });
        }


    }
}
