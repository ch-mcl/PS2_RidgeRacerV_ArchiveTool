using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RidgeRacerVArchiveTool.RR5_Lib
{
    class RR5_TOC_Table
    {
        public DataTable tocTable;

        /// <summary>
        /// TOC Table.
        /// </summary>
        public const string TBL_NAME = "RR5 TOC Address";
        /// <summary>
        /// Region.
        /// </summary>
        public string COL_NAME_REGION = "region";
        /// <summary>
        /// Archive File name.
        /// </summary>
        public string COL_NAME_ARC = "arcName";
        /// <summary>
        /// elf file name.
        /// </summary>
        public string COL_NAME_ELF = "elfName";
        /// <summary>
        /// TOC address at elf file.
        /// </summary>
        public string COL_NAME_TOC_ADR = "tocAddress";
        /// <summary>
        /// Max of TOC Entrys.
        /// </summary>
        public string COL_NAME_MAX_TOC = "maxCount";

        public RR5_TOC_Table()
        {
            DataSet dataSet = new DataSet();
            tocTable = new DataTable(TBL_NAME);

            tocTable.Columns.Add(COL_NAME_REGION);  // 0
            tocTable.Columns.Add(COL_NAME_ARC);     // 1
            tocTable.Columns.Add(COL_NAME_ELF);     // 2
            tocTable.Columns.Add(COL_NAME_TOC_ADR, Type.GetType("System.Int32")); // 3
            tocTable.Columns.Add(COL_NAME_MAX_TOC, Type.GetType("System.Int32")); // 4

            dataSet.Tables.Add(tocTable);

            // JP
            DataRow row = tocTable.NewRow();
            row[COL_NAME_REGION] = "JP";
            row[COL_NAME_ARC] = "R5.ALL";
            row[COL_NAME_ELF] = "SLPS_200.01";
            row[COL_NAME_TOC_ADR] = 0x10BFE8;
            row[COL_NAME_MAX_TOC] = 1136;
            dataSet.Tables[TBL_NAME].Rows.Add(row);

            // USA
            row = tocTable.NewRow();
            row[COL_NAME_REGION] = "US";
            row[COL_NAME_ARC] = "R5.ALL";
            row[COL_NAME_ELF] = "SLUS_200.02";
            row[COL_NAME_TOC_ADR] = 0x10D258;
            row[COL_NAME_MAX_TOC] = 1136;
            dataSet.Tables[TBL_NAME].Rows.Add(row);

            // PAL
            row = tocTable.NewRow();
            row[COL_NAME_REGION] = "PAL";
            row[COL_NAME_ARC] = "R5.ALL";
            row[COL_NAME_ELF] = "SCES_500.00";
            row[COL_NAME_TOC_ADR] = 0x1103B8;
            row[COL_NAME_MAX_TOC] = 1208;
            dataSet.Tables[TBL_NAME].Rows.Add(row);

            // AC_RRV3_A (Arcade Battle RRV3 Ver A)
            row = tocTable.NewRow();
            row[COL_NAME_REGION] = "AC_RRV3_A";
            row[COL_NAME_ARC] = "RRV1_A";
            row[COL_NAME_ELF] = "rrv3vera.ic002";
            row[COL_NAME_TOC_ADR] = 0x1AB398;
            row[COL_NAME_MAX_TOC] = 1155;
            dataSet.Tables[TBL_NAME].Rows.Add(row);

        }

    }

}
