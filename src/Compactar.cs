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
        UpdateVersao(ultimaVersao + 1);
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
    public bool UpdateVersao(uint versao)
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

    //soma as letras a partir de uma posição em uma string
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
    // public string CompactarLZW(Compactar dic, string args)
    // {
    //   // UpdateUltimaVersao();

    //   var contaNumeros = 0.0;
    //   String numeros = "";
    //   for (int i = 0; i < args.Length; i++)
    //   {
    //     for (int j = dic.Dicionario.Count - 1; j != -1; j--)
    //     {
    //       var palav = somarLetras(args, i, dic.Dicionario[j].Length);
    //       var posicao = i + dic.Dicionario[j].Length;

    //       if (palav == dic.Dicionario[j])
    //       {
    //         numeros += " " + (j);
    //         contaNumeros++;
    //         if (posicao < args.Length)
    //         {
    //           dic.Dicionario.Add(palav + "" + args[posicao]);
    //           i += dic.Dicionario[j].Length - 1;
    //         }
    //         else
    //         {
    //           i = args.Length;
    //         }
    //         break;
    //       }
    //       else if (j == 0)
    //       {
    //         if (posicao < args.Length)
    //         {
    //           contaNumeros++;
    //           numeros += dic.Dicionario.Count + " ";
    //           dic.Dicionario.Add(palav);
    //         }
    //         else
    //         {
    //           i = args.Length;
    //         }
    //         break;
    //       }

    //     }
    //   }
    //   var economia = 1.00 - ((contaNumeros * 4) / (args.Length * 2));
    //   Console.WriteLine("conta numeros:" + (contaNumeros));
    //   Console.WriteLine("Economia de espaço/perda: " + Math.Round(economia, 4) * 100 + "%");

    //   WriteDicionario(UpdateCabeca(), dic.Dicionario);

    //   return numeros;
    // }

    //utilição do metodo de compactação LZW
    public string CompactarLZW(string args)
    {
      string numeros = "";//string de numero que sera retornada com a compactação
      for (int i = 0; i < args.Length; i++)
      {
        for (int j = Dicionario.Count - 1; j != -1; j--)
        {
          var palav = somarLetras(args, i, Dicionario[j].Length);
          var posicao = i + Dicionario[j].Length;

          //verifica sempre a mesma quantidade de letras da posicao do dicionario atual
          if (palav == Dicionario[j])
          {
            numeros += " " + (j);
            //impede a verificação acima da quantidade de letras da palavra
            if (posicao < args.Length) // Ex: não verificar as 2 letras restantes com 3 da posicao atual do dicionario
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
          else if (j == 0)//acabou o dicionario e adiciona a palavra nova ao dicionario
          {//esse caso seria utilizado com um dicionario imcompleto
            if (posicao < args.Length)
            {
              numeros += " " + Dicionario.Count;
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

      return numeros;
    }

    //Descompactação utilizando LZW
    public string DesCompactarLZW(List<int> numeros)
    {
      String palavra = "";//retorna a string descompactada
      String add = "";
      for (int i = 0; i < numeros.Count; i++)
      {
        add = Dicionario[numeros[i]];
        palavra += add;

        if (i != numeros.Count - 1)//so adiciona ao dicionario se não for o ultimo numero
        {
          Dicionario.Add(add);
          Dicionario[Dicionario.Count - 1] += Dicionario[numeros[i + 1]][0];
        }
      }
      return palavra;
    }

    //Escreve um novo dicionario atualizado
    public void WriteDicionarioAtual()
    {
      WriteDicionario(UpdateCabeca(), Dicionario);
    }

    //escreve um novo dicionario na proxima linha do arquivo
    static void WriteDicionario(uint versao, List<string> dicionario)
    {
      FileStream sb = new FileStream(Program.fileCompactar, FileMode.OpenOrCreate);
      sb.Seek(0, SeekOrigin.End);
      StreamWriter sw = new StreamWriter(sb);

      sw.Write(versao);

      for (int i = 0; i < dicionario.Count; i++)
      {
        sw.Write("*" + dicionario[i]);// o * divide cada palavra do dicionario  
      }
      sw.Write("\n");
      sw.Close();
      sb.Close();
    }

    // Incrementa o último id no início do arquivo e retorna o novo id
    static uint UpdateCabeca()
    {
      var ultimoId = ReadCabeca();

      var newId = ultimoId + 1; // converte e incrementa

      WriteCabeca(newId);

      return newId;
    }

    //Leitura do uint cabeça, de um arquivo passado como parametro
    // static uint ReadCabeca()
    // {
    //   FileStream sb = new FileStream(Program.fileCompactar, FileMode.OpenOrCreate);

    //   StreamReader sr = new StreamReader(sb);

    //   var cabeca = sr.ReadLine();

    //   sr.Close();
    //   sb.Close();
    //   if (cabeca == null)
    //   {
    //     WriteCabeca(0);
    //     return 0;
    //   }

    //   return Convert.ToUInt32(cabeca);
    // }
    public static uint ReadCabeca()
    {
      var cabeca = new byte[4];

      var stream = new FileStream(Program.fileCompactar, FileMode.OpenOrCreate, FileAccess.ReadWrite);

      stream.Read(cabeca, 0, cabeca.Length);

      stream.Close();

      return BitConverter.ToUInt32(Utils.ReverseBytes(cabeca));
    }

    //Escreve na posicao inicial do arquivo um uint passado como parametro
    // static void WriteCabeca(uint cabeca)
    // {
    //   FileStream sb = new FileStream(Program.fileCompactar, FileMode.OpenOrCreate, FileAccess.ReadWrite);

    //   StreamWriter sr = new StreamWriter(sb);

    //   sr.WriteLine(cabeca); // escreve o id incrementado

    //   sr.Close();
    //   sb.Close();
    // }
    static void WriteCabeca(uint cabeca)
    {
      var stream = new FileStream(Program.fileCompactar, FileMode.OpenOrCreate, FileAccess.ReadWrite);

      var newBytes = Utils.ReverseBytes(BitConverter.GetBytes(cabeca));

      stream.Write(newBytes); // escreve o id incrementado

      stream.Close();
    }

    //Pega todoas as palavras de uma mesma string, e retorna uma lista com todas as palavras separadas
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

    //faz a leitura de uma linha onde esta todo o dicionario de uma versao especifica
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
  }
}