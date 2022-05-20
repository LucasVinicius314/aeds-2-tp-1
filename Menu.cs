namespace Aeds3TP1
{
  // classe para representar a "interface", nesse caso a interação do usuário com o terminal
  class Menu
  {
    // método responsável pelo controle do menu principal
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

          var input = Console.ReadLine(); // input do usuário

          if (input == null)
          {
            throw new Exception();
          }

          switch (input)
          {
            case "0":
              // Criar conta bancária
              CriarConta();
              break;

            case "1":
              // Realizar transferência
              Transferencia();
              break;

            case "2":
              // Ler registro
              LerRegistro();
              break;

            case "3":
              // Atualizar registro
              AtualizarRegistro();
              break;

            case "4":
              // Deletar registro
              ExcluirRegistro();
              break;

            case "5":
              // Sair
              return;

            default:
              throw new Exception();
          }
        }
        catch (Exception)
        {
          Console.WriteLine("Opção inválida.");

          throw;
        }
      }
    }

    // método responsável pelo controle do menu de criação de conta
    static void CriarConta()
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

          var nome = Console.ReadLine(); // input do usuário

          if (nome == null)
          {
            throw new Exception();
          }

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

          var cpf = Console.ReadLine(); // input do usuário

          if (cpf == null || cpf.Length != 11)
          {
            throw new Exception();
          }

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
          Console.WriteLine("Digite a cidade:");

          var cidade = Console.ReadLine(); // input do usuário

          if (cidade == null)
          {
            throw new Exception();
          }

          conta.Cidade = cidade;

          break;
        }
        catch (Exception)
        {
          Console.WriteLine("Cidade inválida.");
        }
      }

      Program.Write(conta);
    }

    // método responsável pelo controle do menu de transferência
    static void Transferencia()
    {
      Console.WriteLine("=== Realizar transferência");

      Console.WriteLine("Digite o ID da conta a ser debitado o valor:");

      var idInput1 = Console.ReadLine(); // input do usuário

      if (idInput1 == null)
      {
        Console.WriteLine("Id inválido.");

        return;
      }

      var idDebitar = uint.Parse(idInput1);

      var contaDebitar = Program.PesquisarConta(idDebitar);

      if (contaDebitar == null)
      {
        Console.WriteLine("Id inválido.");

        return;
      }

      Console.WriteLine("Digite o valor a ser debitado:");

      var valorDebitado = Console.ReadLine(); // input do usuário

      if (valorDebitado == null)
      {
        Console.WriteLine("Valor inválido.");

        return;
      }

      var debitar = float.Parse(valorDebitado);

      Console.WriteLine("Digite o ID da conta a ser creditado o valor:");

      var idInput2 = Console.ReadLine(); // input do usuário

      if (idInput2 == null)
      {
        Console.WriteLine("Id inválido.");

        return;
      }

      var idCreditar = uint.Parse(idInput2);

      var contaCreditar = Program.PesquisarConta(idCreditar);

      if (contaDebitar == null)
      {
        Console.WriteLine("Id inválido.");

        return;
      }

      var resultado = Program.TransferenciaConta(contaDebitar, debitar, contaCreditar);

      if (resultado != null)
        Console.WriteLine(resultado);
    }

    // método responsável pelo controle do menu de leitura de registro
    static void LerRegistro()
    {
      Console.WriteLine("=== Ler registro");

      Console.WriteLine("Digite o ID da conta a ser lida:");

      var idInput = Console.ReadLine(); // input do usuário

      if (idInput == null)
      {
        Console.WriteLine("Id inválido.");

        return;
      }

      var id = uint.Parse(idInput);

      var conta = Program.PesquisarConta(id);

      Console.WriteLine(conta);
    }

    // método responsável pelo controle do menu de exclusão de registro
    static void ExcluirRegistro()
    {
      Console.WriteLine("=== Excluir registro");

      Console.WriteLine("Digite o ID da conta a ser excluida:");

      var idInput = Console.ReadLine(); // input do usuário

      if (idInput == null)
      {
        Console.WriteLine("Id inválido.");

        return;
      }

      var id = uint.Parse(idInput);

      Program.ExcluirId(id);
    }

    // método responsável pelo controle do menu de atualização de registro
    static void AtualizarRegistro()
    {
      Console.WriteLine("=== Atualizar registro");

      Console.WriteLine("Digite o ID da conta a ser alterada:");

      var idInput = Console.ReadLine(); // input do usuário

      if (idInput == null)
      {
        Console.WriteLine("Id inválido.");

        return;
      }

      var id = uint.Parse(idInput);

      var conta = Program.PesquisarConta(id);

      Console.WriteLine(conta);

      if (conta == null)
      {
        Console.WriteLine("Id inválido.");

        return;
      }

      while (true)
      {
        Console.WriteLine(@"Qual atributo deseja alterar?
0 - Nome
1 - CPF
2 - Cidade
3 - Saldo
4 - Voltar");

        var resposta = Console.ReadLine(); // input do usuário

        if (resposta == null)
        {
          Console.WriteLine("Valor inválido.");

          return;
        }

        switch (resposta)
        {
          case "0":
            try
            {
              Console.WriteLine("Digite o novo nome:");

              var nome = Console.ReadLine(); // input do usuário

              if (nome == null)
              {
                throw new Exception();
              }

              conta.NomePessoa = nome;
            }
            catch (Exception)
            {
              Console.WriteLine("Nome inválido.");
            }

            break;

          case "1":
            try
            {
              Console.WriteLine("Digite o novo CPF:");

              var cpf = Console.ReadLine(); // input do usuário

              if (cpf == null || cpf.Length != 11)
              {
                throw new Exception();
              }

              conta.Cpf = cpf;
            }
            catch (Exception)
            {
              Console.WriteLine("CPF inválido.");
            }

            break;

          case "2":
            try
            {
              Console.WriteLine("Digite a nova cidade:");

              var cidade = Console.ReadLine(); // input do usuário

              if (cidade == null)
              {
                throw new Exception();
              }

              conta.Cidade = cidade;
            }
            catch (Exception)
            {
              Console.WriteLine("Cidade inválida.");
            }

            break;

          case "3":
            try
            {
              Console.WriteLine("Digite o novo saldo:");

              var saldoContaInput = Console.ReadLine(); // input do usuário

              if (saldoContaInput == null)
              {
                throw new Exception();
              }

              var saldoConta = float.Parse(saldoContaInput);

              conta.SaldoConta = saldoConta;
            }
            catch (Exception)
            {
              Console.WriteLine("Saldo inválido.");
            }

            break;

          case "4":
            Program.Update(id, conta);

            return;

          default:
            Console.WriteLine("Digite um valor válido.");

            break;
        }
      }
    }
  }
}