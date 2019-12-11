﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
using System.IO;
using Microsoft.Win32;  //ファイル選択ダイアログ使用のため追加
using System.Text.RegularExpressions;  //ファイルから文字列を検索するためのメソッドを呼び出すため追加
using System.Runtime.InteropServices;  //マウスクリックイベント処理のため追加
using Microsoft.VisualBasic; //ソリューションエクスプローラーの参照から追加する必要がある。
using System.Windows.Diagnostics;
using System.Windows.Controls.Primitives;
                                       /// </summary>



namespace ButtonClick3
{
    class DllImportSample
    {
        [DllImport("USER32.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern void SetCursorPos(int X, int Y);

        [DllImport("USER32.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

    }
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public DateTime dt1 = new DateTime(2019,1,1,0,0,00);
        public DateTime dt2;
        public string now;
        public string filePath2;
        public string comboValue1;
        public string comboValue2;
        public string comboValue3;
        public string comboValue4;
        public string comboValue5;
        public string comboValue6;
        public string comboValue7;
        public string comboValue8;
        public StreamWriter sw2;
        public int cnt = 0;
        public int cnt2 = 1;
        public int cnt3 = 1;
        public int cnt4 = 1;
        public int cnt5 = 0;
        public string line1;
        public string line2;
        public string line3;
        public string line4;
        private const int MOUSEEVENTF_LEFTDOWN = 0x2;
        private const int MOUSEEVENTF_LEFTUP = 0x4;
        public const int WM_LBUTTONDOWN = 0x201;
        public const int WM_LBUTTONUP = 0x202;
        public const int MK_LBUTTON = 0x0001;
        public const int BM_CLICK = 0x00F5;
        public const int VM_COMMAND = 0x0111;
        public const int CB_SELSTRING = 0x014D;
        public int n; //HSカメラの倍率

        [DllImport("user32.dll")]
        public static extern int PostMessage(IntPtr hWnd, uint Msg, uint wParam, uint lParam);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, string lParam);

        [DllImport("user32.dll",CharSet =CharSet.Auto)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr hwndparent, IntPtr hwndChildafter, string lpszClass, string lpszWindow);

        public MainWindow()
        {
            InitializeComponent();


            

            //TagDateフォルダが作成されているかチェックを行う。なければ作成する。
            string folderpath1 = @"C:\TagAdding\TagDate";

            if (Directory.Exists(folderpath1))
            {
                //Folderがある場合は何もしない
            }
            else
            {
                DirectoryInfo di1 = new DirectoryInfo(folderpath1);
                di1.Create();

                MessageBox.Show("TagDateフォルダを作成しました。");
            }


            //TagListフォルダが作成されているかチェックを行う。なければ作成する。
            string folderpath3 = @"C:\TagAdding\TagList";

            if (Directory.Exists(folderpath3))
            {
                //Folderがある場合は何もしない
            }
            else
            {
                DirectoryInfo di3 = new DirectoryInfo(folderpath3);
                di3.Create();

                MessageBox.Show("TagListフォルダを作成しました。");
            }

            //Wowza-vbsフォルダが作成されているかチェックを行う。なければ作成する。
            string folderpath4 = @"C:\TagAdding\Wowza-vbs";

            if (Directory.Exists(folderpath4))
            {
                //Folerがある場合は何もしない
            }
            else
            {
                DirectoryInfo di4 = new DirectoryInfo(folderpath4);
                di4.Create();

                MessageBox.Show("Wowza-vbsフォルダを作成しました。");
            }



            /// ComboBox2に、Name-List1で記載した名前一覧を名前のComboBoxに表示させる処理
            try
            {
                StreamReader file1 = new StreamReader(@"C:\TagAdding\TagList\\Name-List1.txt", Encoding.Default);
                {
                    while ((line1 = file1.ReadLine()) != null)
                    {
                        comboBox2.Items.Add(line1);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Name-List1ファイルが見つかりませんでした。");
                MessageBox.Show(@"C:\TagAdding\TagList\Name-List1.txt" + "を格納してください。");
            }


            /// ComboBox8に、Name-List2で記載した名前一覧を名前のComboBoxに表示させる処理
            try
            {
                StreamReader file2 = new StreamReader(@"C:\TagAdding\TagList\\Name-List2.txt", Encoding.Default);
                {
                    while ((line2 = file2.ReadLine()) != null)
                    {
                        comboBox8.Items.Add(line2);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Name-List2ファイルが見つかりませんでした。");
                MessageBox.Show(@"C:\TagAdding\TagList\Name-List2.txt" + "を格納してください。");
            }

            comboValue3 = comboBox3.Text; //Hチーム
            textBox1.AppendText("①" + comboValue3);

            comboValue2 = comboBox2.Text; //H名前
            textBox1.AppendText("②" + comboValue2);

            comboValue7 = comboBox7.Text; //Vチーム
            textBox1.AppendText("③" + comboValue7);

            comboValue8 = comboBox8.Text; //V名前
            textBox1.AppendText("④" + comboValue8);

            comboValue4 = comboBox4.Text; //回数
            textBox1.AppendText("⑤" + comboValue4);

            comboValue5 = comboBox5.Text; //カウント
            textBox1.AppendText("⑥" + comboValue5);

            comboValue6 = comboBox6.Text; //球速
            textBox1.AppendText("⑦" + comboValue6 + "⑧");
        }


        private void Button_Click(object sender, RoutedEventArgs e) ///"録画開始+基準時間記録"ボタン押下時の処理
        {


            if (cnt == 0)
            {
                if (radioButton1.IsChecked == true)　//NWカメラ録画開始の場合
                {
                    // NWカメラの録画開始(wowza録画vbsファイルの呼び出し)
                    System.Diagnostics.Process p = System.Diagnostics.Process.Start(@"C:\TagAdding\Wowza-vbs\\Wowza record start.vbs");
                }
                else if (radioButton2.IsChecked == true) //HSカメラ（×２）録画開始の場合
                {
                    // hWndc9のハンドルを取り出すメソッドを呼び出し
                    IntPtr hWndc9 = Multi_Video_Handle9(); 

                    IntPtr hWndc10 = FindWindowEx(hWndc9, IntPtr.Zero, "WindowsForms10.BUTTON.app.0.fb11c8_r17_ad1", "フレームレート");
                    System.Diagnostics.Trace.WriteLine("フレームレート：" + hWndc10);

                    IntPtr hWndc11 = FindWindowEx(hWndc9, IntPtr.Zero, "WindowsForms10.COMBOBOX.app.0.fb11c8_r17_ad1", "");
                    System.Diagnostics.Trace.WriteLine("300FPS：" + hWndc11);

                    SendMessage(hWndc11, CB_SELSTRING, -1, "120 FPS (640x360)"); //ComboBox 120FPS選択

                    PostMessage(hWndc10, BM_CLICK, 0, 0); //フレームレートボタン押下

                    System.Threading.Thread.Sleep(10000);

                    // hWndc6のハンドルを取り出すメソッドを呼び出し
                    IntPtr hWndc6 = Multi_Video_Handle6();

                    IntPtr hWndc7 = FindWindowEx(hWndc6, IntPtr.Zero, "WindowsForms10.BUTTON.app.0.fb11c8_r17_ad1", "REC START");
                    System.Diagnostics.Trace.WriteLine("REC START_" + hWndc7);

                    PostMessage(hWndc7, BM_CLICK, 0, 0);  //REC STARTボタン押下
                }
                else if (radioButton3.IsChecked == true) //HSカメラ（×４）録画開始の場合
                {
                    MessageBox.Show("HS Cam(x4)は現在録画できません。");
                }
                else if (radioButton4.IsChecked == true) //HSカメラ（×５）録画開始の場合
                {
                    MessageBox.Show("HS Cam(x5)は現在録画できません。");
                }
                else if (radioButton5.IsChecked == true) //HSカメラ（×１０）録画開始の場合
                {
                    MessageBox.Show("HS Cam(x10)は現在録画できません。");
                }
                else if (radioButton6.IsChecked == true)
                {
                    // hWndc9のハンドルを取り出すメソッドを呼び出し
                    IntPtr hWndc9 = Multi_Video_Handle9();

                    IntPtr hWndc10 = FindWindowEx(hWndc9, IntPtr.Zero, "WindowsForms10.BUTTON.app.0.fb11c8_r17_ad1", "フレームレート");
                    System.Diagnostics.Trace.WriteLine("フレームレート：" + hWndc10);

                    IntPtr hWndc11 = FindWindowEx(hWndc9, IntPtr.Zero, "WindowsForms10.COMBOBOX.app.0.fb11c8_r17_ad1", "");
                    System.Diagnostics.Trace.WriteLine("300FPS：" + hWndc11);

                    SendMessage(hWndc11, CB_SELSTRING, -1, "120 FPS (640x360)"); //ComboBox 120FPS選択

                    PostMessage(hWndc10, BM_CLICK, 0, 0); //フレームレートボタン押下

                    System.Threading.Thread.Sleep(10000);

                    // hWndc6のハンドルを取り出すメソッドを呼び出し
                    IntPtr hWndc6 = Multi_Video_Handle6();

                    IntPtr hWndc7 = FindWindowEx(hWndc6, IntPtr.Zero, "WindowsForms10.BUTTON.app.0.fb11c8_r17_ad1", "REC START");
                    System.Diagnostics.Trace.WriteLine("REC START_" + hWndc7);

                    PostMessage(hWndc7, BM_CLICK, 0, 0);  //REC STARTボタン押下

                    System.Threading.Thread.Sleep(800); // HSカメラの録画開始がNSカメラの録画より0.8秒ほど遅いのでWaitを設定

                    // NWカメラの録画開始(wowza録画vbsファイルの呼び出し)
                    System.Diagnostics.Process p = System.Diagnostics.Process.Start(@"C:\TagAdding\Wowza-vbs\\Wowza record start.vbs");


                }



                string filePath1 = @"C:\TagAdding\TagDate\genzaijikoku.txt";

                dt1 = DateTime.Now;

                StreamWriter sw1 = new StreamWriter(filePath1, false, Encoding.UTF8);

                string result1 = dt1.ToString("HH:mm:ss");

                //MessageBox.Show(dt1.ToString());

                sw1.Write(result1);

                sw1.Close();

                Button2.IsEnabled = true; //録画停止ボタンを活性化
                Button1.IsEnabled = false; //録画開始ボタンを非活性化

                // 全てのradioButtonを録画停止までは押せなくする
                radioButton1.IsEnabled = false;
                radioButton2.IsEnabled = false;
                radioButton3.IsEnabled = false;
                radioButton4.IsEnabled = false;
                radioButton5.IsEnabled = false;
                radioButton6.IsEnabled = false;
                radioButton7.IsEnabled = false;
                radioButton8.IsEnabled = false;
                radioButton9.IsEnabled = false;
            }
            else
            {
                MessageBox.Show("既に録画中です。「録画停止+タグ停止」ボタンを押下してください。");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) ///"タグ時間記録"ボタン押下時の処理
        {

            DateTime dt3 = new DateTime(2019, 1, 1, 0, 0, 00);

            if (dt1 == dt3)
            {
                MessageBox.Show("基準時間が取得されていません。");
            }
            else if (radioButton1.IsChecked == true)
            {
                DateTime dt2 = DateTime.Now;

                filePath2 = @"C:\TagAdding\TagDate\";

                TimeSpan interval = dt2 - dt1;

                //MessageBox.Show(dt1.ToString());
                //MessageBox.Show(dt2.ToString());

                int seconds = interval.Seconds;
                int minutes = interval.Minutes;
                int hours = interval.Hours;

                TimeSpan ts1 = new TimeSpan(hours, minutes, seconds);

                if (cnt == 0)
                {
                    now = dt1.ToString("yyyyMMddHHmmss");

                }



                sw2 = new StreamWriter(filePath2 + now + ".txt", true, Encoding.UTF8);
                cnt++;
                
                
                sw2.Write(ts1 + "…");

                string textValue = textBox1.Text;
                //MessageBox.Show(textValue);

                sw2.Write(textValue);
                sw2.Write(Environment.NewLine);
                sw2.Close();
            }
            else if (radioButton2.IsChecked == true)
            {
                //MessageBox.Show("HS Cam(x2)が選択されています。");

                n = 2;

                seconds_calculation(n);


            }
            else if (radioButton3.IsChecked == true)
            {
                MessageBox.Show("HS Cam(x4)が選択されています。");
            }
            else if (radioButton4.IsChecked == true)
            {
                MessageBox.Show("HS Cam(x5)が選択されています。");
            }
            else if (radioButton5.IsChecked == true)
            {
                MessageBox.Show("HS Cam(x10)が選択されています。");
            }
        }


        private void Button_Click_6(object sender, RoutedEventArgs e) ///クリアボタン
        {
            textBox1.Clear();

            comboValue3 = comboBox3.Text; //Hチーム
            textBox1.AppendText("①" + comboValue3);

            comboValue2 = comboBox2.Text; //H名前
            textBox1.AppendText("②" + comboValue2);

            comboValue7 = comboBox7.Text; //Vチーム
            textBox1.AppendText("③" + comboValue7);

            comboValue8 = comboBox8.Text; //V名前
            textBox1.AppendText("④" + comboValue8);

            comboValue4 = comboBox4.Text; //回数
            textBox1.AppendText("⑤" + comboValue4);

            comboValue5 = comboBox5.Text; //カウント
            textBox1.AppendText("⑥" + comboValue5);

            comboValue6 = comboBox6.Text; //球速
            textBox1.AppendText("⑦" + comboValue6 + "⑧");
        }


        private void ComboBox3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string str1_bf = "";
            string str1_af = "";
            int check1 = 0;

            comboValue3 = comboBox3.SelectedItem.ToString();
            comboValue3 = comboValue3.Replace("System.Windows.Controls.ListBoxItem: ", "");

            str1_bf = textBox1.Text;

            for (int ch1 = 0; ch1 < str1_bf.Length; ch1++)
            {
                if (str1_bf[ch1] == '①')
                {
                    str1_af += str1_bf[ch1].ToString();　//①を設定
                    check1 = 1;
                }
                else if (str1_bf[ch1] == '②')
                {
                    str1_af += comboValue3; //comboValue3の文字列を設定する
                    str1_af += str1_bf[ch1].ToString(); //②を設定
                    check1 = 0;
                }
                else if (check1 == 1)
                {
                    //①と②の間の文字は設定せずに破棄する
                }
                else if (check1 == 0)
                {
                    str1_af += str1_bf[ch1].ToString();  //①と②の間以外はそのまま設定する
                }
            }
            textBox1.Text = str1_af;


        }


        private void Button_Click_3(object sender, RoutedEventArgs e) //"録画停止+タグ付停止"ボタン押下時の処理
        {


            if (cnt == 0) //タグファイルが生成されていない状態で録画停止ボタンが押された場合の処理
            {
                string message = "タグファイルが作成されていません。録画停止しますか？";
                string caption = "Delete";

                MessageBoxResult result = MessageBox.Show(message, caption, MessageBoxButton.YesNo);


                if (result == MessageBoxResult.Yes) //タグファイル未作成+録画停止"Yes"の場合
                {
                    if (radioButton1.IsChecked == true) //NWカメラ録画開始の場合
                    {
                        System.Diagnostics.Process p = System.Diagnostics.Process.Start(@"C:\TagAdding\Wowza-vbs\\Wowza record stop.vbs");
                    }
                    else if (radioButton2.IsChecked == true || radioButton3.IsChecked == true || radioButton4.IsChecked == true || radioButton5.IsChecked == true)
                    {
                        // hWndc6のハンドルを取り出すメソッドを呼び出し
                        IntPtr hWndc6 = Multi_Video_Handle6();

                        // REC STOPのハンドル取り出し
                        IntPtr hWndc8 = FindWindowEx(hWndc6, IntPtr.Zero, "WindowsForms10.BUTTON.app.0.fb11c8_r17_ad1", "REC STOP");
                        System.Diagnostics.Trace.WriteLine("REC STOP_" + hWndc8);

                        // REC STOPボタンを押下
                        PostMessage(hWndc8, BM_CLICK, 0, 0);
                    }

                    cnt = 0;

                    dt1 = new DateTime(2019, 1, 1, 0, 0, 00);

                    Button2.IsEnabled = false; //録画停止ボタンを非活性化
                    
                    Button1.IsEnabled = true;  //録画再生ボタンを活性化

                    // 録画停止したことにより全てのradioボタンを押せるようにする
                    radioButton1.IsEnabled = true;
                    radioButton2.IsEnabled = true;
                    radioButton3.IsEnabled = true;
                    radioButton4.IsEnabled = true;
                    radioButton5.IsEnabled = true;
                    radioButton6.IsEnabled = true;
                    radioButton7.IsEnabled = true;
                    radioButton8.IsEnabled = true;
                    radioButton9.IsEnabled = true;
                }
            }
            else if (cnt != 0) //初期化処理＋録画停止処理(タグファイル作成済みの場合)
            {

                if (radioButton1.IsChecked == true) //NWカメラ録画開始の場合
                {
                    System.Diagnostics.Process p = System.Diagnostics.Process.Start(@"C:\TagAdding\Wowza-vbs\\Wowza record stop.vbs");
                }
                else if (radioButton2.IsChecked == true || radioButton3.IsChecked == true || radioButton4.IsChecked == true || radioButton5.IsChecked == true)
                {

                    // hWndc6のハンドルを取り出すメソッドを呼び出し
                    IntPtr hWndc6 = Multi_Video_Handle6();

                    // REC STOPのハンドル取り出し
                    IntPtr hWndc8 = FindWindowEx(hWndc6, IntPtr.Zero, "WindowsForms10.BUTTON.app.0.fb11c8_r17_ad1", "REC STOP");
                    System.Diagnostics.Trace.WriteLine("REC STOP_" + hWndc8);

                    // REC STOPボタンを押下
                    PostMessage(hWndc8, BM_CLICK, 0, 0);
                }

                cnt = 0;

                dt1 = new DateTime(2019, 1, 1, 0, 0, 00);

                Button2.IsEnabled = false;
                Button1.IsEnabled = true;

                radioButton1.IsEnabled = true;
                radioButton2.IsEnabled = true;
                radioButton3.IsEnabled = true;
                radioButton4.IsEnabled = true;
                radioButton5.IsEnabled = true;
                radioButton6.IsEnabled = true;
                radioButton7.IsEnabled = true;
                radioButton8.IsEnabled = true;
                radioButton9.IsEnabled = true;
            }
        }
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)  //①抽出するファイルパス取得ボタンの処理
        {
            textBox3.Clear(); //textBox3の初期値をクリア

            var dialog1 = new OpenFileDialog();

            dialog1.InitialDirectory = @"C:\TagAdding\TagDate"; //フォルダ指定

            dialog1.Title = "抽出元のファイルを選んでください"; //ダイアログタイトル指定

            dialog1.Filter = "テキストファイル(*.txt)|*.txt|全てのファイル(*.*)|*.*";

            if (dialog1.ShowDialog() == true)
            {
                textBox3.AppendText(dialog1.FileName);
            }

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_2(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click_5(object sender, RoutedEventArgs e) //④抽出ボタンの処理
        {
            Regex rgx = new Regex(comboBox9.Text, RegexOptions.IgnoreCase); //comboBox9に表示される名前をrgxに設定

            if (comboBox9.Text == "③名前を選択")
            {
                MessageBox.Show("抽出する名前が選ばれていません");
            }

            if (textBox3.Text == "")
            {
                MessageBox.Show("ベースタグファイルが選ばれていません");
            }
            else
            {
                StreamReader file4 = new StreamReader(textBox3.Text, Encoding.Default); //textBox3に表示されたファイルをfile4に設定
                {
                    line4 = "";　//line4初期化

                    string str1 = Regex.Replace(textBox3.Text, @"[^0-9]", ""); //TextBox3に表示されたパスから数値だけを抜き出す。ファイル名に使用するため

                    while ((line4 = file4.ReadLine()) != null) //file4の情報を1行ずつ読み込み。情報なくなったら終了
                    {
                        if (rgx.Match(line4).Success)　//comboBox9で表示された名前がtextBox3の1行にあるかどうか判定。ある場合処理を行う
                        {
                            
                            StreamWriter sw3 = new StreamWriter(@"C:\TagAdding\TagDate\" + str1 +  "-" + comboBox9.Text + ".txt", true, Encoding.Default);

                            sw3.Write(line4);
                            sw3.Write(Environment.NewLine);
                            sw3.Close();

                            cnt5++;
                        }
                    }

                    if (cnt5 == 0)
                    {
                        MessageBox.Show("該当するデータがありませんでした。");
                    }
                    else
                    {
                        MessageBox.Show("抽出完了" + " " + "該当するデータは"+cnt5+"件でした。");
                        cnt5 = 0;
                    }
                }
            }
        }

        private void ComboBox9_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void Button_Click_2(object sender, RoutedEventArgs e) //②Name-Listを選択ボタンの処理
        {
            comboBox9.Items.Clear();
            comboBox9.Items.Add("③名前を選択");

            var dialog2 = new OpenFileDialog();

            dialog2.InitialDirectory = @"C:\TagAdding\TagList";

            dialog2.Title = "抽出したい名前があるName-Listを選んでください";

            dialog2.Filter = "テキストファイル(*.txt)|*.txt|全てのファイル(*.*)|*.*";

            if (dialog2.ShowDialog() == true)
            {
                StreamReader file3 = new StreamReader(dialog2.FileName, Encoding.Default);
                {
                    while ((line3 = file3.ReadLine()) != null)
                    {
                        comboBox9.Items.Add(line3);
                    }
                }
            }

        }


        private void ComboBox5_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string str1_bf = "";
            string str1_af = "";
            int check1 = 0;

            comboValue5 = comboBox5.SelectedItem.ToString();
            comboValue5 = comboValue5.Replace("System.Windows.Controls.ListBoxItem: ", "");

            str1_bf = textBox1.Text;

            for (int ch1 = 0; ch1 < str1_bf.Length; ch1++)
            {
                if (str1_bf[ch1] == '⑥')
                {
                    str1_af += str1_bf[ch1].ToString();　//⑥を設定
                    check1 = 1;
                }
                else if (str1_bf[ch1] == '⑦')
                {
                    str1_af += comboValue5; //comboValue5の文字列を設定する
                    str1_af += str1_bf[ch1].ToString(); //⑦を設定
                    check1 = 0;
                }
                else if (check1 == 1)
                {
                    //⑥と⑦の間の文字は設定せずに破棄する
                }
                else if (check1 == 0)
                {
                    str1_af += str1_bf[ch1].ToString();  //①と②の間以外はそのまま設定する
                }
            }
            textBox1.Text = str1_af;
            
        }

        private void ComboBox7_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string str1_bf = "";
            string str1_af = "";
            int check1 = 0;

            comboValue7 = comboBox7.SelectedItem.ToString();
            comboValue7 = comboValue7.Replace("System.Windows.Controls.ListBoxItem: ", "");

            str1_bf = textBox1.Text;

            for (int ch1 = 0; ch1 < str1_bf.Length; ch1++)
            {
                if (str1_bf[ch1] == '③')
                {
                    str1_af += str1_bf[ch1].ToString();　//③を設定
                    check1 = 1;
                }
                else if (str1_bf[ch1] == '④')
                {
                    str1_af += comboValue7; //comboValue7の文字列を設定する
                    str1_af += str1_bf[ch1].ToString(); //④を設定
                    check1 = 0;
                }
                else if (check1 == 1)
                {
                    //③と④の間の文字は設定せずに破棄する
                }
                else if (check1 == 0)
                {
                    str1_af += str1_bf[ch1].ToString();  //③と④の間以外はそのまま設定する
                }
            }
            textBox1.Text = str1_af;
        }

        private void ComboBox6_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string str1_bf = "";
            string str1_af = "";
            int check1 = 0;

            comboValue6 = comboBox6.SelectedItem.ToString();
            comboValue6 = comboValue6.Replace("System.Windows.Controls.ListBoxItem: ", "");

            str1_bf = textBox1.Text;

            for (int ch1 = 0; ch1 < str1_bf.Length; ch1++)
            {
                if (str1_bf[ch1] == '⑦')
                {
                    str1_af += str1_bf[ch1].ToString();　//⑦を設定
                    check1 = 1;
                }
                else if (str1_bf[ch1] == '⑧')
                {
                    str1_af += comboValue6; //comboValue6の文字列を設定する
                    str1_af += str1_bf[ch1].ToString(); //⑧を設定
                    check1 = 0;
                }
                else if (check1 == 1)
                {
                    //①と②の間の文字は設定せずに破棄する
                }
                else if (check1 == 0)
                {
                    str1_af += str1_bf[ch1].ToString();  //①と②の間以外はそのまま設定する
                }
            }
            textBox1.Text = str1_af;
        }

        private void ComboBox8_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string str1_bf = "";
            string str1_af = "";
            int check1 = 0;

            comboValue8 = comboBox8.SelectedItem.ToString();
            comboValue8 = comboValue8.Replace("System.Windows.Controls.ListBoxItem: ", "");

            str1_bf = textBox1.Text;

            for (int ch1 = 0; ch1 < str1_bf.Length; ch1++)
            {
                if (str1_bf[ch1] == '④')
                {
                    str1_af += str1_bf[ch1].ToString();　//④を設定
                    check1 = 1;
                }
                else if (str1_bf[ch1] == '⑤')
                {
                    str1_af += comboValue8; //comboValue8の文字列を設定する
                    str1_af += str1_bf[ch1].ToString(); //④を設定
                    check1 = 0;
                }
                else if (check1 == 1)
                {
                    //④と⑤の間の文字は設定せずに破棄する
                }
                else if (check1 == 0)
                {
                    str1_af += str1_bf[ch1].ToString();  //④と⑤の間以外はそのまま設定する
                }
            }
            textBox1.Text = str1_af;
        }

        private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            string str1_bf = "";
            string str1_af = "";
            int check1 = 0;

            comboValue4 = comboBox4.SelectedItem.ToString();
            comboValue4 = comboValue4.Replace("System.Windows.Controls.ListBoxItem: ", "");

            str1_bf = textBox1.Text;

            for (int ch1 = 0; ch1 < str1_bf.Length; ch1++)
            {
                if (str1_bf[ch1] == '⑤')
                {
                    str1_af += str1_bf[ch1].ToString();　//⑤を設定
                    check1 = 1;
                }
                else if (str1_bf[ch1] == '⑥')
                {
                    str1_af += comboValue4; //comboValue4の文字列を設定する
                    str1_af += str1_bf[ch1].ToString(); //⑥を設定
                    check1 = 0;
                }
                else if (check1 == 1)
                {
                    //⑤と⑥の間の文字は設定せずに破棄する
                }
                else if (check1 == 0)
                {
                    str1_af += str1_bf[ch1].ToString();  //⑤と⑥の間以外はそのまま設定する
                }
            }
            textBox1.Text = str1_af;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string str1_bf = "";
            string str1_af = "";
            int check1 = 0;

            comboValue2 = comboBox2.SelectedItem.ToString();
            comboValue2 = comboValue2.Replace("System.Windows.Controls.ListBoxItem: ", "");

            str1_bf = textBox1.Text;

            for (int ch1 = 0; ch1 < str1_bf.Length; ch1++)
            {
                if (str1_bf[ch1] == '②')
                {
                    str1_af += str1_bf[ch1].ToString();　//②を設定
                    check1 = 1;
                }
                else if (str1_bf[ch1] == '③')
                {
                    str1_af += comboValue2; //comboValue2の文字列を設定する
                    str1_af += str1_bf[ch1].ToString(); //③を設定
                    check1 = 0;
                }
                else if (check1 == 1)
                {
                    //②と③の間の文字は設定せずに破棄する
                }
                else if (check1 == 0)
                {
                    str1_af += str1_bf[ch1].ToString();  //②と③の間以外はそのまま設定する
                }
            }
            textBox1.Text = str1_af;
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {

        }

        private IntPtr Multi_Video_Handle9()
        {
            IntPtr hWnd = FindWindow(null, "Multi-Video Viewer");
            System.Diagnostics.Trace.WriteLine("①" + hWnd);

            IntPtr hWndc1 = FindWindowEx(hWnd, IntPtr.Zero, "WindowsForms10.Window.8.app.0.fb11c8_r17_ad1", "");
            System.Diagnostics.Trace.WriteLine("②" + hWndc1);

            IntPtr hWndc2 = FindWindowEx(hWndc1, IntPtr.Zero, "WindowsForms10.Window.8.app.0.fb11c8_r17_ad1", "");
            System.Diagnostics.Trace.WriteLine("③" + hWndc2);

            IntPtr hWndc3 = FindWindowEx(hWndc2, IntPtr.Zero, "WindowsForms10.SysTabControl32.app.0.fb11c8_r17_ad1", "");
            System.Diagnostics.Trace.WriteLine("④" + hWndc3);

            IntPtr hWndc4 = FindWindowEx(hWndc3, IntPtr.Zero, "WindowsForms10.Window.8.app.0.fb11c8_r17_ad1", "Rec");
            System.Diagnostics.Trace.WriteLine("⑤" + hWndc4);

            IntPtr hWndc5 = FindWindowEx(hWndc4, IntPtr.Zero, "WindowsForms10.Window.8.app.0.fb11c8_r17_ad1", "");
            System.Diagnostics.Trace.WriteLine("⑥" + hWndc5);

            IntPtr hWndc6 = FindWindowEx(hWndc5, IntPtr.Zero, "WindowsForms10.Window.8.app.0.fb11c8_r17_ad1", "");
            System.Diagnostics.Trace.WriteLine("⑦" + hWndc6);

            IntPtr hWndc7 = FindWindowEx(hWndc6, IntPtr.Zero, "WindowsForms10.BUTTON.app.0.fb11c8_r17_ad1", "REC START");
            System.Diagnostics.Trace.WriteLine("REC START_" + hWndc7);

            IntPtr hWndc9 = FindWindowEx(hWndc5, hWndc6, "WindowsForms10.Window.8.app.0.fb11c8_r17_ad1", "");
            System.Diagnostics.Trace.WriteLine("⑨" + hWndc9);


            return hWndc9;

        }
        private IntPtr Multi_Video_Handle6()
        {
            IntPtr hWnd = FindWindow(null, "Multi-Video Viewer");
            System.Diagnostics.Trace.WriteLine("①" + hWnd);

            IntPtr hWndc1 = FindWindowEx(hWnd, IntPtr.Zero, "WindowsForms10.Window.8.app.0.fb11c8_r17_ad1", "");
            System.Diagnostics.Trace.WriteLine("②" + hWndc1);

            IntPtr hWndc2 = FindWindowEx(hWndc1, IntPtr.Zero, "WindowsForms10.Window.8.app.0.fb11c8_r17_ad1", "");
            System.Diagnostics.Trace.WriteLine("③" + hWndc2);

            IntPtr hWndc3 = FindWindowEx(hWndc2, IntPtr.Zero, "WindowsForms10.SysTabControl32.app.0.fb11c8_r17_ad1", "");
            System.Diagnostics.Trace.WriteLine("④" + hWndc3);

            IntPtr hWndc4 = FindWindowEx(hWndc3, IntPtr.Zero, "WindowsForms10.Window.8.app.0.fb11c8_r17_ad1", "Rec");
            System.Diagnostics.Trace.WriteLine("⑤" + hWndc4);

            IntPtr hWndc5 = FindWindowEx(hWndc4, IntPtr.Zero, "WindowsForms10.Window.8.app.0.fb11c8_r17_ad1", "");
            System.Diagnostics.Trace.WriteLine("⑥" + hWndc5);

            IntPtr hWndc6 = FindWindowEx(hWndc5, IntPtr.Zero, "WindowsForms10.Window.8.app.0.fb11c8_r17_ad1", "");
            System.Diagnostics.Trace.WriteLine("⑦" + hWndc6);

            return hWndc6;
        }
        private void seconds_calculation(int n)
        {
            DateTime dt2 = DateTime.Now;

            filePath2 = @"C:\TagAdding\TagDate\";

            TimeSpan interval = dt2 - dt1;

            int seconds = interval.Seconds * n;
            int minutes_seconds = (interval.Minutes * n) * 60;
            int hours_seconds = (interval.Hours * n) * 3600;

            seconds = seconds + minutes_seconds + hours_seconds;

            //MessageBox.Show(seconds.ToString());
            //MessageBox.Show(interval.ToString());

            while (seconds >= 360)
            {
                seconds -= 360;
                cnt2++;
            }

            int minutes = seconds / 60;
            seconds = seconds % 60;

            TimeSpan ts1 = new TimeSpan(0, minutes, seconds);

            if (cnt == 0)
            {
                now = now = dt1.ToString("yyyyMMddHHmmss");

            }

            if (cnt2 < 10)
            {
                sw2 = new StreamWriter(filePath2 + now + "-0" + cnt2 + ".txt", true, Encoding.UTF8);
            }
            else if (cnt2 >= 10)
            {
                sw2 = new StreamWriter(filePath2 + now + "-" + cnt2 + ".txt", true, Encoding.UTF8);
            }

            cnt++;
            cnt2 = 1;



            sw2.Write(ts1 + "…");

            string textValue = textBox1.Text;

            sw2.Write(textValue);
            sw2.Write(Environment.NewLine);
            sw2.Close();
        }

        private void TextBox1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox1_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }
    }
}
