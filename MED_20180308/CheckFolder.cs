using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MED_20180308
{
    class CheckFolder
    {
        public void Run(string refPath, string hypPath, string outputPath)
        {
            var list = CompareRootFolder(refPath, hypPath);
            File.WriteAllLines(outputPath, list);
        }

        private IEnumerable<string> CompareRootFolder(string refRootPath, string hypRootPath)
        {
            foreach(string refFolderPath in Directory.EnumerateDirectories(refRootPath))
            {
                string folderName = refFolderPath.Split('\\').Last();
                string hypFolderPath = Path.Combine(hypRootPath, folderName);
                foreach(var item in CompareFolder(refFolderPath, hypFolderPath))
                {
                    yield return item;
                }
            }
        }

        private IEnumerable<string> CompareFolder(string refFolderPath, string hypFolderPath)
        {
            string folderName = refFolderPath.Split('\\').Last();
            var refList = Directory.GetFiles(refFolderPath);
            var hypList = Directory.GetFiles(hypFolderPath);
            for(int i = 0; i < refList.Length; i++)
            {
                string hypFilePath = hypList[i];
                string refFilePath = refList[i];
                yield return CompareFile(refFilePath, hypFilePath, folderName);
            }
        }

        private string CompareFile(string refFilePath, string hypFilePath, string folderName)
        {
            string refString = File.ReadAllText(refFilePath).Replace("[XXX]", string.Empty);
            string hypString = File.ReadAllText(hypFilePath).Replace("[XXX]", string.Empty);
            MED med = new MED();
            med.Run(refString, hypString);
            string refFileName = refFilePath.Split('\\').Last();
            string hypFileName = hypFilePath.Split('\\').Last();
            return folderName + "\t" + refFileName + "\t" + hypFileName+"\t"
                + med.INS + "\t" + med.DEL + "\t" + med.SUB + "\t"
                + med.ERR + "\t" + med.REF + "\t" + (med.ErrorRate * 100).ToString("0.00");
        }
    }
}
