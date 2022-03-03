using System.Text;

namespace Aeds3TP1
{
  class Program
  {
    static string filePath = "data.dat";

    static void MainUpdate()
    {
      Console.WriteLine("Digite o ID da conta a ser alterada:");

#if DEBUG

      var idInput = "1";

#else

      var idInput = Console.ReadLine();

#endif


      if (idInput == null)
      {
        Console.WriteLine("Id inválido.");

        return;
      }

      var id = uint.Parse(idInput);

      var res = ReadId(id);

      var posicao = res.Item1;
      var conta = res.Item2;

      while (true)
      {
        Console.WriteLine("Digite qual atributo você deseja alterar:");
        Console.WriteLine("1 - Nome, 2 - CPF, 3 - Cidade, 4 - Saldo, 5 - Sair");

#if DEBUG

        var respostaInput = "1";

#else

      var respostaInput = Console.ReadLine();

#endif


        if (respostaInput == null)
        {
          Console.WriteLine("Valor inválido.");

          return;
        }

        var resposta = short.Parse(respostaInput);

        switch (resposta)
        {
          case 1:
            Console.WriteLine("Digite o novo nome:");

            var nome = /* Console.ReadLine() */ "caio";

            if (nome != null)
            {
              conta.NomePessoa = nome;
            }

#if DEBUG

            return;

#else

            break;

#endif

          case 2:
            Console.WriteLine("Digite o novo CPF:");

            var cpf = /* Console.ReadLine() */ "123123123";

            if (cpf != null)
            {
              conta.Cpf = cpf;
            }

            break;

          case 3:
            Console.WriteLine("Digite a nova cidade:");

            var cidade = /* Console.ReadLine() */ "São Paulo";

            if (cidade != null)
            {
              conta.Cidade = cidade;
            }

            break;

          case 4:
            Console.WriteLine("Digite o novo saldo:");

            var saldoConta = /* Console.ReadLine() */ 2000f;

            // if (saldoConta != null)
            // {
            conta.SaldoConta = saldoConta;
            // }

            break;

          case 5:
            Update(id, conta);

            return;

          default:
            Console.WriteLine("Digite um numero válido.");

            break;
        }
      }
    }

    static void MainCreate()
    {
      var conta = new Conta
      {
        Lapide = '\0',
        TotalBytes = 0,
        IdConta = 0,
        NomePessoa = "",
        Cpf = "",
        Cidade = "",
        TransferenciasRealizadas = 0,
        SaldoConta = 1000,
      };

      // nomePessoa, cpf, estado

      while (true)
      {
        try
        {
          Console.WriteLine("Digite o nome:");

#if DEBUG
          var nome = "marcelo";
#else
          var nome = Console.ReadLine();
#endif

          if (nome == null)
          {
            throw new Exception();
          }

          conta.NomePessoa = nome;

          break;
        }
        catch (Exception)
        {
          Console.WriteLine("Nome inválido.");
        }
      }

      while (true)
      {
        try
        {
          Console.WriteLine("Digite o CPF:");

#if DEBUG
          var cpf = "098098098";
#else
          var cpf = Console.ReadLine();
#endif

          if (cpf == null)
          {
            throw new Exception();
          }

          conta.Cpf = cpf;

          break;
        }
        catch (Exception)
        {
          Console.WriteLine("CPF inválido.");
        }
      }

      while (true)
      {
        try
        {
          Console.WriteLine("Digite a Cidade:");

#if DEBUG
          var cidade = "maringá";
#else
          var cidade = Console.ReadLine();
#endif

          if (cidade == null)
          {
            throw new Exception();
          }

          conta.Cidade = cidade;

          break;
        }
        catch (Exception)
        {
          Console.WriteLine("Cidade inválida.");
        }
      }

      WriteUsuario(conta);
    }

    static void Main(string[] args)
    {
      // var conta = new Conta
      // {
      //   Lapide = '\0',
      //   TotalBytes = 0,
      //   IdConta = 0,
      //   NomePessoa = "joao",
      //   Cpf = "123456789",
      //   Cidade = "belo horizonte",
      //   TransferenciasRealizadas = 10,
      //   SaldoConta = 1000,
      // };

      // var conta2 = new Conta
      // {
      //   Lapide = '\0',
      //   TotalBytes = 0,
      //   IdConta = 0,
      //   NomePessoa = "lucas",
      //   Cpf = "10987654321",
      //   Cidade = "alagoas",
      //   TransferenciasRealizadas = 0,
      //   SaldoConta = 1000,
      // };

      // Console.WriteLine(conta);

      // Criar conta(Write): dados a serem digitados(nomePessoa, cpf, estado)
      // WriteUsuario(conta);
      // WriteUsuario(conta2);

      // Console.WriteLine(ReadId(2).Item2);
      // Console.WriteLine(ReadId(3).Item2);

      MainCreate();

      // MainUpdate();
    }

    static byte[] ReverseBytes(byte[] a)
    {
      Array.Reverse(a, 0, a.Length);

      return a;
    }

