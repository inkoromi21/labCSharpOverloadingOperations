using System;
using System.Text;

namespace MatrixCalculator
{
  public class SquareMatrix : IComparable<SquareMatrix>, IEquatable<SquareMatrix>
  {
    private double[,] data;
    public int Size { get; private set; }

    public SquareMatrix(int size)
    {
      if (size <= 0)
        throw new InvalidMatrixSizeException("Matrix size must be positive");

      Size = size;
      data = new double[size, size];
    }

    public SquareMatrix(int size, bool random) : this(size)
    {
      if (random)
      {
        Random rand = new Random();
        for (int i = 0; i < size; i++)
          for (int j = 0; j < size; j++)
            data[i, j] = rand.Next(-10, 11);
      }
    }

    public SquareMatrix(double[,] matrix)
    {
      if (matrix == null)
        throw new MatrixNullException("Matrix cannot be null");

      if (matrix.GetLength(0) != matrix.GetLength(1))
        throw new NonSquareMatrixException("Matrix must be square");

      Size = matrix.GetLength(0);
      data = new double[Size, Size];

      for (int i = 0; i < Size; i++)
        for (int j = 0; j < Size; j++)
          data[i, j] = matrix[i, j];
    }

    public double this[int i, int j]
    {
      get
      {
        if (i < 0 || i >= Size || j < 0 || j >= Size)
          throw new IndexOutOfRangeException("Index out of matrix bounds");
        return data[i, j];
      }
      set
      {
        if (i < 0 || i >= Size || j < 0 || j >= Size)
          throw new IndexOutOfRangeException("Index out of matrix bounds");
        data[i, j] = value;
      }
    }

    public static SquareMatrix operator +(SquareMatrix a, SquareMatrix b)
    {
      if (a == null || b == null)
        throw new MatrixNullException("Matrices cannot be null");

      if (a.Size != b.Size)
        throw new MatrixDimensionMismatchException("Matrices must have same size for addition");

      SquareMatrix result = new SquareMatrix(a.Size);
      for (int i = 0; i < a.Size; i++)
        for (int j = 0; j < a.Size; j++)
          result[i, j] = a[i, j] + b[i, j];

      return result;
    }

    public static SquareMatrix operator *(SquareMatrix a, SquareMatrix b)
    {
      if (a == null || b == null)
        throw new MatrixNullException("Matrices cannot be null");

      if (a.Size != b.Size)
        throw new MatrixDimensionMismatchException("Matrices must have same size for multiplication");

      SquareMatrix result = new SquareMatrix(a.Size);
      for (int i = 0; i < a.Size; i++)
      {
        for (int j = 0; j < a.Size; j++)
        {
          double sum = 0;
          for (int k = 0; k < a.Size; k++)
            sum += a[i, k] * b[k, j];
          result[i, j] = sum;
        }
      }
      return result;
    }

    public static bool operator >(SquareMatrix a, SquareMatrix b)
    {
      if (a == null || b == null)
        throw new MatrixNullException("Matrices cannot be null");

      return a.Determinant() > b.Determinant();
    }

    public static bool operator <(SquareMatrix a, SquareMatrix b)
    {
      if (a == null || b == null)
        throw new MatrixNullException("Matrices cannot be null");

      return a.Determinant() < b.Determinant();
    }

    public static bool operator >=(SquareMatrix a, SquareMatrix b)
    {
      if (a == null || b == null)
        throw new MatrixNullException("Matrices cannot be null");

      return a.Determinant() >= b.Determinant();
    }

    public static bool operator <=(SquareMatrix a, SquareMatrix b)
    {
      if (a == null || b == null)
        throw new MatrixNullException("Matrices cannot be null");

      return a.Determinant() <= b.Determinant();
    }

    public static bool operator ==(SquareMatrix a, SquareMatrix b)
    {
      if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
        return true;
      if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
        return false;

      return a.Equals(b);
    }

    public static bool operator !=(SquareMatrix a, SquareMatrix b)
    {
      return !(a == b);
    }

