using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MED_20180308
{
    class MED
    {
        public int INS { get; private set; } = 0;
        public int DEL { get; private set; } = 0;
        public int SUB { get; private set; } = 0;
        public int REF { get; private set; } = 0;
        public int ERR => INS + DEL + SUB;
        public int HYP { get; private set; } = 0;
        public double ErrorRate => 100.0 * ERR / REF;
        public MED() { }

        public void Run<T>(IEnumerable<T> refs, IEnumerable<T> hyps, bool useLocal=true)
        {
            var refArray = refs.ToArray();
            var hypArray = hyps.ToArray();
            REF = refArray.Length;
            HYP = hypArray.Length;
            if (REF == 0)
            {
                throw new Exception("Reference is empty.");
            }
            if (HYP == 0)
            {
                throw new Exception("Hypothesis is empty.");
            }
            else
            {
                if (useLocal)
                {
                    DpLocal(refArray, hypArray);
                }
                else
                {
                    var matrix = DP(refArray, hypArray);
                    BackTrack(matrix, refArray, hypArray);
                }
            }
        }

        private void BackTrack<T>(int[,] matrix, T[] refArray, T[] hypArray)
        {
            int r = refArray.Length;
            int h = hypArray.Length;
            while (r >= 0 && h >= 0)
            {
                if (r == 0 && h == 0)
                    break;
                if (r == 0)
                {
                    h--;
                    INS++;
                    continue;
                }
                if (h == 0)
                {
                    r--;
                    DEL++;
                    continue;
                }
                if (refArray[r - 1].Equals(hypArray[h - 1]))
                {
                    r--;
                    h--;
                    continue;
                }
                int current = matrix[r, h];
                int preIns = matrix[r, h - 1];
                int preDel = matrix[r - 1, h];
                int preSub = matrix[r - 1, h - 1];
                if (current == preIns + 1)
                {
                    h--;
                    INS++;
                    continue;
                }
                if (current == preDel + 1)
                {
                    r--;
                    DEL++;
                    continue;
                }
                r--;
                h--;
                SUB++;
            }
        }

        private int[,] DP<T>(T[] refArray, T[] hypArray)
        {
            int[,] matrix = new int[REF + 1, HYP + 1];
            for(int i = 1; i <= REF; i++)
            {
                matrix[i, 0] = i;
            }
            for(int j = 1; j <= HYP; j++)
            {
                matrix[0, j] = j;
            }
            for (int i = 1; i < REF + 1; i++)
            {
                for (int j = 1; j < HYP + 1; j++)
                {
                    int left = matrix[i, j - 1] + 1;
                    int down = matrix[i - 1, j] + 1;
                    int diag = matrix[i - 1, j - 1] + (refArray[i - 1].Equals(hypArray[j - 1]) ? 0 : 1);
                    matrix[i, j] = Math.Min(Math.Min(left, down), diag);
                }
            }
            return matrix;
        }
        
        private void DpLocal<T>(T[] refArray, T[] hypArray)
        {
            int n = refArray.Length + 1;
            Cell[] Row = new Cell[n];
            for (int i = 0; i < n; i++)
                Row[i] = new Cell { DEL = i };
            for(int i = 1; i < hypArray.Length + 1; i++)
            {
                var diagCell = Row[0].Clone();
                Row[0] = new Cell { INS = i };
                for(int j = 1; j < n; j++)
                {
                    var bottomCell = Row[j];
                    var leftCell = Row[j - 1];

                    bool equalFlag = refArray[j-1].Equals(hypArray[i-1]);
                    int diag = diagCell.ERR + (equalFlag ? 0 : 1);
                    int left = leftCell.ERR + 1;
                    int bottom = bottomCell.ERR + 1;
                    int min = Math.Min(diag, Math.Min(left, bottom));
                    Cell current = new Cell();
                    if (left == min)
                    {
                        current = leftCell.Clone();
                        current.DEL++;
                    }
                    else if (bottom == min)
                    {
                        current = bottomCell.Clone();
                        current.INS++;
                    }
                    else
                    {
                        current = diagCell.Clone();
                        if (!equalFlag)
                            current.SUB++;
                    }

                    diagCell = Row[j].Clone();
                    Row[j] = current.Clone();
                }
            }

            DEL = Row[n - 1].DEL;
            INS = Row[n - 1].INS;
            SUB = Row[n - 1].SUB;
        }
    }

    class Cell
    {
        public int INS { get; set; } = 0;
        public int SUB { get; set; } = 0;
        public int DEL { get; set; } = 0;
        public int ERR => INS + SUB + DEL;
        public Cell Clone()
        {
            return new Cell { INS = INS, SUB = SUB, DEL = DEL };
        }
    }
}
