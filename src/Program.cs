using System.Text;

namespace Aeds3TP1
{
  class Program
  {
    // caminho do arquivo de dados
    public static string caminhoAquivo = "res/";
    public static string principalArquivo = "data";
    public static string tipoArquivo = ".dat";
    public static string filePath = "res/data.dat";
    public static string indexPath = "res/index.dat";
    public static string filePessoa = "res/listainvertidapessoa.dat";
    public static string fileCidade = "res/listainvertidacidade.dat";
    public static string fileCompactar = "res/dicionario.dat";
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
      // InsertTest();
      Test();
      //TestOrdem();
#else
      Menu.Principal();
#endif
    }

    // Teste de inserção de valores
    static void InsertTest()
    {
      LimpaArquivo(filePath);
      LimpaArquivo(indexPath);
      LimpaArquivo(fileCidade);
      LimpaArquivo(filePessoa);
      // LimpaArquivo(fileCompactar);
      Write(new Conta
      {
        Cidade = "sergipe",
        Cpf = "190890890",
        IdConta = 32,
        Lapide = '\0',
        NomePessoa = "leandro",
        SaldoConta = 4000,
        TotalBytes = 0,
        TransferenciasRealizadas = 0,
      });
      Write(new Conta
      {
        Cidade = "belo horizonte",
        Cpf = "290890890",
        IdConta = 64,
        Lapide = '\0',
        NomePessoa = "tiago pedro",
        SaldoConta = 4000,
        TotalBytes = 0,
        TransferenciasRealizadas = 0,
      });
      Write(new Conta
      {
        Cidade = "sergipe",
        Cpf = "390890890",
        IdConta = 68,
        Lapide = '\0',
        NomePessoa = "pedro leandro",
        SaldoConta = 4000,
        TotalBytes = 0,
        TransferenciasRealizadas = 0,
      });

      "".ToString();

      Write(new Conta
      {
        Cidade = "joao pessoa",
        Cpf = "490890890",
        IdConta = 9,
        Lapide = '\0',
        NomePessoa = "marcelinho jose",
        SaldoConta = 4000,
        TotalBytes = 0,
        TransferenciasRealizadas = 0,
      });
      Write(new Conta
      {
        Cidade = "maranhao",
        Cpf = "590890890",
        IdConta = 2,
        Lapide = '\0',
        NomePessoa = "jose carvalho",
        SaldoConta = 4000,
        TotalBytes = 0,
        TransferenciasRealizadas = 0,
      });
      Write(new Conta
      {
        Cidade = "sergipe",
        Cpf = "690890890",
        IdConta = 3,
        Lapide = '\0',
        NomePessoa = "lucas jose",
        SaldoConta = 4000,
        TotalBytes = 0,
        TransferenciasRealizadas = 0,
      });
    }

    // método de teste para testar o funcionamento das operações de forma mais isolada


