using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using RYoshiga.Demo.Infrastructure;
using Xunit;
using Shouldly;

namespace RYoshiga.Demo.AdapterTests
{
    public class StorageAccountFileSaverShould
    {
        private readonly StorageAccountFileManager _fileManager;

        public StorageAccountFileSaverShould()
        {
            var storageAccountConfiguration = new StorageAccountConfiguration()
            {
                ConnectionString = "UseDevelopmentStorage=true"
            };
            _fileManager = new StorageAccountFileManager(storageAccountConfiguration);
        }

        [Theory]
        [InlineData("test.pdf")]
        [InlineData("another.pdf")]
        public async Task SaveFile(string uploadFileName)
        {
            var testFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "testFile.pdf");

            await _fileManager.Save(uploadFileName, File.OpenRead(testFilePath));
            
            var readFile = await _fileManager.Read(uploadFileName);
            var actual = ReadFully(readFile);
            var expected = ReadFully(File.OpenRead(testFilePath));
            actual.ShouldBe(expected);

            await _fileManager.Delete(uploadFileName);
        }

        public static byte[] ReadFully(Stream input)
        {
            using (var ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }


}
