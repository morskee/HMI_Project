using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Recognition; //w odwołaniach dodane zostało System.Speech to poprawnego działania
using System.Speech.Synthesis;using System.Threading;


namespace HMI_project
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            RecognizeSpeech();

            label12.Text = DateTime.Now.ToLongTimeString();

            label13.Text = DateTime.Now.ToLongDateString();

            label5.Text = "123456KM";

        }

        private void trackBar1_Scroll(object sender, EventArgs e) //Scroll czyli co bedzie sie dziac jesli sie bedzie ruszalo trackBarem ;)
        {
            aGauge1.Value = trackBar1.Value; //wartosc zegara aGauge1 jest równa wartosci z trackbara po to aby trackbar zmienial wartosc na zegarze

            label11.Text = "";
            if (trackBar2.Value > 0 && trackBar1.Value > 0) //petla if jesli spelnione sa dwa waurnki 
            {
                float zmienna = trackBar2.Value + trackBar1.Value; //zmienna - dodanie wartosci z obrotomierza i predkosciomierza
                float zmienna2 = zmienna / 25; //zmienna2 - to wartosc spalania na L/100KM przez 25 bo tak dobrze liczby wychodzily 

                label11.Text = "" + zmienna2.ToString("F2") + "L/100KM"; //wpisanie do labela wartosci "zmienna2"; ToString("F2") oznacza pokazywanie się 2 miejsc po przecinku w "zmienna2"
            }
        }

        private void timer1_Tick(object sender, EventArgs e) //timer1_Tick co ma się dziać za każdym ticknięciem timera 
        {
            label12.Text = DateTime.Now.ToLongTimeString(); //pobranie godziny z systemu
            
            label13.Text = DateTime.Now.ToLongDateString(); //pobranie daty z systemu

            timer1.Start(); //wlaczenie timera
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
            chart1.Series["Speed"].Points.AddXY(0, zmienna2); //seria Speed na wykresie X to 0 ale zmienia się to za kazdym tickinieciem timera2
            chart1.Series["Temp"].Points.AddXY(0, zmienna_temp); //seria Temp również posiada X równy 0 
            timer2.Start();
        }
        //kod pobrany z Lab3_Instrukcja.pdf
        //kazde kolejne komendy dodane zostaly metoda kopiuj/wklej
        static SpeechRecognitionEngine _recognizer = null;
        SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer();
        void RecognizeSpeech()
        {
            _recognizer = new SpeechRecognitionEngine();
            _recognizer.LoadGrammar(new DictationGrammar());
            _recognizer.LoadGrammar(new Grammar(new GrammarBuilder("activate autopilot")));
            _recognizer.LoadGrammar(new Grammar(new GrammarBuilder("deactivate autopilot")));
            _recognizer.LoadGrammar(new Grammar(new GrammarBuilder("start engine")));
            _recognizer.LoadGrammar(new Grammar(new GrammarBuilder("stop engine")));
            _recognizer.LoadGrammar(new Grammar(new GrammarBuilder("lights on")));
            _recognizer.LoadGrammar(new Grammar(new GrammarBuilder("lights off")));
            _recognizer.SpeechRecognized += _recognizeSpeech_SpeechRecognized;
            _recognizer.SpeechRecognitionRejected += _recognizeSpeech_SpeechRecognitionRejected;
            _recognizer.SetInputToDefaultAudioDevice();
            _recognizer.RecognizeAsync(RecognizeMode.Multiple);
        }
        void _recognizeSpeech_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Text == "activate autopilot")
            {
                this.listBox1.Items.Add(">SpeechRecognitionEngine: AUTOPILOT START!");
                speechSynthesizer.Speak("Initiating automatic driving mode");
            }
            else if (e.Result.Text == "deactivate autopilot")
            {
                this.listBox1.Items.Add(">SpeechRecognitionEngine: AUTOPILOT STOP!");
                speechSynthesizer.Speak("Deactivating automatic driving mode");
            }
            else if (e.Result.Text == "start engine")
            {
                this.listBox1.Items.Add(">SpeechRecognitionEngine: ENGINE START!");
                speechSynthesizer.Speak("Engine start.");
            }
            else if (e.Result.Text == "stop engine")
            {
                this.listBox1.Items.Add(">SpeechRecognitionEngine: ENGINE STOP!");
                speechSynthesizer.Speak("Engine stop.");
            }
            else if (e.Result.Text == "lights on")
            {
                this.listBox1.Items.Add(">SpeechRecognitionEngine: LIGHTS ON!");
                speechSynthesizer.Speak("Turn on the lights.");
            }
            else if (e.Result.Text == "lights off")
            {
                this.listBox1.Items.Add(">SpeechRecognitionEngine: LIGHTS OFF!");
                speechSynthesizer.Speak("Turn off the lights.");
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
        
        private void checkBox1_CheckedChanged(object sender, EventArgs e) 
        {
            if (checkBox1.Checked) //jesli checkBox1 jest zaznaczony
            {
                this.pictureBox16.Load("temp.jpg"); //jesli zaznaczony laduje obrazek ten
            }
            else
                this.pictureBox16.Load("nic.jpg"); //jesli nie zaznaczony to ten

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                this.pictureBox7.Load("ster.jpg");
            }
            else
                this.pictureBox7.Load("nic.jpg");
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                this.pictureBox1.Load("ABS.jpg");
            }
            else
                this.pictureBox1.Load("nic.jpg");
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
            {
                this.pictureBox17.Load("nic.jpg");
            }
            else
                this.pictureBox17.Load("esp.jpg");
        }
    }
}