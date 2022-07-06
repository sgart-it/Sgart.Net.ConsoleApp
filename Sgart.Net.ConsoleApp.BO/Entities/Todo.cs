using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sgart.Net.ConsoleApp.BO.Entities
{
    /// <summary>
    /// classe di esempio mappata sul DB (tabella Todos)
    /// </summary>
    public class Todo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TodoId { get; set; }

        /// <summary>
        /// questa proprietà la savo con stringa Json in un campo NVarChar(max)
        /// </summary>
        public TodoData DataJson { get; set; }

        [Required]
        //[DateTimeKind(DateTimeKind.Utc)]
        public DateTime CreatedUTC { get; set; }

        [Required]
        //[DateTimeKind(DateTimeKind.Utc)]
        public DateTime ModifiedUTC { get; set; }

        //[StringLength(100)]
        //public string Note { get; set; }
    }
}
