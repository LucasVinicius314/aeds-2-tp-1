using System.Text;
namespace Aeds3TP1
{
  // classe de utilidades
  class Compactar
  {
    //writeDicionario
    //writeCompactado
    //readDicionario
    //readDescompactar
    //pega o update cabeca
    public uint Versao { get; set; }
    public List<string> Dicionario = new List<string>() { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", " ", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

    public uint GetSomaBytes()
    {
      var totalbytes = 0;
      for (int i = 0; i < Dicionario.Count; i++)
      {
        totalbytes += Encoding.Unicode.GetBytes(Dicionario[i]).Length + 1;
      }

      return BitConverter.ToUInt32(BitConverter.GetBytes(totalbytes));
    }
    string somarLetras(string frase, int posicao, int quantLetras)
    {
      string soma = "";

      for (int i = posicao; i < quantLetras + posicao; i++)
      {
        if (i < frase.Length)
        {
          soma += frase[i];
        }
      }

      return soma;
    }
    public void CompactarLZW(string args)
    {
      //{ "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", " ", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" }
      // List<String> dicionario = new List<String>() { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", " " };
      // List<String> dicionario = new List<String>() { "a", "b", "w" };
      // var pa = Console.ReadLine(); // input do usuário
      // var pa = "ABRA CADABRA";
      // var pa = "wabbawabba";
      // String args = "" + pa;
      // var contar = (frase);
      String numeros = "";
      for (int i = 0; i < args.Length; i++)
      {
        for (int j = Dicionario.Count - 1; j != -1; j--)
        {
          var palav = somarLetras(args, i, Dicionario[j].Length);
          var posicao = i + Dicionario[j].Length;

          if (palav == Dicionario[j])
          {
            numeros += (j) + " ";
            if (posicao < args.Length)
            {
              Dicionario.Add(palav + "" + args[posicao]);
              i += Dicionario[j].Length - 1;
            }
            else
            {
              i = args.Length;
            }
            break;
          }
          else if (j == 0)
          {
            if (posicao < args.Length)
            {
              numeros += Dicionario.Count + " ";
              Dicionario.Add(palav);
            }
            else
            {
              i = args.Length;
            }
            break;
          }

        }
      }
      var a = numeros;
      Console.WriteLine("conta letras:" + Dicionario.Count + 1);
      Console.WriteLine("Economia de espaço: " + args.Length * 8);

    }

    static long WriteDicionario(Compactar comp)
    {
      var stream = new FileStream(Program.fileCompactar, FileMode.OpenOrCreate, FileAccess.ReadWrite);

      stream.Seek(0, SeekOrigin.End);

      var pular = Encoding.Unicode.GetBytes("\n");
      stream.Write(pular);

      var pos = stream.Position;
      // pegar cada conjunto de bytes de acordo com seu tipo e tamanho
      var totalbytes = comp.GetSomaBytes();

      var versaoBytes = Utils.ReverseBytes(BitConverter.GetBytes(comp.Versao));
      var totalBytesBytes = Utils.ReverseBytes(BitConverter.GetBytes(totalbytes));

      stream.Write(versaoBytes);
      stream.Write(totalBytesBytes);

      for (int i = 0; i < comp.Dicionario.Count; i++)
      {
        var letras = Encoding.Unicode.GetBytes(comp.Dicionario[i]);
        var letrasLength = (byte)letras.Length;

        stream.WriteByte(letrasLength); // escreve o próximo byte do arquivo, correspondentes à quantidade de bytes da string
        stream.Write(letras); // escreve os próximos 2x bytes do arquivo correspondentes ao nome da conta, onde x é a quantidade de caracteres da string

      }
      stream.Close();
      return pos;
    }

    public static Compactar ReadDicionario(uint versao)
    {
      // ler cara conjunto de bytes de acordo com seu respectivo tipo e tamanho, para cada atributo da classe
      var a = versao;
      StreamReader sr = new StreamReader(Program.fileCompactar);
      for (int i = 1; i <= versao; i++)
      {
        sr.ReadLine();
      }

      stream.Seek(versao, SeekOrigin.Begin);

      #region Versao

      var versaoBytes = new byte[4];

      stream.Read(versaoBytes, 0, versaoBytes.Length);

      versaoBytes = Utils.ReverseBytes(versaoBytes);

      var versao = BitConverter.ToUInt32(versaoBytes);

      #endregion

      #region TotalBytes

      var totalBytesBytes = new byte[4];

      stream.Read(totalBytesBytes, 0, totalBytesBytes.Length);

      totalBytesBytes = Utils.ReverseBytes(totalBytesBytes);

      var totalBytes = BitConverter.ToUInt32(totalBytesBytes);

      #endregion


      var dicionarioTemp = new List<string>() { };

      for (int i = 0; i < totalBytes; i++)
      {
        byte letrasTamanho = (byte)stream.ReadByte();

        var letrasBytes = new byte[letrasTamanho];

        stream.Read(letrasBytes, 0, letrasBytes.Length);

        var letras = Encoding.Unicode.GetString(letrasBytes);

        dicionarioTemp.Add(letras);
      }

      sr.Close();

      return new Compactar
      {
        Versao = versao,
        di
      }
    }
  }
}