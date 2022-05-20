using System.Text;

namespace Aeds3TP1
{
  class Conta
  {
    // getters e setters
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

    // retorna o tamanho em bytes dos atributos do registro
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

    // override no toString padrão para melhorar a visualização do objeto
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
  class IndiceConta
  {
    // getters e setters
    public uint PosInd { get; set; }
    public char Lapide { get; set; }
    public byte TotalBytes { get; set; }
    public uint IdConta { get; set; }
    public long Posicao { get; set; }

    // retorna o tamanho em bytes dos atributos do registro
    public int GetSomaBytes()
    {
      return BitConverter.GetBytes(IdConta).Length +
                       BitConverter.GetBytes(Posicao).Length + 2;

    }

    // override no toString padrão para melhorar a visualização do objeto
    public override string ToString()
    {
      var hashCode = base.GetHashCode();

      return $@"IndiceConta [{hashCode}]
    > Lapide ({Lapide.GetType()}): {Lapide}
    > TotalBytes ({TotalBytes.GetType()}): {TotalBytes}
    > IdConta ({IdConta.GetType()}): {IdConta}
    > SaldoConta ({Posicao.GetType()}): {Posicao}";
    }
  }

  class ListaInvertida
  {
    // getters e setters
    public uint TotalBytes { get; set; }
    public string IdsContas
    {
      get { return idsContas; }
      set
      {
        if (value == null) throw new FormatException();

        idsContas = value;
      }
    }

    public string PessoaCidade
    {
      get { return pessoaCidade; }
      set
      {
        if (value == null) throw new FormatException();

        pessoaCidade = value;
      }
    }

    string pessoaCidade = String.Empty;
    string idsContas = String.Empty;

    // retorna o tamanho em bytes dos atributos do registro
    public uint GetSomaBytes()
    {
      var totalbytes = Encoding.Unicode.GetBytes(pessoaCidade).Length +
      Encoding.Unicode.GetBytes(pessoaCidade).Length +
                       BitConverter.GetBytes(TotalBytes).Length + 2;

      return BitConverter.ToUInt32(BitConverter.GetBytes(totalbytes));

    }

    // override no toString padrão para melhorar a visualização do objeto
    public override string ToString()
    {
      var hashCode = base.GetHashCode();

      return $@"ListaInvertida [{hashCode}]
    > TotalBytes ({TotalBytes.GetType()}): {TotalBytes}
    > IdsContas ({idsContas?.GetType()}): {idsContas ?? ""}
    > NomePessoa ({pessoaCidade?.GetType()}): {pessoaCidade ?? ""}";
    }
  }
}