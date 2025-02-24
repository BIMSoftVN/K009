using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static iHR.Test.ChucNang;


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
            int SoNguyen2 = (int)SoThuc;


            Console.WriteLine(SoNguyen2);
        }

        enum Season
        {
            Spring = 100,
            Summer = 200,
            Autumn = 300,
            Winter = 400
        }

        struct Point
        {
            public double X;
            public double Y;
        }


        [Test]
        public void TestVar()
        {
            //bool? IsSunny = null;

            //double Sunny = Convert.ToDouble(IsSunny);


            //char KyTu = 'T';
            //string Chuoi = "Tôi là K";

            //Season Mua = Season.Autumn;

            //Point Diem1 = new Point { X = 100, Y = 200 };
            //Point Diem2 = new Point { X = 200, Y = 500 };

            //double x = 10;
            //double y = 3;

            //bool ss1 = false;
            //bool ss2 = false;

            //Console.WriteLine((!ss1));

            //int Age = 16;

            //if (Age>=18)
            //{
            //    Console.WriteLine("Đã đủ tuổi lái xe");
            //}
            //else if (Age >= 16)
            //{
            //    Console.WriteLine("Đủ tuổi tập xe");
            //}
            //else
            //{
            //    Console.WriteLine("Không đủ tuổi lái xe");
            //}   


            //string DayOfWeek = "Thursday";


            //switch (DayOfWeek)
            //{
            //    case "Monday":
            //        Console.WriteLine("Đầu tuần");
            //        break;
            //    case "Tuesday":
            //    case "Wednesday":
            //    case "Thursday":
            //    case "Friday":
            //        Console.WriteLine("Ngày trong tuần");
            //        break;
            //    case "Saturday":
            //    case "Sunday":
            //        Console.WriteLine("Cuối tuần");
            //        break;
            //    default:
            //        Console.WriteLine("Không xác định");
            //        break;
            //}


            for (long i = 1; i <= 10; i += 1)
            {
                Console.WriteLine("Guid " + Guid.NewGuid().ToString());
            }

            //string[] strings = new string[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

            //foreach (string s in strings)
            //{
            //    if (s == "Thursday")
            //    {
            //        break;
            //    }

            //    Console.WriteLine(s);
            //}

            Console.WriteLine(10 > 3);

            //for (int i = 0; i < strings.Length; i++)
            //{
            //    Console.WriteLine(strings[i]);
            //}

            //Console.WriteLine("Lần thứ 1");
            //Console.WriteLine("Lần thứ 2");
            //Console.WriteLine("Lần thứ 3");
            //Console.WriteLine("Lần thứ 4");
            //Console.WriteLine("Lần thứ 5");

            //int i = 1;

            //while (i <= 10)
            //{
            //    if (i == 5)
            //    {
            //        i++;
            //        break;
            //    }

            //    Console.WriteLine("Lần thứ " + i);
            //    i++;
            //} 
        }

        [Test]
        public void TestMethod2()
        {
            int kWh = 250;
            int[] levels = { 50, 100, 200, 300, 400 };
            double[] prices = { 1806, 1866, 2167, 2729, 3050, 3151 };

            var totalCost = CalculateElectricityBill(kWh, levels, prices);
            Console.WriteLine($"Số tiền phải trả: {totalCost:N0} đồng");
        }

    }

    public static class ChucNang
    {
        public static double CalculateElectricityBill(int kWh, int[] levels, double[] prices)
        {
            double totalCost = 0;
            int previousLevel = 0;
            int SoDienConLai = kWh;

            for (int i = 0; i < levels.Length; i++)
            {
                if (SoDienConLai > 0)
                {
                    int SoDien = Math.Min((levels[i] - previousLevel) , SoDienConLai);
                    totalCost += SoDien * prices[i];
                    previousLevel = levels[i];
                    SoDienConLai = SoDienConLai - SoDien;
                }
            }

            return totalCost * 1.08;
        }
    }

}
