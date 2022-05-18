using System.Text;

namespace Aeds3TP1
{
  class Program
  {
    // caminho do arquivo de dados
    static string filePath = "data.dat";
    static string filePath2 = "index.dat";
    static string filePath3 = "listainvertida.dat";

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
      Console.WriteLine("=== Conta");

      var conta = new Conta
      {
        Cidade = "alagoas",
        Cpf = "123123123",
        IdConta = 0,
        Lapide = '\0',
        NomePessoa = "joao",
        SaldoConta = 1000,
        TotalBytes = 0,
        TransferenciasRealizadas = 0,
      };

      Console.WriteLine(conta);

      Write(conta); // teste de escrita

      Console.WriteLine("=== Obj");

      var obj = ReadId(1); // teste de leitura

      Console.WriteLine(obj);

      var conta2 = new Conta
      {
        Cidade = "sergipe",
        Cpf = "890890890",
        IdConta = 3,
        Lapide = '\0',
        NomePessoa = "marcelo",
        SaldoConta = 4000,
        TotalBytes = 0,
        TransferenciasRealizadas = 0,
      };

      Update(1, conta2); // teste de atualização

      Console.WriteLine("=== Obj2");

      var obj2 = ReadId(1); // teste de leitura

      Console.WriteLine(obj2);
    }

    // incrementa o último id no início do arquivo e retorna o novo id
    static uint UpdateCabeca()
    {
      var ultimoId = new byte[4];

      var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

      stream.Read(ultimoId, 0, ultimoId.Length);

      stream.Position = 0;

      var newId = BitConverter.ToUInt32(Utils.ReverseBytes(ultimoId)) + 1; // converte e incrementa

      var newIdBytes = Utils.ReverseBytes(BitConverter.GetBytes(newId));

      stream.Write(newIdBytes); // escreve o id incrementado

      stream.Close();

      return newId;
    }

    // marca um registro como excluído a partir de um id
    public static void ExcluirId(uint id)
    {
      uint posicao = ReadId(id).Item1;

      MarcarExcluido(posicao);
    }

    // marca o registro como excluído, mudando o byte da lápide a partir de um offset específico
    static void MarcarExcluido(uint posicao)
    {
      var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

      stream.Seek(posicao, SeekOrigin.Begin); // ir para a posição da lápide

      stream.WriteByte((byte)'*'); // marcar o registro como excluído

      stream.Close();
    }

    // retorna uma conta a partir de um offset e sua origem, lendo o registro a partir daquele offset no arquivo
    static List<ListaInvertida> ReadListaInvertida()//jao continuar aqui
    {
      #region Arquivo

      var stream = new FileStream(filePath3, FileMode.OpenOrCreate, FileAccess.ReadWrite);

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

    }
    static Conta Read(long offset, SeekOrigin seekOrigin)
    {
      // ler cara conjunto de bytes de acordo com seu respectivo tipo e tamanho, para cada atributo da classe

      #region Arquivo

      var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

      stream.Seek(offset, seekOrigin);

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
    // static void TransfereIndice()//lembrar de deletar o que ja estiver escrito no arquivo antes
    // {
    //   var position = (uint)4; // colocar na posição da primeira lápide 
    //   var conta = Read(position, SeekOrigin.Begin);
    //   var listaindiceContas = new List<IndiceConta>();
    //   var indiceContas = new IndiceConta();
    //   var posicao = new List<long>();

    //   while (conta.IdConta != 0) // ver se a posição existe
    //   {
    //     if (conta.Lapide == '\0')
    //     {
    //       indiceContas.Add() =
    //       indiceContas.Id(conta.IdConta);
    //       posicao.Add(position);
    //     }
    //     position += conta.TotalBytes; // usa o tamanho junto ao offset atual pra ir para a posição do próximo registro
    //     conta = Read((long)position, SeekOrigin.Begin);

    //   }
    //   WriteIndice(id, posicao, SeekOrigin.Begin);
    // }
    static IndiceConta ReadIndice(long offset, SeekOrigin seekOrigin, string filename)
    {
      // ler cara conjunto de bytes de acordo com seu respectivo tipo e tamanho, para cada atributo da classe
      var indiceConta = new IndiceConta();
      var stream = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite);
      stream.Seek(offset, seekOrigin);
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

      indiceConta.Lapide = lapide;
      indiceConta.TotalBytes = (byte)totalBytesBytes;


      stream.Close();

      return indiceConta;
    }

