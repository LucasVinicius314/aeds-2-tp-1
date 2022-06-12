namespace Aeds3TP1
{
  // classe de utilidades
  static class Utils
  {
    static string palavraCifra = "ioae";
    // método para inverter a ordem de um array de bytes
    public static byte[] ReverseBytes(byte[] bytes)
    {
      Array.Reverse(bytes, 0, bytes.Length);

      return bytes;
    }

    //Transforma uma lista de ids uint em uma String
    public static String IdsToString(List<uint> ids)
    {
      var listaId = String.Empty;

      ids.Sort();

      listaId += ids[0];

      if (ids.Count > 1)
      {
        for (int i = 0; i < ids.Count; i++)
        {
          listaId += " " + ids[i];
        }
      }

      return listaId;
    }

    public static List<uint> ExtrairIds(string palavras)
    {
      var ids = Extrair(palavras);
      ids.Sort();
      return ids;
    }

    //Retorna umas lista com todos os ids da string
    public static List<uint> Extrair(string palavras)
    {
      var num = String.Empty;
      var tamanho = palavras.Length;

      var listaid = new List<uint>();
      var i = 0;
      if (palavras[0] == ' ')
      {
        i++;
      }

      if (palavras != String.Empty)
      {
        for (; i < tamanho; i++)
        {
          if (palavras[i] != ' ')
          {
            num += palavras[i];
          }
          else
          {
            listaid.Add(UInt32.Parse(num));

            num = String.Empty;
          }
        }
        listaid.Add(UInt32.Parse(num));
      }

      return listaid;
    }

    //transforma uma string com numeros em uma List<int> com numeros
    public static List<int> ExtrairNumeros(string palavras)
    {
      var ouint = Extrair(palavras);
      var lista = new List<int>() { };
      for (int i = 0; i < ouint.Count; i++)
      {
        lista.Add((int)ouint[i]);
      }
      return lista;
    }

    //Pega todoas as palavras de uma mesma string, e retorna uma lista com todas as palavras separadas
    public static List<string> ExtrairPalavra(string palavras)
    {
      var palavra = String.Empty;
      var tamanho = palavras.Length;

      var listapalavras = new List<string>();

      for (int i = 0; i < tamanho; i++)
      {
        if (palavras[i] != ' ')
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

    public static List<string> ExtrairArquivo(string palavras)
    {
      var palavra = String.Empty;
      var tamanho = palavras.Length;

      var listapalavras = new List<string>();

      for (int i = 0; i < tamanho; i++)
      {
        if (palavras[i] != ' ')
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
    static List<Cifra> MontaArrayCifra()
    {
      var array = new List<Cifra>() { };
      for (int i = 0; i < palavraCifra.Length; i++)
      {
        array.Add(new Cifra(Letra: (byte)palavraCifra[i], Letras: "", Quant: 0));
      }

      return array;
    }
    // static List<Tuple<byte, T>> MontaArrayCifra<T>(T val)
    // {
    //   var array = new List<Tuple<byte, T>>() { };
    //   for (int i = 0; i < palavraCifra.Length; i++)
    //   {
    //     var a = val is String;
    //     var b = a? "":0;
    //     array.Add(new Tuple<byte, T>((byte)palavraCifra[i],  ));
    //   }

    //   return array;
    // }
    // static List<Tuple<byte, int>> MontaArrayCifraQuantidade()
    // {
    //   var array = new List<Tuple<byte, int>>() { };
    //   for (int i = 0; i < palavraCifra.Length; i++)
    //   {
    //     array.Add(new Tuple<byte, int>((byte)palavraCifra[i], 0));
    //   }

    //   return array;
    // }
    public static string CrifraColunas(string palavra)
    {
      var arrayCifra = MontaArrayCifra();

      var j = 0;
      for (int i = 0; i < palavra.Length; i++)
      {
        arrayCifra[j] = new Cifra(arrayCifra[j].Letra, arrayCifra[j].Letras + palavra[i], 0);
        if (j == arrayCifra.Count - 1)
        {
          j = -1;
        }
        j++;
      }

      arrayCifra.Sort((a, b) => a.Letra - b.Letra);

      var palavraCifrada = "";
      for (int i = 0; i < arrayCifra.Count; i++)
      {
        palavraCifrada += arrayCifra[i].Letras;
      }

      return palavraCifrada;
    }
    static List<Cifra> OrganizaArrayCifra(List<Cifra> cifras)
    {
      var arrayCifra = MontaArrayCifra();

      for (int i = 0; i < arrayCifra.Count; i++)
      {
        for (int j = 0; j < cifras.Count; j++)
        {
          if (arrayCifra[i].Letra == cifras[j].Letra)
          {
            arrayCifra[i] = cifras[j];
          }
        }
      }

      return arrayCifra;
    }
    public static string DesCrifraColunas(string palavra)
    {
      var arrayCifra = MontaArrayCifra();
      var modulo = (palavra.Length * 1.0) % palavraCifra.Length;
      var divisao = palavra.Length / palavraCifra.Length;
      for (int i = 0; i < arrayCifra.Count; i++)
      {
        if (i < modulo)
        {
          arrayCifra[i] = new Cifra(arrayCifra[i].Letra, arrayCifra[i].Letras, divisao + 1);
        }
        else
        {
          arrayCifra[i] = new Cifra(arrayCifra[i].Letra, arrayCifra[i].Letras, divisao);
        }
      }

      arrayCifra.Sort((a, b) => a.Letra - b.Letra);
      var j = 0;

      for (int i = 0; i < arrayCifra.Count; i++)
      {
        var temp = "";
        var rodar = arrayCifra[i].Quant + j;
        for (; j < rodar; j++)
        {
          temp += palavra[j];
        }
        arrayCifra[i] = new Cifra(arrayCifra[i].Letra, arrayCifra[i].Letras + temp, arrayCifra[i].Quant);
      }

      arrayCifra = OrganizaArrayCifra(arrayCifra);
      var frase = "";

      for (int i = 0; i < divisao + 1; i++)
      {
        for (j = 0; j < arrayCifra.Count; j++)
        {
          if (i < arrayCifra[j].Quant)
          {
            frase += arrayCifra[j].Letras[i];
          }
        }
      }
      return frase;

    }
  }
}