using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data;

namespace AoC.Utils.OCR;

[TestClass]
public class TextReaderTests
{

    [TestMethod]
    [TestCategory("OCR")]
    /*
    ##    ##    ####      ####    ##    ##  ##    ##  
    ##    ##  ##    ##  ##    ##  ##    ##  ##    ##  
    ##    ##  ##        ##        ##    ##  ########  
    ##    ##  ##  ####  ##        ##    ##  ##    ##  
    ##    ##  ##    ##  ##    ##  ##    ##  ##    ##  
      ####      ######    ####      ####    ##    ##  
    */
    [DataRow("UGCUH", "##    ##    ####      ####    ##    ##  ##    ##  \r\n##    ##  ##    ##  ##    ##  ##    ##  ##    ##  \r\n##    ##  ##        ##        ##    ##  ########  \r\n##    ##  ##  ####  ##        ##    ##  ##    ##  \r\n##    ##  ##    ##  ##    ##  ##    ##  ##    ##  \r\n  ####      ######    ####      ####    ##    ##  ")]

    /*
        ####    ######    ########  ##    ##  ######    ##    ##  ######    ######        
      ##    ##  ##    ##  ##        ##  ##    ##    ##  ##  ##    ##    ##  ##    ##      
      ##    ##  ##    ##  ######    ####      ##    ##  ####      ######    ##    ##      
      ########  ######    ##        ##  ##    ######    ##  ##    ##    ##  ######        
      ##    ##  ##        ##        ##  ##    ##  ##    ##  ##    ##    ##  ##  ##        
      ##    ##  ##        ##        ##    ##  ##    ##  ##    ##  ######    ##    ##      
    */
    [DataRow("APFKRKBR", "    ####    ######    ########  ##    ##  ######    ##    ##  ######    ######        \r\n  ##    ##  ##    ##  ##        ##  ##    ##    ##  ##  ##    ##    ##  ##    ##      \r\n  ##    ##  ##    ##  ######    ####      ##    ##  ####      ######    ##    ##      \r\n  ########  ######    ##        ##  ##    ######    ##  ##    ##    ##  ######        \r\n  ##    ##  ##        ##        ##  ##    ##  ##    ##  ##    ##    ##  ##  ##        \r\n  ##    ##  ##        ##        ##    ##  ##    ##  ##    ##  ######    ##    ##      ")]


    public void TestRead(string expected, string data)
    {
        Assert.AreEqual(expected, OCR.TextReader.Read(data));
    }

    [TestMethod]
    [TestCategory("OCR")]
    [DataRow('A', " ## \r\n#  #\r\n#  #\r\n####\r\n#  #\r\n#  #")]
    [DataRow('B', "### \r\n#  #\r\n### \r\n#  #\r\n#  #\r\n### ")]
    [DataRow('C', " ## \r\n#  #\r\n#   \r\n#   \r\n#  #\r\n ## ")]
    [DataRow('E', "####\r\n#   \r\n### \r\n#   \r\n#   \r\n####")]
    [DataRow('F', "####\r\n#   \r\n### \r\n#   \r\n#   \r\n#   ")]
    [DataRow('G', " ## \r\n#  #\r\n#   \r\n# ##\r\n#  #\r\n ###")]
    [DataRow('H', "#  #\r\n#  #\r\n####\r\n#  #\r\n#  #\r\n#  #")]
    [DataRow('I', " ###\r\n  # \r\n  # \r\n  # \r\n  # \r\n ###")]
    [DataRow('J', "  ##\r\n   #\r\n   #\r\n   #\r\n#  #\r\n ## ")]
    [DataRow('K', "#  #\r\n# # \r\n##  \r\n# # \r\n# # \r\n#  #")]
    [DataRow('L', "#   \r\n#   \r\n#   \r\n#   \r\n#   \r\n####")]
    [DataRow('O', " ## \r\n#  #\r\n#  #\r\n#  #\r\n#  #\r\n ## ")]
    [DataRow('P', "### \r\n#  #\r\n#  #\r\n### \r\n#   \r\n#   ")]
    [DataRow('R', "### \r\n#  #\r\n#  #\r\n### \r\n# # \r\n#  #")]
    [DataRow('S', " ###\r\n#   \r\n#   \r\n ## \r\n   #\r\n### ")]
    [DataRow('U', "#  #\r\n#  #\r\n#  #\r\n#  #\r\n#  #\r\n ## ")]
    [DataRow('Z', "####\r\n   #\r\n  # \r\n #  \r\n#   \r\n####")]
    public void DoAlpha(char expected, string input)
    {
        Assert.AreEqual(expected.ToString(), OCR.TextReader.Read(input));
    }

}
