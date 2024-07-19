using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace cellphone_project
{
    public partial class Form1 : Form
    {
        private System.Windows.Forms.Timer inputTimer;
        private string currentText = "";        
        private int currentIndex = -1;
        private int lastButton = -1;
        private bool isCharacterSaved = true;
        private Dictionary<string, string[]> wordDictionary;
        private string[] buttonMappings = new string[]
        {
            "0 ",
            "1",
            "2abc",
            "3def",
            "4ghi",
            "5jkl",
            "6mno",
            "7pqrs",
            "8tuv",
            "9wxyz"
        };
        public Form1()
        {
            InitializeComponent();

            InitializeButtons();

            InitializeTimer();

            LoadDictionary();

            richTextBox1.TextChanged += RichTextBox1_TextChanged;
        }

        private void RichTextBox1_TextChanged(object sender, EventArgs e)
        {
            UpdatePredictions();
        }

        private void LoadDictionary()
        {
            string json = File.ReadAllText("D:\\Study\\V2\\cellphone-project\\cellphone-project\\MyDictionary.json");
            wordDictionary = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(json);
        }

        private void InitializeButtons()
        {
            for (int i = 1; i <= 10; i++)
            {
                Button button = Controls.Find("button" + i, true).FirstOrDefault() as Button;
                if (button != null)
                {
                    button.Tag = i % 10;
                    button.Click += Button_Click;
                }
            }

            button11.Click += ButtonRight_Click;
            button12.Click += ButtonLeft_Click;
            button13.Click += ButtonDel_Click;
        }

        private void InitializeTimer()
        {
            inputTimer = new System.Windows.Forms.Timer();
            inputTimer.Interval = 1000;
            inputTimer.Tick += InputTimer_Tick;
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                int buttonNumber = int.Parse(button.Tag.ToString());

                if (lastButton == buttonNumber && !isCharacterSaved)
                {
                    currentIndex = (currentIndex + 1) % buttonMappings[buttonNumber].Length;
                }
                else
                {
                    lastButton = buttonNumber;
                    currentIndex = 0;
                }

                currentText = buttonMappings[buttonNumber][currentIndex].ToString();

                int cursorPosition = richTextBox1.SelectionStart;
                if (cursorPosition > 0 && !isCharacterSaved)
                {
                    richTextBox1.Text = richTextBox1.Text.Remove(cursorPosition - 1, 1);
                }

                if (cursorPosition < 0)
                {
                    cursorPosition = 0;
                }
                if (cursorPosition > richTextBox1.Text.Length)
                {
                    cursorPosition = richTextBox1.Text.Length;
                }

                richTextBox1.Text = richTextBox1.Text.Insert(cursorPosition, currentText);
                richTextBox1.SelectionStart = cursorPosition + 1;

                isCharacterSaved = false;

                inputTimer.Stop();
                inputTimer.Start();
            }
        }

        private void InputTimer_Tick(object sender, EventArgs e)
        {
            inputTimer.Stop();
            isCharacterSaved = true;
            lastButton = -1;
            currentIndex = -1;
        }

        private void ButtonDel_Click(object sender, EventArgs e)
        {
            int cursorPosition = richTextBox1.SelectionStart;
            if (cursorPosition > 0)
            {
                richTextBox1.Text = richTextBox1.Text.Remove(cursorPosition - 1, 1);
                richTextBox1.SelectionStart = cursorPosition - 1;
            }
        }

        private void ButtonLeft_Click(object sender, EventArgs e)
        {
            if (richTextBox1.SelectionStart > 0)
            {
                richTextBox1.SelectionStart--;
            }
        }

        private void ButtonRight_Click(object sender, EventArgs e)
        {
            if (richTextBox1.SelectionStart < richTextBox1.Text.Length)
            {
                richTextBox1.SelectionStart++;
            }
        }

        private void UpdatePredictions()
        {
            string input = GetCurrentWord().ToLower();
            List<string> predictions = new List<string>();

            foreach (var entry in wordDictionary)
            {
                if (entry.Key.StartsWith(input))
                {
                    predictions.AddRange(entry.Value);
                }
            }

            Console.WriteLine("Current word input: " + input);
            Console.WriteLine("Predictions: " + string.Join(", ", predictions));

            if (predictions.Count >= 1)
                button15.Text = predictions[0];
            else
                button15.Text = "";

            if (predictions.Count >= 2)
                button16.Text = predictions[1];
            else
                button16.Text = "";

            if (predictions.Count >= 3)
                button17.Text = predictions[2];
            else
                button17.Text = "";
        }

        private string GetCurrentWord()
        {
            int cursorPosition = richTextBox1.SelectionStart;
            int start = cursorPosition - 1;
            while (start >= 0 && !char.IsWhiteSpace(richTextBox1.Text[start]))
            {
                start--;
            }
            start++;

            int end = cursorPosition;
            while (end < richTextBox1.Text.Length && !char.IsWhiteSpace(richTextBox1.Text[end]))
            {
                end++;
            }

            return richTextBox1.Text.Substring(start, end - start);
        }

        private void AddPredictionToTextBox(string prediction)
        {
            int cursorPosition = richTextBox1.SelectionStart;
            int start = cursorPosition - 1;
            while (start >= 0 && !char.IsWhiteSpace(richTextBox1.Text[start]))
            {
                start--;
            }
            start++;

            int end = cursorPosition;
            while (end < richTextBox1.Text.Length && !char.IsWhiteSpace(richTextBox1.Text[end]))
            {
                end++;
            }

            richTextBox1.Text = richTextBox1.Text.Remove(start, end - start);
            richTextBox1.Text = richTextBox1.Text.Insert(start, prediction);
            richTextBox1.SelectionStart = start + prediction.Length;
        }

        private void button15_Click(object sender, EventArgs e)
        {
            AddPredictionToTextBox(button15.Text);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            AddPredictionToTextBox(button16.Text);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            AddPredictionToTextBox(button17.Text);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            string currentWord = GetCurrentWord().ToLower();

            string addedWord = "water";

            if (!wordDictionary.ContainsKey(currentWord))
            {
                wordDictionary[currentWord] = new string[] { addedWord };

                SaveDictionaryToJson();
            }
        }

        private void SaveDictionaryToJson()
        {
            string json = JsonConvert.SerializeObject(wordDictionary, Formatting.Indented);
            File.WriteAllText("D:\\Study\\V2\\cellphone-project\\cellphone-project\\MyDictionary.json", json);
        }
    }
}
