using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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


namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
     	class Calculator
	{
	private const int IN = 0;
	private const int OUT = 1;
	public Stack Val = new Stack();//Stack for storing values.
	Stack opt = new Stack();//Stack for storing operators.

	private bool IsDigit(char c)//Checking Number
	{
		return (c >= '0' && c <= '9');
	}
	private string Operat = "+-*/%=.)(";//Character used in program.
	private bool IsCharacter(char theChar)//Checking Character.
	{
		for (int i = 0; i < 9; i++)
		{
			if (theChar == Operat[i])
				return true;
		}
		return false;
	}
	public bool DelSpace(string theString)//For deleting spaces and checking numbers and characters.
	{
		string store = null;
		for (int i = 0; i < theString.Length; i++)
		{
			if (IsCharacter(theString[i]) || IsDigit(theString[i]))
			{
				store += theString[i];
			}
			else if (theString[i] == ' ') { }
			else
			{
				return false;
			}
		}
		theString = store;
		return true;
	}

	private int BODMAS(char c)// Applying BODMAS rule.
	{
		switch (c)
		{
			case '(': return 0;
			case '+':
			case '-': return 1;
			case '*':
			case '/': return 2;
			case '%': return 3;
			case ')': return 4;
			default: return -1;
		}
	}
	public bool changeIntoNumber(string start, out string end)//For Conversion into Number string.
	{
		end = "";
		int val = 0;
		int state = OUT;
		char c;
		for (int i = 0; i < start.Length; i++)
		{
			c = start[i];
			if (IsDigit(c))
			{
				end = end + c;
				val *= 10;
				val += c - '0';
				state = IN;
			}
			else
			{
				if (state == IN && c == '.')
				{
					end = end + '.';
					val = 0;
					continue;
				}
				if (state == IN && c != '.')
				{
					end += ' ';
					val = 0;
				}
				if (c == '=')
					break;
				else if (c == '(')
				{
					opt.Push(c);
				}
				else if (c == ')')
				{
					while (opt.Count > 0 && (char)opt.Peek() != '(')
					{
						end += opt.Peek();
						end += ' ';
						opt.Pop();
					}
					opt.Pop();
				}
				else
				{
					while (true)
					{
						if (opt.Count == 0)
						{
							opt.Push(c);
						}
						else if (BODMAS(c) > BODMAS((char)opt.Peek()))
						{
							opt.Push(c);
						}
						else
						{
							end += opt.Peek();
							end += ' ';
							opt.Pop();
							continue;
						}
						break;
					}
				}
				state = OUT;
			}
		}
		while (opt.Count > 0)
		{
			end += opt.Peek();
			end += ' ';
			opt.Pop();
		}
		return true;
	}
	public bool EvaluateEquation(string equation)//For Calculation.
	{
		int value = 0;
		int state = OUT;
		char c;
		bool dot = false;
		double count = 1.0;
		for (int i = 0; i < equation.Length; i++)
		{
			c = equation[i];
			if (IsDigit(c) || c == '.')
			{
				if (IsDigit(c))
				{
					value *= 10;
					value += c - '0';
					state = IN;
					if (dot == true)
						count *= 10.0;
				}
				if (c == '.')
				{
					dot = true;
					continue;
				}				
			}
			else
			{
				dot = false;
				double ans = value / count;
				count = 1.0;
				if (state == IN)
				{
					Val.Push(ans);
					value = 0;
				}
				
				if (c != ' ')
				{
					var val1 = (double)Val.Peek(); Val.Pop();
					var val2 = (double)Val.Peek(); Val.Pop();
					switch (c)
					{
						case '+': Val.Push(val1 + val2); break;
						case '-': Val.Push(val2 - val1); break;
						case '*': Val.Push(val1 * val2); break;
						case '/': Val.Push(val2 / val1); break;
						case '%': Val.Push((long)val2 % (long)val1); break;
					}
				}
				state = OUT;
			}
		}
		if (equation.Length<2)
			Val.Push(value);
		return true;
	}
	
}

public partial class MainWindow : Window
{
		string equation;//initizing string to store the equation.
		Calculator calc = new Calculator();//Inilizing Calculator Class.
		public MainWindow()
        {
            InitializeComponent();
        }
        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            equation = textBox.Text + button.Content;
			textBox.Text = equation;
		}
        private void ClearClick(object sender, RoutedEventArgs e)
        {
            textBox.Clear();
        }
		private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			
        }
        private void EqualClick(object sender, RoutedEventArgs e)
        {
			calc.DelSpace(equation);
			string endString;// New string to store the infinix value.
			calc.changeIntoNumber(equation, out endString);//Converts into numbers.
			calc.EvaluateEquation(endString);//Calculate the Answer and store in Stack Val.
			var answer = calc.Val.Peek();
			textBox.Text = equation + "=" + answer;
			
		}
        private void ListClick(object sender, RoutedEventArgs e)
        {
			textBox.Clear();
			foreach (var s in calc.Val)
			{
				textBox.Text = textBox.Text + "-> " + s + "\n";
			}
		}
        private void UndoButton(object sender, RoutedEventArgs e)
        {
			textBox.Undo();
        }
        private void BackSpace(object sender, RoutedEventArgs e)
        {
			if (textBox.Text.Length>0)
            {
				textBox.Text = textBox.Text.Remove(textBox.Text.Length - 1, 1);
            }
        }
    }
    
}
