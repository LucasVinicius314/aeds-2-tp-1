using System.Text;

namespace Aeds3TP1
{
  class Program
  {
    // caminho do arquivo de dados
    public static string filePath = "res/data.dat";
    public static string indexPath = "res/index.dat";
    public static string filePessoa = "res/listainvertidapessoa.dat";
    public static string fileCidade = "res/listainvertidacidade.dat";
    public static string tempFile1 = "res/temp1.tmp";
    public static string tempFile2 = "res/temp2.tmp";
    public static string tempFile3 = "res/temp3.tmp";
    public static string tempFile4 = "res/temp4.tmp";

    static void Main(string[] args)
    {
      // separação do caminho de execução
      // executar o método de teste caso esteja rodando como debug
      // executar o programa normalmente caso não esteja rodando como debug

#if DEBUG
      // Test();
      TestOrdem();
      // InsertTest();
#else
      // Menu.Principal();
#endif
    }

    static void InsertTest()
    {
      Write(new Conta
      {
        Cidade = "sergipe",
        Cpf = "890890890",
        IdConta = 32,
        Lapide = '\0',
        NomePessoa = "marcelo pedro",
        SaldoConta = 4000,
        TotalBytes = 0,
        TransferenciasRealizadas = 0,
      });
      Write(new Conta
      {
        Cidade = "sergipe",
        Cpf = "890890890",
        IdConta = 64,
        Lapide = '\0',
        NomePessoa = "marcelo pedro",
        SaldoConta = 4000,
        TotalBytes = 0,
        TransferenciasRealizadas = 0,
      });
      Write(new Conta
      {
        Cidade = "sergipe",
        Cpf = "890890890",
        IdConta = 68,
        Lapide = '\0',
        NomePessoa = "marcelo pedro",
        SaldoConta = 4000,
        TotalBytes = 0,
        TransferenciasRealizadas = 0,
      });

      "".ToString();

      // Write(new Conta
      // {
      //   Cidade = "sergipe",
      //   Cpf = "890890890",
      //   IdConta = 9,
      //   Lapide = '\0',
      //   NomePessoa = "marcelo pedro",
      //   SaldoConta = 4000,
      //   TotalBytes = 0,
      //   TransferenciasRealizadas = 0,
      // });
      // Write(new Conta
      // {
      //   Cidade = "sergipe",
      //   Cpf = "890890890",
      //   IdConta = 2,
      //   Lapide = '\0',
      //   NomePessoa = "marcelo pedro",
      //   SaldoConta = 4000,
      //   TotalBytes = 0,
      //   TransferenciasRealizadas = 0,
      // });
      // Write(new Conta
      // {
      //   Cidade = "sergipe",
      //   Cpf = "890890890",
      //   IdConta = 3,
      //   Lapide = '\0',
      //   NomePessoa = "marcelo pedro",
      //   SaldoConta = 4000,
      //   TotalBytes = 0,
      //   TransferenciasRealizadas = 0,
      // });
    }

    // método de teste para testar o funcionamento das operações de forma mais isolada
    static void Test()
    {
      LimpaArquivo(filePath);
      LimpaArquivo(indexPath);
      LimpaArquivo(fileCidade);
      LimpaArquivo(filePessoa);

      Console.WriteLine("=== Conta");

      var conta = new Conta
      {
        Cidade = "alagoas",
        Cpf = "123123123",
        IdConta = 0,
        Lapide = '\0',
        NomePessoa = "joao pedro",
        SaldoConta = 1000,
        TotalBytes = 0,
        TransferenciasRealizadas = 0,
      };

      Console.WriteLine(conta);

      Write(conta); // teste de escrita
      Write(conta); // teste de escrita
      Console.WriteLine("=== Obj");

      //var obj = ReadId(1); // teste de leitura

      //Console.WriteLine(obj);

      var conta2 = new Conta
      {
        Cidade = "sergipe",
        Cpf = "890890890",
        IdConta = 3,
        Lapide = '\0',
        NomePessoa = "marcelo pedro",
        SaldoConta = 4000,
        TotalBytes = 0,
        TransferenciasRealizadas = 0,
      };

      Update(1, conta2); // teste de atualização

      Console.WriteLine("=== Obj2");

      //var obj2 = ReadId(1); // teste de leitura
      var obj2 = Conta.PesquisarConta(1);
      Console.WriteLine(obj2);

      var resposta = PesquisarPalavra("sergipe", fileCidade);
      Console.WriteLine(resposta);

      resposta = PesquisarPalavra("joao", filePessoa);
      Console.WriteLine(resposta);
      resposta = PesquisarPalavra("joaa", filePessoa);
      Console.WriteLine(resposta);

      var resposta1 = IndiceConta.ReadIdPesquisaBinaria((uint)1);
      Console.WriteLine(resposta1);

      resposta1 = IndiceConta.ReadIdPesquisaBinaria((uint)2);
      Console.WriteLine(resposta1);

      ExcluirId(1);
    }

