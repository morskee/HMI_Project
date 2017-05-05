using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Recognition;
using System.Speech.Synthesis;using System.Threading;


namespace HMI_project
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            aGauge1.Value = trackBar1.Value;

            label11.Text = "";
            if (trackBar2.Value > 0 && trackBar1.Value > 0)
            {
                float zmienna = trackBar2.Value + trackBar1.Value;
                float zmienna2 = zmienna / 25;

                label11.Text = "" + zmienna2.ToString("F2") + "L/100KM";
            }
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            RecognizeSpeech();

            label3.Text = DateTime.Now.ToLongTimeString();

            label4.Text = DateTime.Now.ToLongDateString();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label3.Text = DateTime.Now.ToLongTimeString();
            
            label4.Text = DateTime.Now.ToLongDateString();

            timer1.Start();
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            aGauge2.Value = trackBar2.Value;
            label6.Text = "";
            if (trackBar2.Value >= 200) {
                label6.Text = "WARNING!";
            }
            label11.Text = "";
            if (trackBar2.Value > 0 && trackBar1.Value > 0)
            {
                float zmienna = trackBar2.Value + trackBar1.Value;
                float zmienna2 = zmienna / 25;
                label11.Text = "" + zmienna2.ToString("F2") + "L/100KM";
            }
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            aGauge3.Value = trackBar3.Value;
        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            aGauge4.Value = trackBar4.Value;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            float zmienna = trackBar2.Value + trackBar1.Value;
            float zmienna2 = zmienna / 25;
            float zmienna_temp = trackBar3.Value;
            chart1.Series["Speed"].Points.AddXY(0, zmienna2);
            chart1.Series["Temp"].Points.AddXY(0, zmienna_temp);
            timer2.Start();
        }

        static SpeechRecognitionEngine _recognizer = null;
        SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer();
        void RecognizeSpeech()
        {
            _recognizer = new SpeechRecognitionEngine();
            _recognizer.LoadGrammar(new DictationGrammar());
            _recognizer.LoadGrammar(new Grammar(new GrammarBuilder("activate")));
            _recognizer.LoadGrammar(new Grammar(new GrammarBuilder("deactivate")));
            _recognizer.SpeechRecognized += _recognizeSpeech_SpeechRecognized;
            _recognizer.SpeechRecognitionRejected += _recognizeSpeech_SpeechRecognitionRejected;
            _recognizer.SetInputToDefaultAudioDevice();
            _recognizer.RecognizeAsync(RecognizeMode.Multiple);
        }
        void _recognizeSpeech_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Text == "activate")
            {
                this.listBox1.Items.Add(">SpeechRecognitionEngine: AUTOPILOT START!");
                speechSynthesizer.Speak("Initiating automatic driving mode");
            }
            else if (e.Result.Text == "deactivate")
            {
                this.listBox1.Items.Add(">SpeechRecognitionEngine: AUTOPILOT STOP!");
                speechSynthesizer.Speak("Deactivating automatic driving mode");
            }
            else
            {
                this.listBox1.Items.Add(">SpeechRecognitionEngine: " + e.Result.Text);
            }
        }
        void _recognizeSpeech_SpeechRecognitionRejected(object sender,
        SpeechRecognitionRejectedEventArgs e)
        {
            this.listBox1.Items.Add(">SpeechRecognitionEngine: Unrecognized command...");
        }
    }
}