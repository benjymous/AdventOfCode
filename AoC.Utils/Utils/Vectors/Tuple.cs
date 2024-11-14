namespace AoC.Utils.Vectors
{
    public static class Tuple
    {
        public static T CrossX<T>((T X, T Y, T Z) a, (T X, T Y, T Z) b) where T : INumber<T> => (a.Y * b.Z) - (a.Z * b.Y);

        public static T CrossY<T>((T X, T Y, T Z) a, (T X, T Y, T Z) b) where T : INumber<T> => (a.Z * b.X) - (a.X * b.Z);
        public static T CrossZ<T>((T X, T Y, T Z) a, (T X, T Y, T Z) b) where T : INumber<T> => (a.X * b.Y) - (a.Y * b.X);
        public static (T X, T Y, T Z) Subtract<T>((T X, T Y, T Z) a, (T X, T Y, T Z) b) where T : INumber<T> => (a.X - b.X, a.Y - b.Y, a.Z - b.Z);

        public static T[,] InvertMatrix<T>(T[,] input) where T : INumber<T>
        {
            ArgumentNullException.ThrowIfNull(input);
            if (input.GetLength(0) != input.GetLength(1)) throw new ArgumentException("Input matrix must be square.");

            int n = input.GetLength(0);
            T[,] result = new T[n, n];
            T[,] augmented = new T[n, 2 * n];

            // Augment the input matrix with an identity matrix
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    augmented[i, j] = input[i, j];
                }
                augmented[i, i + n] = T.One;
            }

            // Perform row operations to transform the left half of the augmented matrix into the identity matrix
            for (int i = 0; i < n; i++)
            {
                T pivot = augmented[i, i];
                if (pivot == T.Zero) throw new ArgumentException("Input matrix is singular.");

                for (int j = 0; j < 2 * n; j++)
                {
                    augmented[i, j] /= pivot;
                }

                for (int j = 0; j < n; j++)
                {
                    if (j != i)
                    {
                        T factor = augmented[j, i];
                        for (int k = 0; k < 2 * n; k++)
                        {
                            augmented[j, k] -= factor * augmented[i, k];
                        }
                    }
                }
            }

            // Extract the inverted matrix from the right half of the augmented matrix
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    result[i, j] = augmented[i, j + n];
                }
            }

            return result;
        }

        public static T[] MultiplyMatrixAndVector<T>(T[,] matrix, T[] vector) where T : INumber<T>
        {
            ArgumentNullException.ThrowIfNull(matrix);
            ArgumentNullException.ThrowIfNull(vector);

            if (matrix.GetLength(1) != vector.Length)
                throw new ArgumentException("Matrix column count must match vector length.");

            int rowCount = matrix.GetLength(0);
            int colCount = matrix.GetLength(1);
            T[] result = new T[rowCount];

            for (int i = 0; i < rowCount; i++)
            {
                T sum = T.Zero;
                for (int j = 0; j < colCount; j++)
                {
                    sum += matrix[i, j] * vector[j];
                }
                result[i] = sum;
            }

            return result;
        }
    }
}
