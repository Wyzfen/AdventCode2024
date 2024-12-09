using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2024
{
    [TestClass]
    public class Day09
    {
        private static readonly string day = System.Reflection.MethodBase.GetCurrentMethod()!.DeclaringType!.Name.ToLower();
        readonly int [] values =  File.ReadAllText($"{day}.txt").Select(c => c - '0').ToArray();

        readonly int [] test = "2333133121414131402".Select(c => c - '0').ToArray();

        private record struct FileIndex(int Index, int Length);



        (List<FileIndex> files, List<FileIndex> free) GetFiles(int [] filelist)
        {
            var files = new List<FileIndex>();
            var empty = new List<FileIndex>();
            int index = 0;
            
            for (int i = 0; i < filelist.Length; i++)
            {
                var length = filelist[i];
                var file = new FileIndex(index, length);

                if ((i & 1) == 0)
                {
                    files.Add(file);
                }
                else if(length > 0)
                {
                    empty.Add(file);
                }

                index += length;
            }
            
            return (files, empty);
        }
        
        [TestMethod]
        public void Problem1()
        {
            checked
            {
                long result = 0;

                var (files, free) = GetFiles(values);

                IEnumerator<FileIndex> freeSpace = free.GetEnumerator();
                freeSpace.MoveNext();
                var (index, count) = freeSpace.Current;
                
                for (int fileId = files.Count - 1; fileId >= 0; fileId--)
                {
                    int fileLength = files[fileId].Length;
                    
                    for(; fileLength > 0; fileLength--)
                    {
                        if (index >= files[fileId].Index) break;
                        
                        result += index++ * fileId;

                        if (--count == 0 && freeSpace.MoveNext())
                        {
                            (index, count) = freeSpace.Current;
                        }
                    }
                    
                    if (fileLength > 0) // Ran out of space to the left - checksum in place
                    {
                        index = files[fileId].Index;
                        for (; fileLength > 0; fileLength--, index++)
                        {
                            result += index * fileId;
                        }
                    }
                }

                Assert.AreEqual(result, 6341711060162);
            }
        }

        [TestMethod]
        public void Problem2()
        {
            int result = 0;

            Assert.AreEqual(result, 4267809);
        }
    }
}
