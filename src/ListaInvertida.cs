using System.Text;

namespace Aeds3TP1
{
  class ListaInvertida
  {
    // getters e setters
    public uint TotalBytes { get; set; }
    public string IdsContas
    {
      get { return idsContas; }
      set
      {
        if (value == null) throw new FormatException();

        idsContas = value;
      }
    }

    public string PessoaCidade
    {
      get { return pessoaCidade; }
      set
      {
        if (value == null) throw new FormatException();

        pessoaCidade = value;
      }
    }

    string pessoaCidade = String.Empty;
    string idsContas = String.Empty;

    // retorna o tamanho em bytes dos atributos do registro
    public uint GetSomaBytes()
    {
      var totalbytes = Encoding.Unicode.GetBytes(pessoaCidade).Length +
      Encoding.Unicode.GetBytes(pessoaCidade).Length +
                       BitConverter.GetBytes(TotalBytes).Length + 2;

      return BitConverter.ToUInt32(BitConverter.GetBytes(totalbytes));

    }

    // override no toString padrão para melhorar a visualização do objeto
    public override string ToString()
    {
      var hashCode = base.GetHashCode();

      return $@"ListaInvertida [{hashCode}]
    > TotalBytes ({TotalBytes.GetType()}): {TotalBytes}
    > IdsContas ({idsContas?.GetType()}): {idsContas ?? ""}
    > NomePessoa ({pessoaCidade?.GetType()}): {pessoaCidade ?? ""}";
    }

    public static void UpdateInvertida(uint id, Conta conta, Conta contaModificada)
    {
      ExcluirListaInvertida(id, conta.Cidade, Program.fileCidade);
      ExcluirListaInvertida(id, conta.NomePessoa, Program.filePessoa);
      WriteListaInvertidaCidade(contaModificada);
      WriteListaInvertidaPessoa(contaModificada);
    }

    public static void ExcluirListaInvertida(uint id, string pessoacidade, string file)
    {
      // var listapessoa = ReadListaInvertida(filepessoa);
      // var listacidade = ReadListaInvertida(filecidade);
      // PesquisarPalavra();
      var listapesquisar = ReadListaInvertida(file);
      var listapalavras = Utils.ExtrairPalavra(pessoacidade);

      for (int j = 0; j < listapalavras.Count; j++)
      {
        for (int i = 0; i < listapesquisar.Count; i++)
        {
          if (listapesquisar[i].PessoaCidade == listapalavras[j])
          {
            var idremovido = Program.PesquisarIdExcluir(Utils.ExtrairIds(listapesquisar[i].IdsContas), id);
            if (idremovido.Count > 0)
            {
              listapesquisar[i].IdsContas = Utils.IdsToString(idremovido);
            }
            else
            {
              listapesquisar.RemoveAt(i);
            }
            // listapesquisar[i].IdsContas += " " + ininv.IdsContas;
            // break;
          }
        }
      }

      Program.ResetarArquivo(listapesquisar, file);
    }

    public static void WriteListaInvertida(string file, List<ListaInvertida> listainserir)
    {
      var stream = new FileStream(file, FileMode.OpenOrCreate, FileAccess.ReadWrite);
      stream.Seek(4, SeekOrigin.Begin);

      for (int i = 0; i < listainserir.Count; i++)
      {
        var pessoaCidade = Encoding.Unicode.GetBytes(listainserir[i].PessoaCidade);
        var pessoaCidadeLength = (byte)pessoaCidade.Length;

        var idsContas = Encoding.Unicode.GetBytes(listainserir[i].IdsContas);
        var idsContasLength = (byte)idsContas.Length;

        var totalbytes = listainserir[i].GetSomaBytes();

        var totalBytesBytes = Utils.ReverseBytes(BitConverter.GetBytes(totalbytes));

        stream.Write(totalBytesBytes); // escreve os próximos 4 bytes do arquivo, correspondentes ao tamanho do registro

        stream.WriteByte(pessoaCidadeLength); // escreve o próximo byte do arquivo, correspondentes à quantidade de bytes da string
        stream.Write(pessoaCidade); // escreve os próximos 2x bytes do arquivo correspondentes ao nome da conta, onde x é a quantidade de caracteres da string

        stream.WriteByte(idsContasLength); // escreve o próximo byte do arquivo, correspondentes à quantidade de bytes da string
        stream.Write(idsContas);
      }

      stream.Close();
    }

