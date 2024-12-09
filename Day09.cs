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



        (List<FileIndex> files, List<FileIndex> empty) GetFiles(int [] filelist)
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

                var (files, empty) = GetFiles(values);

                //var debug = new StringBuilder(new string('.', files.Sum(f => f.Length)));

                int emptyID = 0;
                int count = empty[emptyID].Length;
                int i = empty[emptyID].Index;    
                
                for (int fileID = files.Count - 1; fileID >= 0; fileID--)
                {
                    int fileLength = files[fileID].Length;

                    if (emptyID >= empty.Count || empty[emptyID].Index >= files[fileID].Index)
                    {                                
                        emptyID = empty.Count;
                        i = files[fileID].Index;
                    }
                    
                    while (fileLength > 0)
                    {
                        //debug[i] = (char)('0' + fileID); 
                        result += i * fileID;

                        fileLength--;
                        count--;
                        
                        if(fileLength >= 0) i++;
                        
                        if (count == 0)
                        {
                            emptyID++;
                            if (emptyID < empty.Count && empty[emptyID].Index < files[fileID].Index)
                            {
                                count = empty[emptyID].Length;
                                i = empty[emptyID].Index;    
                            }
                            else
                            {
                                emptyID = empty.Count;
                                count = -1;
                                i = files[fileID].Index;
                            }
                        }
                    }
                }

                //Console.WriteLine(debug.ToString());
                Assert.AreEqual(result, 1928); //4851270996170 is too low   6341711076053 is too high  
                //                                                          6341711049568 is wrong
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
