
    namespace StudentBazaar.Web.Models
    {
        public class ChatMessage : BaseEntity
        {
            [Required]
            public int SenderId { get; set; }

            [Required]
            public int ReceiverId { get; set; }

            [Required]
            [MaxLength(2000)]
            public string Content { get; set; } = string.Empty;

            public DateTime SentAt { get; set; } = DateTime.UtcNow;

            public bool IsRead { get; set; } = false;

            public int? ProductId { get; set; }

            [ForeignKey(nameof(ProductId))]
            public Product? Product { get; set; }

            [ForeignKey(nameof(SenderId))]
            public ApplicationUser Sender { get; set; } = null!;

            [ForeignKey(nameof(ReceiverId))]
            public ApplicationUser Receiver { get; set; } = null!;
        }
    }
