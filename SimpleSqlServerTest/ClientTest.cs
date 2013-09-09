using System;
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleSqlServer.Models;

namespace SimpleSqlServerTest
{
    /// <summary>
    /// Descripción resumida de UnitTest1
    /// </summary>
    [TestClass]
    public class ClientTest
    {
        private static SimpleSqlServer.Client _client;

        public ClientTest()
        {
            //
            // TODO: Agregar aquí la lógica del constructor
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Obtiene o establece el contexto de las pruebas que proporciona
        ///información y funcionalidad para la ejecución de pruebas actual.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Atributos de prueba adicionales
        //
        // Puede usar los siguientes atributos adicionales conforme escribe las pruebas:
        //
        // Use ClassInitialize para ejecutar el código antes de ejecutar la primera prueba en la clase
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            _client = SimpleSqlServer.Client.ConnectTo(new ConnectionInfo
            {
                Server = "SAPSERVER",
                Database = "SAPDB",
                Username = "user",
                Password = "password"
            });
        }

        //
        // Use ClassCleanup para ejecutar el código una vez ejecutadas todas las pruebas en una clase
        [ClassCleanup()]
        public static void MyClassCleanup()
        {
            if (_client != null)
            {
                _client.Dispose();
            }
        }
        
        // Usar TestInitialize para ejecutar el código antes de ejecutar cada prueba 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup para ejecutar el código una vez ejecutadas todas las pruebas
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        //[TestMethod]
        //public void TestCreateNewClient()
        //{
        //    using (var client = SimpleSqlServer.Client.ConnectTo(new ConnectionInfo
        //                                                          {
        //                                                              Server = "SAPSERVER",
        //                                                              Database = "SAPDB",
        //                                                              Username = "user",
        //                                                              Password = "password"
        //                                                          }))
        //    {
        //        Assert.IsInstanceOfType(client, typeof (SimpleSqlServer.Client));
        //    }
        //}

        [TestMethod]
        public void TestExecuteScalarWithoutParameters()
        {
            Assert.AreEqual(1, Convert.ToInt32(_client.ExecuteScalar("SELECT 1")));
        }

        [TestMethod]
        public void TestExecuteScalarWithParameters()
        {
            Assert.AreEqual(100, Convert.ToInt32(_client.ExecuteScalar("SELECT GroupCode FROM OCRD WHERE CardCode = @cardCode", "cardCode:C0005")));
        }

        [TestMethod]
        public void TestExecuteNonQueryWithoutParameters()
        {
            Assert.AreEqual(0, Convert.ToInt32(_client.ExecuteScalar("UPDATE OCRD SET GroupCode = 100 WHERE CardCode = 'C0005'")));
        }

        [TestMethod]
        public void TestExecuteNonQueryWithParameters()
        {
            Assert.AreEqual(0, Convert.ToInt32(_client.ExecuteScalar("UPDATE OCRD SET GroupCode = @groupCode WHERE CardCode = @cardCode", "groupCode:100", "cardCode:C0005")));
        }

        [TestMethod]
        public void TestRecordSet()
        {
            Assert.AreEqual(
                    _client.ExecuteScalar("SELECT COUNT(*) FROM OCRD"),
                    _client.RecordSet("SELECT CardCode, CardName FROM OCRD").Rows.Count
                );
        }
    }
}
