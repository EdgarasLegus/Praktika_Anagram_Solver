using AnagramSolver.Interfaces;
using AnagramSolver.WebApp.Controllers;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AnagramSolver.Tests.Controllers
{
    public class DownloadControllerTests
    {
        DownloadController _downloadController;

        [SetUp]
        public void Setup()
        {
            _downloadController = new DownloadController();
        }

        [Test]
        public void Test_IfDownloadedContent_IsSameLengthAsOriginalFile()
        {
            var existingFileLength = File.ReadAllLines(@"./zodynas.txt").Length;

            var downloadedFile = _downloadController.DownloadDictionary() as FileContentResult;
            var downloadedFileContents = downloadedFile.FileContents;

            var fileContentsToString = Encoding.Default.GetString(downloadedFileContents);
            var downloadedFileLength = string.Join('\n', fileContentsToString).Count(f => f == '\n');

            Assert.AreEqual(existingFileLength, downloadedFileLength);
        }
    }
}
