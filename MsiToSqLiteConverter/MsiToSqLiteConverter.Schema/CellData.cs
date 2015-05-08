using System.ComponentModel.DataAnnotations;

namespace MsiToSqLiteConverter.Schema
{
    public class CellData
    {
        [Required]
        public string CellName { get; set; }

        public ColumnType DataType { get; set; }

        public object Data { get; set; }
    }
}
