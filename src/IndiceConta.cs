namespace Aeds3TP1
{
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

    public static IndiceConta ReadIndice(long offset, SeekOrigin seekOrigin, string filename)
    {
      // ler cara conjunto de bytes de acordo com seu respectivo tipo e tamanho, para cada atributo da classe
      var indiceConta = new IndiceConta();
      var stream = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite);
      stream.Seek(offset, seekOrigin);
      var posind = stream.Position;
      // stream.Seek(0, SeekOrigin.Begin);
      char lapide = (char)stream.ReadByte();

      int totalBytesBytes = stream.ReadByte();

      #region Id conta

      var idContaBytes = new byte[4];

      stream.Read(idContaBytes, 0, idContaBytes.Length);

      idContaBytes = Utils.ReverseBytes(idContaBytes);

      indiceConta.IdConta = BitConverter.ToUInt32(idContaBytes);

      #endregion

      #region Posicao conta

      var posicaoContaBytes = new byte[8];

      stream.Read(posicaoContaBytes, 0, posicaoContaBytes.Length);

      posicaoContaBytes = Utils.ReverseBytes(posicaoContaBytes);

      indiceConta.Posicao = BitConverter.ToUInt32(posicaoContaBytes);

      #endregion

      indiceConta.PosInd = (uint)posind;
      indiceConta.Lapide = lapide;
      indiceConta.TotalBytes = (byte)totalBytesBytes;


      stream.Close();

      return indiceConta;
    }

    public static IndiceConta? ReadIdPesquisaBinaria(uint chave)
    {
      var temp = new IndiceConta();
      var tamanho = temp.GetSomaBytes();
      var cabeca = Program.ReadCabeca(Program.filePath2);
      int inf = 0;     // limite inferior (o primeiro índice de vetor em C é zero          )
      int sup = (int)cabeca - 1; // limite superior (termina em um número a menos. 0 a 9 são 10 números)
      int meio;
      while (inf <= sup)
      {
        meio = (inf + sup) / 2;
        var posicao = 4 + meio * tamanho;
        var indice = IndiceConta.ReadIndice(posicao, SeekOrigin.Begin, Program.filePath2);
        if (chave == indice.IdConta)
          if (indice.Lapide != '*')
            return indice;
        if (chave < indice.IdConta)
          sup = meio - 1;
        else
          inf = meio + 1;
      }
      return null;   // não encontrado
    }

    public static void CriarIndice(uint id, Conta conta, long posicao)
    {
      var indiceConta = new IndiceConta();
      indiceConta.Lapide = conta.Lapide;
      indiceConta.TotalBytes = (byte)indiceConta.GetSomaBytes();
      indiceConta.IdConta = id;
      indiceConta.Posicao = posicao;

      WriteIndice(0, indiceConta, Program.filePath2, SeekOrigin.End);
      OrdenaIndice(); //fazer mais tarde
    }

    public static void WriteIndice(uint offset, IndiceConta indiceConta, string file, SeekOrigin seekOrigin)
    {
      if (seekOrigin == SeekOrigin.End)
      {
        Program.UpdateCabeca(Program.filePath2);
      }
      var stream = new FileStream(file, FileMode.OpenOrCreate, FileAccess.ReadWrite);
      stream.Seek(offset, seekOrigin);
      stream.WriteByte((byte)indiceConta.Lapide);
      stream.WriteByte(indiceConta.TotalBytes);
      stream.Write(Utils.ReverseBytes(BitConverter.GetBytes(indiceConta.IdConta)));
      stream.Write(Utils.ReverseBytes(BitConverter.GetBytes(indiceConta.Posicao)));
      //var listaIndiceConta = ReadIndices(); pega a lista com todos, usar na busca binaria
      // for (int i = 0; i != listaIndiceConta.Count; i++)
      // {
      //   stream.WriteByte(listaIndiceConta[i].TotalBytes);
      //   stream.Write(Utils.ReverseBytes(BitConverter.GetBytes(listaIndiceConta[i].IdConta)));
      //   stream.Write(Utils.ReverseBytes(BitConverter.GetBytes(listaIndiceConta[i].Posicao)));

      // }
      stream.Close();
    }

    public static void OrdenaIndice()
    {
      "".ToString();


    }
  }
}