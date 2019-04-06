using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Diagnostics;
using System.ComponentModel;
using System.Threading;


namespace ChildrenMathProblem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        enum MagicNumber : int { zero=0,one,two,three,four,five,six,seven,eight,nine};
        internal class ChildrenMathmechics
        {
            protected internal int SearchRangeMin;
            protected internal int SearchRangeMax;

            protected internal Dictionary<string, int> Answer = new Dictionary<string, int>();
            protected internal List<int[]> DictAns = new List<int[]>();

            protected internal Dictionary<string, int>.ValueCollection value;

            private void IntiDictionary()
            {
                Answer["zero" ] = 1;//0
                Answer["one"  ] = 0;//1
                Answer["two"  ] = 0;//2
                Answer["three"] = 0;//3
                Answer["four" ] = 1;//4
                Answer["five" ] = 0;//5
                Answer["six"  ] = 1;//6
                Answer["seven"] = 0;//7
                Answer["eight"] = 2;//8
                Answer["nine" ] = 1;//9
            }

            public ChildrenMathmechics(int max, int min)
            {
                SearchRangeMax = max;
                SearchRangeMin = min;

                for (int i = 0; i < 10; i++)
                {
                    Answer.Add(((MagicNumber)i).ToString(), 0); //init all values to 0
                }

                //IntiDictionary();

                value = Answer.Values;
            }

            public void PrintAnswer(Paragraph para)
            {
                foreach (KeyValuePair<string, int> kvp in Answer)
                {
                    para.Inlines.Add($"\nValue of the Key {{{kvp.Key}/{(int)Enum.Parse(typeof(MagicNumber), kvp.Key, true)}}} is : {kvp.Value}");
                }
            }

            public void UpdateValues(int[] input)
            {
                for (int i = 0; i < 10; i++)
                {
                    Answer[((MagicNumber)i).ToString()] = input[i];
                }
            }

            public int Calculate(int input)
            {
                int temp = 0;
                if (input == 0)
                {
                    temp = 4 * Answer["zero"];
                }
                else if ((input > 9999) && (input < 1000))
                {
                    Debug.WriteLine("数据范围异常");
                }
                else
                {
                    for (int i = 0; i < 4; i++)
                    {
                        temp += Answer[((MagicNumber)(input % 10)).ToString()];
                        input /= 10;
                    }
                }

                return temp;
            }

            public bool NumSpecialCompare(int digital, int ans)
            {
                return Calculate(digital) == ans;
            }
        }

        public BackgroundWorker bgworker = new BackgroundWorker();

        internal ChildrenMathmechics Problem = new ChildrenMathmechics(2, 0);

        public MainWindow()
        {
            InitializeComponent();
        }

        public void InitWork()
        {
            bgworker.WorkerReportsProgress = true;
            bgworker.WorkerSupportsCancellation = true;
            bgworker.DoWork += new DoWorkEventHandler(DoWork);
            bgworker.ProgressChanged += new ProgressChangedEventHandler(BgworkChange);
            bgworker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BgworkCompleted);
        }

        public void DoWork(object sender, DoWorkEventArgs e)
        {
            CrackMagicNum(Problem, e, (int i) => bgworker.ReportProgress(i));
        }

        public void BgworkChange(object sender, ProgressChangedEventArgs e)
        {
            pbProgressBar.Value = e.ProgressPercentage;
        }

        public void BgworkCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ParagraphTips.Inlines.Clear();

            if (e.Error != null)
            {
                ParagraphTips.Inlines.Add($"\n---------->>>计算失败:{e.Error}");
            }
            else if (e.Cancelled)
            {
                ParagraphTips.Inlines.Add("\n---------->>>计算取消!");
            }
            else
            {
                for (int i = 0; i < Problem.DictAns.Count; i++)
                {
                    ParagraphResult.Inlines.Add($"\nAnswer {i} :");
                    Problem.UpdateValues(Problem.DictAns[i]);
                    Problem.PrintAnswer(ParagraphResult);
                    ParagraphResult.Inlines.Add($"\n计算答案 : 2581 = {Problem.Calculate(2581)}");
                }

                ParagraphTips.Inlines.Add($"\n---------->>>计算结束:{e.Result}");
            }

            btStartCalculate.IsEnabled = true;
            btnUserAbort.IsEnabled = false;
        }

        internal void CrackMagicNum(ChildrenMathmechics ChildProblem, DoWorkEventArgs e, Action<int> UpdatePercent )
        {
            int[,] Material = {
                { 8809, 6},
                { 7111, 0},
                { 2172, 0},
                { 6666, 4},
                { 1111, 0},
                { 3213, 0},
                { 7662, 2},
                { 9313, 1},
                { 0000, 4},
                { 2222, 0},
                { 3333, 0},
                { 5555, 0},
                { 8193, 3},
                { 8096, 5},
                { 1012, 1},
                { 7777, 0},
                { 9999, 4},
                { 7756, 1},
                { 6855, 3},
                { 9881, 5},
                { 5531, 0}
                //{ 2581, 2},
            };

            Debug.WriteLine("information:");
            Debug.WriteLine($"Material.GetLength(0) = {Material.GetLength(0)}    Material.GetLength(1) = {Material.GetLength(1)}");

            #region Issta

            ulong total = (ulong)Math.Pow(ChildProblem.SearchRangeMax - ChildProblem.SearchRangeMin + 1, 10);
            ulong time = 0;

            for (int i0 = ChildProblem.SearchRangeMin; i0 <= ChildProblem.SearchRangeMax; i0++)
            {
                for (int i1 = ChildProblem.SearchRangeMin; i1 <= ChildProblem.SearchRangeMax; i1++)
                {
                    for (int i2 = ChildProblem.SearchRangeMin; i2 <= ChildProblem.SearchRangeMax; i2++)
                    {
                        for (int i3 = ChildProblem.SearchRangeMin; i3 <= ChildProblem.SearchRangeMax; i3++)
                        {
                            for (int i4 = ChildProblem.SearchRangeMin; i4 <= ChildProblem.SearchRangeMax; i4++)
                            {
                                for (int i5 = ChildProblem.SearchRangeMin; i5 <= ChildProblem.SearchRangeMax; i5++)
                                {
                                    for (int i6 = ChildProblem.SearchRangeMin; i6 <= ChildProblem.SearchRangeMax; i6++)
                                    {
                                        for (int i7 = ChildProblem.SearchRangeMin; i7 <= ChildProblem.SearchRangeMax; i7++)
                                        {
                                            for (int i8 = ChildProblem.SearchRangeMin; i8 <= ChildProblem.SearchRangeMax; i8++)
                                            {
                                                for (int i9 = ChildProblem.SearchRangeMin; i9 <= ChildProblem.SearchRangeMax; i9++)
                                                {
                                                    ChildProblem.Answer["zero" ] = i0;
                                                    ChildProblem.Answer["one"  ] = i1;
                                                    ChildProblem.Answer["two"  ] = i2;
                                                    ChildProblem.Answer["three"] = i3;
                                                    ChildProblem.Answer["four" ] = i4;
                                                    ChildProblem.Answer["five" ] = i5;
                                                    ChildProblem.Answer["six"  ] = i6;
                                                    ChildProblem.Answer["seven"] = i7;
                                                    ChildProblem.Answer["eight"] = i8;
                                                    ChildProblem.Answer["nine" ] = i9;

                                                    time++;
                                                    UpdatePercent((int)(time * 100 / total));

                                                    if (bgworker.CancellationPending)
                                                    {
                                                        e.Cancel = true;
                                                        return;
                                                    }

                                                    foreach (KeyValuePair<string, int> kvp in ChildProblem.Answer)
                                                    {
                                                        Debug.Write($"{{{kvp.Key}/{(int)Enum.Parse(typeof(MagicNumber), kvp.Key, true)}}}={kvp.Value}\t");
                                                    }
                                                    Debug.Write("--");
                                                    Debug.WriteLine($"{i0,2}{i1,2}{i2,2}{i3,2}{i4,2}{i5,2}{i6,2}{i7,2}{i8,2}{i9,2}");
                                                    //
                                                    int verify = 0;

                                                    for (int j = 0; j < Material.GetLength(0); j++)
                                                    {
                                                        if (ChildProblem.NumSpecialCompare(Material[j, 0], Material[j, 1]))
                                                        {
                                                            verify++;
                                                        }
                                                        else
                                                        {
                                                            break;
                                                        }
                                                    }

                                                    if (verify == Material.GetLength(0))
                                                    {
                                                        foreach (var item in ChildProblem.value)
                                                        {
                                                            Debug.Write($"\t\t\t{item,3}");
                                                        }
                                                        Debug.WriteLine("Successful once ....");

                                                        int[] array = ChildProblem.Answer.Values.ToArray<int>();
                                                        ChildProblem.DictAns.Add(array);
                                                    }

                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            #endregion
        }

        private void BtStartCalculate_Click(object sender, RoutedEventArgs e)
        {
            if (btStartCalculate.IsEnabled)
            {
                btStartCalculate.IsEnabled = false;
                btnUserAbort.IsEnabled = true;

                //rtxbProcedureInfo.Document.Blocks.Clear();
                txbCondition.Clear();
                txbCondition.AppendText("\nOnly Integral types Support; Only For Add Operation");
                txbCondition.AppendText($"\nInteger Range : From {Problem.SearchRangeMin} to {Problem.SearchRangeMax}");

                ParagraphTips.Inlines.Clear();
                ParagraphTips.Inlines.Add("---------->>>开始解算....");

                InitWork();
                bgworker.RunWorkerAsync();

                //WindowCalculate CalWin = new WindowCalculate();
                //CalWin.Owner = this;
                //CalWin.ShowDialog();//CalWin.Show();
            }
        }

        private void BtnUserAbort_Click(object sender, RoutedEventArgs e)
        {
            if (!btStartCalculate.IsEnabled)
            {
                if (bgworker.IsBusy)
                {
                    bgworker.CancelAsync();
                }
            }
        }
    }
}
