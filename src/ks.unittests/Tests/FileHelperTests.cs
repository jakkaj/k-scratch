using ks.unittests;
using ks.model.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ks.unittests.Tests
{

    [TestClass]
    public class FileHelperTests : TestBase
    {
        [TestMethod]
        public void TestFileHelperPathAbsolute()
        {
            var tempDir = Path.GetTempPath();

            Directory.SetCurrentDirectory(tempDir);

            var result = FileRepo.PathOffset("c:\\somepath\\someotherpath");

            Assert.AreEqual(result, "c:\\somepath\\someotherpath");
        }

        [TestMethod]
        public void TestFileHelperPathRelative()
        {
            var tempDir = Path.GetTempPath();

            Directory.SetCurrentDirectory(tempDir);

            var result = FileRepo.PathOffset("someotherpath");

            Assert.AreEqual(result, tempDir + "someotherpath");
        }

        [TestMethod]
        public void TestFileHelperPathRelativeWithBackslash()
        {
            var tempDir = Path.GetTempPath();

            Directory.SetCurrentDirectory(tempDir);

            var result = FileRepo.PathOffset("\\someotherpath");

            Assert.AreEqual(result, tempDir + "someotherpath");
        }

        [TestMethod]
        public void TestFileHelperPathRelativeWithBackslash2()
        {
            var tempDir = Path.GetTempPath();

            Directory.SetCurrentDirectory(tempDir);

            var result = FileRepo.PathOffset("\\someotherpath\\");

            Assert.AreEqual(result, tempDir + "someotherpath");
        }

        [TestMethod]
        public void TestFileHelperPathRelativeWithForwardSlash()
        {
            var tempDir = Path.GetTempPath();

            Directory.SetCurrentDirectory(tempDir);

            var result = FileRepo.PathOffset("/someotherpath");

            Assert.AreEqual(result, tempDir + "someotherpath");
        }

        [TestMethod]
        public void TestFileHelperPathRelativeWithForwardSlash2()
        {
            var tempDir = Path.GetTempPath();

            Directory.SetCurrentDirectory(tempDir);

            var result = FileRepo.PathOffset("/someotherpath/");

            Assert.AreEqual(result, tempDir + "someotherpath");
        }


        [TestMethod]
        public void TestFileHelperPathRelativeWithFileName()
        {
            var tempDir = Path.GetTempPath();

            Directory.SetCurrentDirectory(tempDir);

            var result = FileRepo.PathOffset("someotherpath/somefile.csproj");

            Assert.AreEqual(result, tempDir + "someotherpath\\somefile.csproj");
        }

        [TestMethod]
        public void TestFileHelperPathRelativeWithFileName2()
        {
            var tempDir = Path.GetTempPath();

            Directory.SetCurrentDirectory(tempDir);

            var result = FileRepo.PathOffset("someotherpath\\somefile.csproj");

            Assert.AreEqual(result, tempDir + "someotherpath\\somefile.csproj");
        }

        [TestMethod]
        public void TestFileHelperPathRelativeWithFileName3()
        {
            var tempDir = Path.GetTempPath();

            Directory.SetCurrentDirectory(tempDir);

            var result = FileRepo.PathOffset("/someotherpath/somefile.csproj");

            Assert.AreEqual(result, tempDir + "someotherpath\\somefile.csproj");
        }

        [TestMethod]
        public void TestFileHelperPathRelativeWithFileName4()
        {
            var tempDir = Path.GetTempPath();

            Directory.SetCurrentDirectory(tempDir);

            var result = FileRepo.PathOffset("\\someotherpath/somefile.csproj");

            Assert.AreEqual(result, tempDir + "someotherpath\\somefile.csproj");
        }
    }
}
