namespace Aeds3TP1
{
  // classe de utilidades
  class Utils
  {
    // método para inverter a ordem de um array de bytes
    public static byte[] ReverseBytes(byte[] bytes)
    {
      Array.Reverse(bytes, 0, bytes.Length);

      return bytes;
    }
  }
}