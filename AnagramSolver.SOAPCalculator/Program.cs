using CalculatorReference;
using System;
using System.Threading.Tasks;

namespace AnagramSolver.SOAPCalculator
{
    class Program
    {
        private static readonly Action<string> _print = new Action<string>(CalculatorUI.WriteLineConsole);
        private static readonly CalculatorUI _calculatorUI = new CalculatorUI();

        static async Task Main(string[] args)
        {
            int input;
            string retry = "No";

            do
            {
                _print("Calculator is ready. Choose your option: ");
                _print("---Select Mode: " + "\n" + "0 - Addition" + "\n" + "1 - Substraction"
                    + "\n" + "2 - Multiplication" + "\n" + "3 - Division");
                input = _calculatorUI.GetUserInput();
                switch (input)
                {
                    case 0:
                        await _calculatorUI.Addition();
                        _print("Action is finished or there are no such mode available. Do you want to select another mode? (Press any key/No)");
                        retry = Console.ReadLine();
                        break;

                    case 1:
                        await _calculatorUI.Substraction();
                        _print("Action is finished or there are no such mode available. Do you want to select another mode? (Press any key/No)");
                        retry = Console.ReadLine();
                        break;

                    case 2:
                        await _calculatorUI.Multiplication();
                        _print("Action is finished or there are no such mode available. Do you want to select another mode? (Press any key/No)");
                        retry = Console.ReadLine();
                        break;

                    case 3:
                        await _calculatorUI.Division();
                        _print("Action is finished or there are no such mode available. Do you want to select another mode? (Press any key/No)");
                        retry = Console.ReadLine();
                        break;

                    default:
                        Console.WriteLine("Action is finished or there are no such mode available. Do you want to select another mode? (Press any key/No)");
                        retry = Console.ReadLine();
                        break;
                }
            }
            while (retry != "No");

        }
    }
}
