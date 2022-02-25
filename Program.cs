namespace Aeds3TP1
{
  class Program
  {
    static string filePath = "data.dat";

    static void Main(string[] args)
    {
      Write();

      Read();

      var conta = new Conta
      {
        IdConta = 0,
        NomePessoa = "a",
        Cpf = "a",
        Cidade = "a",
        TransferenciasRealizadas = 0,
        SaldoConta = 0,
      };
    }

    static void Read()
    {
      var ultimoId = new byte[4];

      var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

      stream.Read(ultimoId, 0, ultimoId.Length);

      var lapide = (char)stream.ReadByte();

      stream.Close();
    }

    static void Write()
    {
      var bytes = BitConverter.GetBytes(0);

      var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

      stream.Write(bytes);

      stream.WriteByte((byte)'\0');

      stream.Close();
    }
  }
}
