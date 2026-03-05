using System;
using System.Text;

namespace MatrixCalculator
{
  public class SquareMatrix : IComparable<SquareMatrix>, IEquatable<SquareMatrix>
  {
    private double[,] _matrixData;

    public int Size { get; private set; }

    public SquareMatrix(int matrixSize)
    {
      if (matrixSize <= 0)
      {
        throw new InvalidMatrixSizeException("Matrix size must be positive");
      }

      Size = matrixSize;
      _matrixData = new double[matrixSize, matrixSize];
    }

    public SquareMatrix(double[,] sourceArray)
    {
      int columnDimension;
      columnDimension = 1;

      if (sourceArray == null)
      {
        throw new MatrixNullException("Matrix cannot be null");
      }

      if (sourceArray.GetLength(0) != sourceArray.GetLength(columnDimension))
      {
        throw new NonSquareMatrixException("Matrix must be square");
      }

      Size = sourceArray.GetLength(0);
      _matrixData = new double[Size, Size];

      for (int rowIndex = 0; rowIndex < Size; ++rowIndex)
      {
        for (int columnIndex = 0; columnIndex < Size; ++columnIndex)
        {
          _matrixData[rowIndex, columnIndex] = sourceArray[rowIndex, columnIndex];
        }
      }
    }

    public double this[int rowIndex, int columnIndex]
    {
      get
      {
        if (rowIndex < 0 || rowIndex >= Size || columnIndex < 0 || columnIndex >= Size)
        {
          throw new IndexOutOfRangeException("Index out of matrix bounds");
        }

        return _matrixData[rowIndex, columnIndex];
      }

      set
      {
        if (rowIndex < 0 || rowIndex >= Size || columnIndex < 0 || columnIndex >= Size)
        {
          throw new IndexOutOfRangeException("Index out of matrix bounds");
        }

        _matrixData[rowIndex, columnIndex] = value;
      }
    }

    public static SquareMatrix operator +(SquareMatrix firstMatrix, SquareMatrix secondMatrix)
    {
      if (firstMatrix == null || secondMatrix == null)
      {
        throw new MatrixNullException("Matrices cannot be null");
      }

      if (firstMatrix.Size != secondMatrix.Size)
      {
        throw new MatrixDimensionMismatchException("Matrices must have same size for addition");
      }

      SquareMatrix resultMatrix = new SquareMatrix(firstMatrix.Size);

      for (int rowIndex = 0; rowIndex < firstMatrix.Size; ++rowIndex)
      {
        for (int columnIndex = 0; columnIndex < firstMatrix.Size; ++columnIndex)
        {
          resultMatrix[rowIndex, columnIndex] = firstMatrix[rowIndex, columnIndex] + secondMatrix[rowIndex, columnIndex];
        }
      }

      return resultMatrix;
    }

    public static SquareMatrix operator *(SquareMatrix firstMatrix, SquareMatrix secondMatrix)
    {
      double sumValue;

      if (firstMatrix == null || secondMatrix == null)
      {
        throw new MatrixNullException("Matrices cannot be null");
      }

      if (firstMatrix.Size != secondMatrix.Size)
      {
        throw new MatrixDimensionMismatchException("Matrices must have same size for multiplication");
      }

      SquareMatrix resultMatrix = new SquareMatrix(firstMatrix.Size);

      for (int rowIndex = 0; rowIndex < firstMatrix.Size; ++rowIndex)
      {
        for (int columnIndex = 0; columnIndex < firstMatrix.Size; ++columnIndex)
        {
          sumValue = 0;

          for (int innerIndex = 0; innerIndex < firstMatrix.Size; ++innerIndex)
          {
            sumValue += firstMatrix[rowIndex, innerIndex] * secondMatrix[innerIndex, columnIndex];
          }

          resultMatrix[rowIndex, columnIndex] = sumValue;
        }
      }

      return resultMatrix;
    }

    public static bool operator >(SquareMatrix firstMatrix, SquareMatrix secondMatrix)
    {
      if (firstMatrix == null || secondMatrix == null)
      {
        throw new MatrixNullException("Matrices cannot be null");
      }

      return firstMatrix.CalculateDeterminant() > secondMatrix.CalculateDeterminant();
    }

    public static bool operator <(SquareMatrix firstMatrix, SquareMatrix secondMatrix)
    {
      if (firstMatrix == null || secondMatrix == null)
      {
        throw new MatrixNullException("Matrices cannot be null");
      }

      return firstMatrix.CalculateDeterminant() < secondMatrix.CalculateDeterminant();
    }

