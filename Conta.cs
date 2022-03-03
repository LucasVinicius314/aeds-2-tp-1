namespace Aeds3TP1
{
  class Conta
  {
    public char Lapide { get; set; }
    public uint TotalBytes { get; set; }
    public uint IdConta { get; set; }
    public string? NomePessoa { get; set; }
    public string? Cpf { get; set; }
    public string? Cidade { get; set; }
    public ushort TransferenciasRealizadas { get; set; }
    public float SaldoConta { get; set; }

    public override string ToString()
    {
      var hashCode = base.GetHashCode();

      return $@"Conta [{hashCode}]
    > Lapide ({Lapide.GetType()}): {Lapide}
    > TotalBytes ({TotalBytes.GetType()}): {TotalBytes}
    > IdConta ({IdConta.GetType()}): {IdConta}
    > NomePessoa ({NomePessoa?.GetType()}): {NomePessoa ?? ""}
    > Cpf ({Cpf?.GetType()}): {Cpf ?? ""}
    > Cidade ({Cidade?.GetType()}): {Cidade ?? ""}
    > TransferenciasRealizadas ({TransferenciasRealizadas.GetType()}): {TransferenciasRealizadas}
    > SaldoConta ({SaldoConta.GetType()}): {SaldoConta}";
    }
  }
}