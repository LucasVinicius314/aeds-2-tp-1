using System.Text;

namespace Aeds3TP1
{
  class Program
  {
    // caminho do arquivo de dados
    static string filePath = "data.dat";
    static string filePath2 = "index.dat";
    static string filepessoa = "listainvertidapessoa.dat";

    static string filecidade = "listainvertidacidade.dat";

    static void Main(string[] args)
    {
      // separação do caminho de execução
      // executar o método de teste caso esteja rodando como debug
      // executar o programa normalmente caso não esteja rodando como debug

#if DEBUG
      Test();
#else
      Menu.Principal();
#endif
    }

    // método de teste para testar o funcionamento das operações de forma mais isolada
    static void Test()
    {
      LimpaArquivo(filePath);
      LimpaArquivo(filePath2);
      LimpaArquivo(filecidade);
      LimpaArquivo(filepessoa);

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
      var obj2 = PesquisarConta(1);
      Console.WriteLine(obj2);

      var resposta = PesquisarPalavra("sergipe", filecidade);
      Console.WriteLine(resposta);

      resposta = PesquisarPalavra("joao", filepessoa);
      Console.WriteLine(resposta);
      resposta = PesquisarPalavra("joaa", filepessoa);
      Console.WriteLine(resposta);

      var resposta1 = ReadIdPesquisaBinaria((uint)1);
      Console.WriteLine(resposta1);

      resposta1 = ReadIdPesquisaBinaria((uint)2);
      Console.WriteLine(resposta1);

      ExcluirId(1);

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
    static void CriarIndice(uint id, Conta conta, long posicao)
    {
      var indeceConta = new IndiceConta();
      indeceConta.Lapide = conta.Lapide;
      indeceConta.TotalBytes = (byte)indeceConta.GetSomaBytes();
      indeceConta.IdConta = id;
      indeceConta.Posicao = posicao;

      WriteIndice(0, indeceConta, filePath2, SeekOrigin.End);
      OrdenaIndice();//fazer mais tarde
    }

    public static void Write(Conta conta)
    {
      var id = UpdateCabeca(filePath);
      conta.IdConta = id;
      var posicao = Write(0, conta, SeekOrigin.End);
      WriteListaInvertidaPessoa(conta);
      WriteListaInvertidaCidade(conta);
      CriarIndice(id, conta, posicao);
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

    static void WriteIndice(uint offset, IndiceConta indiceConta, string file, SeekOrigin seekOrigin)
    {
      if (seekOrigin == SeekOrigin.End)
      {
        UpdateCabeca(filePath2);
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


    public static void WriteListaInvertidaPessoa(Conta conta)
    {
      WriteListaInvertida(filepessoa, InserirListaInvertida(conta.IdConta, conta.NomePessoa, filepessoa));
    }
    public static void WriteListaInvertidaCidade(Conta conta)
    {
      WriteListaInvertida(filecidade, InserirListaInvertida(conta.IdConta, conta.Cidade, filecidade));
    }

    static List<ListaInvertida> InserirListaInvertida(uint id, string pessoacidade, string file)
    {
      //mecher nos parametros mais tarde, inv n faz sentido
      var listapesquisar = ReadListaInvertida(file);
      var listapalavras = ExtrairPalavra(pessoacidade);

      var tamanhopesquisa = listapesquisar.Count;

      for (int j = 0; j < listapalavras.Count; j++)
      {
        var ininv = new ListaInvertida();
        ininv.IdsContas += id;
        ininv.PessoaCidade = listapalavras[j];
        ininv.TotalBytes = ininv.GetSomaBytes();
        for (int i = 0; i < tamanhopesquisa; i++)
        {
          if (listapesquisar[i].PessoaCidade == listapalavras[j])
          {
            listapesquisar[i].IdsContas += " " + ininv.IdsContas;
            break;
          }
          if (i == tamanhopesquisa - 1)
          {
            listapesquisar.Add(ininv);
            UpdateCabeca(file);
            break;
          }
        }
        if (tamanhopesquisa == 0)
        {
          listapesquisar.Add(ininv);
          UpdateCabeca(file);
        }
      }

      return listapesquisar;
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

    static void WriteListaInvertida(string file, List<ListaInvertida> listainserir)
    {
      var stream = new FileStream(file, FileMode.OpenOrCreate, FileAccess.ReadWrite);
      stream.Seek(4, SeekOrigin.Begin);

      for (int i = 0; i < listainserir.Count; i++)
      {
        var pessoaCidade = Encoding.Unicode.GetBytes(listainserir[i].PessoaCidade);
        var pessoaCidadeLength = (byte)pessoaCidade.Length;

        var idsContas = Encoding.Unicode.GetBytes(listainserir[i].IdsContas);
        var idsContasLength = (byte)idsContas.Length;

        var totalbytes = listainserir[i].GetSomaBytes();

        var totalBytesBytes = Utils.ReverseBytes(BitConverter.GetBytes(totalbytes));

        stream.Write(totalBytesBytes); // escreve os próximos 4 bytes do arquivo, correspondentes ao tamanho do registro

        stream.WriteByte(pessoaCidadeLength); // escreve o próximo byte do arquivo, correspondentes à quantidade de bytes da string
        stream.Write(pessoaCidade); // escreve os próximos 2x bytes do arquivo correspondentes ao nome da conta, onde x é a quantidade de caracteres da string

        stream.WriteByte(idsContasLength); // escreve o próximo byte do arquivo, correspondentes à quantidade de bytes da string
        stream.Write(idsContas);
      }
      stream.Close();
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////EXCLUIR/////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static void ExcluirId(uint id)
    {
      var indiceConta = ReadIdPesquisaBinaria(id);
      if (indiceConta != null)
      {
        var conta = Read(indiceConta.Posicao);
        MarcarExcluido(indiceConta.Posicao, filePath);
        MarcarExcluido(indiceConta.PosInd, filePath2);
        ExcluirListaInvertida(indiceConta.IdConta, conta.Cidade, filecidade);
        ExcluirListaInvertida(indiceConta.IdConta, conta.NomePessoa, filepessoa);
      }
    }

    static void ExcluirListaInvertida(uint id, string pessoacidade, string file)
    {
      // var listapessoa = ReadListaInvertida(filepessoa);
      // var listacidade = ReadListaInvertida(filecidade);
      // PesquisarPalavra();]
      var listapesquisar = ReadListaInvertida(file);
      var listapalavras = ExtrairPalavra(pessoacidade);

      for (int j = 0; j < listapalavras.Count; j++)
      {
        for (int i = 0; i < listapesquisar.Count; i++)
        {
          if (listapesquisar[i].PessoaCidade == listapalavras[j])
          {
            var idremovido = PesquisarIdExcluir(ExtrairIds(listapesquisar[i].IdsContas), id);
            if (idremovido.Count > 0)
            {
              listapesquisar[i].IdsContas = IdstoString(idremovido);
            }
            else
            {
              listapesquisar.RemoveAt(i);
            }
            // listapesquisar[i].IdsContas += " " + ininv.IdsContas;
            // break;
          }
        }
      }
      ResetarArquivo(listapesquisar, file);
    }

    static void ResetarArquivo(List<ListaInvertida> listatudo, string file)
    {
      // var quantregistro = QuantidadeRegistros(file) - 1;
      var quantregistro = (uint)listatudo.Count;
      LimpaArquivo(file);
      WriteCabeca(quantregistro, file);
      WriteListaInvertida(file, listatudo);
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
    static uint ReadCabeca(string file)
    {
      var ultimoId = new byte[4];

      var stream = new FileStream(file, FileMode.OpenOrCreate, FileAccess.ReadWrite);

      stream.Read(ultimoId, 0, ultimoId.Length);

      stream.Close();
      return BitConverter.ToUInt32(Utils.ReverseBytes(ultimoId));
    }
    static List<ListaInvertida> ReadListaInvertida(string file)
    {
      var listainv = new List<ListaInvertida>();

      var posicao = 4;

      var quantregistro = QuantidadeRegistros(file);

      var stream = new FileStream(file, FileMode.OpenOrCreate, FileAccess.ReadWrite);

      stream.Seek(posicao, SeekOrigin.Begin);

      for (int i = 0; i < quantregistro; i++)
      {
        var inv = new ListaInvertida();
        #region Arquivo

        var totalBytesBytes = new byte[4];

        stream.Read(totalBytesBytes, 0, totalBytesBytes.Length);

        totalBytesBytes = Utils.ReverseBytes(totalBytesBytes);

        var totalBytes = BitConverter.ToUInt32(totalBytesBytes);

        #endregion

        #region Pessoa cidade

        byte pessoaCidadeTamanho = (byte)stream.ReadByte();

        var pessoaCidadeBytes = new byte[pessoaCidadeTamanho];

        stream.Read(pessoaCidadeBytes, 0, pessoaCidadeBytes.Length);

        var pessoaCidade = Encoding.Unicode.GetString(pessoaCidadeBytes);

        #endregion

        #region Ids contas

        byte idContaTamanho = (byte)stream.ReadByte();

        var idContaBytes = new byte[idContaTamanho];

        stream.Read(idContaBytes, 0, idContaBytes.Length);

        var idConta = Encoding.Unicode.GetString(idContaBytes);

        #endregion

        inv.TotalBytes = totalBytes;
        inv.IdsContas = idConta;
        inv.PessoaCidade = pessoaCidade;

        listainv.Add(inv);
      }
      stream.Close();
      return listainv;
    }
    static Conta Read(long offset)
    {
      // ler cara conjunto de bytes de acordo com seu respectivo tipo e tamanho, para cada atributo da classe

      #region Arquivo

      var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

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

    static IndiceConta ReadIndice(long offset, SeekOrigin seekOrigin, string filename)
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

    static uint QuantidadeRegistros(string file)
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

    // static List<IndiceConta> TodosIndices()
    // {
    //   // ler cara conjunto de bytes de acordo com seu respectivo tipo e tamanho, para cada atributo da classe
    //   var listaIndiceConta = new List<IndiceConta>();
    //   uint posicao = 0;
    //   var indiceConta = ReadIndice(posicao, SeekOrigin.Begin, filePath2);
    //   // stream.Seek(0, SeekOrigin.Begin);
    //   while (indiceConta.IdConta != 0)
    //   {
    //     if (indiceConta.Lapide != '*')
    //     {
    //       indiceConta.PosInd = posicao;
    //       listaIndiceConta.Add(indiceConta);
    //     }
    //     posicao += indiceConta.TotalBytes;
    //     indiceConta = ReadIndice(posicao, SeekOrigin.Begin, filePath2);
    //   }


    //   return listaIndiceConta;
    // }

    // retorna uma tupla com uma conta de um id específico e seu offset no arquivo
    // public static Tuple<uint, Conta> ReadId(uint id)
    // {
    //   var position = (uint)4; // colocar na posição da primeira lápide 
    //   var conta = Read(position);

    //   while (conta.IdConta != 0) // ver se a posição existe
    //   {
    //     if (conta.Lapide == '\0') // verifica a lápide
    //     {
    //       if (conta.IdConta == id) // verifica se o id bate
    //       {
    //         return new Tuple<uint, Conta>(position, conta);
    //       }
    //     }

    //     position += conta.TotalBytes; // usa o tamanho junto ao offset atual pra ir para a posição do próximo registro

    //     conta = Read((long)position);
    //   }

    //   return new Tuple<uint, Conta>(position, conta);
    // }
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////// PESQUISAR ////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public static Conta? PesquisarConta(uint chave)
    {
      var resp = ReadIdPesquisaBinaria(chave);
      if (resp != null)
      {
        return Read(resp.Posicao);
      }
      return null;
    }
    static IndiceConta? ReadIdPesquisaBinaria(uint chave)
    {
      var temp = new IndiceConta();
      var tamanho = temp.GetSomaBytes();
      var cabeca = ReadCabeca(filePath2);
      int inf = 0;     // limite inferior (o primeiro índice de vetor em C é zero          )
      int sup = (int)cabeca - 1; // limite superior (termina em um número a menos. 0 a 9 são 10 números)
      int meio;
      while (inf <= sup)
      {
        meio = (inf + sup) / 2;
        var posicao = 4 + meio * tamanho;
        var indice = ReadIndice(posicao, SeekOrigin.Begin, filePath2);
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

    public static List<uint>? PesquisarPalavra(string palavra, string file)
    {
      var listapalavras = ExtrairPalavra(palavra);
      var listapesquisar = ReadListaInvertida(file);
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
        return ExtrairIds(listaids[0]);
      }
      return null;
    }

    static List<uint> PesquisarIdString(List<string> idsstring)
    {
      var ids = ExtrairIds(idsstring[0]);

      for (int i = 1; i < ids.Count; i++)
      {
        var idspesquisar = ExtrairIds(idsstring[i]);

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

    static List<uint> PesquisarIdExcluir(List<uint> idsatual, uint idspesquisar)
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
    static uint UpdateCabeca(string file)
    {
      var ultimoId = ReadCabeca(file);

      var newId = ultimoId + 1; // converte e incrementa

      WriteCabeca(newId, file);

      return newId;
    }

    static void UpdateInvertida(uint id, Conta conta, Conta contaModificada)
    {
      ExcluirListaInvertida(id, conta.Cidade, filecidade);
      ExcluirListaInvertida(id, conta.NomePessoa, filepessoa);
      WriteListaInvertidaCidade(contaModificada);
      WriteListaInvertidaPessoa(contaModificada);
    }

    public static void Update(uint id, Conta contaModificada)
    {
      var indiceConta = ReadIdPesquisaBinaria(id);
      if (indiceConta != null)
      {
        var conta = Read(indiceConta.Posicao);
        contaModificada.IdConta = id;
        contaModificada.TotalBytes = contaModificada.GetSomaBytes();

        if (contaModificada.TotalBytes > conta.TotalBytes)
        {
          // o registro modificado é maior que o registro antes da modificação
          // colocar o registro anterior como excluído e criar um novo

          MarcarExcluido(indiceConta.Posicao, filePath);
          // MarcarExcluido(indiceConta.PosInd, filePath2);
          UpdateInvertida(id, conta, contaModificada);
          var posicao = Write(0, contaModificada, SeekOrigin.End);
          indiceConta.Posicao = posicao;
          WriteIndice(indiceConta.PosInd, indiceConta, filePath2, SeekOrigin.Begin);
        }
        else
        {
          // o registro modificado é menor ou do mesmo tamanho que o registro antes da modificação
          // sobrescrever o registro antigo com o novo

          contaModificada.TotalBytes = conta.TotalBytes;
          UpdateInvertida(id, conta, contaModificada);
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

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////// EXTRAS ///////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    static void OrdenaIndice()
    {

    }


    static List<string> ExtrairPalavra(string palavras)
    {
      var listapalavras = new List<string>();
      var pala = "";
      var tamanho = palavras.Length;
      for (int i = 0; i < tamanho; i++)
      {
        if (palavras[i] != ' ')
        {
          pala += palavras[i];
        }
        else
        {
          listapalavras.Add(pala);
          pala = "";
        }
      }
      listapalavras.Add(pala);
      return listapalavras;
    }

    static List<uint> ExtrairIds(string palavras)
    {
      var listaid = new List<uint>();
      var num = "";
      var tamanho = palavras.Length;
      if (palavras != "")
      {
        for (int i = 0; i < tamanho; i++)
        {
          if (palavras[i] != ' ')
          {
            num += palavras[i];
          }
          else
          {
            listaid.Add((uint)Int32.Parse(num));
            num = "";
          }
        }
        listaid.Add((uint)Int32.Parse(num));
        listaid.Sort();
      }
      return listaid;
    }

    static String IdstoString(List<uint> ids)
    {
      ids.Sort();
      var listaid = "";
      listaid += ids[0];

      if (ids.Count > 1)
      {
        for (int i = 0; i < ids.Count; i++)
        {
          listaid += " " + ids[i];
        }
      }

      return listaid;
    }


    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
  }
}
