using System.Text;

namespace Aeds3TP1
{
  class Conta
  {
    public char Lapide { get; set; }
    public uint TotalBytes { get; set; }
    public uint IdConta { get; set; }
    public string NomePessoa
    {
      get { return nomePessoa; }
      set
      {
        if (value == null) throw new FormatException();

        nomePessoa = value;
      }
    }
    public string Cpf
    {
      get { return cpf; }
      set
      {
        if (value == null) throw new FormatException();

        cpf = value;
      }
    }
    public string Cidade
    {
      get { return cidade; }
      set
      {
        if (value == null) throw new FormatException();

        cidade = value;
      }
    }
    public ushort TransferenciasRealizadas { get; set; }
    public float SaldoConta { get; set; }

    string nomePessoa = String.Empty;
    string cpf = String.Empty;
    string cidade = String.Empty;

    public uint GetSomaBytes()
    {
      var totalbytes = BitConverter.GetBytes(IdConta).Length +
        Encoding.Unicode.GetBytes(NomePessoa).Length +
        Encoding.Unicode.GetBytes(Cpf).Length +
        Encoding.Unicode.GetBytes(Cidade).Length +
        BitConverter.GetBytes(TransferenciasRealizadas).Length +
        BitConverter.GetBytes(SaldoConta).Length + 8;

      return BitConverter.ToUInt32(BitConverter.GetBytes(totalbytes));
    }

    public override string ToString()
    {
      var hashCode = base.GetHashCode();

      return $@"Conta [{hashCode}]
    > Lapide ({Lapide.GetType()}): {Lapide}
    > TotalBytes ({TotalBytes.GetType()}): {TotalBytes}
    > IdConta ({IdConta.GetType()}): {IdConta}
    > NomePessoa ({NomePessoa?.GetType()}): {NomePessoa ?? ""}
    > Cpf ({Cpf?.GetType()}): {Cpf ?? ""}
    > Cidade ({Cidade?.GetType()}): {Cidade ?? ""}
    > TransferenciasRealizadas ({TransferenciasRealizadas.GetType()}): {TransferenciasRealizadas}
    > SaldoConta ({SaldoConta.GetType()}): {SaldoConta}";
    }
  }
}