    static List<IndiceConta> TodosIndices()
    {
      // ler cara conjunto de bytes de acordo com seu respectivo tipo e tamanho, para cada atributo da classe
      var listaIndiceConta = new List<IndiceConta>();
      var posicao = 0;
      var indiceConta = ReadIndice(posicao, SeekOrigin.Begin, filePath2);
      // stream.Seek(0, SeekOrigin.Begin);

      while (indiceConta.IdConta != 0)
      {
        if (indiceConta.Lapide != '*')
        {
          listaIndiceConta.Add(indiceConta);
        }
        posicao += indiceConta.TotalBytes;
        indiceConta = ReadIndice(posicao, SeekOrigin.Begin, filePath2);
      }


      return listaIndiceConta;
    }

    // retorna uma tupla com uma conta de um id específico e seu offset no arquivo
    public static Tuple<uint, Conta> ReadId(uint id)
    {
      var position = (uint)4; // colocar na posição da primeira lápide 
      var conta = Read(position, SeekOrigin.Begin);

      while (conta.IdConta != 0) // ver se a posição existe
      {
        if (conta.Lapide == '\0') // verifica a lápide
        {
          if (conta.IdConta == id) // verifica se o id bate
          {
            return new Tuple<uint, Conta>(position, conta);
          }
        }

        position += conta.TotalBytes; // usa o tamanho junto ao offset atual pra ir para a posição do próximo registro

        conta = Read((long)position, SeekOrigin.Begin);
      }

      return new Tuple<uint, Conta>(position, conta);
    }

    static void ListaInvertida()
    {
      var position = (uint)4; // colocar na posição da primeira lápide 
      var conta = Read(position, SeekOrigin.Begin);
      var listaIndiceConta = new List<IndiceConta>();
      while (conta.IdConta != 0) // ver se a posição existe
      {
        if (conta.Lapide == '\0') // verifica a lápide
        {

        }
      }
    }
    static void Ordena(string arq1, string arq2, int contador)
    {
      var arqname = "arq4";
      var arqname2 = "arq3";
      LimpaArquivo(arqname);
      LimpaArquivo(arqname2);
      var position1 = 0;
      var position2 = 0;
      var trocar = false;
      var conta1 = ReadIndice(position1, SeekOrigin.Begin, arq1);
      var conta2 = ReadIndice(position2, SeekOrigin.Begin, arq2);
      var salvaid = conta1.IdConta;
      var salvaid2 = conta2.IdConta;

      for (int i = 0; i < contador; i++)
      {
        if (trocar == false)
        {
          if (salvaid > conta1.IdConta)
          {

          }
          switch (ComparaConta(conta1, conta2, arqname))
          {
            case -1:
              position2 += conta2.TotalBytes;
              conta2 = ReadIndice(position2, SeekOrigin.Begin, arq2);
              break;
            case 0:
              position1 += conta1.TotalBytes;
              conta1 = ReadIndice(position1, SeekOrigin.Begin, arq1);
              break;
            case 1:

              break;
            case 2:
              WriteIndice(conta1, arqname);
              break;
            default:
              break;
          }

          switch (ComparaConta(conta1, conta2, arqname2))
          {
            case -1:
              position2 += conta2.TotalBytes;
              conta2 = ReadIndice(position2, SeekOrigin.Begin, arq2);
              break;
            case 0:
              position1 += conta1.TotalBytes;
              conta1 = ReadIndice(position1, SeekOrigin.Begin, arq1);
              break;
            case 1:

              break;
            case 2:
              WriteIndice(conta1, arqname2);
              break;
            default:
              break;
          }
        }

      }
    }

    static int ComparaConta(IndiceConta conta1, IndiceConta conta2, string arqname)
    {
      // 0 = sem problema, o id da conta 1 é 0, o ida da conta 2 é 0
      if (conta1.IdConta != 0)
      {
        if (conta2.IdConta != 0)
        {
          if (conta1.IdConta < conta2.IdConta)
          {
            WriteIndice(conta1, arqname);
            return 0;
          }
          else
          {
            WriteIndice(conta2, arqname);
            return -1;
          }
        }
        return 2;

      }

      return 1;
    }

