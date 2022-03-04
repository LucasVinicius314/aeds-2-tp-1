namespace Aeds3TP1
{
  class Menu
  {
    public static void Principal()
    {
      while (true)
      {
        try
        {
          Console.WriteLine(@"Listagem de operações:
  0 - Criar conta bancária
  1 - Realizar transferência
  2 - Ler registro
  3 - Atualizar registro
  4 - Deletar registro
  5 - Sair");

#if DEBUG
          var input = "0";
#else
          var input = Console.ReadLine();
#endif

          if (input == null)
          {
            throw new Exception();
          }

          switch (input)
          {
            case "0":
              Create();
              break;

            case "1":
              throw new NotImplementedException();

            case "2":
              throw new NotImplementedException();

            case "3":
              throw new NotImplementedException();

            case "4":
              throw new NotImplementedException();

            case "5":
              return;

            default:
              throw new Exception();
          }
        }
        catch (NotImplementedException)
        {
          Console.WriteLine("Não implementado.");

          throw;
        }
        catch (Exception)
        {
          Console.WriteLine("Opção inválida.");

          throw;
        }
      }

      static void Create()
      {
        Console.WriteLine("=== Criar conta bancária");

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

            conta.Cidade = cidade;

            break;
          }
          catch (Exception)
          {
            Console.WriteLine("Cidade inválida.");
          }
        }

        Program.WriteUsuario(conta);
      }
    }
  }
}