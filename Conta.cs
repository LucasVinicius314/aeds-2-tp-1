namespace Aeds3TP1
{
  class Conta
  {
    public uint IdConta { get; set; }
    public string NomePessoa { get; set; }
    public string Cpf { get; set; }
    public string Cidade { get; set; }
    public ushort TransferenciasRealizadas { get; set; }
    public float SaldoConta { get; set; }
  }
}