using System;

namespace ETicaretAPI.Models.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; } // Her kaydın benzersiz numarası
        public DateTime CreatedDate { get; set; } = DateTime.Now; // Oluşturulma tarihi
        public DateTime? UpdatedDate { get; set; } // Güncellenme tarihi (Boş olabilir)
        public bool IsDeleted { get; set; } = false; // Veriyi silmek yerine gizlemek için (Soft Delete)
    }
}