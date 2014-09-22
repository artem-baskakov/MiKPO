using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using met1;
using System.IO;

namespace metTest1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CalcTriangle_Test()
        {
            double a = 3, b = 4, gamma = 90;
            double[] res = Program.CalcTriangle(a, b, gamma);
            double[] expected = new double[] { 3, 4, 5, 36.87, 53.13, 90 };
            CollectionAssert.AreEqual(expected, res);
        }

        [TestMethod]
        public void EmptyFile_Test()
        {
            Program.work("../../../met1/bin/Debug/empty.txt", "test.txt");
            string s = File.ReadAllText("test.txt");
            Assert.AreEqual(s, "");
        }

        [TestMethod]
        public void File_Test()
        {
            Program.work("../../../met1/bin/Debug/a.txt", "test.txt");
            string res = File.ReadAllText("test.txt");
            string expected = File.ReadAllText("normal_a.txt");
            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void FileNotExists_Test()
        {
            Program.work("123.txt", "test.txt");            
        }

        [TestMethod]
        public void BigFile_Test()
        {
            Program.work("../../../met1/bin/Debug/big.txt", "test.txt");
            string res=File.ReadAllText("test.txt");
            Assert.IsTrue(res.Length > 0);
        }

    }
}