    static Conta Read(long ler, SeekOrigin seekOrigin)
    {
      #region Arquivo

      // var ultimoIdBytes = new byte[4];

      var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

      // stream.Read(ultimoIdBytes, 0, ultimoIdBytes.Length);

      stream.Seek(ler, seekOrigin);

      var lapide = (char)stream.ReadByte();

      var totalBytesBytes = new byte[4];

      stream.Read(totalBytesBytes, 0, totalBytesBytes.Length);

      totalBytesBytes = ReverseBytes(totalBytesBytes);

      var totalBytes = BitConverter.ToUInt32(totalBytesBytes);

      #endregion

      #region Id conta

      var idContaBytes = new byte[4];

      stream.Read(idContaBytes, 0, idContaBytes.Length);

      idContaBytes = ReverseBytes(idContaBytes);

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

      #region Transferencias Realizadas

      var transferenciasRealizadasBytes = new byte[2];

      stream.Read(transferenciasRealizadasBytes, 0, transferenciasRealizadasBytes.Length);

      transferenciasRealizadasBytes = ReverseBytes(transferenciasRealizadasBytes);

      var transferenciasRealizadas = BitConverter.ToUInt16(transferenciasRealizadasBytes);

      #endregion

      #region Saldo Conta

      var saldoContaBytes = new byte[4];

      stream.Read(saldoContaBytes, 0, saldoContaBytes.Length);

      saldoContaBytes = ReverseBytes(saldoContaBytes);

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

    static Tuple<uint, Conta> ReadId(uint id) // consulta do usuario a um id especifico
    {
      var position = (uint)4; // colocar na posicao da primeira lapide 
      var conta = Read(position, SeekOrigin.Begin);

      while (conta.IdConta != 0) // ver se a posicao existe
      {
        if (conta.Lapide == '\0') //verifica a lapide
        {
          if (conta.IdConta == id)
          {
            return new Tuple<uint, Conta>(position, conta);
          }
        }

        position += conta.TotalBytes; // usa o pular pra ir pra posicao prox posicao

        conta = Read((long)position, SeekOrigin.Begin);
      }

      return new Tuple<uint, Conta>(position, conta);
    }

    static void WriteUsuario(Conta conta)
    {
      Write(UpdateCabeca(), 0, conta, SeekOrigin.End);
    }

    static void WritePrograma(uint id, Conta conta, uint posicao, SeekOrigin seekOrigin)
    {
      Write(id, posicao, conta, seekOrigin);
    }

    static uint SomaBytes(Conta conta)
    {
      var totalbytes = BitConverter.GetBytes(conta.IdConta).Length +
        Encoding.Unicode.GetBytes(conta.NomePessoa).Length +
        Encoding.Unicode.GetBytes(conta.Cpf).Length +
        Encoding.Unicode.GetBytes(conta.Cidade).Length +
        BitConverter.GetBytes(conta.TransferenciasRealizadas).Length +
        BitConverter.GetBytes(conta.SaldoConta).Length + 8;

      return BitConverter.ToUInt32(BitConverter.GetBytes(totalbytes));
    }

    static void Write(uint id, long posicao, Conta conta, SeekOrigin seekOrigin)
    {
      var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

      stream.Seek(posicao, seekOrigin);

      var idConta = ReverseBytes(BitConverter.GetBytes(id));

      var nomePessoa = Encoding.Unicode.GetBytes(conta.NomePessoa);
      var nomePessoaLength = (byte)nomePessoa.Length;

      var cpf = Encoding.Unicode.GetBytes(conta.Cpf);
      var cpfLength = (byte)cpf.Length;

      var cidade = Encoding.Unicode.GetBytes(conta.Cidade);
      var cidadeLength = (byte)cidade.Length;

      var transferenciasRealizadas = ReverseBytes(BitConverter.GetBytes(conta.TransferenciasRealizadas));

      var saldoConta = ReverseBytes(BitConverter.GetBytes(conta.SaldoConta));

      var totalbytes = idConta.Length + nomePessoa.Length + 1 + cpf.Length + 1 + cidade.Length + 1 + transferenciasRealizadas.Length + saldoConta.Length + 5;

      var totalBytesBytes = ReverseBytes(BitConverter.GetBytes(totalbytes));

      // stream.Write(idConta); // escreve os primeiros 4 bytes do arquivo, correspondentes ao último id

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

    static uint UpdateCabeca()
    {
      var ultimoId = new byte[4];

      var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

      stream.Read(ultimoId, 0, ultimoId.Length);

      stream.Position = 0;

      var newId = BitConverter.ToUInt32(ReverseBytes(ultimoId)) + 1;

      var newIdBytes = ReverseBytes(BitConverter.GetBytes(newId));

      stream.Write(newIdBytes);

      stream.Close();

      return newId;
    }

    static void Lapide(uint posicao)
    {
      var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

      stream.Seek(posicao, SeekOrigin.Begin);

      stream.WriteByte((byte)'*');

      stream.Close();
    }

    static void Update(uint id, Conta contaUsuario)
    {
      var obj = ReadId(id);
      var posicao = obj.Item1;
      var conta = obj.Item2;
      contaUsuario.TotalBytes = SomaBytes(contaUsuario);

      if (conta.TotalBytes < contaUsuario.TotalBytes)
      {
        // colocar como morto o atual
        Lapide(posicao);
        WritePrograma(id, contaUsuario, 0, SeekOrigin.End);
      }
      else if (conta.TotalBytes > contaUsuario.TotalBytes)
      {
        contaUsuario.TotalBytes = conta.TotalBytes;
        WritePrograma(id, contaUsuario, posicao, SeekOrigin.Begin);
      }
      else
        WritePrograma(id, contaUsuario, posicao, SeekOrigin.Begin);
    }
  }
}