    static void Test()
    {
      // InsertTest();
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
      Write(new Conta
      {
        Cidade = "sergipe",
        Cpf = "690890890",
        IdConta = 3,
        Lapide = '\0',
        NomePessoa = "lucas jose",
        SaldoConta = 4000,
        TotalBytes = 0,
        TransferenciasRealizadas = 0,
      });
      Console.WriteLine(conta);

      Write(conta); // teste de escrita
      Write(conta); // teste de escrita

      Console.WriteLine("=== Obj");
      var obj = Conta.PesquisarConta(1);
      Console.WriteLine(obj);

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
      var obj2 = Conta.PesquisarConta(1);
      Console.WriteLine(obj2);

      var resposta = PesquisarCidade("sergipe");
      Console.WriteLine(resposta);

      resposta = PesquisarPessoa("joao");
      Console.WriteLine(resposta);
      resposta = PesquisarPessoa("joaa");
      Console.WriteLine(resposta);

      var resposta1 = IndiceConta.ReadIdPesquisaBinaria((uint)1);
      Console.WriteLine(resposta1);

      resposta1 = IndiceConta.ReadIdPesquisaBinaria((uint)2);
      Console.WriteLine(resposta1);

      var obj3 = Conta.PesquisarConta(4);
      Console.WriteLine(obj3);

      ExcluirId(1);
      Compactar compactar = new Compactar();
      // compactar.CompactarLZW("ABRA CADABRA");
      // compactar.CompactarLZW("AB");
      // var lista = new List<int> { 77, 74, 72 };
      // compactar.DesCompactarVersao(4, lista);
      // DescompactarContas();
      CompactarContas();
      DescompactarContas("res/dataCompressao1.dat");
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

    //Cria uma nova conta, juntamente com a seu indice e lista invertida de pessoa/cidade; 
    public static void Write(Conta conta)
    {
      var id = UpdateCabeca(filePath);

      conta.IdConta = id;

      var posicao = Write(0, conta, SeekOrigin.End, filePath);

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

    //Escreve na posicao inicial do arquivo um uint passado como parametro
    static void WriteCabeca(uint cabeca, string file)
    {
      var stream = new FileStream(file, FileMode.OpenOrCreate, FileAccess.ReadWrite);

      var newBytes = Utils.ReverseBytes(BitConverter.GetBytes(cabeca));

      stream.Write(newBytes); // escreve o id incrementado

      stream.Close();
    }

    // Escreve uma nova conta no arquivo com o offset desejado e sua origem
    static long Write(long posicao, Conta conta, SeekOrigin seekOrigin, string file)
    {
      var stream = new FileStream(file, FileMode.OpenOrCreate, FileAccess.ReadWrite);

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
    //Exclui tudo relacionado a um id, tanto na lista invertida, como no indice e no arquivo principal data.dat
    public static void ExcluirId(uint id)
    {
      var indiceConta = IndiceConta.ReadIdPesquisaBinaria(id);

      if (indiceConta != null)
      {
        var conta = Conta.Read(indiceConta.Posicao, filePath);

        MarcarExcluido(indiceConta.Posicao, filePath);
        MarcarExcluido(indiceConta.PosInd, indexPath);

        ListaInvertida.ExcluirListaInvertida(indiceConta.IdConta, conta.Cidade, fileCidade);
        ListaInvertida.ExcluirListaInvertida(indiceConta.IdConta, conta.NomePessoa, filePessoa);
      }
    }

    //Limpa um arquivo, escreve uma lista de ListaInvertidas(com pessoa ou cidade) e atualiza a 
    //quantidade de registros no arquivo, passado como parametro
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

    //Cria um arquivo novo, por cima do arquivo passado como parametro, limpado o arquivo
    static void LimpaArquivo(string file)
    {
      var stream = new FileStream(file, FileMode.Create, FileAccess.ReadWrite);

      stream.Close();
    }
    static void LimpaTudo()
    {
      LimpaArquivo(filePath);
      LimpaArquivo(indexPath);
      LimpaArquivo(fileCidade);
      LimpaArquivo(filePessoa);
    }


    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////READ////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    //Leitura do uint cabeça, de um arquivo passado como parametro
    public static uint ReadCabeca(string file)
    {
      var cabeca = new byte[4];

      var stream = new FileStream(file, FileMode.OpenOrCreate, FileAccess.ReadWrite);

      stream.Read(cabeca, 0, cabeca.Length);

      stream.Close();

      return BitConverter.ToUInt32(Utils.ReverseBytes(cabeca));
    }

    static List<Conta> ReadTodasContas(string file)
    {
      List<Conta> contas = new List<Conta>() { };
      long posicao = 4;
      var conta = Conta.Read(posicao, file);
      while (conta.IdConta != 0)
      {
        if (conta.Lapide != '*')
        {
          contas.Add(conta);
        }
        posicao += conta.TotalBytes;
        conta = Conta.Read(posicao, file);
      }

      return contas;
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////// PESQUISAR ////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Retorna as contas com a cidade passada de parametro
    public static List<Conta>? PesquisarCidade(string palavra)
    {
      var ids = PesquisarPalavra(palavra, fileCidade);
      if (ids != null)
      {
        return PesquisarPessoaCidade(ids);
      }
      return null;
    }

    //Retorna as contas com a pessoa passada de parametro
    public static List<Conta>? PesquisarPessoa(string palavra)
    {

      var ids = PesquisarPalavra(palavra, filePessoa);
      if (ids != null)
      {
        return PesquisarPessoaCidade(ids);
      }
      return null;
    }

    //Retorna a lista com de todos os ids passados como parametros
    public static List<Conta> PesquisarPessoaCidade(List<uint> ids)
    {
      var contas = new List<Conta>();

      for (int i = 0; i < ids.Count; i++)
      {
        contas.Add(Conta.PesquisarConta(ids[i]));
      }

      return contas;
    }

    //Pesquisa uma string(nome ou cidade) em um arquivo especifico
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
      if (listaids.Count > 1)//È utilizado quando tiver 2 ou mais listas de ids
      {
        return PesquisarIdString(listaids);
      }
      if (listaids.Count != 0)//È utilizado somente quando so tiver sido encontrado uma palavra no arquivo
      {
        return Utils.ExtrairIds(listaids[0]);
      }

      return null;//se não houver nenhuma palavra retorna null
    }

    //Retorna a intersecao de todos os ids, nas strings passadas como parametro
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
    //Retorna a intersecao de todos os ids, nas duas listas uint passadas como parametro
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

    //Retorna a lista passada como parametro, mas sem o idspesquisar
    //parametro( {1,2,3,4,5}, 3) = return ( {1,2,4,5} )
    public static List<uint> PesquisarIdExcluir(List<uint> idsatual, uint idspesquisar)
    {
      var a = PesquisaBinaria(idsatual, idspesquisar);
      if (a != -1)
      {
        idsatual.RemoveAt(a);
      }

      return idsatual;
    }
    //Retorna a posicao do idspesquisar na lista
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

    // Incrementa o último id no início do arquivo e retorna o novo id
    public static uint UpdateCabeca(string file)
    {
      var ultimoId = ReadCabeca(file);

      var newId = ultimoId + 1; // converte e incrementa

      WriteCabeca(newId, file);

      return newId;
    }
    //Update na conta, no indice e nas listas invertidas
    public static void Update(uint id, Conta contaModificada)
    {
      var indiceConta = IndiceConta.ReadIdPesquisaBinaria(id);
      if (indiceConta != null)
      {
        var conta = Conta.Read(indiceConta.Posicao, filePath);
        contaModificada.IdConta = id;
        contaModificada.TotalBytes = contaModificada.GetSomaBytes();

        if (contaModificada.TotalBytes > conta.TotalBytes)
        {
          // o registro modificado é maior que o registro antes da modificação
          // colocar o registro anterior como excluído e criar um novo

          MarcarExcluido(indiceConta.Posicao, filePath);
          ListaInvertida.UpdateInvertida(id, conta, contaModificada);
          var posicao = Write(0, contaModificada, SeekOrigin.End, filePath);
          indiceConta.Posicao = posicao;
          //Muda somente a posição nova do arquivo
          IndiceConta.WriteIndice(indiceConta.PosInd, indiceConta, indexPath, SeekOrigin.Begin);
        }
        else
        {
          // o registro modificado é menor ou do mesmo tamanho que o registro antes da modificação
          // sobrescrever o registro antigo com o novo e da update nas listas invertidas

          contaModificada.TotalBytes = conta.TotalBytes;
          ListaInvertida.UpdateInvertida(id, conta, contaModificada);
          Write(indiceConta.Posicao, contaModificada, SeekOrigin.Begin, filePath);
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
    public static void CompactarContas()
    {
      CompactarContas(ReadTodasContas(filePath), "data", ReadCabeca(filePath), filePath);
    }
    static void CompactarContas(List<Conta> contas, string purefile, uint cabeca, string file)
    {
      Compactar compactar = new Compactar();
      var aquivo = caminhoAquivo + purefile + "Compressao" + compactar.Versao + tipoArquivo;
      LimpaArquivo(aquivo);
      WriteCabeca(cabeca, aquivo);
      for (int i = 0; i < contas.Count; i++)
      {
        contas[i].Cidade = compactar.CompactarLZW(contas[i].Cidade);
        contas[i].NomePessoa = compactar.CompactarLZW(contas[i].NomePessoa);
        contas[i].Cpf = compactar.CompactarLZW(contas[i].Cpf);
        contas[i].TotalBytes = contas[i].GetSomaBytes();
        Program.Write(0, contas[i], SeekOrigin.End, aquivo);
      }
      compactar.WriteDicionarioAtual();
    }
    static uint ExtrairNum(string nome)
    {
      var num = new string(nome.Where(char.IsDigit).ToArray());
      return Convert.ToUInt32(num);
    }
    static void DescompactarContas(string file)
    {
      var versao = ExtrairNum(file);
      var contas = ReadTodasContas(file);
      Compactar compactar = new Compactar();
      compactar.UpdateVersao(versao);
      // var lista = new List<int> { 0, 1, 17, 0, 26, 2, 0, 3, 63, 65 }; versao 1
      // var lista = new List<int> { 71, 66, 68, 70, 64, 0 }; versao 2
      // var a = compactar.DesCompactarLZW(lista);
      for (int i = 0; i < contas.Count; i++)
      {
        contas[i].Cidade = compactar.DesCompactarLZW(Utils.ExtrairNumeros(contas[i].Cidade));
        contas[i].NomePessoa = compactar.DesCompactarLZW(Utils.ExtrairNumeros(contas[i].NomePessoa));
        contas[i].Cpf = compactar.DesCompactarLZW(Utils.ExtrairNumeros(contas[i].Cpf));
        contas[i].TotalBytes = contas[i].GetSomaBytes();
      }
      LimpaTudo();
      for (int i = 0; i < contas.Count; i++)
      {
        Write(contas[i]);
      }
    }
  }
}