    public static bool operator >=(SquareMatrix firstMatrix, SquareMatrix secondMatrix)
    {
      if (firstMatrix == null || secondMatrix == null)
      {
        throw new MatrixNullException("Matrices cannot be null");
      }

      return firstMatrix.CalculateDeterminant() >= secondMatrix.CalculateDeterminant();
    }

    public static bool operator <=(SquareMatrix firstMatrix, SquareMatrix secondMatrix)
    {
      if (firstMatrix == null || secondMatrix == null)
      {
        throw new MatrixNullException("Matrices cannot be null");
      }

      return firstMatrix.CalculateDeterminant() <= secondMatrix.CalculateDeterminant();
    }

    public static bool operator ==(SquareMatrix firstMatrix, SquareMatrix secondMatrix)
    {
      if (ReferenceEquals(firstMatrix, null) && ReferenceEquals(secondMatrix, null))
      {
        return true;
      }

      if (ReferenceEquals(firstMatrix, null) || ReferenceEquals(secondMatrix, null))
      {
        return false;
      }

      return firstMatrix.Equals(secondMatrix);
    }

    public static bool operator !=(SquareMatrix firstMatrix, SquareMatrix secondMatrix)
    {
      return !(firstMatrix == secondMatrix);
    }

    public static explicit operator double[,](SquareMatrix sourceMatrix)
    {
      double[,] resultArray;

      if (sourceMatrix == null)
      {
        throw new MatrixNullException("Matrix cannot be null");
      }

      resultArray = new double[sourceMatrix.Size, sourceMatrix.Size];

      for (int rowIndex = 0; rowIndex < sourceMatrix.Size; ++rowIndex)
      {
        for (int columnIndex = 0; columnIndex < sourceMatrix.Size; ++columnIndex)
        {
          resultArray[rowIndex, columnIndex] = sourceMatrix[rowIndex, columnIndex];
        }
      }

      return resultArray;
    }

    public static implicit operator SquareMatrix(double[,] sourceArray)
    {
      return new SquareMatrix(sourceArray);
    }

    public static bool operator true(SquareMatrix sourceMatrix)
    {
      if (sourceMatrix == null)
      {
        return false;
      }

      const double epsilon = 1e-10;
      return Math.Abs(sourceMatrix.CalculateDeterminant()) > epsilon;
    }

    public static bool operator false(SquareMatrix sourceMatrix)
    {
      if (sourceMatrix == null)
      {
        return true;
      }

      const double epsilon = 1e-10;
      return Math.Abs(sourceMatrix.CalculateDeterminant()) <= epsilon;
    }

    public double CalculateDeterminant()
    {
      int pairMatrixSize;
      int scalarMatrixSize;
      int secondRow;
      int secondColumn;

      secondRow = 1;
      pairMatrixSize = 2;
      secondColumn = 1;
      scalarMatrixSize = 1;


      if (Size == scalarMatrixSize)
      {
        return _matrixData[0, 0];
      }

      if (Size == pairMatrixSize)
      {
        return _matrixData[0, 0] * _matrixData[secondRow, secondColumn] - _matrixData[0, secondColumn] * _matrixData[secondRow, 0];
      }

      double determinantValue;
      determinantValue = 0;

      for (int columnIndex = 0; columnIndex < Size; ++columnIndex)
      {
        determinantValue += _matrixData[0, columnIndex] * CalculateCofactor(0, columnIndex);
      }

      return determinantValue;
    }

    private double CalculateCofactor(int rowIndex, int columnIndex)
    {
      double positiveSign;
      double negativeSign;
      int parityCheckDivisor;
      int indexSum;

      positiveSign = 1.0;
      negativeSign = -1.0;
      parityCheckDivisor = 2;

      double sign;
      indexSum = rowIndex + columnIndex;

      if (indexSum % parityCheckDivisor == 0)
      {
        sign = positiveSign;
      }
      else
      {
        sign = negativeSign;
      }

      return CalculateMinor(rowIndex, columnIndex) * sign;
    }

    private double CalculateMinor(int excludedRow, int excludedColumn)
    {
      int minorSizeDecrement;
      minorSizeDecrement = 1;

      SquareMatrix minorMatrix = new SquareMatrix(Size - minorSizeDecrement);
      int targetRow;
      int targetColumn;

      targetColumn = 0;
      targetRow = 0;

      for (int sourceRow = 0; sourceRow < Size; ++sourceRow)
      {
        if (sourceRow == excludedRow)
        {
          continue;
        }

        targetColumn = 0;

        for (int sourceColumn = 0; sourceColumn < Size; ++sourceColumn)
        {
          if (sourceColumn == excludedColumn)
          {
            continue;
          }

          minorMatrix[targetRow, targetColumn] = _matrixData[sourceRow, sourceColumn];
          ++targetColumn;
        }

        ++targetRow;
      }

      return minorMatrix.CalculateDeterminant();
    }

