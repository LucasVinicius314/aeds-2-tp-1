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

    public bool IsEmpty()
    {
      return this.IdConta == 0;
    }

    public static IndiceConta Read(long offset, SeekOrigin seekOrigin, string filename)
    {
      // ler cada conjunto de bytes de acordo com seu respectivo tipo e tamanho, para cada atributo da classe
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
      var cabeca = Program.ReadCabeca(Program.indexPath);
      int inf = 0;     // limite inferior (o primeiro índice de vetor em C é zero          )
      int sup = (int)cabeca - 1; // limite superior (termina em um número a menos. 0 a 9 são 10 números)
      int meio;

      while (inf <= sup)
      {
        meio = (inf + sup) / 2;
        var posicao = 4 + meio * tamanho;
        var indice = Read(posicao, SeekOrigin.Begin, Program.indexPath);
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

      WriteIndice(0, indiceConta, Program.indexPath, SeekOrigin.End);
      OrdenaIndice(); //fazer mais tarde
    }

    public static void WriteIndice(uint offset, IndiceConta indiceConta, string file, SeekOrigin seekOrigin)
    {
      if (seekOrigin == SeekOrigin.End)
      {
        Program.UpdateCabeca(Program.indexPath);
      }

      var stream = new FileStream(file, FileMode.OpenOrCreate, FileAccess.ReadWrite);

      stream.Seek(offset, seekOrigin);
      indiceConta.WriteToStream(stream);

      stream.Close();
    }

    public void WriteToStream(FileStream fs)
    {
      fs.WriteByte((byte)this.Lapide);
      fs.WriteByte(this.TotalBytes);
      fs.Write(Utils.ReverseBytes(BitConverter.GetBytes(this.IdConta)));
      fs.Write(Utils.ReverseBytes(BitConverter.GetBytes(this.Posicao)));
    }

    public static void OrdenaIndice()
    {
      var quantidadeRegistros = Program.ReadCabeca(Program.indexPath);

      var halfQuantidade = Math.Floor((double)quantidadeRegistros / 2);

      var enumerator = new IndiceContaEnumerator();

      var stream1 = new FileStream(Program.tempFile1, FileMode.Create, FileAccess.ReadWrite);
      var stream2 = new FileStream(Program.tempFile2, FileMode.Create, FileAccess.ReadWrite);

      for (int i = 0; i < halfQuantidade; i++)
      {
        var list = new List<IndiceConta>();

        var one = enumerator.Next();

        var two = enumerator.Next();

        if (one == null && two == null)
        {
          break;
        }

        if (one == null)
        {
          list.Add(two!);
        }
        else if (two == null)
        {
          list.Add(one);
        }
        else
        {
          if (one.IdConta < two.IdConta)
          {
            list.Add(one);
            list.Add(two);
          }
          else
          {
            list.Add(two);
            list.Add(one);
          }
        }

        foreach (var contaIndex in list)
        {
          if (i % 2 == 0)
          {
            contaIndex.WriteToStream(stream1);
          }
          else
          {
            contaIndex.WriteToStream(stream2);
          }
        }
      }

      stream1.Close();
      stream2.Close();

      enumerator.ToString();
    }
  }

  class IndiceContaEnumerator
  {
    int curIndex = 4;

    public IndiceConta? Next()
    {
      var read = IndiceConta.Read(curIndex, SeekOrigin.Begin, Program.indexPath);

      if (read.IsEmpty())
      {
        return null;
      }

      curIndex += read.TotalBytes;

      return read;
    }

    public void Reset()
    {
      curIndex = 4;
    }
  }
}


// 1 byte => lápide
// 1 byte => total bytes
// 4 byte => id
// 8 byte => posição no arquivo