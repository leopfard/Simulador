﻿using System;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;

namespace Simulador.Classes
{
    class ExcelHelper
    {
        public struct SheetInformations
        {
            public string file;
            public string indexStr;
            public int indexInt;
            public string newName;
        }
        public void createFile(SheetInformations[] sheetInformationsArray, string destFile)
        {
            Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
            Workbook curWorkBook = null;
            Workbook destWorkbook = null;
            Worksheet workSheet = null;
            Worksheet newWorksheet = null;
            Object defaultArg = Type.Missing;

            try
            {
                // Criando Arquivo Excel se ele não existe
                Workbook tmpWorkBook = app.Workbooks.Add(defaultArg);
                // newWorksheet = (Worksheet) destWorkbook.Worksheets[1];
                // newWorksheet.Delete();
                tmpWorkBook.SaveAs(destFile);
                tmpWorkBook.Close(true, defaultArg, defaultArg);
                Marshal.ReleaseComObject(tmpWorkBook);
                foreach (var sheetInfo in sheetInformationsArray)
                {
                    Console.WriteLine(sheetInfo.file);
                    Console.WriteLine(sheetInfo.newName);
                    Console.WriteLine(sheetInfo.indexStr);
                    Console.WriteLine();

                    curWorkBook = app.Workbooks.Open(sheetInfo.file,
                        defaultArg, defaultArg, defaultArg, defaultArg,
                        defaultArg, defaultArg, defaultArg, defaultArg, defaultArg, defaultArg, defaultArg, defaultArg,
                        defaultArg, defaultArg);
                    workSheet = (Worksheet)curWorkBook.Sheets[sheetInfo.indexStr];
                    workSheet.UsedRange.Copy(defaultArg);


                    destWorkbook = app.Workbooks.Open(
                        destFile, defaultArg, false,
                        defaultArg, defaultArg,
                        defaultArg, defaultArg, defaultArg, defaultArg, defaultArg, defaultArg, defaultArg, defaultArg,
                        defaultArg, defaultArg);
                    newWorksheet = (Worksheet)destWorkbook.Worksheets.Add(defaultArg, defaultArg, defaultArg, defaultArg);
                    newWorksheet.Name = sheetInfo.newName;
                    // newWorksheet = (Worksheet) destWorkbook.Worksheets[sheetInfo.newName];
                    newWorksheet.UsedRange._PasteSpecial(XlPasteType.xlPasteValues,
                        XlPasteSpecialOperation.xlPasteSpecialOperationNone, false, false);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (curWorkBook != null)
                {
                    curWorkBook.Save();
                    curWorkBook.Close(defaultArg, defaultArg, defaultArg);
                }

                if (destWorkbook != null)
                {
                    destWorkbook.Save();
                    destWorkbook.Close(defaultArg, defaultArg, defaultArg);
                }
            }

            Marshal.ReleaseComObject(curWorkBook);
            Marshal.ReleaseComObject(destWorkbook);
            app.Quit();
        }
    }
}
