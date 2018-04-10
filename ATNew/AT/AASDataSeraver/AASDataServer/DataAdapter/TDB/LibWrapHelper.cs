using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AASDataServer.DataAdapter.TDB
{
    public class LibWrapHelper
    {
        //nDstLen指定目标字符串的长度，当<=0时，为str.Length;
        public static byte[] String2AnsiArr(string str, int nDstLen)
        {
            Encoding coderMBCS = Encoding.GetEncoding(936);
            byte[] src = coderMBCS.GetBytes(str);
            if (nDstLen <= 0)
            {
                nDstLen = str.Length;
            }
            byte[] dst = new byte[nDstLen];
            src.CopyTo(dst, 0);
            return dst;
        }

        public static string AnsiArr2String(byte[] btArr, int nStart, int nLen)
        {
            if (nLen <= 0)
            {
                return "";
            }

            int nLenExcludeZero = 0;
            for (int i = nStart; i < nStart + nLen; i++)
            {
                if (btArr[i] > 0)
                {
                    nLenExcludeZero++;
                }
                else
                {
                    break;
                }
            }
            byte[] dstArr = new byte[nLenExcludeZero];
            System.Array.Copy(btArr, nStart, dstArr, 0, nLenExcludeZero);
            Encoding coderMBCS = Encoding.GetEncoding(936);
            return coderMBCS.GetString(dstArr);
        }


        public static IntPtr CopyStructToGlobalMem(object obj, System.Type typeInfo)
        {
            IntPtr pRet = Marshal.AllocHGlobal(Marshal.SizeOf(typeInfo));
            Marshal.StructureToPtr(obj, pRet, false);
            return pRet;
        }

        public static int[] CopyIntArr(object intArray)
        {
            int[] nums = (int[])intArray;
            int[] nRet = new int[nums.Length];
            System.Array.Copy(nums, nRet, nums.Length);
            return nRet;
        }

        public static uint[] CopyUIntArr(object uintArray)
        {
            uint[] nums = (uint[])uintArray;
            uint[] nRet = new uint[nums.Length];
            System.Array.Copy(nums, nRet, nums.Length);
            return nRet;
        }
    }
}
