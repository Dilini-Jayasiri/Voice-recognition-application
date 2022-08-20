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
using System.Speech.Synthesis;
using System.IO;

namespace Speech_Recognition_application
{
    public partial class Form1 : Form
    {
        SpeechRecognitionEngine _recognizer = new SpeechRecognitionEngine();
        SpeechSynthesizer Dilini = new SpeechSynthesizer();
        SpeechRecognitionEngine startlistening = new SpeechRecognitionEngine();
        Random rnd = new Random();
        int RecOutTime = 0;
        DateTime timeNow = DateTime.Now;
        public Form1()
        {
            InitializeComponent();
        }

        private void lstCommands_SelectedIndexChanged(object sender, EventArgs e)
        {
            _recognizer.SetInputToDefaultAudioDevice();
            _recognizer.LoadGrammarAsync(new Grammar(new GrammarBuilder(new Choices(File.ReadAllLines(@"DefaultCommands.txt")))));
            _recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(Default_SpeechRecognized);
            _recognizer.SpeechDetected += new EventHandler<SpeechDetectedEventArgs>(_recognizer_SpeechRecognized);
            _recognizer.RecognizeAsync(RecognizeMode.Multiple);

            startlistening.SetInputToDefaultAudioDevice();
            startlistening.LoadGrammarAsync(new Grammar(new GrammarBuilder(new Choices(File.ReadAllLines(@"DefaultCommands.txt")))));
            startlistening.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(startlistening_SpeechRecognized);
        }

        private void Default_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            int randomNum;
            string speech = e.Result.ToString();

            if (speech == "Hello")
            {
                Dilini.SpeakAsync("Hello I am here");
            }
            if (speech == "How are you")
            {
                Dilini.SpeakAsync(" I am Working normally");
            }
            if (speech == "What is time now")
            {
                Dilini.SpeakAsync(DateTime.Now.ToString("h mm tt"));
            }
            if(speech=="Stop talking")
            {
                Dilini.SpeakAsyncCancelAll();
                randomNum = rnd.Next(1, 2);

                if(randomNum == 1)
                {
                    Dilini.SpeakAsync("Yes madam");
                }
                if(randomNum == 2)
                {
                    Dilini.SpeakAsync("Sorry madam i will quiet");
                }
            }

            if(speech=="Stop listning")
            {
                Dilini.SpeakAsync("If you need just ask");
                _recognizer.RecognizeAsyncCancel();
                startlistening.RecognizeAsync(RecognizeMode.Multiple);
            }

            if(speech=="Show commands")
            {
                string[] commands = (File.ReadAllLines(@"DefaultCommands.txt"));
                lstCommands.Items.Clear();
                lstCommands.SelectionMode = SelectionMode.None;
                lstCommands.Visible = true;

                foreach(string command in commands)
                {
                    lstCommands.Items.Add(command);
                }

            }
            if(speech=="Hide commands")
            {
                lstCommands.Visible=false;
            }
        }

        private void _recognizer_SpeechRecognized(object sender, SpeechDetectedEventArgs e)
        {
            RecOutTime = 0;
        }
        private void startlistening_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string speech = e.Result.ToString();

            if(speech == "Wake up")
            {
                startlistening.RecognizeAsyncCancel();
                Dilini.SpeakAsync("Yes i am here");
                _recognizer.RecognizeAsync(RecognizeMode.Multiple);
            }
        }

        private void TmrSpeaking_Tick(object sender, EventArgs e)
        {
            if (RecOutTime == 10)
            {
                _recognizer.RecognizeAsyncCancel();
            }
            if (RecOutTime == 11)
            {
                TmrSpeaking.Stop();
                startlistening.RecognizeAsync(RecognizeMode.Multiple);
                RecOutTime = 0;
            }
        }
    }
}
