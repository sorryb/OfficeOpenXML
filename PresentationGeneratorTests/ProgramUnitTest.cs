using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using PresentationGenerator;

namespace PresentationGeneratorTests
{
    [TestClass]
    public class ProgramUnitTest
    {
        [TestMethod]
        public void TestCreatePresentationMethod()
        {

            //Arrange
            string path = string.Empty;

            //Act
            PresentationGenerator.Program.CreatePresentation(path);

            //Assert
        }
    }
}
