using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Person.Domain.Entities
{
    [Serializable]
    [Table("Persons")]
    public class Person
    {
        [Key]
        public Guid PersonalId { get; set; }

        [Required, StringLength(50)]
        [RegularExpression(@"^[А-Яа-яЁё]+$")]
        public string LastName { get; set; }

        [Required, StringLength(50)]
        [RegularExpression(@"^[А-Яа-яЁё]+$")]
        public string FirstName { get; set; }

        [Required, StringLength(50)]
        [RegularExpression(@"^[А-Яа-яЁё]+$")]
        public string Patronymic { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required, StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required, StringLength(100)]
        [RegularExpression(@"^\+\d+$")]
        public string Phone { get; set; }
    }
}