using System.Text;

namespace Aeds3TP1
{
    class Program
    {
        static string filePath = "data.dat";

        static void Main(string[] args)
        {
            var conta = new Conta
            {
                IdConta = 0,
                NomePessoa = "joao",
                Cpf = "123456789",
                Cidade = "alagoas",
                TransferenciasRealizadas = 0,
                SaldoConta = 1000,
            };

            Write(conta);
            Write(conta);
            // var a = Read();
            // a.ToString();
        }

        // static void Read()
        // {
        //     var ultimoId = new byte[4];

        //     var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

        //     stream.Read(ultimoId, 0, ultimoId.Length);

    

        //     var lapide = (char)stream.ReadByte();

        //     stream.Close();
            
        // }

        static int Read()
        {
            var ultimoId = new byte[4];

            var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

            stream.Read(ultimoId, 0, ultimoId.Length);

            var lapide = (char)stream.ReadByte();

            stream.Close();
            
            return BitConverter.ToInt32(ultimoId);
        }

        static byte[] ReverseBytes(byte[] a)
        {
            Array.Reverse(a, 0, a.Length);

            return a;
        }
        
        static int UpdateCabeca()
        {
            var ultimoId = new byte[4];

            var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            
            stream.Read(ultimoId, 0, ultimoId.Length);

            stream.Position = 0;
            var newId = BitConverter.ToInt32(ReverseBytes(ultimoId))+1;
            stream.Write(BitConverter.GetBytes(newId));

            stream.Close();

            return newId;
        }

        static void Write(Conta conta)
        {
            var id = UpdateCabeca();
           
            var bytes = ReverseBytes(BitConverter.GetBytes((int)id));

            var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

            stream.Write(bytes);

            stream.WriteByte((byte)'\0');


            var IdConta = ReverseBytes(BitConverter.GetBytes(id));
            var NomePessoa = Encoding.Unicode.GetBytes(conta.NomePessoa);
            var Cpf = Encoding.Unicode.GetBytes(conta.Cpf);
            var Cidade = Encoding.Unicode.GetBytes(conta.Cidade);
            var TransferenciasRealizadas = ReverseBytes(BitConverter.GetBytes(conta.TransferenciasRealizadas));
            var SaldoConta = ReverseBytes(BitConverter.GetBytes(conta.SaldoConta));


            var totalbytes = IdConta.Length + NomePessoa.Length + Cpf.Length + Cidade.Length + TransferenciasRealizadas.Length + SaldoConta.Length;

            stream.Write(ReverseBytes(BitConverter.GetBytes(totalbytes)));
            stream.Write(IdConta);
            stream.Write(NomePessoa);
            stream.Write(Cpf);
            stream.Write(Cidade);
            stream.Write(TransferenciasRealizadas);
            stream.Write(SaldoConta);


            stream.Close();
        }

        static void Update()
        {
            var bytes = BitConverter.GetBytes(0);

            var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

            stream.Write(bytes);

            stream.WriteByte((byte)'*');

            stream.Close();
        }
    }
}
