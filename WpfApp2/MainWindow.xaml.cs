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
     	class calculator
	{
	private const int IN = 0;
	private const int OUT = 1;
	public Stack val = new Stack();
	Stack opt = new Stack();

	private bool isDigit(char c)
	{
		return (c >= '0' && c <= '9');
	}
	private string operat = "+-*/%=.)(";
	private bool in_set(char theChar)
	{
		for (int i = 0; i < 9; i++)
		{
			if (theChar == operat[i])
				return true;
		}
		return false;
	}
	public bool del_space(string theString)
	{
		string store = null;
		for (int i = 0; i < theString.Length; i++)
		{
			if (in_set(theString[i]) || isDigit(theString[i]) || theString == "exit" || theString == "list" || theString == "del")
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

	private int BODMAS(char c)
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
	public bool change(string start, out string end)
	{
		end = "";
		int val = 0;
		int state = OUT;
		char c;
		for (int i = 0; i < start.Length; i++)
		{
			c = start[i];
			if (isDigit(c))
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
	public bool Evaluate(string Equa)
	{
		int value = 0;
		int state = OUT;
		char c;
		bool dot = false;
		double count = 1.0;
		for (int i = 0; i < Equa.Length; i++)
		{
			c = Equa[i];
			if (isDigit(c) || c == '.')
			{
				if (isDigit(c))
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
					val.Push(ans);
					value = 0;
				}
				
				if (c != ' ')
				{

					var val1 = (double)val.Peek(); val.Pop();
					var val2 = (double)val.Peek(); val.Pop();

					switch (c)
					{
						case '+': val.Push(val1 + val2); break;
						case '-': val.Push(val2 - val1); break;
						case '*': val.Push(val1 * val2); break;
						case '/': val.Push(val2 / val1); break;
						case '%': val.Push((long)val2 % (long)val1); break;
					}


				}
				state = OUT;
			}
		}
		return true;
	}
	
	}

public partial class MainWindow : Window
    {
		string equation;
	 Stack List = new Stack();
		public MainWindow()
        {
            InitializeComponent();
        }

        private void button_click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            equation = text_box.Text + button.Content;
			text_box.Text = equation;
		
		}

        private void clear_Click(object sender, RoutedEventArgs e)
        {
            text_box.Clear();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
			
        }

        private void equal_click(object sender, RoutedEventArgs e)
        {
			calculator calc = new calculator();
			//text_box.Clear();
            
            string end;
			calc.change(equation, out end);
			calc.Evaluate(end);
			var answer = calc.val.Peek();
			
			text_box.Text = "" + answer;
		}

        private void list_Click(object sender, RoutedEventArgs e)
        {
			
		
		}

        private void undo_button(object sender, RoutedEventArgs e)
        {
			text_box.Undo();
			
        }
    }
    
}
