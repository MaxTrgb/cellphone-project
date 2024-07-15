namespace cellphone_project
{
    public partial class Form1 : Form
    {
        private System.Windows.Forms.Timer inputTimer;
        private string currentText = "";
        private string[] buttonMappings = new string[]
        {
            "0 ",
            "1",
            "2ABC",
            "3DEF",
            "4GHI",
            "5JKL",
            "6MNO",
            "7PQRS",
            "8TUV",
            "9WXYZ"            
        };
        private int currentIndex = -1;
        private int lastButton = -1;
        private bool isCharacterSaved = true;

        public Form1()
        {
            InitializeComponent();

            button1.Tag = 1;
            button2.Tag = 2;
            button3.Tag = 3;
            button4.Tag = 4;
            button5.Tag = 5;
            button6.Tag = 6;
            button7.Tag = 7;
            button8.Tag = 8;
            button9.Tag = 9;
            button10.Tag = 0;

            button1.Click += Button_Click;
            button2.Click += Button_Click;
            button3.Click += Button_Click;
            button4.Click += Button_Click;
            button5.Click += Button_Click;
            button6.Click += Button_Click;
            button7.Click += Button_Click;
            button8.Click += Button_Click;
            button9.Click += Button_Click;
            button10.Click += Button_Click;
            button11.Click += ButtonRight_Click;
            button12.Click += ButtonLeft_Click;           
            button13.Click += ButtonDel_Click;
            button14.Click += ButtonAdd_Click;

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

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
        }
    }
}