    public static explicit operator double[,](SquareMatrix m)
    {
      if (m == null)
        throw new MatrixNullException("Matrix cannot be null");

      double[,] result = new double[m.Size, m.Size];
      for (int i = 0; i < m.Size; i++)
        for (int j = 0; j < m.Size; j++)
          result[i, j] = m[i, j];

      return result;
    }

    public static implicit operator SquareMatrix(double[,] array)
    {
      return new SquareMatrix(array);
    }

    public static bool operator true(SquareMatrix m)
    {
      if (m == null)
        return false;
      return Math.Abs(m.Determinant()) > 1e-10;
    }

    public static bool operator false(SquareMatrix m)
    {
      if (m == null)
        return true;
      return Math.Abs(m.Determinant()) <= 1e-10;
    }

    public double Determinant()
    {
      if (Size == 1)
        return data[0, 0];

      if (Size == 2)
        return data[0, 0] * data[1, 1] - data[0, 1] * data[1, 0];

      double det = 0;
      for (int j = 0; j < Size; j++)
      {
        det += data[0, j] * Cofactor(0, j);
      }
      return det;
    }

    private double Cofactor(int row, int col)
    {
      return Minor(row, col) * ((row + col) % 2 == 0 ? 1 : -1);
    }

    private double Minor(int row, int col)
    {
      SquareMatrix minor = new SquareMatrix(Size - 1);
      int r = 0, c = 0;

      for (int i = 0; i < Size; i++)
      {
        if (i == row) continue;
        c = 0;
        for (int j = 0; j < Size; j++)
        {
          if (j == col) continue;
          minor[r, c] = data[i, j];
          c++;
        }
        r++;
      }

      return minor.Determinant();
    }

    public SquareMatrix Inverse()
    {
      double det = Determinant();
      if (Math.Abs(det) < 1e-10)
        throw new SingularMatrixException("Matrix is singular, cannot find inverse");

      SquareMatrix inverse = new SquareMatrix(Size);

      for (int i = 0; i < Size; i++)
      {
        for (int j = 0; j < Size; j++)
        {
          inverse[j, i] = Cofactor(i, j) / det;
        }
      }

      return inverse;
    }

    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();
      for (int i = 0; i < Size; i++)
      {
        for (int j = 0; j < Size; j++)
        {
          sb.Append(data[i, j].ToString("F2").PadLeft(8));
        }
        sb.AppendLine();
      }
      return sb.ToString();
    }

    public int CompareTo(SquareMatrix other)
    {
      if (other == null)
        return 1;

      return Determinant().CompareTo(other.Determinant());
    }

    public override bool Equals(object obj)
    {
      return Equals(obj as SquareMatrix);
    }

    public bool Equals(SquareMatrix other)
    {
      if (other == null)
        return false;

      if (Size != other.Size)
        return false;

      for (int i = 0; i < Size; i++)
      {
        for (int j = 0; j < Size; j++)
        {
          if (Math.Abs(data[i, j] - other[i, j]) > 1e-10)
            return false;
        }
      }
      return true;
    }

    public override int GetHashCode()
    {
      int hash = 17;
      foreach (double val in data)
      {
        hash = hash * 31 + val.GetHashCode();
      }
      return hash;
    }

    public SquareMatrix Clone()
    {
      SquareMatrix clone = new SquareMatrix(Size);
      for (int i = 0; i < Size; i++)
        for (int j = 0; j < Size; j++)
          clone[i, j] = data[i, j];
      return clone;
    }

    public static SquareMatrix ReadFromConsole()
    {
      Console.Write("Enter matrix size: ");
      int size = int.Parse(Console.ReadLine());

      SquareMatrix matrix = new SquareMatrix(size);
      Console.WriteLine("Enter matrix elements row by row (space separated):");

      for (int i = 0; i < size; i++)
      {
        Console.Write($"Row {i + 1}: ");
        string[] values = Console.ReadLine().Split(' ');
        for (int j = 0; j < size; j++)
        {
          matrix[i, j] = double.Parse(values[j]);
        }
      }

      return matrix;
    }
  }
}
