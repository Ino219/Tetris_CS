using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris_CS
{
    public partial class Form1 : Form
    {
        //メインフィールド用変数
        Bitmap bmp;
        Graphics gr;
        //NextBoxのフィールド用変数
        Bitmap bmp_next;
        Graphics gr_next;
        //縦軸と横軸のブロック数
        const int xBlock = 10;
        const int yBlock = 20;
        int spaceX;
        int spaceY;
        int spaceX_next;
        int spaceY_next;
        BlockPattern currentBP;
        bool GO = false;
        int NextBPNum = 0;
        bool finishFlg = false;

        //マスデータ定義
        public struct mathData
        {
            //描画用の左上座標
            public int x;
            public int y;
            //升目が埋まっているかどうかの判定
            public bool fill;
            public Color col;
            public mathData(int x, int y, bool fill,Color col) {
                this.x = 0;
                this.y = 0;
                this.fill = false;
                this.col = Color.White;
            }
        }

        List<mathData> mathDataList;
        List<mathData> NextmathDataList;

        //ブロックパターンの定義
        public struct BlockPattern
        {
            public int first;
            //1~9の相対位置でブロックのパターンを定義
            public int Num1;
            public int Num2;
            public int Num3;
            public int Num4;
            public int[] blockArr;
            //パターン番号
            public int PatternNum;
            public bool SpecialPattern;
            public BlockPattern(int first,int Num1,int Num2,int Num3,int Num4,int[] blockArr,int PatternNum,bool SpecialPattern)
            {
                this.first = 0;
                this.Num1 = 0;
                this.Num2 = 0;
                this.Num3 = 0;
                this.Num4 = 0;
                this.blockArr = new int[4];
                this.PatternNum = 0;
                this.SpecialPattern = false;
            }
        }

        public Form1()
        {
            InitializeComponent();
            //キー操作
            KeyPreview = true;
            //フィールドサイズの定義
            Field.Width = 200;
            Field.Height = 400;
            //ネクストのフィールド定義
            NextBox.Width = 90;
            NextBox.Height = 90;
            //マスのデータリスト作成
            mathDataList = new List<mathData>();
            //描画処理準備
            bmp = new Bitmap(Field.Width,Field.Height);
            Field.Image = bmp;
            gr = Graphics.FromImage(Field.Image);
            Pen pn = new Pen(Color.Gray,1);

            bmp_next = new Bitmap(NextBox.Width, NextBox.Height);
            NextBox.Image = bmp_next;
            gr_next = Graphics.FromImage(NextBox.Image);

            //1マスのサイズを定義
            spaceX = Field.Width / xBlock;
            spaceY = Field.Height / yBlock;
            //枠線の描画
            PaintLine();
            //升データの定義
            mathDataList=setMathData();
            
            //ブロック生成と描画
            currentBP = retBlockPattern(BlockPatternSetting());
            PaintBlock(currentBP);
            //タイマーをセット
            timer1.Enabled = true;
            //次に乱数生成されるブロックの番号を引数として、Nextのフィールドを生成
            MakeNextBox(BlockPatternSetting());
        }
        
        private int BlockPatternSetting()
        {
            Random rnd = new Random();
            int rndm = rnd.Next(1, 8);
            return rndm;
        }
        private void MakeNextBox(int next)
        {
            NextBPNum = next;
            int mathData = 0 ;
            int[] disp = new int[4];
            //Nextが6,7,8でなければ、5*5のフィールドとする
            if (next != 6 && next != 7 && next != 8)
            {
                mathData = 5;
                //3*3の場合
                switch (next)
                {
                    case 1:
                        disp = new int[4] { 17, 12, 7, 6 };

                        break;
                    case 2:
                        disp = new int[4] { 16, 11, 6, 7 };

                        break;
                    case 3:
                        disp = new int[4] { 16, 17, 12, 13 };

                        break;
                    case 4:
                        disp = new int[4] { 17, 18, 11, 12 };

                        break;
                    case 5:
                        disp = new int[4] { 17, 11, 12, 13 };
                        break;
                }
            }
            else
            {
                //6*6のフィールドを設定
                mathData = 6;
                switch (next)
                {
                    case 6:
                        disp = new int[4] { 14, 15, 20, 21 };
                        break;
                    case 7:
                        disp = new int[4] { 8, 14, 20, 26 };
                        break;
                }
            }
            //1マスのサイズを計算
            spaceX_next = NextBox.Width / mathData;
            spaceY_next = NextBox.Width / mathData;
            //フィールドに線を引く
            PaintLine_next(mathData);
            //升データリストを定義
            NextmathDataList = setNextMathData(mathData);
            //全マスに反復処理をかけて、上記のリストのブロック番号をfill状態にする
            PaintBlock_next(disp, next);
            //全マスをチェックし、trueに着色
            CheckMathSweep_next(mathData);
        }
        private void PaintBlock_next(int[] dataPattern,int bpNum)
        {
            for(int i = 0; i < NextmathDataList.Count; i++)
            {
                bool checkVal = false;
                for(int j = 0; j < dataPattern.Length; j++)
                {
                    if (i == dataPattern[j])
                    {
                        fillValueChange(NextmathDataList,bpNum, i, true);

                        checkVal = true;
                    }
                }
                if (checkVal == false)
                {
                    fillValueChange(NextmathDataList,bpNum, i, false);

                }
            }
        }
        private void fillValueChange(List<mathData> dataList,int bpNum, int num, bool tf)
        {
            //指定のマス番号の升データを指定
            mathData tmpMathData = dataList[num];
            //ブロックのパターン番号に応じて着色設定
            switch (bpNum)
            {
                case 1:
                    tmpMathData.col = Color.LightPink;
                    break;
                case 2:
                    tmpMathData.col = Color.DeepSkyBlue;
                    break;
                case 3:
                    tmpMathData.col = Color.Tan;
                    break;
                case 4:
                    tmpMathData.col = Color.LawnGreen;
                    break;
                case 5:
                    tmpMathData.col = Color.Aquamarine;
                    break;
                case 6:
                    tmpMathData.col = Color.Orange;
                    break;
                case 7:
                    tmpMathData.col = Color.PaleVioletRed;
                    break;
                case 8:
                    tmpMathData.col = Color.PaleVioletRed;
                    break;
            }
            //指定の真偽値を設定
            tmpMathData.fill = tf;
            dataList[num] = tmpMathData;
        }
        private void PaintLine_next(int math)
        {
            for (int i = 1; i < math; i++)
            {
                for (int j = 1; j < math; j++)
                {
                    //マス描画
                    Pen sLine = new Pen(Color.Gray, 1);
                    Point p1 = new Point(i * spaceX_next, 0);
                    Point p2 = new Point(i * spaceX_next, NextBox.Height);
                    gr_next.DrawLine(sLine, p1, p2);
                    Point p3 = new Point(0, j * spaceY_next);
                    Point p4 = new Point(NextBox.Height, j * spaceY_next);
                    gr_next.DrawLine(sLine, p3, p4);

                }
            }
        }
        //升番号の相対的な位置にある升番号を返す関数
        public int retRelativeBlock(int FirstBlockNum, int dir)
        {
            int result = 0;
            switch (dir)
            {
                //上方向
                case 0:
                    result = FirstBlockNum + xBlock;
                    break;
                //右方向
                case 1:
                    result = FirstBlockNum + 1;
                    break;
                //下方向
                case 2:
                    result = FirstBlockNum - xBlock;
                    break;
                //左方向
                case 3:
                    result = FirstBlockNum - 1;
                    break;
            }
            return result;
        }
        //入力した種類番号のブロックパターンを返す関数
        public BlockPattern retBlockPattern(int Pattern)
        {
            //出力位置の指定
            Random random = new Random();
            int rnd_ = random.Next(3, 6);
            BlockPattern ret = new BlockPattern();
            switch (Pattern)
            {
                case 1:
                    ret.Num1 = 190 + rnd_;
                    ret.Num2 = retRelativeBlock(ret.Num1, 2);
                    ret.Num3 = retRelativeBlock(ret.Num2, 2);
                    ret.Num4 = retRelativeBlock(ret.Num3, 3);

                    ret.first = retRelativeBlock(ret.Num1, 3);
                    ret.blockArr = new int[] { 2, 5, 8, 7 };
                    ret.PatternNum = Pattern;
                    break;

                case 2:
                    ret.Num1 = 190 + rnd_;
                    ret.Num2 = retRelativeBlock(ret.Num1, 2);
                    ret.Num3 = retRelativeBlock(ret.Num2, 2);
                    ret.Num4 = retRelativeBlock(ret.Num3, 1);

                    ret.first = ret.Num1;
                    ret.blockArr = new int[] { 1, 4, 7, 8 };
                    ret.PatternNum = Pattern;
                    break;
                case 3:
                    ret.Num1 = 190 + rnd_;
                    ret.Num2 = retRelativeBlock(ret.Num1, 1);
                    ret.Num3 = retRelativeBlock(ret.Num2, 2);
                    ret.Num4 = retRelativeBlock(ret.Num3, 1);

                    ret.first = ret.Num1;
                    ret.blockArr = new int[] { 1, 2, 5, 6 };
                    ret.PatternNum = Pattern;
                    ret.SpecialPattern = true;
                    break;
                case 4:
                    ret.Num1 = 190 + rnd_;
                    ret.Num2 = retRelativeBlock(ret.Num1, 3);
                    ret.Num3 = retRelativeBlock(ret.Num2, 2);
                    ret.Num4 = retRelativeBlock(ret.Num3, 3);

                    ret.first = ret.Num1-2;
                    ret.blockArr = new int[] { 2, 3, 4, 5 };
                    ret.PatternNum = Pattern;
                    ret.SpecialPattern = true;
                    break;
                case 5:
                    ret.Num1 = 190 + rnd_;
                    ret.Num2 = retRelativeBlock(ret.Num1, 2);
                    ret.Num3 = retRelativeBlock(ret.Num2, 3);
                    ret.Num4 = retRelativeBlock(ret.Num2, 1);

                    ret.first = ret.Num1 - 1;
                    ret.blockArr = new int[] { 2, 4, 5, 6 };
                    ret.PatternNum = Pattern;
                    break;
                case 6:
                    ret.Num1 = 190 + rnd_;
                    ret.Num2 = retRelativeBlock(ret.Num1, 1);
                    ret.Num3 = retRelativeBlock(ret.Num2, 2);
                    ret.Num4 = retRelativeBlock(ret.Num3, 3);

                    ret.first = ret.Num1;
                    ret.blockArr = new int[] { 1, 2, 4, 5 };
                    ret.PatternNum = Pattern;
                    break;
                case 7:
                    ret.Num1 = 190 + rnd_;
                    ret.Num2 = retRelativeBlock(ret.Num1, 2);
                    ret.Num3 = retRelativeBlock(ret.Num2, 2);
                    ret.Num4 = retRelativeBlock(ret.Num3, 2);

                    ret.first = ret.Num1;
                    //回転のメソッドが他と違うので、この配列は使わない
                    ret.blockArr = new int[] { 1, 4, 7, 10 };
                    ret.PatternNum = Pattern;
                    break;
            }
            //もしも生成したときにブロックとの衝突が判定されるとGameOverとする
            if (checkHit(ret))
            {
                //GameOverのフラグをON
                GO = true;
                MessageBox.Show("GAME OVER");
            }
            return ret;
        }

        public BlockPattern rotateBlock(BlockPattern bp)
        {
            //6番目のブロックの場合は回転を行わない
            if (bp.PatternNum == 6)
            {
                return bp;
            }
            //縦4マスのブロックの場合は特別に指定
            if (bp.PatternNum == 7)
            {
                bp.Num1 = bp.Num4;
                bp.Num2 = retRelativeBlock(bp.Num1, 1);
                bp.Num3 = retRelativeBlock(bp.Num2, 1);
                bp.Num4 = retRelativeBlock(bp.Num3, 1);

                bp.PatternNum += 1;
                return bp;
            }
            if (bp.PatternNum == 8)
            {
                bp.Num4 = bp.Num1;
                bp.Num3 = retRelativeBlock(bp.Num4, 0);
                bp.Num2 = retRelativeBlock(bp.Num3, 0);
                bp.Num1 = retRelativeBlock(bp.Num2, 0);

                bp.PatternNum -= 1;
                return bp;
            }
            //各ブロックパターンの位置のブロック番号を取得
            //firstのブロック位置を基準として、1~9の番号を割り振り、反復処理で各ブロック位置に相対位置の番号を付与
            int[] absoluteNumberList = new int[9];
            for (int i = 0; i < 9; i++)
            {
                int calcNum = i % 3;
                int firstCalc = bp.first + calcNum;
                absoluteNumberList[i] = firstCalc;
                if (i > 2)
                {
                    absoluteNumberList[i] = firstCalc - xBlock;
                }
                if (i > 5)
                {
                    absoluteNumberList[i] = firstCalc - (xBlock * 2);
                }
            }

            //相対位置の番号に計算処理を加えて回転後の相対位置を取得
            int[] relativeBlockPatterns = bp.blockArr;
            int[] afterRotateBlockPatterns = new int[4];
            for (int i = 0; i < relativeBlockPatterns.Length; i++)
            {
                int temp = relativeBlockPatterns[i];
                int m = 0;
                if (temp % 3 != 0)
                {
                    m = 3 - (temp % 3);
                }
                int k = (temp + 2) / 3;
                int result = 3 * m + k;
                afterRotateBlockPatterns[i] = result;
            }

            bp.Num1 = absoluteNumberList[afterRotateBlockPatterns[0] - 1];
            bp.Num2 = absoluteNumberList[afterRotateBlockPatterns[1] - 1];
            bp.Num3 = absoluteNumberList[afterRotateBlockPatterns[2] - 1];
            bp.Num4 = absoluteNumberList[afterRotateBlockPatterns[3] - 1];

            bp.blockArr = afterRotateBlockPatterns;
            
            return bp;
        }

        public void rotationBlock(BlockPattern bp)
        {
            //処理前にタイマー一時停止
            timer1.Enabled = false;
            //回転前のチェック処理
            if (checkRotateBlock(bp) == false)
            {
                //ブロックの升埋め属性をリセット
                resetBlock(bp);
                //ブロックの座標回転
                bp = rotateBlock(bp);
                //ブロックの描画
                PaintBlock(bp);
                //操作ブロックの更新
                currentBP = bp;
            }
            //タイマー再開
            timer1.Enabled = true;
        }

        //外枠と内線の描画
        private void PaintLine()
        {
            for (int i = 1; i < xBlock; i++)
            {
                for(int j = 1; j < yBlock; j++)
                {
                    //マス描画
                    Pen sLine = new Pen(Color.Gray, 1);
                    Point p1 = new Point(i*spaceX,0);
                    Point p2 = new Point(i * spaceX, Field.Height);
                    gr.DrawLine(sLine, p1, p2);
                    Point p3 = new Point(0, j * spaceY);
                    Point p4 = new Point(Field.Height, j * spaceY);
                    gr.DrawLine(sLine, p3, p4);
                    
                }
            }
        }
        //升データのリスト作成関数
        private List<mathData> setMathData()
        {
            List<mathData> resList = new List<mathData>();

            for (int j = yBlock-1; j >= 0; j--)
            {
                for (int i = 0; i < xBlock; i++)
                {
                //描画処理に必要な座標を定義
                    mathData tmpData = new mathData();
                    tmpData.x = i * spaceX;
                    tmpData.y = j * spaceY;
                    resList.Add(tmpData);
                }
            }
            return resList;
        }
        private List<mathData> setNextMathData(int math)
        {
            List<mathData> resList = new List<mathData>();
            for (int j = math - 1; j >= 0; j--)
            {
                for (int i = 0; i < math; i++)
                {
                    //描画処理に必要な座標を定義
                    mathData tmpData = new mathData();
                    tmpData.x = i * spaceX_next;
                    tmpData.y = j * spaceY_next;
                    resList.Add(tmpData);
                }
            }
            return resList;
        }
        //ブロックパターン構成升の埋めチェックを変更
        private void PaintBlock(BlockPattern bp)
        {
            //マスデータの真偽値を変更
            fillValueChange(mathDataList,bp.PatternNum, bp.Num1, true);
            fillValueChange(mathDataList, bp.PatternNum, bp.Num2, true);
            fillValueChange(mathDataList, bp.PatternNum, bp.Num3, true);
            fillValueChange(mathDataList, bp.PatternNum, bp.Num4, true);
            //全マスのチェックと描画
            CheckMathSweep();
        }
        private void CheckMathSweep()
        {
            for (int i = 0; i < mathDataList.Count; i++)
            {
                //マスが埋まっている場合は色付きで矩形描画
                if (mathDataList[i].fill)
                {
                    Brush brush = new SolidBrush(mathDataList[i].col);
                    mathData tmp = mathDataList[i];
                    Point tPoint = new Point(tmp.x, tmp.y);
                    Size tSize = new Size(spaceX, spaceY);

                    gr.DrawRectangle((new Pen(Color.Gray)), (new Rectangle(tPoint, tSize)));
                    gr.FillRectangle(brush, (new Rectangle(tPoint, tSize)));
                }
                //マスが埋まっていなければ、背景色で矩形描画
                else
                {
                    mathData tmp = mathDataList[i];
                    Point tPoint = new Point(tmp.x, tmp.y);
                    Size tSize = new Size(spaceX, spaceY);
                    gr.FillRectangle(Brushes.White, (new Rectangle(tPoint, tSize)));
                }
            }
            //枠線描画と更新処理
            PaintLine();
            Field.Refresh();
        }
        private void CheckMathSweep_next(int math)
        {
            for (int i = 0; i < NextmathDataList.Count; i++)
            {
                //マスが埋まっている場合は色付きで矩形描画
                if (NextmathDataList[i].fill)
                {
                    Brush brush = new SolidBrush(NextmathDataList[i].col);
                    mathData tmp = NextmathDataList[i];
                    Point tPoint = new Point(tmp.x, tmp.y);
                    Size tSize = new Size(spaceX_next, spaceY_next);

                    gr_next.DrawRectangle((new Pen(Color.Gray)), (new Rectangle(tPoint, tSize)));
                    gr_next.FillRectangle(brush, (new Rectangle(tPoint, tSize)));
                }
                //マスが埋まっていなければ、背景色で矩形描画
                else
                {
                    mathData tmp = NextmathDataList[i];
                    Point tPoint = new Point(tmp.x, tmp.y);
                    Size tSize = new Size(spaceX_next, spaceY_next);
                    gr_next.FillRectangle(Brushes.White, (new Rectangle(tPoint, tSize)));
                }
            }
            //枠線描画と更新処理
            PaintLine_next(math);
            NextBox.Refresh();
        }
        private void MoveBlock(BlockPattern bp,int dir)
        {
            finishFlg = false;
            //ブロックの移動が可能な場合は処理
            if (checkMoveBlock(bp, dir) == false)
            {
                //ブロックの升埋め属性をリセット
                resetBlock(bp);
                //ブロックの移動
                bp = MoveBlockDir(bp, dir);
                //ブロックの描画
                PaintBlock(bp);
                //操作ブロックの更新
                currentBP = bp;
            }
            //ブロックの移動ができない場合は処理中断
            else
            {
                //下移動ができない場合は移動終了とする
                if ((dir == 1)&&(finishFlg==false))
                {
                    FinishBlock();
                }
            }
        }
        //指定のブロックマスをすべてマス埋め開放する
        private void resetBlock(BlockPattern bp)
        {
            fillValueChange(mathDataList,bp.PatternNum, bp.Num1, false);
            fillValueChange(mathDataList, bp.PatternNum, bp.Num2, false);
            fillValueChange(mathDataList, bp.PatternNum, bp.Num3, false);
            fillValueChange(mathDataList, bp.PatternNum, bp.Num4, false);
            CheckMathSweep();
        }
        //指定の方向へブロックを移動させる
        private BlockPattern MoveBlockDir(BlockPattern bp,int dir)
        {
            BlockPattern pattern = bp;
            switch (dir)
            {
                case 0:
                    //右に移動
                    pattern.Num1 += 1;
                    pattern.Num2 += 1;
                    pattern.Num3 += 1;
                    pattern.Num4 += 1;
                    pattern.first += 1;
                    break;
                case 1:
                    //下に移動
                    pattern.Num1 -= xBlock;
                    pattern.Num2 -= xBlock;
                    pattern.Num3 -= xBlock;
                    pattern.Num4 -= xBlock;
                    pattern.first -= xBlock;
                    break;
                case 2:
                    //左に移動
                    pattern.Num1 -= 1;
                    pattern.Num2 -= 1;
                    pattern.Num3 -= 1;
                    pattern.Num4 -= 1;
                    pattern.first -= 1;
                    break;
            }
            bp = pattern;
            return bp;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //timer1.Enabled = true;
            //MoveBlock(currentBP, 1);
        }
        //キー操作処理
        protected override bool ProcessDialogKey(Keys keyData)
        {
                switch (keyData)
                {
                    case Keys.Left:
                        MoveBlock(currentBP, 2);
                        break;
                    case Keys.Down:
                        MoveBlock(currentBP, 1);
                        break;
                    case Keys.Right:
                        MoveBlock(currentBP, 0);
                        break;
                    case Keys.Enter:

                        rotationBlock(currentBP);
                        break;
                }

            return base.ProcessDialogKey(keyData);
        }
        //ブロック移動前のチェック
        private bool checkMoveBlock(BlockPattern bp,int dir)
        {
            List<int> bpList = new List<int> {bp.Num1,bp.Num2,bp.Num3,bp.Num4 };
            bool hit = false;
            int key = xBlock;
            switch (dir)
            {
                //右方向の指定
                case 0:
                    for (int i = 0; i < bpList.Count; i++)
                    {
                        //右の端にブロックが位置する場合はNGとする
                        if (((bpList[i]+1) % key) == 0)
                        {
                            hit = true;
                        }
                    }
                    break;
                //下方向の指定
                case 1:
                    for (int i = 0; i < bpList.Count; i++)
                    {
                        //ブロックのうち、一つでも最下層に位置するものがあれば、それ以上、下方向に進めないのでNGとする
                        if ((bpList[i]-9) <= 0)
                        {
                            hit = true;
                            FinishBlock();
                            break;
                        }
                    }
                    break;
                //左方向の指定
                case 2:
                    for (int i = 0; i < bpList.Count; i++)
                    {
                        //左の端にブロックが位置する場合はNGとする
                        if ((bpList[i] % key) == 0)
                        {
                            hit = true;
                        }
                    }
                    break;
            }
            if (hit == false)
            {
                //移動後のブロックパターンを取得
                BlockPattern afterMoveList = MoveBlockDir(bp, dir);
                //移動後に別ブロックとの衝突があればNGとする
                if (checkBlockConflict(bp, afterMoveList))
                {
                    hit = true;
                }
            }
            //判定を返す
            return hit;
        }
        //回転前のチェック処理
        private bool checkRotateBlock(BlockPattern bp)
        {
            //現在のブロックが回転したときに端に抵触する可能性があるかどうか判定
            //現在の座標によって、左右、下のどの端に接触する可能性があるか判定を行う
            bool right = false;
            bool down = false;
            bool left = false;
            int[] blocks = new int[4] { bp.Num1,bp.Num2,bp.Num3,bp.Num4};
            if (bp.PatternNum == 7)
            {
                if ((bp.first % xBlock) >= xBlock - 3)

                {
                    right = true;
                }
            }
            
            else if(bp.PatternNum != 8)
            {
                //右端ブロックに位置
                if ((bp.first % xBlock) == xBlock - 2)
                {
                    right = true;
                }
                //下ブロックに位置
                else if ((bp.first / xBlock) == 1)
                {
                    down = true;
                }
                //左端ブロックに位置
                else if ((bp.first % xBlock) == xBlock - 1)
                {
                    left = true;
                }
            }
            //その判定によって、必要な判定を行う
            bool checkRes = false;
            //回転後のブロックパターンを取得
            BlockPattern afterRotate = rotateBlock(bp);
            //各方向の判定結果を取得
            bool[] dirCheckList = new bool[3] { right, down, left };
            
            //もし、右端ブロックに抵触していれば、右端ブロックの包含チェックを行う
            for (int i = 0; i < dirCheckList.Length; i++)
            {
                //チェックされた方向がTrueであれば判定
                if (dirCheckList[i])
                {
                    //指定した方向への包含チェックを行う
                    if (checkContains(afterRotate, i))
                    {
                        checkRes = true;
                        break;
                    }
                }
            }
            
            //回転後のブロックとfill状態のマスが一致すれば、checkResをtrueにする
            //回転後のブロックから現在のブロックと一致するマスを除く
            //残ったマスがfill状態であれば、回転中止
            int[] afterRotationBlocks = new int[4] { afterRotate.Num1, afterRotate.Num2, afterRotate.Num3, afterRotate.Num4 };
            List<int> unmatchList = new List<int> { };

            if (checkBlockConflict(bp, afterRotate))
            {
                checkRes = true;
            }
            
            return checkRes;
        }
        private bool checkContains(BlockPattern bp, int dir)
        {
            //検索対象のブロックパターンからデータを抽出
            int key = bp.first;
            int[] blocks = new int[4] { bp.Num1, bp.Num2, bp.Num3, bp.Num4 };
            // dirで三方向のどの方向のチェックをするかを判定
            int[] hitList=new int[3];
            switch (dir)
            {
                //右方向
                case 0:
                    if (bp.PatternNum == 8)
                    {
                        hitList = new int[3] { bp.first + 1 - (xBlock * 3), bp.first + 2 - (xBlock*3), bp.first + 3 - (xBlock * 3) };
                        break;
                    }
                    hitList = new int[3] { bp.first + 2, bp.first + 2 - xBlock, bp.first + 2 - (xBlock * 2) };
                    break;
                //下方向
                case 1:
                    hitList = new int[3] { bp.first - (xBlock * 2), bp.first - (xBlock * 2) + 1, bp.first - (xBlock * 2) + 2 };
                    break;
              　//左方向
                case 2:
                    hitList = new int[3] { bp.first, bp.first - xBlock, bp.first - (2*xBlock) };
                    break;
                default:
                    MessageBox.Show("hitListが定義されていません");
                    break;
            }
            bool result = false;
            for (int i = 0; i < blocks.Length; i++)
            {
                for (int j = 0; j < hitList.Length; j++)
                {
                    if (blocks[i] == hitList[j])
                    {
                        result = true;
                        if (dir == 1)
                        {
                            FinishBlock();
                        }
                    }
                }
            }
            return result;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (GO == false)
            {
                MoveBlock(currentBP, 1);
            }
            else
            {
                timer1.Enabled = false;
            }
        }
        private void FinishBlock()
        {
            //消去列があるかどうかチェック
            deleteRowCheck(currentBP);
            //次のブロックを操作ブロックに指定
            currentBP = retBlockPattern(NextBPNum);
            //NextBoxの指定
            MakeNextBox(BlockPatternSetting());
            finishFlg = true;
        }

        private bool checkBlockConflict(BlockPattern bp,BlockPattern afterBp)
        {
            //他ブロックとの衝突判定
            bool result = false;
            int[] blocks = new int[4] { bp.Num1,bp.Num2,bp.Num3,bp.Num4};
            int[] afterBlocks = new int[4] { afterBp.Num1, afterBp.Num2, afterBp.Num3, afterBp.Num4 };
            //全マスをチェックする処理
            for (int i = 0; i < mathDataList.Count; i++)
            {
                //マス目が埋まっていた場合
                if (mathDataList[i].fill)
                {
                    //そのマス目が移動前ブロックのマスかどうかを判定
                    if (checkSelfBlock(bp, i))
                    {
                        //移動前ブロックのマスであれば、次のループに移行
                        continue;
                    }
                    //回転後ブロックの当たり判定をする
                    for(int j = 0; j < 4; j++)
                    {
                        //もし、回転後ブロックと埋めマスが一致した場合は当たり判定を返す
                        if (i == afterBlocks[j])
                        {
                            result = true;
                        }
                    }
                }
            }
            return result;
        }
        private bool checkSelfBlock(BlockPattern bp,int index)
        {
            bool result = false;
            int[] blocks = new int[4] {bp.Num1,bp.Num2,bp.Num3,bp.Num4 };
            for(int i = 0; i < 4; i++)
            {
                if (index == blocks[i])
                {
                    result = true;
                }
            }
            return result;
        }
        private bool checkHit(BlockPattern bp)
        {
            bool result = false;
            int[] blocks = new int[4] { bp.Num1, bp.Num2, bp.Num3, bp.Num4 };
            for (int i = 0; i < mathDataList.Count; i++)
            {
                if (mathDataList[i].fill)
                {
                    for(int j = 0; j < blocks.Length; j++)
                    {
                        if (i == blocks[j])
                        {
                            result = true;
                        }
                    }
                }
            }
            return result;
        }
        //ブロック補正メソッド
        private BlockPattern correctBlock(BlockPattern bp,int dir)
        {
            BlockPattern retBP = new BlockPattern();
                
            switch (dir)
            {
                //上方向
                case 0:
                    retBP.Num1 = bp.Num1 + xBlock;
                    retBP.Num2 = bp.Num2 + xBlock;
                    retBP.Num3 = bp.Num3 + xBlock;
                    retBP.Num4 = bp.Num4 + xBlock;

                    retBP.blockArr = new int[4] { bp.blockArr[0]-3, bp.blockArr[1]-3, bp.blockArr[2]-3, bp.blockArr[3]-3 };
                    
                    retBP.PatternNum = bp.PatternNum;
                    retBP.SpecialPattern = true;
                    break;
                //右方向
                case 1:
                    retBP.Num1 = bp.Num1 + 1;
                    retBP.Num2 = bp.Num2 + 1;
                    retBP.Num3 = bp.Num3 + 1;
                    retBP.Num4 = bp.Num4 + 1;

                    retBP.blockArr = new int[4] { bp.blockArr[0] +1, bp.blockArr[1] +1, bp.blockArr[2] +1, bp.blockArr[3] +1 };

                    retBP.PatternNum = bp.PatternNum;
                    retBP.SpecialPattern = true;
                    break;
                //下方向
                case 2:
                    retBP.Num1 = bp.Num1 -xBlock;
                    retBP.Num2 = bp.Num2 - xBlock;
                    retBP.Num3 = bp.Num3 - xBlock;
                    retBP.Num4 = bp.Num4 - xBlock;

                    retBP.blockArr = new int[4] { bp.blockArr[0] + 3, bp.blockArr[1] + 3, bp.blockArr[2] + 3, bp.blockArr[3] + 3 };

                    retBP.PatternNum = bp.PatternNum;
                    retBP.SpecialPattern = true;
                    break;
                //左方向
                case 3:
                    retBP.Num1 = bp.Num1 - 1;
                    retBP.Num2 = bp.Num2 - 1;
                    retBP.Num3 = bp.Num3 - 1;
                    retBP.Num4 = bp.Num4 - 1;

                    retBP.blockArr = new int[4] { bp.blockArr[0] -1, bp.blockArr[1] -1, bp.blockArr[2] -1, bp.blockArr[3] -1 };

                    retBP.PatternNum = bp.PatternNum;
                    retBP.SpecialPattern = true;
                    break;
            }
            return retBP;
        }

        private void deleteRowCheck(BlockPattern bp)
        {
            int[] blocks = new int[4]{ bp.Num1,bp.Num2,bp.Num3,bp.Num4};
            Array.Reverse(blocks);

            for (int i = 0; i < blocks.Length; i++)
                {
                //列数
                int xRows=(blocks[i] / xBlock)*xBlock;
                bool res = false;
                for(int j = xRows; j < xRows + xBlock; j++)
                {
                    //マス埋めがされてないコマがあれば、チェックする
                    if (mathDataList[j].fill == false)
                    {
                        res = true;
                        break;
                    }
                }
                //1列チェックしてすべて埋まっていた場合、その列をすべてfalseにする
                if (res==false)
                {
                    for (int n = xRows; n < xRows + xBlock; n++)
                    {
                        //fillValueTrue(bp,n, false);
                        fillValueChange(mathDataList, bp.PatternNum, n, false);
                    }
                    slideBlocks(xRows);
                    i -= 1;
                }
            }
            Field.Refresh();
        }
        private void slideBlocks(int xRows)
        {
            //xRowsの1列上から上方向へ処理を開始
            int startIndex = xRows + xBlock;
            for (int i = startIndex; i < mathDataList.Count; i++)
            {
                //fillマスを見つけたら、fillをfalseにして、一列下のマスをtrueにする
                if (mathDataList[i].fill)
                {
                    //消去マスをfill状態から変更
                    slideBlock(i);
                }
            }
        }
        private void slideBlock(int index)
        {
            mathData tmpMathData = mathDataList[index];
            tmpMathData.fill = false;
            mathDataList[index] = tmpMathData;

            mathData tmpMathData2 = mathDataList[index-xBlock];
            tmpMathData2.fill = true;
            tmpMathData2.col = tmpMathData.col;
            mathDataList[index - xBlock] = tmpMathData2;
        }
    }

    
}
