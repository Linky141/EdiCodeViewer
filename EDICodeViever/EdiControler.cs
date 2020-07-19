using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace EDICodeViever
{
    class EdiControler
    {
        private EdiModel ediModel;
        private MainWindow mainWindow;
        private Dictionary<string, Object> controls;
        public bool LockerToWrite = false;

        public EdiControler(MainWindow window, Dictionary<string, CheckBox> chkbx, 
            Dictionary<string, TextBox> tbx, Dictionary<string, Button> btn, Dictionary<string, ProgressBar> pbars)
        {
            this.mainWindow = window;
            this.ediModel = new EdiModel(this);
            this.controls = new Dictionary<string, Object>();
            this.controls.Add("checkBoxes", chkbx);
            this.controls.Add("textBoxes", tbx);
            this.controls.Add("buttons", btn);
            this.controls.Add("progressBars", pbars);
        }

       public void btnRatowac_Click(object sender, RoutedEventArgs e)
        {
            LockerToWrite = true;
            Dictionary<string, TextBox> txtBoxes = (Dictionary<string, TextBox>)controls["textBoxes"];
            Dictionary<string, CheckBox> checkBoxes = (Dictionary<string, CheckBox>)controls["checkBoxes"];
            //Dictionary<string, Button> buttons = (Dictionary<string, Button>)controls["buttons"];

            if (checkBoxes["chkbxSepare"].IsChecked == true)
            {
                String text = txtBoxes["tbxIn"].Text;
                text = ediModel.Separe(text, txtBoxes["tbxSeparator"].Text, txtBoxes["tbxAction"].Text);
                if (checkBoxes["chkbxCutCharacters"].IsChecked == true)
                {
                    bool chkbxCutEmpty = checkBoxes["chkbxCutEmpty"].IsChecked ?? false;
                    text = ediModel.CutCharacters(text, chkbxCutEmpty);
                }
                if (checkBoxes["chkbxCutCenter"].IsChecked == true)
                {
                    text = ediModel.CutCenter(text, txtBoxes["tbxStart"].Text, txtBoxes["tbxEnd"].Text);
                }

                txtBoxes["tbxOut"].Text = text;

                //zamiana zedytowanego słownika z textBoxami
                controls.Remove("textBoxes");
                controls.Add("textBoxes", txtBoxes);
            }

            LockerToWrite = false;
        }

        private void chkbxSepare_Checked(object sender, RoutedEventArgs e)
        {
            Dictionary<string, TextBox> txtBoxes = (Dictionary<string, TextBox>)controls["textBoxes"];
            Dictionary<string, CheckBox> checkBoxes = (Dictionary<string, CheckBox>)controls["checkBoxes"];
            Dictionary<string, Button> buttons = (Dictionary<string, Button>)controls["buttons"];

            checkBoxes["chkbxCutEmpty"].IsEnabled = false;
            checkBoxes["chkbxCutEmpty"].IsChecked = false;
            checkBoxes["chkbxCutCharacters"].IsEnabled = true;
            checkBoxes["chkbxCutCenter"].IsEnabled = true;
            txtBoxes["tbxSeparator"].IsEnabled = true;
            txtBoxes["tbxAction"].IsEnabled = true;

            this.controls["textBoxes"] = txtBoxes;
            this.controls["checkBoxes"] = checkBoxes;
            this.controls["buttons"] = buttons;

            //this.controls.Remove("textBoxes");
            //this.controls.Remove("checkBoxes");
            //this.controls.Remove("buttons");

            //this.controls.Add("textBoxes", txtBoxes);
            //this.controls.Add("checkBoxes", checkBoxes);
            //this.controls.Add("buttons", buttons);
        }

        private void chkbxSepare_Unchecked(object sender, RoutedEventArgs e)
        {
            Dictionary<string, TextBox> txtBoxes = (Dictionary<string, TextBox>)controls["textBoxes"];
            Dictionary<string, CheckBox> checkBoxes = (Dictionary<string, CheckBox>)controls["checkBoxes"];
            Dictionary<string, Button> buttons = (Dictionary<string, Button>)controls["buttons"];

            checkBoxes["chkbxCutEmpty"].IsChecked = false;
            checkBoxes["chkbxCutCharacters"].IsChecked = false;
            checkBoxes["chkbxCutCenter"].IsChecked = false;
            checkBoxes["chkbxCutEmpty"].IsEnabled = false;
            checkBoxes["chkbxCutCharacters"].IsEnabled = false;
            checkBoxes["chkbxCutCenter"].IsEnabled = false;
            txtBoxes["tbxSeparator"].IsEnabled = false;
            txtBoxes["tbxAction"].IsEnabled = false;
            txtBoxes["tbxStart"].IsEnabled = false;
            txtBoxes["tbxEnd"].IsEnabled = false;

            this.controls["textBoxes"] = txtBoxes;
            this.controls["checkBoxes"] = checkBoxes;
            this.controls["buttons"] = buttons;
        }

        private void chkbxCutCharacters_Checked(object sender, RoutedEventArgs e)
        {
            ((Dictionary<string, CheckBox>)(this.controls["checkBoxes"]))["chkbxCutEmpty"].IsEnabled = true;
            //chkbxCutEmpty.IsEnabled = true;
        }

        private void chkbxCutCharacters_Unchecked(object sender, RoutedEventArgs e)
        {
            ((Dictionary<string, CheckBox>)(this.controls["checkBoxes"]))["chkbxCutEmpty"].IsEnabled = false;
            ((Dictionary<string, CheckBox>)(this.controls["checkBoxes"]))["chkbxCutEmpty"].IsChecked= false;

            //chkbxCutEmpty.IsChecked = false;
            //chkbxCutEmpty.IsEnabled = false;
        }

        private void chkbxCutCenter_Checked(object sender, RoutedEventArgs e)
        {
            ((Dictionary<string, TextBox>)(this.controls["textBoxes"]))["tbxStart"].IsEnabled = true;
            ((Dictionary<string, TextBox>)(this.controls["textBoxes"]))["tbxEnd"].IsEnabled = true;

            //tbxStart.IsEnabled = true;
            //tbxEnd.IsEnabled = true;
        }

        private void chkbxCutCenter_Unchecked(object sender, RoutedEventArgs e)
        {
            ((Dictionary<string, TextBox>)(this.controls["textBoxes"]))["tbxStart"].IsEnabled = false;
            ((Dictionary<string, TextBox>)(this.controls["textBoxes"]))["tbxEnd"].IsEnabled = false;

            //tbxStart.IsEnabled = false;
            //tbxEnd.IsEnabled = false;
        }

        public void actualiseControls()
        {
            mainWindow.actualiseControls(controls);
        }
    }
}