using System.ComponentModel.DataAnnotations;

namespace MsiToSqLiteConverter.Schema
{
    public class ColumnInfo
    {
        [Required]
        public string Name { get; set; }

        public ColumnType ColumnType { get; set; }

        public bool IsKeyColumn { get; set; }

        public bool IsNullable { get; set; }
    }
}
