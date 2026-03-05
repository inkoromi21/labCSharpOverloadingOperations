using System;

namespace MatrixCalculator
{
  public class Program
  {
    public static void Main(string[] arguments)
    {
      Console.WriteLine("MATRIX CALCULATOR");
      Console.WriteLine("=================");

      while (true)
      {
        try
        {
          DisplayMenu();

          string userChoice = Console.ReadLine();

          if (userChoice == "7")
          {
            return;
          }

          ProcessUserChoice(userChoice);
        }
        catch (MatrixException matrixError)
        {
          Console.WriteLine($"Matrix error: {matrixError.Message}");
        }
        catch (FormatException)
        {
          Console.WriteLine("Invalid number format");
        }
        catch (Exception generalError)
        {
          Console.WriteLine($"Error: {generalError.Message}");
        }

        Console.WriteLine();
      }
    }

    private static void DisplayMenu()
    {
      Console.WriteLine("1. Add matrices");
      Console.WriteLine("2. Multiply matrices");
      Console.WriteLine("3. Compare matrices (by determinant)");
      Console.WriteLine("4. Check matrix invertibility");
      Console.WriteLine("5. Find inverse matrix");
      Console.WriteLine("6. Test matrix operators");
      Console.WriteLine("7. Exit");
      Console.Write("Select option: ");
    }

    private static void ProcessUserChoice(string choice)
    {
      switch (choice)
      {
        case "1":
          TestAddition();
          break;

        case "2":
          TestMultiplication();
          break;

        case "3":
          TestComparison();
          break;

        case "4":
          TestInvertibility();
          break;

        case "5":
          TestInverse();
          break;

        case "6":
          TestOperators();
          break;

        default:
          Console.WriteLine("Invalid option");
          break;
      }
    }

    private static void TestAddition()
    {
      Console.WriteLine("\n--- MATRIX ADDITION ---");

      Console.WriteLine("Matrix A:");
      SquareMatrix firstMatrix = SquareMatrix.ReadFromConsole();

      Console.WriteLine("\nMatrix B:");
      SquareMatrix secondMatrix = SquareMatrix.ReadFromConsole();

      SquareMatrix resultMatrix = firstMatrix + secondMatrix;

      Console.WriteLine("\nA + B =");
      Console.WriteLine(resultMatrix.ToString());
    }

    private static void TestMultiplication()
    {
      Console.WriteLine("\n--- MATRIX MULTIPLICATION ---");

      Console.WriteLine("Matrix A:");
      SquareMatrix firstMatrix = SquareMatrix.ReadFromConsole();

      Console.WriteLine("\nMatrix B:");
      SquareMatrix secondMatrix = SquareMatrix.ReadFromConsole();

      SquareMatrix resultMatrix = firstMatrix * secondMatrix;

      Console.WriteLine("\nA * B =");
      Console.WriteLine(resultMatrix.ToString());
    }

    private static void TestComparison()
    {
      Console.WriteLine("\n--- MATRIX COMPARISON (by determinant) ---");

      Console.WriteLine("Matrix A:");
      SquareMatrix firstMatrix = SquareMatrix.ReadFromConsole();

      Console.WriteLine("\nMatrix B:");
      SquareMatrix secondMatrix = SquareMatrix.ReadFromConsole();

      double firstDeterminant = firstMatrix.CalculateDeterminant();
      double secondDeterminant = secondMatrix.CalculateDeterminant();

      Console.WriteLine($"\nDet(A) = {firstDeterminant:F2}");
      Console.WriteLine($"Det(B) = {secondDeterminant:F2}");

      Console.WriteLine($"A > B: {firstMatrix > secondMatrix}");
      Console.WriteLine($"A < B: {firstMatrix < secondMatrix}");
      Console.WriteLine($"A >= B: {firstMatrix >= secondMatrix}");
      Console.WriteLine($"A <= B: {firstMatrix <= secondMatrix}");
      Console.WriteLine($"A == B: {firstMatrix == secondMatrix}");
      Console.WriteLine($"A != B: {firstMatrix != secondMatrix}");
    }

    private static void TestInvertibility()
    {
      Console.WriteLine("\n--- MATRIX INVERTIBILITY TEST ---");

      SquareMatrix testMatrix = SquareMatrix.ReadFromConsole();

      double determinantValue = testMatrix.CalculateDeterminant();
      Console.WriteLine($"\nDeterminant = {determinantValue:F2}");

      if (testMatrix)
      {
        Console.WriteLine("Matrix is invertible (true operator)");
      }
      else
      {
        Console.WriteLine("Matrix is NOT invertible (false operator)");
      }
    }

    private static void TestInverse()
    {
      Console.WriteLine("\n--- MATRIX INVERSE ---");

      SquareMatrix originalMatrix = SquareMatrix.ReadFromConsole();

      Console.WriteLine("\nOriginal matrix:");
      Console.WriteLine(originalMatrix.ToString());

      double determinantValue = originalMatrix.CalculateDeterminant();
      Console.WriteLine($"Determinant = {determinantValue:F2}");

      SquareMatrix inverseMatrix = originalMatrix.CalculateInverse();

      Console.WriteLine("\nInverse matrix:");
      Console.WriteLine(inverseMatrix.ToString());

      SquareMatrix productMatrix = originalMatrix * inverseMatrix;
      Console.WriteLine("A * A^(-1) =");
      Console.WriteLine(productMatrix.ToString());
    }

    private static void TestOperators()
    {
      Console.WriteLine("\n--- OPERATOR TESTS ---");

      const int testMatrixSize = 2;

      double[,] testArray = new double[testMatrixSize, testMatrixSize]
      {
        { 1.0, 2.0 },
        { 3.0, 4.0 }
      };

      SquareMatrix firstMatrix = new SquareMatrix(testArray);
      SquareMatrix secondMatrix = firstMatrix.CreateDeepCopy();

      Console.WriteLine("Original matrix A:");
      Console.WriteLine(firstMatrix.ToString());

      Console.WriteLine("Clone matrix B (should be equal):");
      Console.WriteLine(secondMatrix.ToString());

      Console.WriteLine($"A == B: {firstMatrix == secondMatrix}");
      Console.WriteLine($"A != B: {firstMatrix != secondMatrix}");
      Console.WriteLine($"A.Equals(B): {firstMatrix.Equals(secondMatrix)}");

      int comparisonResult = firstMatrix.CompareTo(secondMatrix);
      Console.WriteLine($"CompareTo result: {comparisonResult}");

      Console.WriteLine("\nHash codes:");
      Console.WriteLine($"A hash: {firstMatrix.GetHashCode()}");
      Console.WriteLine($"B hash: {secondMatrix.GetHashCode()}");

      double[,] convertedArray = (double[,])firstMatrix;

      Console.WriteLine("\nExplicit cast to array:");

      int rowCount = convertedArray.GetLength(0);
      int columnCount = convertedArray.GetLength(1);

      for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
      {
        for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
        {
          Console.Write($"{convertedArray[rowIndex, columnIndex]:F2} ");
        }

        Console.WriteLine();
      }
    }
  }
}