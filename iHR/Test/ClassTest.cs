using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace iHR.Test
{
    [TestFixture]
    public class ClassTest
    {
        [Test]
        public void TestMethod()
        {
            //string Name = "Phong Vũ 2";
            //int i = 10;
            //int j = 20;
            //int k = i + j;

            int SoNguyen = 20;
            double SoThuc = SoNguyen;
            int SoNguyen2 = SoThuc;


            Console.WriteLine(SoNguyen2);
        }
    }
}
