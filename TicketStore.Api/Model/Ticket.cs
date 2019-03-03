using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketStore.Api.Model
{
    [Table("tickets")]
    public class Ticket
    {
        [Column("id")]
        public Int32 Id { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("payment_id")]
        public Int32 PaymentId { get; set; }
        [Column("number")]
        public String Number { get; set; }
    }
}