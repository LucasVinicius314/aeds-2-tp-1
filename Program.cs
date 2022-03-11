using System.Text;

namespace Aeds3TP1
{
  class Program
  {
    static string filePath = "data.dat";

    static void Main(string[] args)
    {
#if DEBUG
      Test();
#else
      Menu.Principal();
#endif
    }

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

      Write(conta);

      Console.WriteLine("=== Obj");

      var obj = ReadId(3);

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

      Update(1, conta2);

      Console.WriteLine("=== Obj2");

      var obj2 = ReadId(1);

      Console.WriteLine(obj2);
    }

    // incrementa o último id no início do arquivo e retorna o novo id
    static uint UpdateCabeca()
    {
      var ultimoId = new byte[4];

      var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

      stream.Read(ultimoId, 0, ultimoId.Length);

      stream.Position = 0;

      var newId = BitConverter.ToUInt32(Utils.ReverseBytes(ultimoId)) + 1;

      var newIdBytes = Utils.ReverseBytes(BitConverter.GetBytes(newId));

      stream.Write(newIdBytes);

      stream.Close();

      return newId;
    }

    public static void ExcluirId(uint id)
    {
      uint posicao = ReadId(id).Item1;
      MarcarExcluido(posicao);
    }

    // marca o registro como excluído, mudando o byte da lápide a partir de um offset específico
    static void MarcarExcluido(uint posicao)
    {
      var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

      stream.Seek(posicao, SeekOrigin.Begin);

      stream.WriteByte((byte)'*');

      stream.Close();
    }

    // retorna uma conta a partir de um offset e sua origem, lendo o registro a partir daquele offset no arquivo
    static Conta Read(long offset, SeekOrigin seekOrigin)
    {
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

    // escreve um novo registro no final do arquivo
    // usado para criar um novo registro
    public static void Write(Conta conta)
    {
      Write(UpdateCabeca(), 0, conta, SeekOrigin.End);
    }

    // escreve um novo registro a partir de um id, a conta, o offset desejado e sua origem
    static void Write(uint id, long posicao, Conta conta, SeekOrigin seekOrigin)
    {
      var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

      stream.Seek(posicao, seekOrigin);

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

    public static string? TransferenciaConta(Conta contaDebitar, float debitar,Conta contaCreditar)
    {
      if(contaDebitar.SaldoConta < debitar){
        return "Saldo na conta insuficiente.";
      }
      
      contaDebitar.SaldoConta -= debitar;
      contaDebitar.TransferenciasRealizadas += 1;

      contaCreditar.SaldoConta += debitar;
      contaCreditar.TransferenciasRealizadas += 1;

      Update(contaDebitar.IdConta,contaDebitar);
      Update(contaCreditar.IdConta,contaCreditar);

      return null;
    }
  }
}