    public static void WriteListaInvertidaPessoa(Conta conta)
    {
      WriteListaInvertida(Program.filePessoa, InserirListaInvertida(conta.IdConta, conta.NomePessoa, Program.filePessoa));
    }

    public static void WriteListaInvertidaCidade(Conta conta)
    {
      WriteListaInvertida(Program.fileCidade, InserirListaInvertida(conta.IdConta, conta.Cidade, Program.fileCidade));
    }

    static List<ListaInvertida> InserirListaInvertida(uint id, string pessoacidade, string file)
    {
      //mecher nos parametros mais tarde, inv n faz sentido
      var listapesquisar = ReadListaInvertida(file);
      var listapalavras = Utils.ExtrairPalavra(pessoacidade);

      var tamanhopesquisa = listapesquisar.Count;

      for (int j = 0; j < listapalavras.Count; j++)
      {
        var ininv = new ListaInvertida();

        ininv.IdsContas += id;
        ininv.PessoaCidade = listapalavras[j];
        ininv.TotalBytes = ininv.GetSomaBytes();

        for (int i = 0; i < tamanhopesquisa; i++)
        {
          if (listapesquisar[i].PessoaCidade == listapalavras[j])
          {
            listapesquisar[i].IdsContas += " " + ininv.IdsContas;
            break;
          }
          if (i == tamanhopesquisa - 1)
          {
            listapesquisar.Add(ininv);
            Program.UpdateCabeca(file);
            break;
          }
        }
        if (tamanhopesquisa == 0)
        {
          listapesquisar.Add(ininv);
          Program.UpdateCabeca(file);
        }
      }

      return listapesquisar;
    }

    public static List<ListaInvertida> ReadListaInvertida(string file)
    {
      var listainv = new List<ListaInvertida>();

      var posicao = 4;

      var quantregistro = Program.QuantidadeRegistros(file);

      var stream = new FileStream(file, FileMode.OpenOrCreate, FileAccess.ReadWrite);

      stream.Seek(posicao, SeekOrigin.Begin);

      for (int i = 0; i < quantregistro; i++)
      {
        var inv = new ListaInvertida();
        #region Arquivo

        var totalBytesBytes = new byte[4];

        stream.Read(totalBytesBytes, 0, totalBytesBytes.Length);

        totalBytesBytes = Utils.ReverseBytes(totalBytesBytes);

        var totalBytes = BitConverter.ToUInt32(totalBytesBytes);

        #endregion

        #region Pessoa cidade

        byte pessoaCidadeTamanho = (byte)stream.ReadByte();

        var pessoaCidadeBytes = new byte[pessoaCidadeTamanho];

        stream.Read(pessoaCidadeBytes, 0, pessoaCidadeBytes.Length);

        var pessoaCidade = Encoding.Unicode.GetString(pessoaCidadeBytes);

        #endregion

        #region Ids contas

        byte idContaTamanho = (byte)stream.ReadByte();

        var idContaBytes = new byte[idContaTamanho];

        stream.Read(idContaBytes, 0, idContaBytes.Length);

        var idConta = Encoding.Unicode.GetString(idContaBytes);

        #endregion

        inv.TotalBytes = totalBytes;
        inv.IdsContas = idConta;
        inv.PessoaCidade = pessoaCidade;

        listainv.Add(inv);
      }

      stream.Close();

      return listainv;
    }
  }
}