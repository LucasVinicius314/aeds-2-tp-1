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

      var enumerator = new IndiceContaEnumerator() { filePath = Program.indexPath };

      var stream1 = new FileStream(Program.tempFile1, FileMode.Create, FileAccess.ReadWrite);
      var stream2 = new FileStream(Program.tempFile2, FileMode.Create, FileAccess.ReadWrite);

      for (int i = 0; i <= halfQuantidade; i++)
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

      OutputFileTemp1();
      OutputFileTemp2();

      OrdenaInterno();

      var temp3 = new IndiceContaEnumerator() { curIndex = 0, filePath = Program.tempFile3 }.AsList();

      var finalStream = new FileStream(Program.indexPath, FileMode.Create, FileAccess.ReadWrite);

      finalStream.Write(Utils.ReverseBytes(BitConverter.GetBytes(temp3.Count)));

      foreach (var item in temp3)
      {
        if (item.Lapide != '*')
        {
          item.WriteToStream(finalStream);
        }
      }

      finalStream.Close();
    }

    static void OrdenaInterno()
    {
      var index = -1;

      var lastPos1 = 0;
      var lastPos2 = 0;

      var lock1 = false;
      var lock2 = false;

      File.Delete(Program.tempFile3);
      File.Delete(Program.tempFile4);

      while (true)
      {
        index++;

        var tempReadStream1 = new IndiceContaEnumerator() { filePath = Program.tempFile1, curIndex = lastPos1 };

        var tempList1 = new List<IndiceConta>();

        uint? last1 = null;

        while (true)
        {
          if (lock1)
          {
            break;
          }

          var read = tempReadStream1.Next();

          if (read == null)
          {
            lock1 = true;

            break;
          }

          if (last1 != null)
          {
            if (last1 > read.IdConta)
            {
              lastPos1 = tempReadStream1.curIndex - 14;

              break;
            }
          }

          last1 = read.IdConta;

          tempList1.Add(read);
        }

        var tempReadStream2 = new IndiceContaEnumerator() { filePath = Program.tempFile2, curIndex = lastPos2 };

        var tempList2 = new List<IndiceConta>();

        uint? last2 = null;

        while (true)
        {
          if (lock2)
          {
            break;
          }

          var read = tempReadStream2.Next();

          if (read == null)
          {
            lock2 = true;

            break;
          }

          if (last2 != null)
          {
            if (last2 > read.IdConta)
            {
              lastPos2 = tempReadStream2.curIndex - 14;

              break;
            }
          }

          last2 = read.IdConta;

          tempList2.Add(read);
        }

        var newList = new List<IndiceConta>();

        newList.AddRange(tempList1);
        newList.AddRange(tempList2);

        newList.Sort((a, b) =>
        {
          if (a.IdConta == b.IdConta)
          {
            return 0;
          }
          else if (a.IdConta > b.IdConta)
          {
            return 1;
          }
          else
          {
            return -1;
          }
        });

        var writeStream = new FileStream(index % 2 == 0 ? Program.tempFile3 : Program.tempFile4, FileMode.OpenOrCreate, FileAccess.ReadWrite);

        writeStream.Seek(0, SeekOrigin.End);

        foreach (var item in newList)
        {
          item.WriteToStream(writeStream);
        }

        writeStream.Close();

        "".ToString();

        if (lock1 && lock2)
        {
          goto main;
        }
      }

    main:

      File.Delete(Program.tempFile1);
      File.Delete(Program.tempFile2);

      try
      {
        File.Copy(Program.tempFile3, Program.tempFile1);
      }
      catch (System.Exception)
      {
      }

      try
      {
        File.Copy(Program.tempFile4, Program.tempFile2);
      }
      catch (System.Exception)
      {
      }

      if (index != 0)
      {
        OrdenaInterno();
      }
    }

    static void OutputFileTemp1()
    {
      var tempReadStream1 = new IndiceContaEnumerator() { filePath = Program.tempFile1, curIndex = 0 };

      while (true)
      {
        var read = tempReadStream1.Next();

        if (read == null)
        {
          return;
        }

        Console.WriteLine("read new value temp 1");
        Console.WriteLine(read?.IdConta);
        Console.WriteLine("\n");
      }
    }

    static void OutputFileTemp2()
    {
      var tempReadStream1 = new IndiceContaEnumerator() { filePath = Program.tempFile2, curIndex = 0 };

      while (true)
      {
        var read = tempReadStream1.Next();

        if (read == null)
        {
          return;
        }

        Console.WriteLine("read new value temp 2");
        Console.WriteLine(read?.IdConta);
        Console.WriteLine("\n");
      }
    }
  }

  class IndiceContaEnumerator
  {
    public string filePath = String.Empty;
    public int curIndex = 4;

    public List<IndiceConta> AsList()
    {
      var oldIndex = curIndex;

      var list = new List<IndiceConta>();

      while (true)
      {
        var read = Next();

        if (read == null)
        {
          break;
        }

        list.Add(read);
      }

      curIndex = oldIndex;

      return list;
    }

    public IndiceConta? Next()
    {
      var read = IndiceConta.Read(curIndex, SeekOrigin.Begin, filePath);

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