    public SquareMatrix CalculateInverse()
    {
      double determinantValue;
      const double epsilon = 1e-10;

      determinantValue = CalculateDeterminant();

      if (Math.Abs(determinantValue) < epsilon)
      {
        throw new SingularMatrixException("Matrix is singular, cannot find inverse");
      }

      SquareMatrix inverseMatrix = new SquareMatrix(Size);

      for (int rowIndex = 0; rowIndex < Size; ++rowIndex)
      {
        for (int columnIndex = 0; columnIndex < Size; ++columnIndex)
        {
          inverseMatrix[columnIndex, rowIndex] = CalculateCofactor(rowIndex, columnIndex) / determinantValue;
        }
      }

      return inverseMatrix;
    }

    public override string ToString()
    {
      StringBuilder resultBuilder = new StringBuilder();
      string numberFormat;
      int paddingSize;
      string formattedNumber;

      paddingSize = 8;
      numberFormat = "F2";

      for (int rowIndex = 0; rowIndex < Size; ++rowIndex)
      {
        for (int columnIndex = 0; columnIndex < Size; ++columnIndex)
        {
          formattedNumber = _matrixData[rowIndex, columnIndex].ToString(numberFormat);
          resultBuilder.Append(formattedNumber.PadLeft(paddingSize));
        }

        resultBuilder.AppendLine();
      }

      return resultBuilder.ToString();
    }

    public int CompareTo(SquareMatrix otherMatrix)
    {
      if (otherMatrix == null)
      {
        return 1;
      }

      return CalculateDeterminant().CompareTo(otherMatrix.CalculateDeterminant());
    }

    public override bool Equals(object comparedObject)
    {
      return Equals(comparedObject as SquareMatrix);
    }

    public bool Equals(SquareMatrix otherMatrix)
    {
      if (otherMatrix == null)
      {
        return false;
      }

      if (Size != otherMatrix.Size)
      {
        return false;
      }

      const double epsilon = 1e-10;

      for (int rowIndex = 0; rowIndex < Size; ++rowIndex)
      {
        for (int columnIndex = 0; columnIndex < Size; ++columnIndex)
        {
          if (Math.Abs(_matrixData[rowIndex, columnIndex] - otherMatrix[rowIndex, columnIndex]) > epsilon)
          {
            return false;
          }
        }
      }

      return true;
    }

    public override int GetHashCode()
    {
      int initialHash;
      int hashMultiplier;

      initialHash = 17;
      hashMultiplier = 31;

      int hashCode;
      hashCode = initialHash;

      foreach (double elementValue in _matrixData)
      {
        hashCode = hashCode * hashMultiplier + elementValue.GetHashCode();
      }

      return hashCode;
    }

    public SquareMatrix CreateDeepCopy()
    {
      SquareMatrix clonedMatrix = new SquareMatrix(Size);

      for (int rowIndex = 0; rowIndex < Size; ++rowIndex)
      {
        for (int columnIndex = 0; columnIndex < Size; ++columnIndex)
        {
          clonedMatrix[rowIndex, columnIndex] = _matrixData[rowIndex, columnIndex];
        }
      }

      return clonedMatrix;
    }

    public static SquareMatrix ReadFromConsole()
    {
      Console.Write("Enter matrix size: ");
      string sizeInput;
      int matrixSize;
      string rowInput;
      double elementValue;
      int displayOffset;
      string[] elementValues;

      sizeInput = Console.ReadLine();
      matrixSize = int.Parse(sizeInput);
      displayOffset = 1;

      SquareMatrix resultMatrix = new SquareMatrix(matrixSize);
      Console.WriteLine("Enter matrix elements row by row (space separated):");

      for (int rowIndex = 0; rowIndex < matrixSize; ++rowIndex)
      {
        Console.Write($"Row {rowIndex + displayOffset}: ");
        rowInput = Console.ReadLine();
        elementValues = rowInput.Split(' ');

        for (int columnIndex = 0; columnIndex < matrixSize; ++columnIndex)
        {
          elementValue = double.Parse(elementValues[columnIndex]);
          resultMatrix[rowIndex, columnIndex] = elementValue;
        }
      }

      return resultMatrix;
    }
  }
}