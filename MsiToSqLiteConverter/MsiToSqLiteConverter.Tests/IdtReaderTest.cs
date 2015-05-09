using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MsiToSqLiteConverter.Tests
{
    using System.Linq;
    using System.Runtime.InteropServices;

    using MsiToSqLiteConverter.MsiProcessing.Domain;

    [TestClass]
    public class IdtReaderTest
    {
        /// <summary>
        /// Reads the IDT.
        /// </summary>
        [TestMethod]
        [DeploymentItem("Resources\\ControlEvent.idt")]
        [DeploymentItem("Resources\\Component_1251.idt")]
        [DeploymentItem("Resources\\Cabs.idt")]
        public void ReadIdt()
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
        }
    }
}
