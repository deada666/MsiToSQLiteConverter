namespace MsiToSqLiteConverter.Tests
{
    using System;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using MsiToSqLiteConverter.MsiProcessing.Domain;
    using MsiToSqLiteConverter.MsiProcessing.Domain.Schema;
    using MsiToSqLiteConverter.Schema;

    /// <summary>
    /// The IDT reader test.
    /// </summary>
    [TestClass]
    [DeploymentItem("Resources\\ControlEvent.idt")]
    [DeploymentItem("Resources\\Component_1251.idt")]
    [DeploymentItem("Resources\\Cabs.idt")]
    public class IdtReaderTest
    {
        /// <summary>
        /// Reads the IDT.
        /// </summary>
        [TestMethod]
        public void ReadIdtTableSchema()
        {
            var reader = new IdtReader();

            var cabSchema = reader.ReadTableSchema("Cabs.idt");
            var componentSchema = reader.ReadTableSchema("Component_1251.idt");
            var controlEventSchema = reader.ReadTableSchema("ControlEvent.idt");

            Assert.AreEqual(cabSchema.TableName, "Cabs", false);
            Assert.AreEqual(componentSchema.TableName, "Component", false);
            Assert.AreEqual(controlEventSchema.TableName, "ControlEvent", false);

            Assert.AreEqual(cabSchema.Columns.Count, 2);
            Assert.AreEqual(componentSchema.Columns.Count, 6);
            Assert.AreEqual(controlEventSchema.Columns.Count, 6);

            Assert.AreEqual(cabSchema.Columns.Where(item => item.Value.IsKeyColumn).ToList().Count, 1);
            Assert.AreEqual(componentSchema.Columns.Where(item => item.Value.IsKeyColumn).ToList().Count, 1);
            Assert.AreEqual(controlEventSchema.Columns.Where(item => item.Value.IsKeyColumn).ToList().Count, 5);

            Assert.IsTrue(cabSchema.Columns["Data"].ColumnType == ColumnType.Binary);
            Assert.IsTrue(cabSchema.Columns["Name"].ColumnType == ColumnType.Text);
            Assert.IsTrue(componentSchema.Columns["Attributes"].ColumnType == ColumnType.Int16);
        }

        /// <summary>
        /// Tests the schema parser.
        /// </summary>
        [TestMethod]
        public void TestSchemaParser()
        {
            Assert.IsTrue(this.IsExceptionAssert(() => { var test = new MsiTableSchema(string.Empty, string.Empty, string.Empty); }));
            Assert.IsTrue(this.IsExceptionAssert(() => { var test = new MsiTableSchema("abra", "abra", "abra"); }));
            Assert.IsTrue(this.IsExceptionAssert(() => { var test = new MsiTableSchema("abra", "s72", "abra"); }));
            Assert.IsFalse(this.IsExceptionAssert(() => { var test = new MsiTableSchema("abra", "s72", "abra\tabra"); }));
        }

        /// <summary>
        /// Reads the content of the IDT table.
        /// </summary>
        [TestMethod]
        public void ReadIdtTableContent()
        {
            var reader = new IdtReader();

            var cabSchema = reader.ReadTableSchema("Cabs.idt");
            var componentSchema = reader.ReadTableSchema("Component_1251.idt");
            var controlEventSchema = reader.ReadTableSchema("ControlEvent.idt");

            var cabContent = reader.GetTableContent(cabSchema);
            var componentContent = reader.GetTableContent(componentSchema);
            var controlEventContent = reader.GetTableContent(controlEventSchema);

            Assert.AreEqual("Cabs\\w1.cab.ibd", cabContent.First().Data["Data"].Data.ToString(), true);
            Assert.IsTrue(componentContent.First().Data["Condition"].Data == null);
            Assert.AreEqual("1", controlEventContent.First().Data["Condition"].Data.ToString(), true);
        }

        /// <summary>
        /// Exceptions the assert.
        /// </summary>
        /// <param name="action">
        /// The action.
        /// </param>
        /// <returns>
        /// True, if exception appeared, false - if not.
        /// </returns>
        private bool IsExceptionAssert(Action action)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception)
            {
                return true;
            }

            return false;
        }
    }
}
