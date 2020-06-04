namespace TCP.Checksum
{
    public static class ArrayExtensions
    {
        public static T[] Join<T>(this T[] arrayA, T[] arrayB)
        {
            var result = new T[arrayA.Length + arrayB.Length];
            for (int i = 0; i < arrayA.Length; i++)
                result[i] = arrayA[i];

            for (int i = 0; i < arrayB.Length; i++)
                result[arrayA.Length + i] = arrayB[i];

            return result;
        }

        public static T[] SubArray<T>(this T[] array, int startIndex, int size)
        {
            var result = new T[size];
            for (int i = 0; i < size; i++)
                result[i] = array[startIndex + i];

            return result;
        }
    }
}
