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
    }
}