using System;

namespace MatrixCalculator
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("MATRIX CALCULATOR");
      Console.WriteLine("=================");

      while (true)
      {
        try
        {
          Console.WriteLine("\n1. Add matrices");
          Console.WriteLine("2. Multiply matrices");
          Console.WriteLine("3. Compare matrices (determinant)");
          Console.WriteLine("4. Check matrix invertibility");
          Console.WriteLine("5. Find inverse matrix");
          Console.WriteLine("6. Test matrix operators");
          Console.WriteLine("7. Exit");
          Console.Write("Select option: ");

          string choice = Console.ReadLine();

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
            case "7":
              return;
            default:
              Console.WriteLine("Invalid option");
              break;
          }
        }
        catch (MatrixException ex)
        {
          Console.WriteLine($"Matrix error: {ex.Message}");
        }
        catch (FormatException)
        {
          Console.WriteLine("Invalid number format");
        }
        catch (Exception ex)
        {
          Console.WriteLine($"Error: {ex.Message}");
        }
      }
    }

    static void TestAddition()
    {
      Console.WriteLine("\n--- MATRIX ADDITION ---");
      Console.WriteLine("Matrix A:");
      SquareMatrix a = SquareMatrix.ReadFromConsole();

      Console.WriteLine("\nMatrix B:");
      SquareMatrix b = SquareMatrix.ReadFromConsole();

      SquareMatrix c = a + b;

      Console.WriteLine("\nA + B =");
      Console.WriteLine(c.ToString());
    }

    static void TestMultiplication()
    {
      Console.WriteLine("\n--- MATRIX MULTIPLICATION ---");
      Console.WriteLine("Matrix A:");
      SquareMatrix a = SquareMatrix.ReadFromConsole();

      Console.WriteLine("\nMatrix B:");
      SquareMatrix b = SquareMatrix.ReadFromConsole();

      SquareMatrix c = a * b;

      Console.WriteLine("\nA * B =");
      Console.WriteLine(c.ToString());
    }

    static void TestComparison()
    {
      Console.WriteLine("\n--- MATRIX COMPARISON (by determinant) ---");
      Console.WriteLine("Matrix A:");
      SquareMatrix a = SquareMatrix.ReadFromConsole();

      Console.WriteLine("\nMatrix B:");
      SquareMatrix b = SquareMatrix.ReadFromConsole();

      Console.WriteLine($"\nDet(A) = {a.Determinant():F2}");
      Console.WriteLine($"Det(B) = {b.Determinant():F2}");

      Console.WriteLine($"A > B: {a > b}");
      Console.WriteLine($"A < B: {a < b}");
      Console.WriteLine($"A >= B: {a >= b}");
      Console.WriteLine($"A <= B: {a <= b}");
      Console.WriteLine($"A == B: {a == b}");
      Console.WriteLine($"A != B: {a != b}");
    }

    static void TestInvertibility()
    {
      Console.WriteLine("\n--- MATRIX INVERTIBILITY TEST ---");
      SquareMatrix a = SquareMatrix.ReadFromConsole();

      double det = a.Determinant();
      Console.WriteLine($"\nDeterminant = {det:F2}");

      if (a)
      {
        Console.WriteLine("Matrix is invertible (true operator)");
      }
      else
      {
        Console.WriteLine("Matrix is NOT invertible (false operator)");
      }
    }

    static void TestInverse()
    {
      Console.WriteLine("\n--- MATRIX INVERSE ---");
      SquareMatrix a = SquareMatrix.ReadFromConsole();

      Console.WriteLine("\nOriginal matrix:");
      Console.WriteLine(a.ToString());

      Console.WriteLine($"Determinant = {a.Determinant():F2}");

      SquareMatrix inv = a.Inverse();
      Console.WriteLine("\nInverse matrix:");
      Console.WriteLine(inv.ToString());

      SquareMatrix product = a * inv;
      Console.WriteLine("A * A^(-1) =");
      Console.WriteLine(product.ToString());
    }

    static void TestOperators()
    {
      Console.WriteLine("\n--- OPERATOR TESTS ---");
      SquareMatrix a = new SquareMatrix(2, true);
      SquareMatrix b = a.Clone();

      Console.WriteLine("Original matrix A:");
      Console.WriteLine(a.ToString());

      Console.WriteLine("Clone matrix B (should be equal):");
      Console.WriteLine(b.ToString());

      Console.WriteLine($"A == B: {a == b}");
      Console.WriteLine($"A != B: {a != b}");
      Console.WriteLine($"A.Equals(B): {a.Equals(b)}");
      Console.WriteLine($"CompareTo result: {a.CompareTo(b)}");

      Console.WriteLine("\nHash codes:");
      Console.WriteLine($"A hash: {a.GetHashCode()}");
      Console.WriteLine($"B hash: {b.GetHashCode()}");

      double[,] array = (double[,])a;
      Console.WriteLine("\nExplicit cast to array:");
      for (int i = 0; i < array.GetLength(0); i++)
      {
        for (int j = 0; j < array.GetLength(1); j++)
        {
          Console.Write($"{array[i, j]:F2} ");
        }
        Console.WriteLine();
      }
    }
  }
}