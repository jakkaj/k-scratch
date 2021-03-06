﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ks.model.Contract;
using ks.model.Contract.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ks.unittests.Tests
{
    [TestClass]
    public class SendFileTests : TestBase
    {
        [TestMethod]
        public async Task Monitor()
        {
            await Init();

            var s = Resolve<IKuduFileService>();

            s.Monitor();

            await Task.Delay(40000);
        }
        
        [TestMethod]
        public async Task SendZip()
        {
            await Init();

            var s = Resolve<IKuduFileService>();

            var dir = Directory.GetCurrentDirectory();

            var result = await s.UploadFiles();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task SendFile()
        {
            await Init();

            var s = Resolve<IKuduFileService>();

            var dir = Directory.GetCurrentDirectory();

            var full = Path.Combine(dir, "Timelapser\\test.txt");

            await s.SendFile(full);
        }
    }
}
