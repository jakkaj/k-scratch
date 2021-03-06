﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ks.model.Contract;
using ks.model.Contract.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ks.unittests.Tests
{
    [TestClass]
    public class FileGetTests : TestBase
    {
        [TestMethod]
        public async Task GetFiles()
        {
            await Init();

            var s = Resolve<IKuduFileService>();

            await s.ListFiles();


        }

        [TestMethod]
        public async Task DownloadFiles()
        {
            await Init();

            var s = Resolve<IKuduFileService>();

            await s.GetFiles();


        }
    }
}
