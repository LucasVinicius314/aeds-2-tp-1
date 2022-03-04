namespace Aeds3TP1
{
  class Utils
  {
    public static byte[] ReverseBytes(byte[] bytes)
    {
      Array.Reverse(bytes, 0, bytes.Length);

      return bytes;
    }
  }
}