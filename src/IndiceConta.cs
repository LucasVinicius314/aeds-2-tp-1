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
    //Le um IndiceConta no arquivo com o offset desejado, sua origem e no arquivo passado como parametro
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

    //Retorna um IndiceConta por meio de pesquisa binaria pelo id
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

    //Copia todos os atributos necessarios da conta e cria um novo indice
    public static void CriarIndice(uint id, Conta conta, long posicao)
    {
      var indiceConta = new IndiceConta();
      indiceConta.Lapide = conta.Lapide;
      indiceConta.TotalBytes = (byte)indiceConta.GetSomaBytes();
      indiceConta.IdConta = id;
      indiceConta.Posicao = posicao;

      WriteIndice(0, indiceConta, Program.indexPath, SeekOrigin.End);
      OrdenaIndice();
    }

    //Escreve um indice no arquivo com o offset desejado, sua origem e no arquivo passado como parametro
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

      OutputFileTemp1();
      OutputFileTemp2();

      var index = -1;

      var lastPos1 = 0;
      var lastPos2 = 0;

      while (true)
      {
        index++;

        var tempReadStream1 = new IndiceContaEnumerator() { filePath = Program.tempFile1, curIndex = lastPos1 };

        var tempList1 = new List<IndiceConta>();

        uint? last1 = null;

        while (true)
        {
          var read = tempReadStream1.Next();

          if (read == null)
          {
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
          var read = tempReadStream2.Next();

          if (read == null)
          {
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

        if (newList.Count == 0)
        {
          "".ToString();
        }

        var writeStream = new FileStream(/* Program.tempFile3 */ index % 2 == 0 ? Program.tempFile3 : Program.tempFile4, FileMode.OpenOrCreate, FileAccess.ReadWrite);

        writeStream.Seek(0, SeekOrigin.End);

        foreach (var item in newList)
        {
          item.WriteToStream(writeStream);
        }

        writeStream.Close();

        "".ToString();
      }

      #region New try

      // OutputFileTemp1();
      // OutputFileTemp2();

      // var tempReadStream1 = new IndiceContaEnumerator() { filePath = Program.tempFile1, curIndex = 0 };

      // var tempList1 = new List<IndiceConta>();

      // uint? last1 = null;

      // var lastPos1 = 0;

      // while (true)
      // {
      //   var read = tempReadStream1.Next();

      //   if (read == null)
      //   {
      //     return;
      //   }

      //   if (last1 != null)
      //   {
      //     if (last1 > read.IdConta)
      //     {
      //       lastPos1 = tempReadStream1.curIndex;

      //       break;
      //     }
      //   }

      //   last1 = read.IdConta;

      //   tempList1.Add(read);
      // }

      // var tempReadStream2 = new IndiceContaEnumerator() { filePath = Program.tempFile2, curIndex = 0 };

      // var tempList2 = new List<IndiceConta>();

      // uint? last2 = null;

      // var lastPos2 = 0;

      // while (true)
      // {
      //   var read = tempReadStream2.Next();

      //   if (read == null)
      //   {
      //     return;
      //   }

      //   if (last2 != null)
      //   {
      //     if (last2 > read.IdConta)
      //     {
      //       lastPos2 = tempReadStream2.curIndex;

      //       break;
      //     }
      //   }

      //   last2 = read.IdConta;

      //   tempList2.Add(read);
      // }

      // var newList = new List<IndiceConta>();

      // newList.AddRange(tempList1);
      // newList.AddRange(tempList2);

      // newList.Sort((a, b) =>
      // {
      //   if (a.IdConta == b.IdConta)
      //   {
      //     return 0;
      //   }
      //   else if (a.IdConta > b.IdConta)
      //   {
      //     return 1;
      //   }
      //   else
      //   {
      //     return -1;
      //   }
      // });

      // var stream3 = new FileStream(Program.tempFile3, FileMode.Create, FileAccess.ReadWrite);

      // foreach (var item in newList)
      // {
      //   item.WriteToStream(stream3);
      // }

      // stream3.Close();

      // "".ToString();

      #endregion

      #region Old try

      // var tempReadStream1 = new IndiceContaEnumerator() { filePath = Program.tempFile1, curIndex = 0 };
      // var tempReadStream2 = new IndiceContaEnumerator() { filePath = Program.tempFile2, curIndex = 0 };

      // var skipCount = 0;

      // var stream3 = new FileStream(Program.tempFile3, FileMode.Create, FileAccess.ReadWrite);

      // Console.WriteLine("=====================");

      // IndiceConta? lastA = null;
      // IndiceConta? lastB = null;

      // var shouldReadA = true;
      // var shouldReadB = true;

      // OutputFileTemp1();
      // OutputFileTemp2();

      // var written = new List<uint>();

      // Console.WriteLine("=====================");

      // while (true)
      // {
      //   Console.WriteLine("=====================    NEW ITERATION");

      //   var shouldReadASession = shouldReadA;
      //   var shouldReadBSession = shouldReadB;

      //   var newA = shouldReadA ? tempReadStream1.Next() : null;
      //   var newB = shouldReadB ? tempReadStream2.Next() : null;

      //   if (shouldReadA)
      //   {
      //     Console.WriteLine("should read A. getting next from temp 1");
      //     Console.WriteLine("newA:");
      //     Console.WriteLine(newA?.IdConta);
      //     Console.WriteLine("\n");

      //     shouldReadA = false;
      //   }

      //   if (shouldReadB)
      //   {
      //     Console.WriteLine("should read B. getting next from temp 2");
      //     Console.WriteLine("newB:");
      //     Console.WriteLine(newB?.IdConta);
      //     Console.WriteLine("\n");

      //     shouldReadB = false;
      //   }

      //   if (newA != null && lastA != null)
      //   {
      //     Console.WriteLine($"    is {newA?.IdConta} < {lastA?.IdConta} ?");
      //     Console.WriteLine("    \n");

      //     if (newA?.IdConta < lastA?.IdConta)
      //     {
      //       Console.WriteLine("    yes");

      //       "".ToString();
      //     }
      //   }

      //   if (newB != null && lastB != null)
      //   {
      //     Console.WriteLine($"    is {newB?.IdConta} < {lastB?.IdConta} ?");
      //     Console.WriteLine("    \n");

      //     if (newB?.IdConta < lastB?.IdConta)
      //     {
      //       Console.WriteLine("    yes");

      //       "".ToString();

      //       shou
      //     }
      //   }

      //   if (newA == null && newB == null)
      //   {
      //     break;
      //   }

      //   var currentA = newA ?? lastA;
      //   var currentB = newB ?? lastB;

      //   if (currentA == null)
      //   {
      //   }
      //   else if (currentB == null)
      //   {
      //   }
      //   else
      //   {
      //     if (currentA.IdConta > currentB.IdConta)
      //     {
      //       currentB.WriteToStream(stream3);

      //       Console.WriteLine("=========== writing b");
      //       Console.WriteLine(currentB?.IdConta);
      //       Console.WriteLine("\n");

      //       written.Add(currentB?.IdConta ?? 0);

      //       shouldReadB = true;
      //     }
      //     else
      //     {
      //       currentA.WriteToStream(stream3);

      //       Console.WriteLine("=========== writing a");
      //       Console.WriteLine(currentA?.IdConta);
      //       Console.WriteLine("\n");

      //       written.Add(currentA?.IdConta ?? 0);

      //       shouldReadA = true;
      //     }
      //   }


      //   if (newA == null || newB == null)
      //   {
      //     "warning".ToString();
      //   }


      //   if (shouldReadASession)
      //   {
      //     lastA = newA;
      //   }

      //   if (shouldReadBSession)
      //   {
      //     lastB = newB;
      //   }

      //   // Console.WriteLine("a");
      //   // Console.WriteLine(a?.IdConta);

      //   // Console.WriteLine("b");
      //   // Console.WriteLine(b?.IdConta);

      //   // Console.WriteLine("\n");
      // }

      // stream3.Close();

      #endregion
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

    public IndiceConta? Next()
    {
      var read = IndiceConta.Read(curIndex, SeekOrigin.Begin, filePath);

      if (read.IsEmpty())
      {
        return null;
      }

      curIndex += read.TotalBytes;

      // Console.WriteLine(">>> ");
      // Console.WriteLine(read?.IdConta);
      // Console.WriteLine("\n");

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