    static void OrdenaIndice()
    {
      var position = 0;
      var cloneposition = 0;
      var contador = 0;
      var conta1 = ReadIndice(position, SeekOrigin.Begin, filePath2);
      IndiceConta conta2 = new IndiceConta();

      bool arq = false;
      var stream1 = "arq1.dat";
      var stream2 = "arq2.dat";
      LimpaArquivo(stream1);
      LimpaArquivo(stream2);

      while (conta1.IdConta != 0) // ver se a posição existe
      {
        if (conta1.Lapide == '\0')
        {
          cloneposition = position + conta1.TotalBytes;
          conta2 = ReadIndice(cloneposition, SeekOrigin.Begin, filePath2);
          if (conta2.IdConta == 0)
          {
            if (!arq)
            {
              WriteIndice(conta1, stream1);
            }
            else
            {
              WriteIndice(conta1, stream2);
            }
            contador++;
          }
          while (conta2.IdConta != 0)
          { // ver se a posição existe
            if (conta2.Lapide == '\0')
            {
              if (conta1.IdConta < conta2.IdConta)
              {
                if (!arq)
                {
                  WriteIndice(conta1, stream1);
                  WriteIndice(conta2, stream1);
                  arq = !arq;
                }
                else
                {
                  WriteIndice(conta1, stream2);
                  WriteIndice(conta2, stream2);
                  arq = !arq;
                }
              }
              else
              {
                if (!arq)
                {
                  WriteIndice(conta2, stream1);
                  WriteIndice(conta1, stream1);
                  arq = !arq;
                }
                else
                {
                  WriteIndice(conta2, stream2);
                  WriteIndice(conta1, stream2);
                  arq = !arq;
                }
              }
              position = cloneposition + conta2.TotalBytes;
              contador += 2;
              if (contador == 10)
              {
                goto Finaliza;
              }
              goto Pular;
            }
            cloneposition += conta2.TotalBytes;

            conta2 = ReadIndice((long)cloneposition, SeekOrigin.Begin, filePath2);
          }
        }

        position += conta1.TotalBytes; // usa o tamanho junto ao offset atual pra ir para a posição do próximo registro
      Pular:
        conta1 = ReadIndice((long)position, SeekOrigin.Begin, filePath2);
      }
    Finaliza:
      Ordena(stream1, stream2, contador);
    }

    static void CriarIndice(uint id)
    {
      var obj = ReadId(id);
      var indeceConta = new IndiceConta();
      indeceConta.Lapide = obj.Item2.Lapide;
      indeceConta.TotalBytes = (byte)indeceConta.GetSomaBytes();
      indeceConta.IdConta = id;
      indeceConta.Posicao = obj.Item1;

      WriteIndice(indeceConta, filePath2);
      OrdenaIndice();
    }
    static void WriteIndice(IndiceConta indiceConta, string file)
    {
      var stream = new FileStream(file, FileMode.OpenOrCreate, FileAccess.ReadWrite);
      stream.Seek(0, SeekOrigin.End);
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

    static void LimpaArquivo(string file)
    {
      var stream = new FileStream(file, FileMode.Create, FileAccess.ReadWrite);
      stream.Close();
    }

    public static void Write(Conta conta)
    {
      var id = UpdateCabeca();
      Write(id, 0, conta, SeekOrigin.End);
      CriarIndice(id);
    }

    // escreve um novo registro a partir de um id, a conta, o offset desejado e sua origem
    static void Write(uint id, long posicao, Conta conta, SeekOrigin seekOrigin)
    {
      var indeceConta = new IndiceConta();
      var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

      stream.Seek(posicao, seekOrigin);

      // pegar cada conjunto de bytes de acordo com seu tipo e tamanho

      var idConta = Utils.ReverseBytes(BitConverter.GetBytes(id));

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
    }
    static void WriteListaInvertida(uint id, String palavra)
    {

    }
    // atualiza um registro de um certo id com os novos dados do registro modificado
    public static void Update(uint id, Conta contaModificada)
    {
      var obj = ReadId(id);
      var posicao = obj.Item1;
      var conta = obj.Item2;

      contaModificada.TotalBytes = contaModificada.GetSomaBytes();

      if (contaModificada.TotalBytes > conta.TotalBytes)
      {
        // o registro modificado é maior que o registro antes da modificação
        // colocar o registro anterior como excluído e criar um novo

        MarcarExcluido(posicao);

        Write(id, 0, contaModificada, SeekOrigin.End);
      }
      else
      {
        // o registro modificado é menor ou do mesmo tamanho que o registro antes da modificação
        // sobrescrever o registro antigo com o novo

        contaModificada.TotalBytes = conta.TotalBytes;

        Write(id, posicao, contaModificada, SeekOrigin.Begin);
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
