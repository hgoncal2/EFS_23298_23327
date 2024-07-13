using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EFS_23298_23327.Models
{
    public class ErrorViewModel
    {
        [DisplayName("Id de Pedido")]
        public string? RequestId { get; set; }
        
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}