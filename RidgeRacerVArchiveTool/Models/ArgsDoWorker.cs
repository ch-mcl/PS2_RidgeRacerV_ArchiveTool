using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RidgeRacerVArchiveTool.Models
{
    class ArgsDoWorker
    {
        public string elfPath;
        public string arcPath;
        public int tocAddress;
        public int fileCount;
        public string destPath;
        public string srcPath;

        public ArgsDoWorker()
        {
            destPath = "";
            srcPath = "";
        }

    }
}