    // método de teste para testar o funcionamento das operações de forma mais isolada
    static void TestOrdem()
    {
      // File.Delete("res/index.dat");

      // File.Copy("res/index1.dat", "res/index.dat");

      IndiceConta.OrdenaIndice();
    }

    // marca um registro como excluído a partir de um id
    // public static void ExcluirId(uint id)
    // {
    //   uint posicao = ReadId(id).Item1;

    //   MarcarExcluido(posicao);
    // }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////Create///////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public static void Write(Conta conta)
    {
      var id = UpdateCabeca(filePath);

      conta.IdConta = id;

      var posicao = Write(0, conta, SeekOrigin.End);

      ListaInvertida.WriteListaInvertidaPessoa(conta);
      ListaInvertida.WriteListaInvertidaCidade(conta);

      IndiceConta.CriarIndice(id, conta, posicao);
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////WRITE///////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    static void WriteCabeca(uint cabeca, string file)
    {
      var stream = new FileStream(file, FileMode.OpenOrCreate, FileAccess.ReadWrite);

      var newIdBytes = Utils.ReverseBytes(BitConverter.GetBytes(cabeca));

      stream.Write(newIdBytes); // escreve o id incrementado

      stream.Close();
    }

    // escreve um novo registro a partir de um id, a conta, o offset desejado e sua origem
    static long Write(long posicao, Conta conta, SeekOrigin seekOrigin)
    {
      var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

      stream.Seek(posicao, seekOrigin);
      var pos = stream.Position;
      // pegar cada conjunto de bytes de acordo com seu tipo e tamanho

      var idConta = Utils.ReverseBytes(BitConverter.GetBytes(conta.IdConta));

      var nomePessoa = Encoding.Unicode.GetBytes(conta.NomePessoa);
      var nomePessoaLength = (byte)nomePessoa.Length;

      var cpf = Encoding.Unicode.GetBytes(conta.Cpf);
      var cpfLength = (byte)cpf.Length;

      var cidade = Encoding.Unicode.GetBytes(conta.Cidade);
      var cidadeLength = (byte)cidade.Length;

      var transferenciasRealizadas = Utils.ReverseBytes(BitConverter.GetBytes(conta.TransferenciasRealizadas));

      var saldoConta = Utils.ReverseBytes(BitConverter.GetBytes(conta.SaldoConta));

      var totalbytes = conta.GetSomaBytes();

      var totalBytesBytes = Utils.ReverseBytes(BitConverter.GetBytes(totalbytes));

      stream.WriteByte((byte)'\0'); // escreve o 5º byte do arquivo, correspondente à lápide

      stream.Write(totalBytesBytes); // escreve os próximos 4 bytes do arquivo, correspondentes ao tamanho do registro

      stream.Write(idConta); // escreve os próximos 4 bytes do arquivo, correspondentes ao id do registro

      stream.WriteByte(nomePessoaLength); // escreve o próximo byte do arquivo, correspondentes à quantidade de bytes da string
      stream.Write(nomePessoa); // escreve os próximos 2x bytes do arquivo correspondentes ao nome da conta, onde x é a quantidade de caracteres da string

      stream.WriteByte(cpfLength); // escreve o próximo byte do arquivo, correspondentes à quantidade de bytes da string
      stream.Write(cpf); // escreve os próximos 2x bytes do arquivo correspondentes ao cpf da conta, onde x é a quantidade de caracteres da string

      stream.WriteByte(cidadeLength); // escreve o próximo byte do arquivo, correspondentes à quantidade de bytes da string
      stream.Write(cidade); // escreve os próximos 2x bytes do arquivo correspondentes à cidade, onde x é a quantidade de caracteres da string

      stream.Write(transferenciasRealizadas); // escreve os próximos 2 bytes do arquivo, correspondentes à quantidade de transferências da conta

      stream.Write(saldoConta); // escreve os próximos 4 bytes do arquivo, correspondentes ao saldo da conta

      stream.Close();

      return pos;
    }


    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////EXCLUIR/////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public static void ExcluirId(uint id)
    {
      var indiceConta = IndiceConta.ReadIdPesquisaBinaria(id);

      if (indiceConta != null)
      {
        var conta = Conta.Read(indiceConta.Posicao);

        MarcarExcluido(indiceConta.Posicao, filePath);
        MarcarExcluido(indiceConta.PosInd, indexPath);

        ListaInvertida.ExcluirListaInvertida(indiceConta.IdConta, conta.Cidade, fileCidade);
        ListaInvertida.ExcluirListaInvertida(indiceConta.IdConta, conta.NomePessoa, filePessoa);
      }
    }


    public static void ResetarArquivo(List<ListaInvertida> listatudo, string file)
    {
      // var quantregistro = QuantidadeRegistros(file) - 1;
      var quantregistro = (uint)listatudo.Count;
      LimpaArquivo(file);
      WriteCabeca(quantregistro, file);
      ListaInvertida.WriteListaInvertida(file, listatudo);
    }

    // marca o registro como excluído, mudando o byte da lápide a partir de um offset específico
    static void MarcarExcluido(long posicao, string file)
    {
      var stream = new FileStream(file, FileMode.OpenOrCreate, FileAccess.ReadWrite);

      stream.Seek(posicao, SeekOrigin.Begin); // ir para a posição da lápide

      stream.WriteByte((byte)'*'); // marcar o registro como excluído

      stream.Close();
    }

    static void LimpaArquivo(string file)
    {
      var stream = new FileStream(file, FileMode.Create, FileAccess.ReadWrite);

      stream.Close();
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////READ////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public static uint ReadCabeca(string file)
    {
      var ultimoId = new byte[4];

      var stream = new FileStream(file, FileMode.OpenOrCreate, FileAccess.ReadWrite);

      stream.Read(ultimoId, 0, ultimoId.Length);

      stream.Close();

      return BitConverter.ToUInt32(Utils.ReverseBytes(ultimoId));
    }

    public static uint QuantidadeRegistros(string file)
    {
      var quantregistroBytes = new byte[4];

      var stream = new FileStream(file, FileMode.OpenOrCreate, FileAccess.ReadWrite);

      stream.Read(quantregistroBytes, 0, quantregistroBytes.Length);

      quantregistroBytes = Utils.ReverseBytes(quantregistroBytes);

      var quantregistro = BitConverter.ToUInt32(quantregistroBytes);

      stream.Close();

      return quantregistro;
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////// PESQUISAR ////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public static List<uint>? PesquisarPalavra(string palavra, string file)
    {
      var listapalavras = Utils.ExtrairPalavra(palavra);
      var listapesquisar = ListaInvertida.ReadListaInvertida(file);
      var listaids = new List<string>();

      for (int j = 0; j < listapalavras.Count; j++)
      {
        for (int i = 0; i < listapesquisar.Count; i++)
        {
          if (listapesquisar[i].PessoaCidade.CompareTo(listapalavras[j]) == 0)
          {
            listaids.Add(listapesquisar[i].IdsContas);
            break;
          }
        }
      }
      if (listaids.Count > 1)
      {
        return PesquisarIdString(listaids);
      }
      if (listaids.Count != 0)
      {
        return Utils.ExtrairIds(listaids[0]);
      }

      return null;
    }

    static List<uint> PesquisarIdString(List<string> idsstring)
    {
      var ids = Utils.ExtrairIds(idsstring[0]);

      for (int i = 1; i < ids.Count; i++)
      {
        var idspesquisar = Utils.ExtrairIds(idsstring[i]);

        ids = PesquisarIdString(ids, idspesquisar);
      }

      return ids;
    }

    // static List<uint> PesquisaBinaria(List<uint> idsatual, List<uint> idspesquisar)
    // {
    //   var ids = new List<uint>();
    //   int inf;
    //   int sup;
    //   int meio;
    //   for (int i = 0; i < idspesquisar.Count; i++)
    //   {
    //     inf = 0;
    //     sup = idsatual.Count - 1;
    //     while (inf <= sup)
    //     {
    //       meio = (inf + sup) / 2;
    //       if (idspesquisar[i] == idsatual[meio])
    //         ids.Add(idspesquisar[i]);
    //       if (idspesquisar[i] < idsatual[meio])
    //         sup = meio - 1;
    //       else
    //         inf = meio + 1;
    //     }
    //   }
    //   return ids;
    // }

    static List<uint> PesquisarIdString(List<uint> idsatual, List<uint> idspesquisar)
    {
      var ids = new List<uint>();
      for (int i = 0; i < idspesquisar.Count; i++)
      {
        var a = PesquisaBinaria(idsatual, idspesquisar[i]);
        if (a != -1)
        {
          ids.Add(idspesquisar[i]);
        }
      }
      return ids;
    }

    public static List<uint> PesquisarIdExcluir(List<uint> idsatual, uint idspesquisar)
    {
      var a = PesquisaBinaria(idsatual, idspesquisar);
      if (a != -1)
      {
        idsatual.RemoveAt(a);
      }

      return idsatual;
    }

    static int PesquisaBinaria(List<uint> idsatual, uint idspesquisar)
    {
      int inf;
      int sup;
      int meio;
      inf = 0;
      sup = idsatual.Count - 1;

      while (inf <= sup)
      {
        meio = (inf + sup) / 2;
        if (idspesquisar == idsatual[meio])
          return meio;
        if (idspesquisar < idsatual[meio])
          sup = meio - 1;
        else
          inf = meio + 1;
      }
      return -1;
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////// UPDATE ///////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    // incrementa o último id no início do arquivo e retorna o novo id
    public static uint UpdateCabeca(string file)
    {
      var ultimoId = ReadCabeca(file);

      var newId = ultimoId + 1; // converte e incrementa

      WriteCabeca(newId, file);

      return newId;
    }

    public static void Update(uint id, Conta contaModificada)
    {
      var indiceConta = IndiceConta.ReadIdPesquisaBinaria(id);
      if (indiceConta != null)
      {
        var conta = Conta.Read(indiceConta.Posicao);
        contaModificada.IdConta = id;
        contaModificada.TotalBytes = contaModificada.GetSomaBytes();

        if (contaModificada.TotalBytes > conta.TotalBytes)
        {
          // o registro modificado é maior que o registro antes da modificação
          // colocar o registro anterior como excluído e criar um novo

          MarcarExcluido(indiceConta.Posicao, filePath);
          // MarcarExcluido(indiceConta.PosInd, filePath2);
          ListaInvertida.UpdateInvertida(id, conta, contaModificada);
          var posicao = Write(0, contaModificada, SeekOrigin.End);
          indiceConta.Posicao = posicao;
          IndiceConta.WriteIndice(indiceConta.PosInd, indiceConta, indexPath, SeekOrigin.Begin);
        }
        else
        {
          // o registro modificado é menor ou do mesmo tamanho que o registro antes da modificação
          // sobrescrever o registro antigo com o novo

          contaModificada.TotalBytes = conta.TotalBytes;
          ListaInvertida.UpdateInvertida(id, conta, contaModificada);
          Write(indiceConta.Posicao, contaModificada, SeekOrigin.Begin);
        }
      }
    }

    // atualiza as TransferenciasRealizadas, das contas passadas como paramentro
    public static string? TransferenciaConta(Conta contaDebitar, float debitar, Conta contaCreditar)
    {
      if (contaDebitar.SaldoConta < debitar)
      {
        return "Saldo na conta insuficiente.";
      }

      contaDebitar.SaldoConta -= debitar;
      contaDebitar.TransferenciasRealizadas += 1;

      contaCreditar.SaldoConta += debitar;
      contaCreditar.TransferenciasRealizadas += 1;

      Update(contaDebitar.IdConta, contaDebitar);
      Update(contaCreditar.IdConta, contaCreditar);

      return null;
    }
  }
}
