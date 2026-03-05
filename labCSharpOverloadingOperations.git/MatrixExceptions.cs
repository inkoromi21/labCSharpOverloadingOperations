using System;

namespace MatrixCalculator
{
  public class MatrixException : Exception
  {
    public MatrixException() { }
    public MatrixException(string message) : base(message) { }
  }

  public class InvalidMatrixSizeException : MatrixException
  {
    public InvalidMatrixSizeException() { }
    public InvalidMatrixSizeException(string message) : base(message) { }
  }

  public class NonSquareMatrixException : MatrixException
  {
    public NonSquareMatrixException() { }
    public NonSquareMatrixException(string message) : base(message) { }
  }

  public class MatrixNullException : MatrixException
  {
    public MatrixNullException() { }
    public MatrixNullException(string message) : base(message) { }
  }

  public class MatrixDimensionMismatchException : MatrixException
  {
    public MatrixDimensionMismatchException() { }
    public MatrixDimensionMismatchException(string message) : base(message) { }
  }

  public class SingularMatrixException : MatrixException
  {
    public SingularMatrixException() { }
    public SingularMatrixException(string message) : base(message) { }
  }
}