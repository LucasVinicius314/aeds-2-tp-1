using System.Text;

namespace Aeds3TP1
{
  class Program
  {
    static string filePath = "data.dat";

    static void MainUpdate(){
      uint x = 0,resposta = 0,id = 1;
      var alterar = "";
      float saldo;
      Console.WriteLine("Digite o id da conta a ser alterada:");
      id = 1;//n sei como pega a resposta

      Conta conta = ReadId(id).Item2;

      while(x == 0){
        Console.WriteLine("Digite qual atributo voce deseja alterar:");
        Console.WriteLine("1 - Nome, 2 - CPF, 3 - Cidade,4 - Saldo, 5 - Sair");
        if(resposta < 5){
          resposta++;
        }
        
        switch (resposta)
        {
          case 1:
            Console.WriteLine("Digite o novo Nome:");
            alterar = "Vitor";
            conta.NomePessoa = alterar;
          break;

          case 2:
            Console.WriteLine("Digite o novo CPF:");
            alterar = "12345678999";
            conta.Cpf = alterar;
          break;

          case 3:
            Console.WriteLine("Digite a nova Cidade:");
            alterar = "Natalll";
            conta.Cidade = alterar;
          break;

          case 4:
            Console.WriteLine("Digite o novo saldo:");
            saldo = (float)2000.00;
            conta.SaldoConta = saldo;
          break;

          case 5:
            Update(id,conta);
            x = 1;
          break;

          default:
            Console.WriteLine("Digite um numero valido");
          break;
        }
      }
    }

    static void MainCreate(){
      var resposta = "";

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

      //nomePessoa, cpf, estado
      Console.WriteLine("Digite o Nome:");
      resposta = "Vitor";
      conta.NomePessoa = resposta;

          
      Console.WriteLine("Digite o CPF:");
      resposta = "12345678999";
      conta.Cpf = resposta;
        

        
      Console.WriteLine("Digite a Cidade:");
      resposta = "Natalll";
      conta.Cidade = resposta;
    }

    static void Main(string[] args)
    {
      var conta = new Conta
      {
        Lapide = '\0',
        TotalBytes = 0,
        IdConta = 0,
        NomePessoa = "joao",
        Cpf = "123456789",
        Cidade = "belo horizonte",
        TransferenciasRealizadas = 10,
        SaldoConta = 1000,
      };
      var conta2 = new Conta
      {
        Lapide = '\0',
        TotalBytes = 0,
        IdConta = 0,
        NomePessoa = "lucas",
        Cpf = "10987654321",
        Cidade = "alagoas",
        TransferenciasRealizadas = 0,
        SaldoConta = 1000,
      };
      //Criar conta(Write): dados a serem digitados(nomePessoa, cpf, estado)
      WriteUsuario(conta);
      WriteUsuario(conta2);

      Console.WriteLine(ReadId(2).Item2);
      Console.WriteLine(ReadId(3).Item2);
      
      MainCreate();
      MainUpdate();

    }

    static byte[] ReverseBytes(byte[] a)
    {
      Array.Reverse(a, 0, a.Length);

      return a;
    }

    static Conta Read(long ler,SeekOrigin seekOrigin)
    {
      #region Arquivo

      // var ultimoIdBytes = new byte[4];

      var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

      // stream.Read(ultimoIdBytes, 0, ultimoIdBytes.Length);

      stream.Seek(ler,seekOrigin);

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

    // static uint ReadId()
    // {
    //   var ultimoIdBytes = new byte[4];

    //   var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

    //   stream.Read(ultimoIdBytes, 0, ultimoIdBytes.Length);

    //   var lapide = (char)stream.ReadByte();

    //   stream.Close();

    //   return BitConverter.ToUInt32(ultimoIdBytes);
    // }
    
    static Tuple<uint, Conta> ReadId(uint id)// consulta do usuario a um id especifico
    {
      var position = (uint)4; //colocar na posicao da primeira lapide 
      var conta = Read(position,SeekOrigin.Begin);

      while(conta.IdConta != 0){ //ver se a posicao existe

        if(conta.Lapide == '\0'){ //verifica a lapide
          
          if(conta.IdConta == id){

            return new Tuple<uint, Conta>(position,conta);

          }
          
        }

        position += conta.TotalBytes; //usa o pular pra ir pra posicao prox posicao

        conta = Read((long)position,SeekOrigin.Begin);

      }

      return new Tuple<uint, Conta>(position,conta);
    }
    static void WriteUsuario(Conta conta)
    {
      Write(UpdateCabeca(),0,conta,SeekOrigin.End);
    }

    static void WritePrograma(uint id,Conta conta,uint posicao,SeekOrigin seekOrigin)
    {
      Write(id,posicao,conta,seekOrigin);
    }
    static uint SomaBytes(Conta conta)
    {
      var totalbytes = BitConverter.GetBytes(conta.IdConta).Length +
        Encoding.Unicode.GetBytes(conta.NomePessoa).Length  +
        Encoding.Unicode.GetBytes(conta.Cpf).Length  +
        Encoding.Unicode.GetBytes(conta.Cidade).Length +
        BitConverter.GetBytes(conta.TransferenciasRealizadas).Length +
        BitConverter.GetBytes(conta.SaldoConta).Length + 8;

        return BitConverter.ToUInt32(BitConverter.GetBytes(totalbytes));
    }
    static void Write(uint id,long posicao,Conta conta, SeekOrigin seekOrigin)
    {
      
      var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

      
      stream.Seek(posicao,seekOrigin);
      

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

    static void Lapide(uint posicao){
      var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

      stream.Seek(posicao,SeekOrigin.Begin);

      stream.WriteByte((byte)'*');

      stream.Close();

    }
    
    static void Update(uint id,Conta contaUsuario)
    {
      var obj = ReadId(id);
      var posicao = obj.Item1;
      var conta = obj.Item2;
      contaUsuario.TotalBytes = SomaBytes(contaUsuario);

      if(conta.TotalBytes < contaUsuario.TotalBytes){
        //colocar como morto o atual
        Lapide(posicao);
        WritePrograma(id,contaUsuario,0,SeekOrigin.End);
      }else if(conta.TotalBytes > contaUsuario.TotalBytes){
        contaUsuario.TotalBytes = conta.TotalBytes;
        WritePrograma(id,contaUsuario,posicao,SeekOrigin.Begin);

      }else
        WritePrograma(id,contaUsuario,posicao,SeekOrigin.Begin);

    }
  }
}
