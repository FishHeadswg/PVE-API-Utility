using Microsoft.VisualStudio.TestTools.UnitTesting;
using PVEAPIUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PVEAPIUtility.Tests
{
    [TestClass()]
    public class Form1Tests
    {
        [TestMethod()]
        public void NextColorTest()
        {
            var form1 = new PVEAPIForm();
            int five = 5;
            form1.NextColor(ref five);
            Assert.IsFalse(five > 3);
        }
    }
}