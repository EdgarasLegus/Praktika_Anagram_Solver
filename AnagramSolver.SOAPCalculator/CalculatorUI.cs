using CalculatorReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AnagramSolver.SOAPCalculator
{
    public class CalculatorUI
    {
        private readonly CalculatorSoapClient _calculator;
        private static readonly Action<string> _print = new Action<string>(WriteLineConsole);

        public CalculatorUI()
        {
            _calculator = new CalculatorSoapClient(CalculatorSoapClient.EndpointConfiguration.CalculatorSoap);
        }

        public int GetUserInput()
        {
            var inputAsString = Console.ReadLine();

            int input;
            while (!int.TryParse(inputAsString, out input))
            {
                _print("Not an integer. Try again.");
                inputAsString = Console.ReadLine();
            }

            return input;
        }

        public static void WriteLineConsole(string msg)
        {
            Console.WriteLine(msg);
        }

        public async Task Addition()
        {
            _print("-----------------------------" + "\n");
            _print("Selected Mode - Addition" + "\n");
            _print("Input first number" + "\n");
            var firstAddNumber = GetUserInput();
            _print("Input second number" + "\n");
            var secondAddNumber = GetUserInput();
            await _calculator.OpenAsync();
            var additionResult = await _calculator.AddAsync(firstAddNumber, secondAddNumber);
            await _calculator.CloseAsync();
            _print("Result" + "\n");
            _print(additionResult.ToString());
        }

        public async Task Substraction()
        {
            _print("-----------------------------" + "\n");
            _print("Selected Mode - Substraction" + "\n");
            _print("Input first number" + "\n");
            var firstSubNumber = GetUserInput();
            _print("Input second number" + "\n");
            var secondSubNumber = GetUserInput();
            var resultSub = await _calculator.SubtractAsync(firstSubNumber, secondSubNumber);
            _print("Result" + "\n");
            _print(resultSub.ToString());
        }

        public async Task Multiplication()
        {
            _print("-----------------------------" + "\n");
            _print("Selected Mode - Multiplication" + "\n");
            _print("Input first number" + "\n");
            var firstMulNumber = GetUserInput();
            _print("Input second number" + "\n");
            var secondMulNumber = GetUserInput();
            var resultMul = await _calculator.MultiplyAsync(firstMulNumber, secondMulNumber);
            _print("Result" + "\n");
            _print(resultMul.ToString());
        }

        public async Task Division()
        {
            _print("-----------------------------" + "\n");
            _print("Selected Mode - Division" + "\n");
            _print("Input first number" + "\n");
            var firstDivNumber = GetUserInput();
            _print("Input second number" + "\n");
            var secondDivNumber = GetUserInput();
            var resultDiv = await _calculator.DivideAsync(firstDivNumber, secondDivNumber);
            _print("Result" + "\n");
            _print(resultDiv.ToString());
        }
    }
}
