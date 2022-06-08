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
    public List<string> Dicionario = new List<string>() { };

    public Compactar()
    {
      var ultimaVersao = ReadCabeca();
      if (ultimaVersao != 0)
      {
        UpdateVersao(ultimaVersao);
      }
      else
      {
        List<string> Dicionario = new List<string>() { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", " ", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        WriteDicionario(UpdateCabeca(), Dicionario);

      }
    }
    void UpdateUltimaVersao()
    {
      var ultimaVersao = ReadCabeca();
      if (Versao != ultimaVersao)
      {
        UpdateVersao(ultimaVersao);
      }
    }
    bool UpdateVersao(uint versao)
    {
      var comp = ReadDicionario(versao);
      if (comp != null)
      {
        this.Versao = comp.Item1;
        this.Dicionario = comp.Item2;
        return true;
      }
      return false;
    }

    uint GetSomaBytes()
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
      UpdateUltimaVersao();
      //{ "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", " ", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" }
      // List<String> dicionario = new List<String>() { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", " " };
      // List<String> dicionario = new List<String>() { "a", "b", "w" };
      // var pa = Console.ReadLine(); // input do usuário
      // var pa = "ABRA CADABRA";
      // var pa = "wabbawabba";
      // String args = "" + pa;
      // var contar = (frase);
      var contaNumeros = 0.0;
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
            contaNumeros++;
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
              contaNumeros++;
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
      var economia = 1.00 - (contaNumeros / args.Length);
      Console.WriteLine("conta numeros:" + (contaNumeros));
      Console.WriteLine("Economia de espaço/perda: " + Math.Round(economia, 4) * 100 + "%");

      WriteDicionario(UpdateCabeca(), Dicionario);
    }
    public string DesCompactarVersao(uint versao, List<int> numeros)
    {
      if (UpdateVersao(versao) != false)
      {
        return DesCompactarLZW(numeros);
      }

      return "Versao Invalida";
    }
    string DesCompactarLZW(List<int> numeros)
    {
      var contaNumeros = 0.0;
      String palavra = "";
      for (int i = 0; i < numeros.Count; i++)
      {
        palavra += Dicionario[numeros[i]];
      }
      return palavra;
    }
    static void WriteDicionario(uint versao, List<string> dicionario)
    {
      FileStream sb = new FileStream(Program.fileCompactar, FileMode.OpenOrCreate);
      sb.Seek(0, SeekOrigin.End);
      StreamWriter sw = new StreamWriter(sb);

      sw.Write(versao);

      for (int i = 0; i < dicionario.Count; i++)
      {
        sw.Write("*" + dicionario[i]);
      }
      sw.Write("\n");
      sw.Close();
      sb.Close();
    }
    // static long WriteDicionario(Compactar comp)
    // {
    //   var stream = new FileStream(Program.fileCompactar, FileMode.OpenOrCreate, FileAccess.ReadWrite);

    //   stream.Seek(0, SeekOrigin.End);

    //   var pular = Encoding.Unicode.GetBytes("\n");
    //   stream.Write(pular);

    //   var pos = stream.Position;
    //   // pegar cada conjunto de bytes de acordo com seu tipo e tamanho
    //   var totalbytes = comp.GetSomaBytes();

    //   var versaoBytes = Utils.ReverseBytes(BitConverter.GetBytes(comp.Versao));
    //   var totalBytesBytes = Utils.ReverseBytes(BitConverter.GetBytes(totalbytes));

    //   stream.Write(versaoBytes);
    //   stream.Write(totalBytesBytes);

    //   for (int i = 0; i < comp.Dicionario.Count; i++)
    //   {
    //     var letras = Encoding.Unicode.GetBytes(comp.Dicionario[i]);
    //     var letrasLength = (byte)letras.Length;

    //     stream.WriteByte(letrasLength); // escreve o próximo byte do arquivo, correspondentes à quantidade de bytes da string
    //     stream.Write(letras); // escreve os próximos 2x bytes do arquivo correspondentes ao nome da conta, onde x é a quantidade de caracteres da string

    //   }
    //   stream.Close();
    //   return pos;
    // }
    static uint UpdateCabeca()
    {
      var ultimoId = ReadCabeca();

      var newId = ultimoId + 1; // converte e incrementa

      WriteCabeca(newId);

      return newId;
    }
    static uint ReadCabeca()
    {
      StreamReader sr = new StreamReader(Program.fileCompactar);

      var cabeca = sr.ReadLine();

      sr.Close();
      if (cabeca == null)
      {
        WriteCabeca(0);
        return 0;
      }

      return Convert.ToUInt32(cabeca);
    }

    static void WriteCabeca(uint cabeca)
    {
      FileStream sb = new FileStream(Program.fileCompactar, FileMode.OpenOrCreate);

      StreamWriter sr = new StreamWriter(sb);

      sr.WriteLine(cabeca); // escreve o id incrementado

      sr.Close();
      sb.Close();
    }

    public static List<string> ExtrairPalavra(string palavras)
    {
      var palavra = String.Empty;
      var tamanho = palavras.Length;

      var listapalavras = new List<string>();

      for (int i = 0; i < tamanho; i++)
      {
        if (palavras[i] != '*')
        {
          palavra += palavras[i];
        }
        else
        {
          listapalavras.Add(palavra);
          palavra = String.Empty;
        }
      }

      listapalavras.Add(palavra);

      return listapalavras;
    }
    public static Tuple<uint, List<string>>? ReadDicionario(uint versaoPesquisa)
    {
      // ler cara conjunto de bytes de acordo com seu respectivo tipo e tamanho, para cada atributo da classe  
      var quantVersap = ReadCabeca();
      if (quantVersap >= versaoPesquisa && versaoPesquisa > 0)
      {
        StreamReader sr = new StreamReader(Program.fileCompactar);

        var linha = "";
        for (int i = 0; i <= versaoPesquisa; i++)
        {
          linha = sr.ReadLine();
        }
        var dicionario = ExtrairPalavra(linha);
        var versao = Convert.ToUInt32(dicionario[0]);
        dicionario.RemoveAt(0);

        sr.Close();

        return new Tuple<uint, List<string>>(versao, dicionario);
      }
      return null;
    }
    // public static Compactar PesquisaDicionario(uint versaoPesquisa)
    // {
    //   StreamReader sr = new StreamReader(Program.fileCompactar);
    //   for (int i = 1; i <= versaoPesquisa; i++)
    //   {
    //     sr.ReadLine();
    //   }
    // }
    // public static Compactar ReadDicionario(uint offset)
    // {
    //   // ler cara conjunto de bytes de acordo com seu respectivo tipo e tamanho, para cada atributo da classe
    //   var stream = new FileStream(Program.fileCompactar, FileMode.OpenOrCreate, FileAccess.ReadWrite);

    //   stream.Seek(offset, SeekOrigin.Begin);

    //   #region Versao

    //   var versaoBytes = new byte[4];

    //   stream.Read(versaoBytes, 0, versaoBytes.Length);

    //   versaoBytes = Utils.ReverseBytes(versaoBytes);

    //   var versao = BitConverter.ToUInt32(versaoBytes);

    //   #endregion

    //   #region TotalBytes

    //   var totalBytesBytes = new byte[4];

    //   stream.Read(totalBytesBytes, 0, totalBytesBytes.Length);

    //   totalBytesBytes = Utils.ReverseBytes(totalBytesBytes);

    //   var totalBytes = BitConverter.ToUInt32(totalBytesBytes);

    //   #endregion


    //   var dicionarioTemp = new List<string>() { };

    //   for (int i = 0; i < totalBytes; i++)
    //   {
    //     byte letrasTamanho = (byte)stream.ReadByte();

    //     var letrasBytes = new byte[letrasTamanho];

    //     stream.Read(letrasBytes, 0, letrasBytes.Length);

    //     var letras = Encoding.Unicode.GetString(letrasBytes);

    //     dicionarioTemp.Add(letras);
    //   }

    //   sr.Close();

    //   return new Compactar
    //   {
    //     Versao = versao,
    //     Dicionario = dicionarioTemp
    //   };
    // }
  }
}