namespace MicroservicesDomain
{
    public class Consumo
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public double ConsumoKwh {  get; set; }
        public string DataMedicao { get; set; }
        public string LocalMedicao  { get; set; }
    }
}
