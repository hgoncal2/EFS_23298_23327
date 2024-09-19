using EFS_23298_23327.Models;

namespace EFS_23298_23327.API.DTOs
{
    public class SalaDTO
    {
        // Propriedades da Sala (completa)
        public int SalaId { get; set; }  // ID da sala
        public int Numero { get; set; }  // Número da sala
        public int Area { get; set; }  // Área da sala
       
        public DateTime DataCriacao { get; set; }  // Data de criação da sala
       
        public string CriadoPorOid { get; set; }  // ID do criador da sala
        public string CriadoPorUsername { get; set; }  // Nome de utilizador do criador da sala

        // Listas para IDs e nomes de relações
        public List<string> ListaAnfitrioes { get; set; }  // Lista de nomes de utilizadores (anfitriões)
        public List<int> ListaReservas { get; set; }  // Lista de IDs de reservas associadas

        // Construtor vazio
        public SalaDTO() {
            ListaAnfitrioes = new List<string>();
            ListaReservas = new List<int>();
        }

        // Construtor para inicializar com valores
        public SalaDTO(Salas sala, List<string> listaAnfitrioes, List<int> listaReservas) {
            SalaId = sala.SalaId;
            Numero = sala.Numero;
            Area = sala.Area;
           
            DataCriacao = sala.DataCriacao;
          
            CriadoPorOid = sala.CriadoPorOid;
            CriadoPorUsername = sala.CriadoPorUsername;
            ListaAnfitrioes = listaAnfitrioes;
            ListaReservas = listaReservas;
        }
    }
}

