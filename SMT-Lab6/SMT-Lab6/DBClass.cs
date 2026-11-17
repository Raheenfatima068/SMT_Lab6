
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;

namespace SMT_Lab6
{
    [Table("MyTable")]
    public class DBClass
    {
        public static string connectionString =
            ConfigurationManager.ConnectionStrings[
                "SMTDBConnectionString"
            ].ConnectionString;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Address { get; set; }

        [StringLength(50)]
        public string MobileNo { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(50)]
        public string RollNo { get; set; }
    }
}
