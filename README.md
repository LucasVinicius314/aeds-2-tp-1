# aeds-3-tp-1

### Rodar

dotnet run

### Rodar teste

dotnet run -c release

### Descrição

Neste trabalho, você deverá implementar um sistema responsável por realizar operações de
CRUD (create, read, update e delete) em um arquivo sequencial. Para a realização dessas
operações, você deve escolher entre dois contextos diferentes: clubes de futebol ou contas
bancárias.

### Orientações

* O sistema deve ser implementado em C, C++, C# ou Java. Todo o código deve ser de autoria
do grupo (com exceção para bibliotecas/classes relacionadas a aberturas e escritas/leituras
de arquivos e conversões entre atributos e campos).

* Todo o código deve ser comentado de modo a se compreender a lógica utilizada. A não
observância desse critério implica na redução da nota final em 50%.

* A organização do arquivo de dados deve ser sequencial (módulo 4 no Canvas).

* O sistema deverá oferecer uma tela inicial (com uso pelo terminal) com um menu com as
seguintes opções:

  + Criar uma conta bancária -> essa escolha deve, a partir da leitura dos dados da
conta bancária pelo terminal (nomePessoa, cpf, estado), criar uma nova conta
no arquivo (saldoConta e transferenciasRealizadas devem ser iniciados com 0).
  + realizar uma transferência -> essa escolha deve atualizar dados em duas contas
no arquivo.
    - Para isso, é necessário permitir ao usuário cadastrar uma operação de transferência, ou seja, informar duas contas (uma para debitar e outra para creditar o valor) e o valor da transferência. Assim, a conta para debitar deve ter uma redução em saldoConta do valor da transferência, enquanto que a conta para creditar deve receber um acréscimo. Então, o programa deve atualizar o campo saldoConta e o campo transferenciasRealizadas (em +1) das duas contas.
  + Ler um registro (id) -> esse método deve receber um id como parâmetro, 
percorrer o arquivo e retornar os dados do id informado.
  + Atualizar um registro -> esse método deve receber novas informações sobre um
objeto e atualizar os valores dele no arquivo. Observe duas possibilidades que
podem acontecer:
    - O registro mantém seu tamanho - Nenhum problema aqui. Basta atualizar
os dados no próprio local.
    - O registro aumenta ou diminui de tamanho - O registro anterior deve ser
apagado (por meio da marcação lápide) e o novo registro deve ser escrito
no fim do arquivo.
  + Deletar um registro (id) -> esse método deve receber um id como parâmetro, 
percorrer o arquivo e colocar uma marcação (lápide) no registro que será
considerado deletado.

### Estrutura do arquivo

* Deve-se utilizar um int no cabeçalho para armazenar o último valor de id utilizado.
* Os registros do arquivo devem ser compostos por:
  
  + Lápide - Byte que indica se o registro é válido ou se é um registro excluído; 
  + Indicador de tamanho do registro - Número inteiro que indica o tamanho do vetor
de bytes; 
  + Vetor de bytes - Bytes que descrevem o objeto.
* Atributos:

  + idConta (deve ser incremental à medida que novos registros forem adicionados)
  + nomePessoa
  + cpf
  + cidade
  + transferenciasRealizadas
  + saldoConta
