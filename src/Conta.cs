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

    public static Conta? PesquisarConta(uint chave)
    {
      var resp = IndiceConta.ReadIdPesquisaBinaria(chave);

      if (resp != null)
      {
        return Read(resp.Posicao);
      }

      return null;
    }

    public static Conta Read(long offset)
    {
      // ler cara conjunto de bytes de acordo com seu respectivo tipo e tamanho, para cada atributo da classe

      #region Arquivo

      var stream = new FileStream(Program.filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

      stream.Seek(offset, SeekOrigin.Begin);

      var lapide = (char)stream.ReadByte();

      var totalBytesBytes = new byte[4];

      stream.Read(totalBytesBytes, 0, totalBytesBytes.Length);

      totalBytesBytes = Utils.ReverseBytes(totalBytesBytes);

      var totalBytes = BitConverter.ToUInt32(totalBytesBytes);

      #endregion

      #region Id conta

      var idContaBytes = new byte[4];

      stream.Read(idContaBytes, 0, idContaBytes.Length);

      idContaBytes = Utils.ReverseBytes(idContaBytes);

      var idConta = BitConverter.ToUInt32(idContaBytes);

      #endregion

      #region Nome pessoa

      byte nomePessoaTamanho = (byte)stream.ReadByte();

      var nomePessoaBytes = new byte[nomePessoaTamanho];

      stream.Read(nomePessoaBytes, 0, nomePessoaBytes.Length);

      var nomePessoa = Encoding.Unicode.GetString(nomePessoaBytes);

      #endregion

      #region Cpf

      byte cpfTamanho = (byte)stream.ReadByte();

      var cpfBytes = new byte[cpfTamanho];

      stream.Read(cpfBytes, 0, cpfBytes.Length);

      var cpf = Encoding.Unicode.GetString(cpfBytes);

      #endregion

      #region Cidade

      byte cidadeTamanho = (byte)stream.ReadByte();

      var cidadeBytes = new byte[cidadeTamanho];

      stream.Read(cidadeBytes, 0, cidadeBytes.Length);

      var cidade = Encoding.Unicode.GetString(cidadeBytes);

      #endregion

      #region Transferencias realizadas

      var transferenciasRealizadasBytes = new byte[2];

      stream.Read(transferenciasRealizadasBytes, 0, transferenciasRealizadasBytes.Length);

      transferenciasRealizadasBytes = Utils.ReverseBytes(transferenciasRealizadasBytes);

      var transferenciasRealizadas = BitConverter.ToUInt16(transferenciasRealizadasBytes);

      #endregion

      #region Saldo conta

      var saldoContaBytes = new byte[4];

      stream.Read(saldoContaBytes, 0, saldoContaBytes.Length);

      saldoContaBytes = Utils.ReverseBytes(saldoContaBytes);

      var saldoConta = BitConverter.ToSingle(saldoContaBytes);

      #endregion

      stream.Close();

      return new Conta
      {
        Lapide = lapide,
        TotalBytes = totalBytes,
        Cidade = cidade,
        Cpf = cpf,
        IdConta = idConta,
        NomePessoa = nomePessoa,
        SaldoConta = saldoConta,
        TransferenciasRealizadas = transferenciasRealizadas,
      };
    }
  }
}