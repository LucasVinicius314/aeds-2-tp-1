namespace Aeds3TP1
{
  // classe de utilidades
  static class Utils
  {
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

    //Retorna umas lista com todos os ids da string
    public static List<uint> ExtrairIds(string palavras)
    {
      var num = String.Empty;
      var tamanho = palavras.Length;

      var listaid = new List<uint>();

      if (palavras != String.Empty)
      {
        for (int i = 0; i < tamanho; i++)
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

        listaid.Sort();
      }

      return listaid;
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
  }
}